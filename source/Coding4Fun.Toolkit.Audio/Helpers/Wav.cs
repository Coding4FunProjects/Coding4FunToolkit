using System;
using System.IO;
using System.Text;

namespace Coding4Fun.Toolkit.Audio.Helpers
{
	// https://ccrma.stanford.edu/courses/422/projects/WaveFormat/
	// http://forums.create.msdn.com/forums/p/99360/591297.aspx

	public static class Wav
	{
		public static MemoryStream GetWavAsMemoryStream(this byte[] data, int sampleRate, int audioChannels = 1, int bitsPerSample = 16)
		{
			var tempBuffer = new MemoryStream();

			WriteHeader(tempBuffer, sampleRate, audioChannels, bitsPerSample);
			SeekPastHeader(tempBuffer);

			tempBuffer.Write(data, 0, data.Length);

			UpdateHeader(tempBuffer);

			return tempBuffer;
		}

		public static MemoryStream GetWavAsMemoryStream(this Stream data, int sampleRate, int audioChannels = 1, int bitsPerSample = 16)
		{
			var tempBuffer = new MemoryStream();

			WriteHeader(tempBuffer, sampleRate, audioChannels, bitsPerSample);
			SeekPastHeader(tempBuffer);

			data.Position = 0;
			data.CopyTo(tempBuffer);

			UpdateHeader(tempBuffer);

			return tempBuffer;
		}

		public static byte[] GetWavAsByteArray(this byte[] data, int sampleRate, int audioChannels = 1, int bitsPerSample = 16)
		{
			return data.GetWavAsMemoryStream(sampleRate, audioChannels, bitsPerSample).ToArray();
		}

		public static byte[] GetWavAsByteArray(this Stream data, int sampleRate, int audioChannels = 1, int bitsPerSample = 16)
		{
			return data.GetWavAsMemoryStream(sampleRate, audioChannels, bitsPerSample).ToArray();
		}

		public static void WriteHeader(Stream stream, int sampleRate, int audioChannels = 1, int bitsPerSample = 16)
		{
			var bytesPerSample = bitsPerSample / 8;

			var encoding = Encoding.UTF8;
            var oldPos = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

			// ChunkID Contains the letters "RIFF" in ASCII form (0x52494646 big-endian form).
			stream.Write(encoding.GetBytes("RIFF"), 0, 4);

			// ChunkSize  36 + SubChunk2Size
			// NOTE this will be filled in later
			stream.Write(BitConverter.GetBytes(0), 0, 4);

			// Format Contains the letters "WAVE"(0x57415645 big-endian form).
			stream.Write(encoding.GetBytes("WAVE"), 0, 4);

			// Subchunk1ID Contains the letters "fmt " (0x666d7420 big-endian form).
			stream.Write(encoding.GetBytes("fmt "), 0, 4);

			// Subchunk1Size 16 for PCM.  This is the size of therest of the Subchunk which follows this number.
			stream.Write(BitConverter.GetBytes(16), 0, 4);

			// AudioFormat PCM = 1 (i.e. Linear quantization) Values other than 1 indicate some form of compression.
			stream.Write(BitConverter.GetBytes((short)1), 0, 2);

			// NumChannels Mono = 1, Stereo = 2, etc.
			stream.Write(BitConverter.GetBytes((short)audioChannels), 0, 2);

			// SampleRate 8000, 44100, etc.
			stream.Write(BitConverter.GetBytes(sampleRate), 0, 4);

			// ByteRate =  SampleRate * NumChannels * BitsPerSample/8
			stream.Write(BitConverter.GetBytes(sampleRate * bytesPerSample * audioChannels), 0, 4);

			// BlockAlign NumChannels * BitsPerSample/8 The number of bytes for one sample including all channels.
			stream.Write(BitConverter.GetBytes((short)(bytesPerSample)), 0, 2);

			// BitsPerSample    8 bits = 8, 16 bits = 16, etc.
			stream.Write(BitConverter.GetBytes((short)(bitsPerSample)), 0, 2);

			// Subchunk2ID Contains the letters "data" (0x64617461 big-endian form).
			stream.Write(encoding.GetBytes("data"), 0, 4);

			// Subchunk2Size == NumSamples * NumChannels * BitsPerSample/8 This is the number of bytes in the data.
			// NOTE to be filled in later
			stream.Write(BitConverter.GetBytes(0), 0, 4);
			UpdateHeader(stream);

            stream.Seek(oldPos, SeekOrigin.Begin);
		}

		public static void SeekPastHeader(Stream stream)
		{
			if (!stream.CanSeek)
				throw new Exception("Can't seek stream to update wav header");

			stream.Seek(44, SeekOrigin.Begin);
		}

		public static void UpdateHeader(Stream stream)
		{
			if (!stream.CanSeek)
				throw new Exception("Can't seek stream to update wav header");

			var oldPos = stream.Position;

			// ChunkSize  36 + SubChunk2Size
			stream.Seek(4, SeekOrigin.Begin);
			stream.Write(BitConverter.GetBytes((int)stream.Length - 8), 0, 4);

			// Subchunk2Size == NumSamples * NumChannels * BitsPerSample/8 This is the number of bytes in the data.
			stream.Seek(40, SeekOrigin.Begin);
			stream.Write(BitConverter.GetBytes((int)stream.Length - 44), 0, 4);

			stream.Seek(oldPos, SeekOrigin.Begin);
		}
	}
}
