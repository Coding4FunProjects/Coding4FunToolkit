// Copyright (C) Microsoft Corporation. All Rights Reserved.
// This code released under the terms of the Microsoft Public License
// (Ms-PL, http://opensource.org/licenses/ms-pl.html).

// David Anson
// http://blogs.msdn.com/b/delay/archive/2012/04/19/quot-if-i-have-seen-further-it-is-by-standing-on-the-shoulders-of-giants-quot-an-alternate-implementation-of-http-gzip-decompression-for-windows-phone.aspx

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Resources;

namespace Coding4Fun.Toolkit.Net
{
    /// <summary>
    /// Class that provides helper methods to add support for GZIP to Windows Phone.
    /// </summary>
    /// <remarks>
	/// <para>
	/// This has since been replaced by a new gzip implementation, and 
	/// this class is only provided for backwards compatibility.</para>
	/// <para>
    /// GZIP file format specification: http://tools.ietf.org/rfc/rfc1952.txt
	/// <br/>
    /// ZIP file specification: http://www.pkware.com/documents/casestudies/APPNOTE.TXT
	/// </para>
    /// </remarks>
	[Obsolete]
    public static class GzipExtensions
    {
    	/// <summary>
    	/// HTTP request header Accept-Encoding string.
    	/// </summary>
    	private const string Gzip = "gzip";
		private const string FileName = "f"; // MUST be 1 character (offsets below assume this)

    	/// <summary>
        /// Adds an HTTP Accept-Encoding header for GZIP.
        /// </summary>
        /// <param name="request">Request to modify.</param>
		[Obsolete]
		public static void AddAcceptEncodingHeader(HttpWebRequest request)
        {
            if (null == request)
            {
                throw new ArgumentNullException("request");
            }

            request.Headers[HttpRequestHeader.AcceptEncoding] = Gzip;
        }

        /// <summary>
        /// Begins an asynchronous request to an Internet resource, using GZIP when supported by the server.
        /// </summary>
        /// <param name="request">Request to act on.</param>
        /// <param name="callback">The AsyncCallback delegate.</param>
        /// <param name="state">The state object for this request.</param>
        /// <returns>An IAsyncResult that references the asynchronous request for a response.</returns>
        /// <remarks>
        /// Functionally equivalent to BeginGetResponse (with GZIP).
        /// </remarks>
		[Obsolete]
		public static IAsyncResult BeginGetCompressedResponse(this HttpWebRequest request, AsyncCallback callback, object state)
        {
            AddAcceptEncodingHeader(request);

			return request.BeginGetResponse(callback, state);
        }

        /// <summary>
        /// Returns the data stream from the Internet resource.
        /// </summary>
        /// <param name="response">Response to act on.</param>
        /// <returns>An instance of the Stream class for reading data from the Internet resource.</returns>
        /// Functionally equivalent to GetResponseStream (with GZIP).
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Returning a Stream for the caller to use.")]
		[Obsolete]
        public static Stream GetCompressedResponseStream(this HttpWebResponse response)
        {
            // Validate arguments
            if (null == response)
            {
                throw new ArgumentNullException("response");
            }

            // Check the response for GZIP
            var responseStream = response.GetResponseStream();
			if (string.Equals(response.Headers[HttpRequestHeader.ContentEncoding], Gzip, StringComparison.OrdinalIgnoreCase))
			{
				return new GzipInflateStream(responseStream);
			}        	
			// Not GZIP-compressed; return stream as-is
        	return responseStream;
        }
    }
}
