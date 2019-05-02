//-----------------------------------------------------------------------
// <copyright file="ApplicationLifecycleExtensions.cs" company="Sphere 10 Software">
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
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Sphere10.Framework;
using Sphere10.Framework.Application;

namespace Sphere10.Framework.Web {
    public static class Sphere10FrameworkExtensions {

        public static void StartAspNetApplication(this Sphere10Framework applicationLifecycle) {
            applicationLifecycle.StartFramework();
            // Set MVC dep resolver
            System.Web.Mvc.DependencyResolver.SetResolver(new ComponentRegistryMvcDependencyResolver(ComponentRegistry.Instance));

            //// Set Web API dep resolver
            System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = new ComponentRegistryWebApiDependencyResolver(ComponentRegistry.Instance);
		}

        public static void EndAspNetApplication(this Sphere10Framework applicationLifecycle, out bool abort, out string abortReason) {
            applicationLifecycle.EndFramework(out abort, out abortReason);
        }

        public static void EndAspNetApplication(this Sphere10Framework applicationLifecycle) {
            var abort = false;
            var abortReason = string.Empty;
            applicationLifecycle.EndFramework(out abort, out abortReason);
        }
    }
}
