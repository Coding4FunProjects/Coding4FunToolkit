// Copyright (C) Microsoft Corporation. All Rights Reserved.
// This code released under the terms of the Microsoft Public License
// (Ms-PL, http://opensource.org/licenses/ms-pl.html).
//
// Morten Nielsen
// http://sharpgis.net/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Coding4Fun.Toolkit.Net
{
	/// <summary>
	/// Web Request Creator
	/// </summary>
	public static class WebRequestCreator
	{
		private static GzipWebRequestFactory gzipCreator;

		/// <summary>
		/// Returns a HttpRequest Creator that supports gzip compressed
		/// web responses.
		/// </summary>
		public static IWebRequestCreate Gzip
		{
			get
			{
				if (gzipCreator == null)
				{
					gzipCreator = new GzipWebRequestFactory();
				}
				return gzipCreator;
			}
		}

		private class GzipWebRequestFactory : IWebRequestCreate
		{
			public WebRequest Create(Uri uri)
			{
				return new GzipHttpWebRequest(uri);
			}
		}
	}
}