//-----------------------------------------------------------------------
// <copyright file="ComponentRegistryExtensions.cs" company="Sphere 10 Software">
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
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sphere10.Framework;

namespace Sphere10.Framework.Application {
    public static class ComponentRegistryExtensions {

        public static void RegisterInitializationTask<TApplicationInitializeTask>(this ComponentRegistry componentRegistry)
            where TApplicationInitializeTask : class, IApplicationInitializeTask {
            componentRegistry.RegisterComponent<IApplicationInitializeTask, TApplicationInitializeTask>(
                typeof(TApplicationInitializeTask).FullName
            );
        }

        public static bool HasInitializationTask<TApplicationInitializeTask>(this ComponentRegistry componentRegistry) 
            where TApplicationInitializeTask : class, IApplicationInitializeTask {
            return componentRegistry.HasImplementationFor<IApplicationInitializeTask>(
                typeof(TApplicationInitializeTask).FullName
            );
        }

        public static void RegisterStartTask<TApplicationStartTask>(this ComponentRegistry componentRegistry)
        where TApplicationStartTask : class, IApplicationStartTask {
            componentRegistry.RegisterComponent<IApplicationStartTask, TApplicationStartTask>(
                typeof(TApplicationStartTask).FullName
            );
        }

        public static void RegisterEndTask<TApplicationEndTask>(this ComponentRegistry componentRegistry)
            where TApplicationEndTask : class, IApplicationEndTask {
            componentRegistry.RegisterComponent<IApplicationEndTask, TApplicationEndTask>(
                typeof(TApplicationEndTask).FullName
            );
        }

        public static bool HasStartTask<TApplicationStartTask>(this ComponentRegistry componentRegistry) 
            where TApplicationStartTask : class, IApplicationStartTask {
            return componentRegistry.HasImplementationFor<IApplicationStartTask>(
                typeof(TApplicationStartTask).FullName
            );
        }

        public static bool HasEndTask<TApplicationEndTask>(this ComponentRegistry componentRegistry) 
            where TApplicationEndTask : class, IApplicationEndTask {
            return componentRegistry.HasImplementationFor<IApplicationEndTask>(
                typeof(TApplicationEndTask).FullName
            );
        }

#if !__MOBILE__

		public static void RegisterComponentSettings<TComponentSettings>(this ComponentRegistry componentRegistry)
            where TComponentSettings : ComponentSettings {
            componentRegistry.RegisterComponent<ComponentSettings, TComponentSettings>(
                typeof(TComponentSettings).FullName
            );
        }

#endif
	}
}
