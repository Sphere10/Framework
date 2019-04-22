//-----------------------------------------------------------------------
// <copyright file="SynchronizedList.cs" company="Sphere 10 Software">
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
	public class SynchronizedList<T> : IList<T>, IThreadSafeObject {
		private readonly ReaderWriterLockSlim _threadLock;
		private readonly IList<T> _internalList;

		public SynchronizedList()
			: this(new List<T>()) {
		}

	    public SynchronizedList(IList<T> internalList) {
	        if (internalList is T[])
	            throw new ArgumentException("provided internalList was an array", "internalList");
	        _internalList = internalList;
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

		#region IList Implementation

		public T this[int index] {
			get { using (EnterReadScope()) return _internalList[index]; }
			set { using (EnterWriteScope()) _internalList[index] = value; }
		}

		public int IndexOf(T item) { using (EnterReadScope()) return _internalList.IndexOf(item); }

        public int IndexOf(T item, IEqualityComparer<T> comparer ) { using (EnterReadScope()) return _internalList.IndexOf(item, comparer); }
        public void Insert(int index, T item) { using (EnterWriteScope()) _internalList.Insert(index, item); }
		public void RemoveAt(int index) { using (EnterWriteScope()) _internalList.RemoveAt(index); }
		public void Add(T item) { using (EnterWriteScope()) _internalList.Add(item); }
		public void Clear() { using (EnterWriteScope()) _internalList.Clear(); }
		public bool Contains(T item) { using (EnterReadScope()) return _internalList.Contains(item); }
		public int Count { get { using (EnterReadScope()) return _internalList.Count; } }
		public bool IsReadOnly { get { using (EnterReadScope()) return _internalList.IsReadOnly; } }
		public void CopyTo(T[] array, int arrayIndex) { using (EnterReadScope())  _internalList.CopyTo(array, arrayIndex); }
		public bool Remove(T item) { using (EnterWriteScope()) return _internalList.Remove(item); }
		public IEnumerator<T> GetEnumerator() { using (EnterReadScope()) return _internalList.GetEnumerator(); }
		IEnumerator IEnumerable.GetEnumerator() { using (EnterReadScope()) return (_internalList as IEnumerable).GetEnumerator(); }

		#endregion

		#region Methods

	    public void AddRange(IEnumerable<T> values) {
	        using (EnterWriteScope()) {
	            _internalList.AddRange(values);
	        }
	    }

	    public void RemoveRange(int index, int count) {
	        using (EnterWriteScope()) {
                _internalList.RemoveRange(index, count);
	        }
	    }

	    #endregion
	}

}

