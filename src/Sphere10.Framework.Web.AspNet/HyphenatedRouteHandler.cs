//-----------------------------------------------------------------------
// <copyright file="HyphenatedRouteHandler.cs" company="Sphere 10 Software">
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
using System.Web.Routing;
using System.Web;
using System.Web.Mvc;

namespace Sphere10.Framework.Web  {
    public class HyphenatedRouteHandler : MvcRouteHandler {
        private readonly bool _underscoreLeadingDigit;

        public HyphenatedRouteHandler() : this(false) {

        }

        public HyphenatedRouteHandler(bool underscoreLeadingDigit) {
            _underscoreLeadingDigit = underscoreLeadingDigit;
        }

        protected override IHttpHandler GetHttpHandler(RequestContext requestContext) {
            // Process Controller
            var controller = requestContext.RouteData.Values["controller"].ToString();
            controller = controller.Replace("-", "_");
            if (_underscoreLeadingDigit) {
                if (controller.Length > 0 && char.IsDigit(controller[0])) {
                    controller = "_" + controller;
                }
            }
            requestContext.RouteData.Values["controller"] = controller;

            // Process Action
            string action =  requestContext.RouteData.Values["action"].ToString();
            action = action.Replace("-", "_");
            if (_underscoreLeadingDigit) {
                if (action.Length > 0 && char.IsDigit(action[0])) {
                    action = "_" + action;
                }
            }
            requestContext.RouteData.Values["action"] = action;
            return base.GetHttpHandler(requestContext);
        }
    }
}
