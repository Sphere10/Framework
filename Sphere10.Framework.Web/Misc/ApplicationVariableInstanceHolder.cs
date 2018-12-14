//-----------------------------------------------------------------------
// <copyright file="ApplicationVariableInstanceHolder.cs" company="Sphere 10 Software">
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

namespace Sphere10.Framework.Web {
    public class ApplicationVariableInstanceHolder<T> : IInstanceHolder<T> where T : class {
        private readonly string _key;

        public ApplicationVariableInstanceHolder(string key = null) {
            _key = key ?? "ApplicationVariableInstanceHolder_" + Guid.NewGuid().ToStrictAlphaString();
        }
        public void Put(T t) {
            System.Web.HttpContext.Current.Application[_key] = t;
        }

        public T Get() {
            return System.Web.HttpContext.Current.Application[_key] as T;
        }

        public bool HasInstance {
            get {
                return System.Web.HttpContext.Current.Application[_key] != null;
            }
        }
    }
}
