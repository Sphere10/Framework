//-----------------------------------------------------------------------
// <copyright file="HttpRequestExtensions.cs" company="Sphere 10 Software">
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
using System.Text;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.ComponentModel;
using System.Reflection;
using System.Web;

namespace Sphere10.Framework.Web {
	public static class HttpRequestExtensions {

		public static string GetParameter(this HttpRequest request, Enum param) {
			return request.GetParameter<string>(param);
		}

		public static bool ContainsParameter(this HttpRequest request, Enum param) {
			PageParameterProcessor processor = new PageParameterProcessor(request);
			return processor.ContainsParameter(param);
		}

		public static T GetParameter<T>(this HttpRequest request, Enum param) {
			PageParameterProcessor processor = new PageParameterProcessor(request);
			return processor.GetParameter<T>(param);
		}
	}

}
