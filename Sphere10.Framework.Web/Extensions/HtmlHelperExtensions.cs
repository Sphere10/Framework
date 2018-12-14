//-----------------------------------------------------------------------
// <copyright file="HtmlHelperExtensions.cs" company="Sphere 10 Software">
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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Sphere10.Framework.Web {
	public static class HtmlHelperExtensions {

		public static bool IsReleaseBuild(this HtmlHelper helper) {
			#if DEBUG
				return false;
			#else
				return true;
			#endif
		}

	}
}
