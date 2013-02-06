// (c) Copyright Morten Nielsen.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.
using System;
using System.Net;
using System.Threading;


namespace Coding4Fun.Toolkit.Net
{
	/// <summary>
	/// This is really just a wrapper class for HttpWebRequest that adds the gzip header,
	/// and checks the response for gzip. If it's gzip'ed, it will uncompress the stream.
	/// </summary>
	/// <remarks>
	/// This class is only used by the <see cref="WebRequestCreator"/>.
	/// </remarks>
	internal sealed class GzipHttpWebRequest : HttpWebRequest
	{
		private readonly System.Net.WebRequest _internalWebRequest;

		public GzipHttpWebRequest(Uri uri)
		{
			_internalWebRequest = System.Net.WebRequest.CreateHttp(uri);
			Headers[HttpRequestHeader.AcceptEncoding] = "gzip";
		}

		public override IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
		{
			return _internalWebRequest.BeginGetRequestStream(callback, state);
		}

		public override System.IO.Stream EndGetRequestStream(IAsyncResult asyncResult)
		{
			return _internalWebRequest.EndGetRequestStream(asyncResult);
		}

		public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
		{
			return _internalWebRequest.BeginGetResponse(callback, state);
		}

		public override WebResponse EndGetResponse(IAsyncResult asyncResult)
		{
			var response = _internalWebRequest.EndGetResponse(asyncResult);
			if (response.Headers[HttpRequestHeader.ContentEncoding] == "gzip" && response is HttpWebResponse)
				return new GzipWebClient.GzipWebResponse(response as HttpWebResponse);
			else
				return response;
		}

		public override string Method
		{
			get { return _internalWebRequest.Method; }
			set { _internalWebRequest.Method = value; }
		}

		public override string ContentType
		{
			get { return _internalWebRequest.ContentType; }
			set { _internalWebRequest.ContentType = value; }
		}

		public override ICredentials Credentials
		{
			get { return _internalWebRequest.Credentials; }
			set { _internalWebRequest.Credentials = value; }
		}

		public override WebHeaderCollection Headers
		{
			get { return _internalWebRequest.Headers; }
			set { _internalWebRequest.Headers = value; }
		}

		public override bool SupportsCookieContainer
		{
			get
			{
				if (_internalWebRequest is HttpWebRequest)
					return (_internalWebRequest as HttpWebRequest).SupportsCookieContainer;
				else
					return base.SupportsCookieContainer;
			}
		}

		public override CookieContainer CookieContainer
		{
			get
			{
				if (_internalWebRequest is HttpWebRequest)
					return (_internalWebRequest as HttpWebRequest).CookieContainer;
				else
					return base.CookieContainer;
			}
			set
			{
				if (_internalWebRequest is HttpWebRequest)
					(_internalWebRequest as HttpWebRequest).CookieContainer = value;
				else
					throw new NotSupportedException();
			}
		}

		public override bool AllowAutoRedirect
		{
			get
			{
				if (_internalWebRequest is HttpWebRequest)
					return (_internalWebRequest as HttpWebRequest).AllowAutoRedirect;
				else
					return base.AllowAutoRedirect;
			}
			set
			{
				if (_internalWebRequest is HttpWebRequest)
					(_internalWebRequest as HttpWebRequest).AllowAutoRedirect = value;
				else
					throw new NotSupportedException();
			}
		}

		public override bool HaveResponse
		{
			get
			{
				if (_internalWebRequest is HttpWebRequest)
					return (_internalWebRequest as HttpWebRequest).HaveResponse;
				else
					return base.HaveResponse;
			}
		}

		public override bool AllowReadStreamBuffering
		{
			get
			{
				if (_internalWebRequest is HttpWebRequest)
					return (_internalWebRequest as HttpWebRequest).AllowReadStreamBuffering;
				else
					return base.AllowReadStreamBuffering;
			}
			set
			{
				if (_internalWebRequest is HttpWebRequest)
					(_internalWebRequest as HttpWebRequest).AllowReadStreamBuffering = value;
				else
					throw new NotSupportedException();
			}
		}

		public override IWebRequestCreate CreatorInstance
		{
			get
			{
				if (_internalWebRequest is HttpWebRequest)
					return (_internalWebRequest as HttpWebRequest).CreatorInstance;
				else
					return base.CreatorInstance;
			}
		}

		public override void Abort()
		{
			_internalWebRequest.Abort();
		}

		public override bool UseDefaultCredentials
		{
			get { return _internalWebRequest.UseDefaultCredentials; }
			set { _internalWebRequest.UseDefaultCredentials = value; }
		}

		public override Uri RequestUri
		{
			get { return _internalWebRequest.RequestUri; }
		}
	}
}
