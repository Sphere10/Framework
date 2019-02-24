////-----------------------------------------------------------------------
//// <copyright file="ApplicationLifecycleExtensions.cs" company="Sphere 10 Software">
////
//// Copyright (c) Sphere 10 Software. All rights reserved. (http://www.sphere10.com)
////
//// Distributed under the MIT software license, see the accompanying file
//// LICENSE or visit http://www.opensource.org/licenses/mit-license.php.
////
//// <author>Herman Schoenfeld</author>
//// <date>2018</date>
//// </copyright>
////-----------------------------------------------------------------------

//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;
//using Sphere10.Framework;
//using Sphere10.Application;

//namespace Sphere10.Application.Web {
//    public static class ApplicationLifecycleExtensions {

//        public static void StartAspNetApplication(this ApplicationLifecycle applicationLifecycle) {
//            applicationLifecycle.StartFramework();
//            // Set MVC dep resolver
//            System.Web.Mvc.DependencyResolver.SetResolver(new ComponentRegistryMvcDependencyResolver(ComponentRegistry.Instance));

//            //// Set Web API dep resolver
//            System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = new ComponentRegistryWebApiDependencyResolver(ComponentRegistry.Instance);
//        }

//        public static void EndAspNetApplication(this ApplicationLifecycle applicationLifecycle, out bool abort, out string abortReason) {
//            applicationLifecycle.EndFramework(out abort, out abortReason);
//        }

//        public static void EndAspNetApplication(this ApplicationLifecycle applicationLifecycle) {
//            var abort = false;
//            var abortReason = string.Empty;
//            applicationLifecycle.EndFramework(out abort, out abortReason);
//        }
//    }
//}
