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
		private const string GzipHeader = "gzip";

		/// <summary>
		/// Initializes a new instance of the <see cref="GzipWebClient"/> class.
		/// </summary>
		[SecuritySafeCritical] // Avoids TypeLoadException
		public GzipWebClient()
		{
		}

		/// <summary>
		/// Returns a <see cref="T:System.Net.WebRequest"/> object for the
		/// specified resource.
		/// </summary>
		/// <param name="address">A <see cref="T:System.Uri"/> that identifies 
		/// the resource to request.</param>
		/// <returns>
		/// A new <see cref="T:System.Net.WebRequest"/> object for the
		/// specified resource.
		/// </returns>
		protected override WebRequest GetWebRequest(Uri address)
		{
			var req = base.GetWebRequest(address);
			req.Headers[HttpRequestHeader.AcceptEncoding] = GzipHeader; //Set GZIP header
			return req;
		}

		/// <summary>
		/// Returns the <see cref="T:System.Net.WebResponse"/> for the specified
		/// <see cref="T:System.Net.WebRequest"/> using the specified 
		/// <see cref="T:System.IAsyncResult"/>.
		/// </summary>
		/// <param name="request">A <see cref="T:System.Net.WebRequest"/> that 
		/// is used to obtain the response.</param>
		/// <param name="result">An <see cref="T:System.IAsyncResult"/> object 
		/// obtained from a previous call to 
		/// <see cref="M:System.Net.WebRequest.BeginGetResponse(System.AsyncCallback,System.Object)"/>.
		/// </param>
		/// <returns>
		/// A <see cref="T:System.Net.WebResponse"/> containing the response for
		/// the specified <see cref="T:System.Net.WebRequest"/>.
		/// </returns>
		protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
		{
			try
			{
				WebResponse response = base.GetWebResponse(request, result);
				if (!(response is GzipWebResponse) && //this would be the case if WebRequestCreator was also used
				 (response.Headers[HttpRequestHeader.ContentEncoding] == GzipHeader) && response is HttpWebResponse)
					return new GzipWebResponse(response as HttpWebResponse); //If gzipped response, uncompress
				else
					return response;
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// Provides an HTTP-specific implementation of the System.Net.WebResponse class
		/// with support for decompressing gzip compressed responses.
		/// </summary>
		internal sealed class GzipWebResponse : HttpWebResponse
		{
			private readonly HttpWebResponse _response;
			private readonly GzipInflateStream _stream;

			/// <summary>
			/// Initializes a new instance of the <see cref="GzipWebResponse" /> class.
			/// </summary>
			/// <param name="response">WebResponse to wrap.</param>
			internal GzipWebResponse(HttpWebResponse response)
			{
				_response = response;
				_stream = new GzipInflateStream(_response.GetResponseStream());
			}

			/// <summary>
			/// Gets the stream that is used to read the body of the response 
			/// from the server.
			/// </summary>
			/// <returns>
			/// A <see cref="T:System.IO.Stream" /> containing the body of the 
			/// response.
			/// </returns>
			public override System.IO.Stream GetResponseStream()
			{
				return _stream;
			}

			/// <summary>
			/// Closes the response stream.
			/// </summary>
			public override void Close()
			{
				_response.Close();
				_stream.Close();
			}

			/// <summary>
			/// Gets the length of the content returned by the request.
			/// </summary>
			/// <returns>The number of bytes returned by the request. Content 
			/// length does not include header information.</returns>
			public override long ContentLength
			{
				get
				{
					return _stream.Length;
				}
			}

			/// <summary>
			/// Gets the content type of the response.
			/// </summary>
			/// <returns>A string that contains the content type of the 
			/// response.</returns>
			public override string ContentType
			{
				get { return _response.ContentType; }
			}

			/// <summary>
			/// Gets the headers that are associated with this response from the 
			/// server.
			/// </summary>
			/// <returns>A <see cref="T:System.Net.WebHeaderCollection" /> that
			/// contains the header information returned with the response.
			/// </returns>
			public override WebHeaderCollection Headers
			{
				get { return _response.Headers; }
			}

			/// <summary>
			/// Gets the URI of the Internet resource that responded to the request.
			/// </summary>
			/// <returns>A <see cref="T:System.Uri" /> that contains the URI of
			/// the Internet resource that responded to the request.</returns>
			public override Uri ResponseUri
			{
				get { return _response.ResponseUri; }
			}

			/// <summary>
			/// Gets a value that indicates whether the
			/// <see cref="P:System.Net.WebResponse.Headers" /> property is 
			/// supported by the descendant class for the
			/// <see cref="T:System.Net.WebResponse" /> instance.
			/// </summary>
			/// <returns>true if the <see cref="P:System.Net.WebResponse.Headers" /> 
			/// property is supported by the <see cref="T:System.Net.HttpWebRequest" />
			/// instance in the descendant class; otherwise, false.</returns>
			public override bool SupportsHeaders
			{
				get { return _response.SupportsHeaders; }
			}

			/// <summary>
			/// Gets the method that is used to return the response.
			/// </summary>
			/// <returns>A string that contains the HTTP method that is used to
			/// return the response.</returns>
			public override string Method
			{
				get { return _response.Method; }
			}

			/// <summary>
			/// Gets the status of the response.
			/// </summary>
			/// <returns>One of the <see cref="T:System.Net.HttpStatusCode" />
			/// values.</returns>
			public override HttpStatusCode StatusCode
			{
				get { return _response.StatusCode; }
			}

			/// <summary>
			/// Gets the status description returned with the response.
			/// </summary>
			/// <returns>A string that describes the status of the response.
			/// </returns>
			public override string StatusDescription
			{
				get { return _response.StatusDescription; }
			}

			/// <summary>
			/// Gets the cookies used to persist state information for the HTTP
			/// response.
			/// </summary>
			/// <returns>The <see cref="T:System.Net.CookieCollection" /> object 
			/// associated with the HTTP response.</returns>
			public override CookieCollection Cookies
			{
				get { return _response.Cookies; }
			}
		}
	}
}
