//-----------------------------------------------------------------------
// <copyright file="ScopeContext.cs" company="Sphere 10 Software">
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

#if !__WP8__
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Sphere10.Framework {

    public abstract class ScopeContext<T> : Disposable where T : ScopeContext<T> {
        private readonly string _contextName;
        private readonly ScopeContextPolicy _policy;

        protected ScopeContext(string contextName, ScopeContextPolicy policy) {
            _policy = policy;
            
            _contextName = contextName;
            var contextObject = CallContext.LogicalGetData(contextName) as T;
            if (contextObject != null) {
                // Nested
                if (_policy == ScopeContextPolicy.MustBeRoot)
                    throw new SoftwareException("A {0} was already declared within the calling context", typeof (T).Name);

                IsRootScope = false;
                RootScope = contextObject;
            } else {
                // Root
                if (_policy == ScopeContextPolicy.MustBeNested)
                    throw new SoftwareException("No {0} was declared in the calling context", typeof (T).Name);
                IsRootScope = true;
                RootScope = (T) this;
                CallContext.LogicalSetData(contextName, this);
            }
        }

        public bool IsRootScope { get; private set; }

        public T RootScope { get; protected set; }

        protected abstract void OnScopeEnd(T rootScope, bool inException);

        protected static T GetCurrent(string contextName) {
            return CallContext.LogicalGetData(contextName) as T;
        }

        protected override sealed void FreeManagedResources() {

#if !__MOBILE__ && !NETSTANDARD
            var inException = Marshal.GetExceptionPointers() != IntPtr.Zero || Marshal.GetExceptionCode() != 0;
#else
            var inException = false;
#endif
            // Remove from registry
            if (IsRootScope) {
                CallContext.LogicalSetData(_contextName, null);
            }

            // Notify end
            OnScopeEnd(RootScope, inException);

        }
    }


    public enum ScopeContextPolicy {
        None,
        MustBeRoot,
        MustBeNested
    }

}

#endif
