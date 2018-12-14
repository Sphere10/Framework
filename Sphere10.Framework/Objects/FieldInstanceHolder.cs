//-----------------------------------------------------------------------
// <copyright file="FieldInstanceHolder.cs" company="Sphere 10 Software">
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

namespace Sphere10.Framework {
    public class FieldInstanceHolder<T> : IInstanceHolder<T>  where T : class {
        private T _t;

        public FieldInstanceHolder() {
            _t = default(T);
        }
        public void Put(T t) {
            _t = t;
        }

        public T Get() {
            return _t;
        }

        public bool HasInstance => _t != default(T);
    }
}
