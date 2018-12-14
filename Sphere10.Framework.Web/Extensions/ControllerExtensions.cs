//-----------------------------------------------------------------------
// <copyright file="ControllerExtensions.cs" company="Sphere 10 Software">
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
using System.Web.Mvc;

namespace Sphere10.Framework.Web.Extensions {
	public static class ControllerExtensions {
        public static string MapUrl(this Controller controller, string path, bool absolute = false, string schemeOverride = null) {
            string appPath = controller.Server.MapPath("/").ToLower();

            var relUrl = string.Format("/{0}", path.ToLower().Replace(appPath, "").Replace(@"\", "/"));
            if (absolute) {
                var scheme = (schemeOverride ?? controller.Request.Url.Scheme).ToLower();
                var port = controller.Request.Url.Port;
                var includePort = string.IsNullOrWhiteSpace(schemeOverride) && (scheme == "http" && port != 80 || scheme == "https" && port != 443);
                relUrl = scheme + "://" + controller.Request.Url.DnsSafeHost + (includePort ? ":" + port : string.Empty) + relUrl;
            }
            return relUrl;
        }


	}
}
