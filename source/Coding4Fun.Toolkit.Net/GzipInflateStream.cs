// Copyright (C) Microsoft Corporation. All Rights Reserved.
// This code released under the terms of the Microsoft Public License
// (Ms-PL, http://opensource.org/licenses/ms-pl.html).
//
// Morten Nielsen
// http://sharpgis.net/
//
// This class is based off of work by David Anson: 
// http://blogs.msdn.com/b/delay/archive/2012/04/19/quot-if-i-have-seen-further-it-is-by-standing-on-the-shoulders-of-giants-quot-an-alternate-implementation-of-http-gzip-decompression-for-windows-phone.aspx

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Resources;

namespace Coding4Fun.Toolkit.Net
{
	/// <summary>
	/// Inflates a gzip-compressed stream by simulating a .xap package and
	/// using the built-in extractor to inflate the stream.
	/// </summary>
	internal sealed class GzipInflateStream : Stream
	{
		private readonly Stream _deflatedStream;
		private Stream _inflatedStream;

		/// <summary>
		/// Initializes a new instance of the <see cref="GZipInflateStream" /> class.
		/// </summary>
		/// <param name="deflatedStream">The deflated stream.</param>
		internal GzipInflateStream(System.IO.Stream deflatedStream)
		{
			_deflatedStream = deflatedStream;
			ProcessStream();
		}

		private void ProcessStream()
		{
			if ((0x1f != _deflatedStream.ReadByte()) || // ID1
			    (0x8b != _deflatedStream.ReadByte()) || // ID2
			    (8 != _deflatedStream.ReadByte())) // CM (8 == deflate)
			{
				throw new NotSupportedException("Compressed data not in the expected format.");
			}

			// Read flags
			var flg = _deflatedStream.ReadByte(); // FLG
			var fhcrc = 0 != (0x2 & flg); // CRC16 present before compressed data
			var fextra = 0 != (0x4 & flg); // extra fields present
			var fname = 0 != (0x8 & flg); // original file name present
			var fcomment = 0 != (0x10 & flg); // file comment present

			// Skip unsupported fields
			if (_deflatedStream.CanSeek)
				_deflatedStream.Seek(6, SeekOrigin.Current);
			else
			{
				_deflatedStream.ReadByte();
				_deflatedStream.ReadByte();
				_deflatedStream.ReadByte();
				_deflatedStream.ReadByte(); // MTIME
				_deflatedStream.ReadByte(); // XFL
				_deflatedStream.ReadByte(); // OS
			}

			if (fextra)
			{
				// Skip XLEN bytes of data
				var xlen = _deflatedStream.ReadByte() | (_deflatedStream.ReadByte() << 8);
				while (0 < xlen)
				{
					_deflatedStream.ReadByte();
					xlen--;
				}
			}
			if (fname)
			{
				// Skip 0-terminated file name
				while (0 != _deflatedStream.ReadByte())
				{
				}
			}
			if (fcomment)
			{
				// Skip 0-terminated file comment
				while (0 != _deflatedStream.ReadByte())
				{
				}
			}
			if (fhcrc)
			{
				_deflatedStream.ReadByte(); _deflatedStream.ReadByte(); // CRC16
			}

			// Read compressed data
			const int zipHeaderSize = 30 + 1; // 30 bytes + 1 character for file name
			const int zipFooterSize = 68 + 1; // 68 bytes + 1 character for file name

			// Download unknown amount of compressed data efficiently (note: Content-Length header is not always reliable)
			var buffers = new List<byte[]>();
			var buffer = new byte[4096];
			var bytesInBuffer = 0;
			var totalBytes = 0;
			var bytesRead = 0;
			do
			{
				if (buffer.Length == bytesInBuffer)
				{
					// Full, allocate another
					buffers.Add(buffer);
					buffer = new byte[buffer.Length];
					bytesInBuffer = 0;
				}
				Debug.Assert(bytesInBuffer < buffer.Length);
				bytesRead = _deflatedStream.Read(buffer, bytesInBuffer, buffer.Length - bytesInBuffer);
				bytesInBuffer += bytesRead;
				totalBytes += bytesRead;
			} while (0 < bytesRead);
			buffers.Add(buffer);

			// "Trim" crc32 and isize fields off the end
			var compressedSize = totalBytes - 4 - 4;
			if (compressedSize < 0)
			{
				throw new NotSupportedException("Compressed data not in the expected format.");
			}

			// Create contiguous buffer
			var compressedBytes = new byte[zipHeaderSize + compressedSize + zipFooterSize];
			var offset = zipHeaderSize;
			var remainingBytes = totalBytes;
			foreach (var b in buffers)
			{
				var length = Math.Min(b.Length, remainingBytes);
				Array.Copy(b, 0, compressedBytes, offset, length);
				offset += length;
				remainingBytes -= length;
			}
			Debug.Assert(0 == remainingBytes);

			// Read footer from end of compressed bytes (note: footer is within zipFooterSize; will be overwritten below)
			Debug.Assert(totalBytes <= compressedSize + zipFooterSize);
			offset = zipHeaderSize + compressedSize;
			var crc32 = compressedBytes[offset + 0] | (compressedBytes[offset + 1] << 8) | (compressedBytes[offset + 2] << 16) | (compressedBytes[offset + 3] << 24);
			var isize = compressedBytes[offset + 4] | (compressedBytes[offset + 5] << 8) | (compressedBytes[offset + 6] << 16) | (compressedBytes[offset + 7] << 24);

			if (0 == isize) // HACK to handle compressed 0-byte streams without figuring out what's really going wrong
			{
				_inflatedStream = new MemoryStream();
				return;
			}

			// Create ZIP file stream
			const string fileName = "f"; // MUST be 1 character (offsets below assume this)
			Debug.Assert(1 == fileName.Length);
			var zipFileMemoryStream = new MemoryStream(compressedBytes);
			var writer = new BinaryWriter(zipFileMemoryStream);

			// Local file header
			writer.Write((uint)0x04034b50); // local file header signature
			writer.Write((ushort)20); // version needed to extract (2.0 == compressed using deflate)
			writer.Write((ushort)0); // general purpose bit flag
			writer.Write((ushort)8); // compression method (8: deflate)
			writer.Write((ushort)0); // last mod file time
			writer.Write((ushort)0); // last mod file date
			writer.Write(crc32); // crc-32
			writer.Write(compressedSize); // compressed size
			writer.Write(isize); // uncompressed size
			writer.Write((ushort)1); // file name length
			writer.Write((ushort)0); // extra field length
			writer.Write((byte)fileName[0]); // file name

			// File data (already present)
			zipFileMemoryStream.Seek(compressedSize, SeekOrigin.Current);

			// Central directory structure
			writer.Write((uint)0x02014b50); // central file header signature
			writer.Write((ushort)20); // version made by
			writer.Write((ushort)20); // version needed to extract (2.0 == compressed using deflate)
			writer.Write((ushort)0); // general purpose bit flag
			writer.Write((ushort)8); // compression method
			writer.Write((ushort)0); // last mod file time
			writer.Write((ushort)0); // last mod file date
			writer.Write(crc32); // crc-32
			writer.Write(compressedSize); // compressed size
			writer.Write(isize); // uncompressed size
			writer.Write((ushort)1); // file name length
			writer.Write((ushort)0); // extra field length
			writer.Write((ushort)0); // file comment length
			writer.Write((ushort)0); // disk number start
			writer.Write((ushort)0); // internal file attributes
			writer.Write((uint)0); // external file attributes
			writer.Write((uint)0); // relative offset of local header
			writer.Write((byte)fileName[0]); // file name
			// End of central directory record
			writer.Write((uint)0x06054b50); // end of central dir signature
			writer.Write((ushort)0); // number of this disk
			writer.Write((ushort)0); // number of the disk with the start of the central directory
			writer.Write((ushort)1); // total number of entries in the central directory on this disk
			writer.Write((ushort)1); // total number of entries in the central directory
			writer.Write((uint)(46 + 1)); // size of the central directory (46 bytes + 1 character for file name)
			writer.Write((uint)(zipHeaderSize + compressedSize)); // offset of start of central directory with respect to the starting disk number
			writer.Write((ushort)0); // .ZIP file comment length

			// Reset ZIP file stream to beginning
			zipFileMemoryStream.Seek(0, SeekOrigin.Begin);

			// Return the decompressed stream
			_inflatedStream = Application.GetResourceStream(
				new StreamResourceInfo(zipFileMemoryStream, null),
				new Uri(fileName, UriKind.Relative))
				.Stream;
		}

