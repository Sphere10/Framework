//-----------------------------------------------------------------------
// <copyright file="UrlExtensions.cs" company="Sphere 10 Software">
//
// Copyright (c) Sphere 10 Software. All rights reserved. (http://www.sphere10.com)
//
// Distributed under the MIT software license, see the accompanying file
// LICENSE or visit http://www.opensource.org/licenses/mit-license.php.
//
// <author>Herman Schoenfeld</author>
// <date>2018</date>
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Web;
using System.Web.Mvc;

namespace Sphere10.Framework.Web {
	public static class UrlExtensions {
		public static string Content(this UrlHelper urlHelper, string contentPath, bool toAbsolute = false) {
			var path = urlHelper.Content(contentPath);
			var url = new Uri(HttpContext.Current.Request.Url, path);

			return toAbsolute ? url.AbsoluteUri : path;
		}
	}
}
