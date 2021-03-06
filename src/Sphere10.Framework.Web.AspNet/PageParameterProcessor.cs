//-----------------------------------------------------------------------
// <copyright file="PageParameterProcessor.cs" company="Sphere 10 Software">
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
using System.Web;

namespace Sphere10.Framework.Web  {

    /// <summary>
    /// Summary description for PageParameters
    /// </summary>
	public class PageParameterProcessor {
		HttpRequest _request = null;


		public PageParameterProcessor(HttpRequest request) {
			_request = request;
		}

		public bool ContainsParameter(Enum parameter) {
			return !string.IsNullOrEmpty(
				DeterminePageParameterName(parameter)
			);
		}

		public T GetParameter<T>(Enum parameter)  {
			if (!ContainsParameter(parameter)) {
				throw new SoftwareException("Page does not have parameter {0}", DeterminePageParameterName(parameter));
			}
			return Tools.Parser.Parse<T>(Uri.UnescapeDataString((string)Request.Params[DeterminePageParameterName(parameter)]));
		}

		public HttpRequest Request {
			get { return _request; }
			set { _request = value; }
		}

		static private string GenerateParameter(Enum param, object value) {
			return
				string.Format("{0}={1}", Uri.EscapeDataString(DeterminePageParameterName(param)), Uri.EscapeDataString(value.ToString()));
				
		}

		static private string GenerateInvalidParameterError(string paramName, string expected) {
			return string.Format("Invalid page parameter '{0}' - expected {1}", paramName, expected);
		}


		#region Auxilliary methods

		private static string DeterminePageParameterName(Enum parameter) {
			// Use Parameter attributes name, or if none, just use the Enum name
			var retval = parameter.ToString();
			var attribute =  parameter.GetAttributes<Parameter>().SingleOrDefault();
			if (attribute != null) {
				retval = attribute.Name;
			}
			return retval;
		}



		#endregion



	}
}
