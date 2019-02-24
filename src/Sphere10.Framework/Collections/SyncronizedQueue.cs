//-----------------------------------------------------------------------
// <copyright file="SyncronizedQueue.cs" company="Sphere 10 Software">
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

    public class SyncronizedQueue<T> : ICollection<T>, IThreadSafeObject {
		private readonly ReaderWriterLockSlim _threadLock;
		private readonly Queue<T> _internalQueue;

		public SyncronizedQueue()
            : this(new Queue<T>()) {
		}
        public SyncronizedQueue(Queue<T> internalQueue) {
            _internalQueue = internalQueue;
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
        public void Add(T item) { using (EnterWriteScope()) _internalQueue.Enqueue(item); }
        public void Clear() { using(EnterWriteScope()) _internalQueue.Clear(); }
        public bool Contains(T item) { using (EnterReadScope()) return _internalQueue.Contains(item); }
        public int Count { get { using (EnterReadScope()) return _internalQueue.Count; } }
        public bool IsReadOnly { get { return false; } }
        public void CopyTo(T[] array, int arrayIndex) { using (EnterReadScope())  _internalQueue.CopyTo(array, arrayIndex); }
        public bool Remove(T item) { throw new NotSupportedException();}
        public IEnumerator<T> GetEnumerator() { using (EnterReadScope()) return _internalQueue.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { using (EnterReadScope()) return (_internalQueue as IEnumerable).GetEnumerator(); }

        #endregion

		#region Methods
        public T Peek() {
            using (EnterReadScope())
                return _internalQueue.Peek();
        }
	    public void Enqueue(T value) {
            this.Add(value);
	    }

        public T Dequeue() {
            using (EnterWriteScope())
                return _internalQueue.Dequeue();
        }

		#endregion
	}

}

