//-----------------------------------------------------------------------
// <copyright file="ComponentRegistryMvcDependencyResolver.cs" company="Sphere 10 Software">
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
using System.Threading.Tasks;
using System.Web.Mvc;
using Sphere10.Framework;
using Sphere10.Framework.Application;

namespace Sphere10.Framework.Web {
    public class ComponentRegistryMvcDependencyResolver : IDependencyResolver {
        private readonly ComponentRegistry _componentRegistry;
        public ComponentRegistryMvcDependencyResolver(ComponentRegistry componentRegistry) {
            _componentRegistry = componentRegistry;
        }
        public object GetService(Type serviceType) {
            try {
                return _componentRegistry.Resolve(serviceType);
            } catch (Exception) {
                return null;
            }
        }
        public IEnumerable<object> GetServices(Type serviceType) {
            try {
                return _componentRegistry.ResolveAll(serviceType);
            } catch (Exception) {
                return Enumerable.Empty<object>();
            }
        }
    }
}
