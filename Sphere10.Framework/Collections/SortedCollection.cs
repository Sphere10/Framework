//-----------------------------------------------------------------------
// <copyright file="SortedCollection.cs" company="Sphere 10 Software">
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
using System.Linq;
using System.Text;

namespace Sphere10.Framework {

	public class SortedCollection<T> : ICollection<T> where T : IComparable<T> {
		private readonly IList<T> _internalList;

		public SortedCollection() : this(new List<T>()) {
		}

		public SortedCollection(IList<T> internalList ) {
			_internalList = internalList;
		}

		public void Add(T item) {
			var index = _internalList.BinarySearch(item);
			var insertionPoint = index >= 0 ? index : ~index;
			_internalList.Insert(insertionPoint, item);
		}

		public void AddRange(IEnumerable<T> items) {
			items.ForEach(Add);
		}

		public void Clear() {
			_internalList.Clear();
		}

		public bool Contains(T item) {
			return _internalList.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex) {
			_internalList.CopyTo(array, arrayIndex);
		}

		public int Count {
			get { return _internalList.Count; }
		}

		public bool IsReadOnly {
			get { return ((ICollection<T>) _internalList).IsReadOnly; }
		}

		public bool Remove(T item) {
			return _internalList.Remove(item);
		}

		public IEnumerator<T> GetEnumerator() {
			return _internalList.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return ((IEnumerable)_internalList).GetEnumerator();
		}
	}
}
