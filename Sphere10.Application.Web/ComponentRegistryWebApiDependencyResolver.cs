//-----------------------------------------------------------------------
// <copyright file="ComponentRegistryWebApiDependencyResolver.cs" company="Sphere 10 Software">
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
using System.Web.Http.Dependencies;
using Sphere10.Framework;

namespace Sphere10.Application.Web {

	public class ComponentRegistryWebApiDependencyResolver : IDependencyResolver {
		private bool _disposed;
		private readonly ComponentRegistry _container;

		public ComponentRegistryWebApiDependencyResolver(ComponentRegistry container) {
			if (container == null)
				throw new ArgumentNullException(nameof(container));

			_container = container;
		}

		public IDependencyScope BeginScope() {
			if (_disposed)
				throw new ObjectDisposedException("this", "This scope has already been disposed.");

			return new ComponentRegistryWebApiDependencyResolver(_container.GetChildRegistry());
		}

		public object GetService(Type serviceType) {
			if (_disposed)
				throw new ObjectDisposedException("this", "This scope has already been disposed.");

			try {
				return _container.Resolve(serviceType);
			} catch (TinyIoCResolutionException) {
				return null;
			}
		}

		public IEnumerable<object> GetServices(Type serviceType) {
			if (_disposed)
				throw new ObjectDisposedException("this", "This scope has already been disposed.");

			try {
				return _container.ResolveAll(serviceType);
			} catch (TinyIoCResolutionException) {
				return Enumerable.Empty<object>();
			}
		}

		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing) {
			if (_disposed)
				return;

			if (disposing)
				_container.Dispose();

			_disposed = true;
		}
	}
}
