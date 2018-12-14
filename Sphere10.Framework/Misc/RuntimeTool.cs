//-----------------------------------------------------------------------
// <copyright file="RuntimeTool.cs" company="Sphere 10 Software">
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
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Sphere10.Framework;
using Sphere10.Framework.FastReflection;

namespace Tools {

    public static class Runtime {
        private static readonly object _threadLock = new object();
        private static bool _hasDeterminedDesignMode = false;
        private static bool _isDesignMode = false;
        private static Assembly _entryAssembly = null;
        private static bool? _isWebApp = null;


        public static bool IsWebApp {
            get {
#if __MOBILE__
            return false;
#else
                if (_isWebApp == null) {
                    lock (_threadLock) {
                        if (_isWebApp == null) {
                            _isWebApp = Path.GetFileName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile).EndsWith("web.config", true, CultureInfo.InvariantCulture);
                        }
                    }
                }
#endif
                return _isWebApp.Value;
            }
        }
#if !__WP8__
        public static Assembly GetEntryAssembly() {
            if (_entryAssembly == null) {
                lock (_threadLock) {
                    if (_entryAssembly == null) {
                        _entryAssembly = IsWebApp ? GetWebEntryAssembly() : Assembly.GetEntryAssembly();
                        if (_entryAssembly == null)
                            throw new SoftwareException("Unable to determine entry assembly");
                    }
                }
            }
            return _entryAssembly;
        }



        private static Assembly GetWebEntryAssembly() {
            //return Assembly.GetExecutingAssembly();
            var httpContextType = TypeResolver.Resolve("System.Web.HttpContext");
            var httpContextCurrent = httpContextType.GetProperty("Current").FastGetValue(null);
            //var httpContextCurrentHandler = httpContextCurrent.GetType().GetProperty("Handler").FastGetValue(httpContextCurrent);
            var httpContextCurrentApplicationInstance = httpContextCurrent.GetType().GetProperty("ApplicationInstance").FastGetValue(httpContextCurrent);
            return httpContextCurrentApplicationInstance.GetType().BaseType.Assembly;
            //if ((System.Web.HttpContext.Current == null) || (System.Web.HttpContext.Current.Handler == null))
            //    return Tools.Runtime.GetEntryAssembly(); // Not a web application
            //return System.Web.HttpContext.Current.Handler.GetType().BaseType.Assembly;
        }

#endif

#if !__WP8__
        public static bool IsDesignMode {
            get {
                if (!_hasDeterminedDesignMode) {
                    lock (_threadLock) {
                        if (!_hasDeterminedDesignMode) {
                            var processName = Process.GetCurrentProcess().ProcessName.ToUpperInvariant();
                            _isDesignMode = processName.Contains("DEVENV") || processName.Contains("SHARPDEVELOP");
                            _hasDeterminedDesignMode = true;
                        }
                    }
                }
                return _isDesignMode;
            }
        }
#endif
    }


}