		/// <summary>
		/// When overridden in a derived class, gets a value indicating whether 
		/// the current stream supports reading.
		/// </summary>
		/// <returns>true if the stream supports reading; otherwise, false.</returns>
		public override bool CanRead
		{
			get { return _inflatedStream.CanRead; }
		}

		/// <summary>
		/// When overridden in a derived class, gets a value indicating whether
		/// the current stream supports seeking.
		/// </summary>
		/// <returns>true if the stream supports seeking; otherwise, false.</returns>
		public override bool CanSeek
		{
			get { return _inflatedStream.CanSeek; }
		}

		/// <summary>
		/// When overridden in a derived class, gets a value indicating whether
		/// the current stream supports writing.
		/// </summary>
		/// <returns>true if the stream supports writing; otherwise, false.</returns>
		public override bool CanWrite
		{
			get { return _inflatedStream.CanWrite; }
		}

		/// <summary>
		/// When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to the underlying device.
		/// </summary>
		public override void Flush()
		{
			_inflatedStream.Flush();
		}

		/// <summary>
		/// When overridden in a derived class, gets the length in bytes of the
		/// stream.
		/// </summary>
		/// <returns>A long value representing the length of the stream in bytes.</returns>
		public override long Length
		{
			get { return _inflatedStream.Length; }
		}

		/// <summary>
		/// When overridden in a derived class, gets or sets the position within
		/// the current stream.
		/// </summary>
		/// <returns>The current position within the stream.</returns>
		public override long Position
		{
			get { return _inflatedStream.Position; }
			set { _inflatedStream.Position = value; }
		}

