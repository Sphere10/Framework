//-----------------------------------------------------------------------
// <copyright file="CircularBuffer.cs" company="Sphere 10 Software">
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
using System.Threading;

namespace Sphere10.Framework {

	public class CircularBuffer<T> : ICollection<T>, IEnumerable<T>, ICollection, IEnumerable {
		private int _capacity;
	    private int _head;
		private int _tail;
		private T[] _buffer;

		[NonSerialized()]
		private object syncRoot;

		public CircularBuffer(int capacity)
			: this(capacity, false) {
		}

		public CircularBuffer(int capacity, bool allowOverflow) {
			if (capacity < 0)
				throw new ArgumentException("The buffer capacity must be greater than or equal to zero.", "capacity");

			this._capacity = capacity;
			Size = 0;
			_head = 0;
			_tail = 0;
			_buffer = new T[capacity];
			AllowOverflow = allowOverflow;
		}

		public bool AllowOverflow {
			get;
			set;
		}

		public int Capacity {
			get { return _capacity; }
			set {
				if (value == _capacity)
					return;

				if (value < Size)
					throw new ArgumentOutOfRangeException("value", "The new capacity must be greater than or equal to the buffer size.");

				var dst = new T[value];
				if (Size > 0)
					CopyTo(dst);
				_buffer = dst;

				_capacity = value;
			}
		}

	    public int Size { get; private set; }

	    public bool Contains(T item) {
			int bufferIndex = _head;
			var comparer = EqualityComparer<T>.Default;
			for (int i = 0; i < Size; i++, bufferIndex++) {
				if (bufferIndex == _capacity)
					bufferIndex = 0;

				if (item == null && _buffer[bufferIndex] == null)
					return true;
				else if ((_buffer[bufferIndex] != null) &&
					comparer.Equals(_buffer[bufferIndex], item))
					return true;
			}

			return false;
		}

		public void Clear() {
			Size = 0;
			_head = 0;
			_tail = 0;
		}

		public int Put(T[] src) {
			return Put(src, 0, src.Length);
		}

		public int Put(T[] src, int offset, int count) {
			if (!AllowOverflow && count > _capacity - Size)
				throw new InvalidOperationException("The buffer does not have sufficient capacity to put new items.");

			int srcIndex = offset;
			for (int i = 0; i < count; i++, _tail++, srcIndex++) {
				if (_tail == _capacity)
					_tail = 0;
				_buffer[_tail] = src[srcIndex];
			}
			Size = Math.Min(Size + count, _capacity);
			return count;
		}

		public void Put(T item) {
			if (!AllowOverflow && Size == _capacity)
				throw new InvalidOperationException("The buffer does not have sufficient capacity to put new items.");

			_buffer[_tail] = item;
			if (++_tail == _capacity)
				_tail = 0;
			Size++;
		}

		public void Skip(int count) {
			_head += count;
			if (_head >= _capacity)
				_head -= _capacity;
		}

		public T[] Get(int count) {
			var dst = new T[count];
			Get(dst);
			return dst;
		}

		public int Get(T[] dst) {
			return Get(dst, 0, dst.Length);
		}

		public int Get(T[] dst, int offset, int count) {
			int realCount = Math.Min(count, Size);
			int dstIndex = offset;
			for (int i = 0; i < realCount; i++, _head++, dstIndex++) {
				if (_head == _capacity)
					_head = 0;
				dst[dstIndex] = _buffer[_head];
			}
			Size -= realCount;
			return realCount;
		}

		public T Get() {
			if (Size == 0)
				throw new InvalidOperationException("The buffer is empty.");

			var item = _buffer[_head];
			if (++_head == _capacity)
				_head = 0;
			Size--;
			return item;
		}

		public void CopyTo(T[] array) {
			CopyTo(array, 0);
		}

		public void CopyTo(T[] array, int arrayIndex) {
			CopyTo(0, array, arrayIndex, Size);
		}

		public void CopyTo(int index, T[] array, int arrayIndex, int count) {
			if (count > Size)
				throw new ArgumentOutOfRangeException("count", "The read count cannot be greater than the buffer size.");

			int bufferIndex = _head;
			for (int i = 0; i < count; i++, bufferIndex++, arrayIndex++) {
				if (bufferIndex == _capacity)
					bufferIndex = 0;
				array[arrayIndex] = _buffer[bufferIndex];
			}
		}

		public IEnumerator<T> GetEnumerator() {
			int bufferIndex = _head;
			for (int i = 0; i < Size; i++, bufferIndex++) {
				if (bufferIndex == _capacity)
					bufferIndex = 0;

				yield return _buffer[bufferIndex];
			}
		}

		public T[] GetBuffer() {
			return _buffer;
		}

		public T[] ToArray() {
			var dst = new T[Size];
			CopyTo(dst);
			return dst;
		}

		#region ICollection<T> Members

		int ICollection<T>.Count {
			get { return Size; }
		}

		bool ICollection<T>.IsReadOnly {
			get { return false; }
		}

		void ICollection<T>.Add(T item) {
			Put(item);
		}

		bool ICollection<T>.Remove(T item) {
			if (Size == 0)
				return false;

			Get();
			return true;
		}

		#endregion

		#region IEnumerable<T> Members

		IEnumerator<T> IEnumerable<T>.GetEnumerator() {
			return GetEnumerator();
		}

		#endregion

		#region ICollection Members

		int ICollection.Count {
			get { return Size; }
		}

		bool ICollection.IsSynchronized {
			get { return false; }
		}

		object ICollection.SyncRoot {
			get {
				if (syncRoot == null)
					Interlocked.CompareExchange(ref syncRoot, new object(), null);
				return syncRoot;
			}
		}

		void ICollection.CopyTo(Array array, int arrayIndex) {
			CopyTo((T[])array, arrayIndex);
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator() {
			return (IEnumerator)GetEnumerator();
		}

		#endregion
	}

}
