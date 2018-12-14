//-----------------------------------------------------------------------
// <copyright file="ScopeTool.cs" company="Sphere 10 Software">
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
using Sphere10.Framework;

namespace Tools {
    public static class Scope {

        public static IDisposable ExecuteOnDispose(Action action) {
            return new ExecuteOnDisposeScope(action);
        }

        public static IDisposable ExecuteOnDispose<T>(Action<T> action, T usingThesePrefetchedValues) {
            return new ExecuteOnDisposeScope(() => action(usingThesePrefetchedValues));
        }

    }

}
