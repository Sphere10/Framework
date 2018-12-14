//-----------------------------------------------------------------------
// <copyright file="MvcTransferResult.cs" company="Sphere 10 Software">
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
using System.Web.Mvc;
using System.Web.Routing;
using System.Net;


namespace Sphere10.Framework.Web
{
    // Allows controllers to transfer execution to other controllers internally.
    // Passing a page message and setting response http status code.
    // Adapted from http://stackoverflow.com/questions/799511/how-to-simulate-server-transfer-in-asp-net-mvc
    public class MvcTransferResult : RedirectResult
    {
        public MvcTransferResult(string url, HttpStatusCode httpStatusCode)
            : this(url, null, httpStatusCode)
        {
        }

        public MvcTransferResult(string url, UserMessage userMessage, HttpStatusCode httpStatusCode)
            : base(url)
        {
            UserMessage = userMessage;
            HttpStatusCode = httpStatusCode;
        }

        public MvcTransferResult(object routeValues, HttpStatusCode httpStatusCode, UserMessage pageMessage)
            : this(GetRouteUrl(routeValues), pageMessage, httpStatusCode)
        {
        }

        public HttpStatusCode HttpStatusCode { get; set; }

        public UserMessage UserMessage { get; set; }

        private static string GetRouteUrl(object routeValues)
        {
            var url = new UrlHelper(new RequestContext(new HttpContextWrapper(HttpContext.Current), new RouteData()), RouteTable.Routes);
            return url.RouteUrl(routeValues);
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var httpContext = HttpContext.Current;

            // Store message to context if applicable

            if (UserMessage != null)
            {
                httpContext.GetUserMessages().Add(UserMessage);
            }

            // Set HTTP response code
            httpContext.Response.StatusCode = (int)HttpStatusCode;

            // ASP.NET MVC 3.0
            //if (context.Controller.TempData != null &&
            //    context.Controller.TempData.Count() > 0)
            //{
            //    throw new ApplicationException("TempData won't work with Server.TransferRequest!");
            //}
            //httpContext.Server.TransferRequest(Url, true); // change to false to pass query string parameters if you have already processed them

            // ASP.NET MVC 2.0
            httpContext.RewritePath(Url, false);
            IHttpHandler httpHandler = new MvcHttpHandler();
            httpHandler.ProcessRequest(HttpContext.Current);
        }
    }
}
