//-----------------------------------------------------------------------
// <copyright file="SyncronizedCollectionEx.cs" company="Sphere 10 Software">
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

	/// <summary>
	/// This class uses a read writer lock to provide data synchronization, but the design of the IList interface itself can lead to
	/// race conditions. Beware of it's use. 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class SyncronizedCollectionEx<T> : ICollection<T>, IThreadSafeObject {
		private readonly ReaderWriterLockSlim _threadLock;
		private readonly ICollection<T> _internalCollection;
	
		public SyncronizedCollectionEx() : this(new List<T>()) {
		}

		public SyncronizedCollectionEx(ICollection<T> internalList) {
			_internalCollection = internalList;
			_threadLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
		}

		#region IThreadSafeObject Implementation

		public ReaderWriterLockSlim ThreadLock {
			get {
				return _threadLock;
			}
		}

		public IDisposable EnterReadScope() {
			return new ThreadSafeScope(this, false);
		}

		public IDisposable EnterWriteScope() {
			return new ThreadSafeScope(this, true);
		}

		#endregion

		#region ICollection Implementation

		public void Add(T item) { using (EnterWriteScope()) _internalCollection.Add(item); }
		public void Clear() { using (EnterWriteScope()) _internalCollection.Clear(); }
		public bool Contains(T item) { using (EnterReadScope()) return _internalCollection.Contains(item); }
		public int Count { get { using (EnterReadScope()) return _internalCollection.Count; } }
		public bool IsReadOnly { get { using (EnterReadScope()) return _internalCollection.IsReadOnly; } }
		public void CopyTo(T[] array, int arrayIndex) { using (EnterReadScope())  _internalCollection.CopyTo(array, arrayIndex); }
		public bool Remove(T item) { using (EnterWriteScope()) return _internalCollection.Remove(item); }
		public IEnumerator<T> GetEnumerator() { using (EnterReadScope()) return _internalCollection.GetEnumerator(); }
		IEnumerator IEnumerable.GetEnumerator() { using (EnterReadScope()) return (_internalCollection as IEnumerable).GetEnumerator(); }

		#endregion
	}

}
