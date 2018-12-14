//-----------------------------------------------------------------------
// <copyright file="Parallel.cs" company="Sphere 10 Software">
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

#if __WP8__
using System;
using System.Collections;
using System.Collections.Generic;
using Sphere10.Framework;

public static class Parallel {
    public static void ForEach<T>(IEnumerable<T> items, Action<T> action) {
        items.ForEach(action);
    }
}

#endif
