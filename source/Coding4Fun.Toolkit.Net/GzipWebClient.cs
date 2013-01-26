// Copyright (C) Microsoft Corporation. All Rights Reserved.
// This code released under the terms of the Microsoft Public License
// (Ms-PL, http://opensource.org/licenses/ms-pl.html).

// David Anson
// http://blogs.msdn.com/b/delay/archive/2012/04/19/quot-if-i-have-seen-further-it-is-by-standing-on-the-shoulders-of-giants-quot-an-alternate-implementation-of-http-gzip-decompression-for-windows-phone.aspx

// help from Morten Nielsen as well
// http://sharpgis.net/

using System;
using System.IO;
using System.Net;
using System.Security;

namespace Coding4Fun.Toolkit.Net
{
    /// <summary>
    /// Class that extends WebClient to use GZIP (when supported by the server).
    /// </summary>
    public class GzipWebClient : WebClient
    {
        /// <summary>
        /// Initializes a new instance of the GzipWebClient class.
        /// </summary>
        [SecuritySafeCritical] // Avoids TypeLoadException
        public GzipWebClient()
        {
        }

        /// <summary>
        /// Returns a WebRequest object for the specified resource.
        /// </summary>
        /// <param name="address">A Uri that identifies the resource to request.</param>
        /// <returns>A new WebRequest object for the specified resource.</returns>
        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            var httpWebRequest = request as HttpWebRequest;

            if (null != httpWebRequest)
            {
                GzipExtensions.AddAcceptEncodingHeader(httpWebRequest);
            }

            return request;
        }

        /// <summary>
        /// Returns the WebResponse for the specified WebRequest using the specified IAsyncResult.
        /// </summary>
        /// <param name="request">A WebRequest that is used to obtain the response.</param>
        /// <param name="result">An IAsyncResult object obtained from a previous call to BeginGetResponse .</param>
        /// <returns>A WebResponse containing the response for the specified WebRequest.</returns>
        protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
        {
			try
			{
				var response = base.GetWebResponse(request, result);

				if (!(response is GZipWebResponse) && //this would be the case if WebRequestCreator was also used
				    (response.Headers[HttpRequestHeader.ContentEncoding] == "gzip") &&
				    response is HttpWebResponse)
				{
					return new GZipWebResponse(response as HttpWebResponse); //If gzipped response, uncompress
				}

				return response;
			}
			catch
			{
				return null;
			}

        }

        /// <summary>
        /// Class that wraps WebResponse to return an uncompressed response stream when GZIP was used.
        /// </summary>
		private class GZipWebResponse : HttpWebResponse
        {
            /// <summary>
            /// Stores the wrapped WebResponse.
            /// </summary>
			private readonly HttpWebResponse _response;

            /// <summary>
            /// Initializes a new instance of the WebResponseWrapper class.
            /// </summary>
            /// <param name="response">WebResponse to wrap.</param>
			public GZipWebResponse(HttpWebResponse response)
            {
                _response = response;
            }

            /// <summary>
            /// Returns the data stream from the Internet resource.
            /// </summary>
            /// <returns>An instance of the Stream class for reading data from the Internet resource.</returns>
            public override Stream GetResponseStream()
            {
                var httpWebResponse = _response;
                
				if (null != httpWebResponse)
                {
                    return httpWebResponse.GetCompressedResponseStream();
                }

	            return _response.GetResponseStream();
            }

            // Pass-through wrapper implementations
            public override void Close()
            {
                _response.Close();
            }
            
			public override long ContentLength
            {
                get { return _response.ContentLength; }
            }
            
			public override string ContentType
            {
                get { return _response.ContentType; }
            }
            
			public override WebHeaderCollection Headers
            {
                get { return _response.Headers; }
            }
            
			public override Uri ResponseUri
            {
                get { return _response.ResponseUri; }
            }
            
			public override bool SupportsHeaders
            {
                get { return _response.SupportsHeaders; }
            }
			
			public override string Method
			{
				get { return _response.Method; }
			}
			
			public override HttpStatusCode StatusCode
			{
				get { return _response.StatusCode; }
			}
			
			public override string StatusDescription
			{
				get { return _response.StatusDescription; }
			}
			
			public override CookieCollection Cookies
			{ 
				get { return _response.Cookies; }
			}
        }
    }
}
