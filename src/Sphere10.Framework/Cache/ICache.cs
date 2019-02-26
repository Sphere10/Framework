//-----------------------------------------------------------------------
// <copyright file="ICache.cs" company="Sphere 10 Software">
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
using System.Threading.Tasks;

namespace Sphere10.Framework {

	public interface ICache<TKey, TValue> : IThreadSafeObject {

        event EventHandlerEx<TKey, TValue> ItemFetched;
	    event EventHandlerEx<TKey, CachedItem<TValue>>  ItemRemoved;

		int ItemCount { get; }

		uint CurrentSize { get; }

        uint MaxCapacity { get; }

		TimeSpan ExpirationDuration { get; }

        ExpirationPolicy ExpirationPolicy { get; }

        CacheReapPolicy ReapPolicy { get; }

        NullValuePolicy NullValuePolicy { get; }

		bool ContainsCachedItem(TKey key);

		TValue this[TKey index] { get; }

		Task<TValue> GetAsync(TKey index);

        void BulkLoad(IEnumerable<KeyValuePair<TKey, TValue>> bulkLoadedValues);

	    IDictionary<TKey, CachedItem<TValue>> GetCachedItems();

	    void Invalidate(TKey key);
	    void Remove(TKey key);
        void Flush();

	}
}
