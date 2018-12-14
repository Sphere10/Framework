//-----------------------------------------------------------------------
// <copyright file="SynchronizedDictionary.cs" company="Sphere 10 Software">
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
using System.Collections;
using System.Threading;

namespace Sphere10.Framework {


	public class SyncronizedDictionary<TDictionary, TKey, TValue> : IDictionary<TKey, TValue>, IThreadSafeObject where TDictionary : IDictionary<TKey, TValue> {
		private readonly TDictionary _internalDictionary;

		public SyncronizedDictionary(TDictionary internalDictionary) {
			_internalDictionary = internalDictionary;
			ThreadLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
		}

		public TDictionary InternalDictionary {
			get {
				if (!ThreadLock.IsWriteLockHeld) 
					throw new SoftwareException("Can only access internal dictionary within a write scope");				
				return _internalDictionary;
			}
		}

		#region IThreadSafeObject Implementation

		public ReaderWriterLockSlim ThreadLock { get; }

		public IDisposable EnterReadScope() {
			return new ThreadSafeScope(this, false);
		}

		public IDisposable EnterWriteScope() {
			return new ThreadSafeScope(this, true);
		}

		#endregion

		#region IDictionary Implementation

		public void Add(TKey key, TValue value) {
			using (EnterWriteScope()) _internalDictionary.Add(key, value);
		}

		public bool ContainsKey(TKey key) {
			using (EnterReadScope()) return _internalDictionary.ContainsKey(key);
		}

		public ICollection<TKey> Keys {
			get {
				using (EnterReadScope()) return _internalDictionary.Keys;
			}
		}

		public bool TryGetValue(TKey key, out TValue value) {
			using (EnterReadScope()) return _internalDictionary.TryGetValue(key, out value);
		}

		public ICollection<TValue> Values {
			get {
				using (EnterReadScope()) return _internalDictionary.Values;
			}
		}

		public TValue this[TKey key] {
			get {
				using (EnterReadScope()) return _internalDictionary[key];
			}
			set {
				using (EnterWriteScope()) _internalDictionary[key] = value;
			}
		}

		public void Add(KeyValuePair<TKey, TValue> item) {
			using (EnterWriteScope()) _internalDictionary.Add(item);
		}

		public bool Contains(KeyValuePair<TKey, TValue> item) {
			using (EnterReadScope()) return _internalDictionary.Contains(item);
		}

		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
			using (EnterReadScope()) _internalDictionary.CopyTo(array, arrayIndex);
		}

		public bool Remove(KeyValuePair<TKey, TValue> item) {
			using (EnterWriteScope()) return _internalDictionary.Remove(item);
		}

		public bool Remove(TKey item) {
			using (EnterWriteScope()) return _internalDictionary.Remove(item);
		}

		public void Clear() {
			using (EnterWriteScope()) _internalDictionary.Clear();
		}

		public int Count {
			get {
				using (EnterReadScope()) return _internalDictionary.Count;
			}
		}

		public bool IsReadOnly {
			get {
				using (EnterReadScope()) return _internalDictionary.IsReadOnly;
			}
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
			using (EnterReadScope()) return _internalDictionary.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			using (EnterReadScope()) return (_internalDictionary as IEnumerable).GetEnumerator();
		}

		#endregion

	}


	public class SyncronizedDictionary<TKey, TValue> : SyncronizedDictionary<IDictionary<TKey,TValue>, TKey,TValue>  {
		public SyncronizedDictionary()
		: this(new Dictionary<TKey, TValue>()) {
		}

		public SyncronizedDictionary(IEqualityComparer<TKey> comparer)
			: this(new Dictionary<TKey, TValue>(comparer)) {
		}

		public SyncronizedDictionary(IDictionary<TKey, TValue> internalDictionary) : base(internalDictionary) {
		}
	}
}

