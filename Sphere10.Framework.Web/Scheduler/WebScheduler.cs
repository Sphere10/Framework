//-----------------------------------------------------------------------
// <copyright file="WebScheduler.cs" company="Sphere 10 Software">
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
using Sphere10.Framework.Scheduler;

namespace Sphere10.Framework.Web {
    public class WebScheduler  {
        public const string ApplicationKey = "52CE10D58A1D4A9CBB9E57AEC17E17D6_Scheduler";

        public static Sphere10.Framework.Scheduler.Scheduler Instance {
            get {
                try {
	                
                    System.Web.HttpContext.Current.Application.Lock();
                    var instance = System.Web.HttpContext.Current.Application[ApplicationKey];
                    if (instance == null) {
                        var scheduler = new Sphere10.Framework.Scheduler.Scheduler(SchedulerPolicy.DontThrow);
                        System.Web.HttpContext.Current.Application[ApplicationKey] = scheduler;
                        scheduler.Start();
                    }
                    return (Sphere10.Framework.Scheduler.Scheduler)System.Web.HttpContext.Current.Application[ApplicationKey];
                } finally {
                    System.Web.HttpContext.Current.Application.UnLock();
                }
            }
        }
    }
}
