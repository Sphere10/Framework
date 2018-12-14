//-----------------------------------------------------------------------
// <copyright file="ICacheExtensions.cs" company="Sphere 10 Software">
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

namespace Sphere10.Framework {
    public static class ICacheExtensions {

        public static TVal Get<TKey, TVal>(this ICache<TKey, TVal> cache, TKey key) {
            return cache[key];
        }

        public static IEnumerable<V> GetAllCachedValues<K, V>(this ICache<K, V> cache) {
            if (cache is BulkFetchCacheBase<K, V>) {
                ((BulkFetchCacheBase<K,V>) cache).ForceRefresh();
            }
            return cache.GetCachedItems().Values.Select(c => c.Value);
        }

        public static void Set<K, V>(this ICache<K, V> cache, K key, V value) {
            cache.BulkLoad(new[] {new KeyValuePair<K, V>(key, value)});
        }
    }
}