		/// <summary>
		/// When overridden in a derived class, reads a sequence of bytes from
		/// the current stream and advances the position within the stream by 
		/// the number of bytes read.
		/// </summary>
		/// <param name="buffer">An array of bytes. When this method returns, 
		/// the buffer contains the specified byte array with the values between
		/// <paramref name="offset" /> and (<paramref name="offset" /> +
		/// <paramref name="count" /> - 1) replaced by the bytes read from the 
		/// current source.</param>
		/// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> 
		/// at which to begin storing the data read from the current stream.</param>
		/// <param name="count">The maximum number of bytes to be read from the
		/// current stream.</param>
		/// <returns>
		/// The total number of bytes read into the buffer. This can be less than
		/// the number of bytes requested if that many bytes are not currently
		/// available, or zero (0) if the end of the stream has been reached.
		/// </returns>
		public override int Read(byte[] buffer, int offset, int count)
		{
			return _inflatedStream.Read(buffer, offset, count);
		}

		/// <summary>
		/// When overridden in a derived class, sets the position within the 
		/// current stream.
		/// </summary>
		/// <param name="offset">A byte offset relative to the <paramref name="origin" /> 
		/// parameter.</param>
		/// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin" /> 
		/// indicating the reference point used to obtain the new position.</param>
		/// <returns>
		/// The new position within the current stream.
		/// </returns>
		public override long Seek(long offset, SeekOrigin origin)
		{
			return _inflatedStream.Seek(offset, origin);
		}

		/// <summary>
		/// When overridden in a derived class, sets the length of the current 
		/// stream.
		/// </summary>
		/// <param name="value">The desired length of the current stream in 
		/// bytes.</param>
		public override void SetLength(long value)
		{
			_inflatedStream.SetLength(value);
		}

		/// <summary>
		/// When overridden in a derived class, writes a sequence of bytes to
		/// the current stream and advances the current position within this 
		/// stream by the number of bytes written.
		/// </summary>
		/// <param name="buffer">An array of bytes. This method copies
		/// <paramref name="count" /> bytes from <paramref name="buffer" /> to
		/// the current stream.</param>
		/// <param name="offset">The zero-based byte offset in <paramref name="buffer" />
		/// at which to begin copying bytes to the current stream.</param>
		/// <param name="count">The number of bytes to be written to the current
		/// stream.</param>
		/// <exception cref="System.NotSupportedException"></exception>
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Closes the current stream and releases any resources (such as sockets
		/// and file handles) associated with the current stream.
		/// </summary>
		public override void Close()
		{
			_deflatedStream.Close();
			_inflatedStream.Close();
		}

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="T:System.IO.Stream" />
		/// and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged 
		/// resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			_deflatedStream.Dispose();
			_inflatedStream.Dispose();
		}

		/// <summary>
		/// Begins an asynchronous read operation.
		/// </summary>
		/// <param name="buffer">The buffer to read the data into.</param>
		/// <param name="offset">The byte offset in <paramref name="buffer" /> 
		/// at which to begin writing data read from the stream.</param>
		/// <param name="count">The maximum number of bytes to read.</param>
		/// <param name="callback">An optional asynchronous callback, to be 
		/// called when the read is complete.</param>
		/// <param name="state">A user-provided object that distinguishes this
		/// particular asynchronous read request from other requests.</param>
		/// <returns>
		/// An <see cref="T:System.IAsyncResult" /> that represents the
		/// asynchronous read, which could still be pending.
		/// </returns>
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return _inflatedStream.BeginRead(buffer, offset, count, callback, state);
		}

		/// <summary>
		/// Reads a byte from the stream and advances the position within the 
		/// stream by one byte, or returns -1 if at the end of the stream.
		/// </summary>
		/// <returns>
		/// The unsigned byte cast to an Int32, or -1 if at the end of the stream.
		/// </returns>
		public override int ReadByte()
		{
			return _inflatedStream.ReadByte();
		}

		/// <summary>
		/// Waits for the pending asynchronous read to complete.
		/// </summary>
		/// <param name="asyncResult">The reference to the pending asynchronous
		/// request to finish.</param>
		/// <returns>
		/// The number of bytes read from the stream, between zero (0) and the
		/// number of bytes you requested. Streams return zero (0) only at the
		/// end of the stream, otherwise, they should block until at least one 
		/// byte is available.
		/// </returns>
		public override int EndRead(IAsyncResult asyncResult)
		{
			return _inflatedStream.EndRead(asyncResult);
		}

		/// <summary>
		/// Gets or sets a value, in miliseconds, that determines how long the 
		/// stream will attempt to read before timing out.
		/// </summary>
		/// <returns>A value, in miliseconds, that determines how long the stream 
		/// will attempt to read before timing out.</returns>
		public override int ReadTimeout
		{
			get { return _inflatedStream.ReadTimeout; }
			set { _inflatedStream.ReadTimeout = value; }
		}

		/// <summary>
		/// Gets a value that determines whether the current stream can time out.
		/// </summary>
		/// <returns>A value that determines whether the current stream can time out.</returns>
		public override bool CanTimeout
		{
			get { return _inflatedStream.CanTimeout; }
		}
	}
}

