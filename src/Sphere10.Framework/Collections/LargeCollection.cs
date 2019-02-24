//-----------------------------------------------------------------------
// <copyright file="LargeCollection.cs" company="Sphere 10 Software">
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
using System.Collections;
using System.Collections.Generic;

namespace Sphere10.Framework {
    public class LargeCollection<T> : PageManager<T>, ICollection<T>, IEnumerable<T> {

        public LargeCollection(int pageSize, Func<T, int> sizeEstimator)
            : base(pageSize, sizeEstimator) {
            IsReadOnly = false;
        }

        public IEnumerator<T> GetEnumerator() {
            return new LargeCollectionEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }


        public bool Contains(T item) {
            throw new NotSupportedException();
        }

        public void CopyTo(T[] array, int arrayIndex) {
            throw new NotSupportedException();
        }

        public bool Remove(T item) {
            throw new NotSupportedException();
        }


        public bool IsReadOnly { get; private set; }

        public T this[int index] {
            get { return Get(index); }
        }

        private class LargeCollectionEnumerator : IEnumerator<T> {
            private readonly LargeCollection<T> _largeCollection;
            private int _index = -1;
            private readonly int _count;
            private bool _disposed = false;

            public LargeCollectionEnumerator(LargeCollection<T> largeCollection) {
                _largeCollection = largeCollection;
                _count = largeCollection.Count;
            }

            public void Dispose() {
                _disposed = true;
            }

            public bool MoveNext() {
                if (_disposed) {
                    throw new SoftwareException("LargeCollectionEnumerator already disposed");
                }

                if (_largeCollection.Count != _count) {
                    throw new SoftwareException("LargeCollection changed during iteration");
                }

                if (_index + 1 < _largeCollection.Count) {
                    Current = _largeCollection[++_index];
                    return true;
                }
                return false;
            }

            public void Reset() {
                if (_disposed) {
                    throw new SoftwareException("Enumerator already disposed");
                }
                _index = 0;
            }

            public T Current { get; private set; }

            object IEnumerator.Current {
                get { return Current; }
            }
        }

    }
}
