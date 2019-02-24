//-----------------------------------------------------------------------
// <copyright file="ObservableList.cs" company="Sphere 10 Software">
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
using Sphere10.Framework.Collections;

namespace Sphere10.Framework {
    
    public class ObservableList<T> : ListDecorator<T>, IObservableCollection<ObservableList<T>, T, int> {

        public ObservableList() : this(new List<T>()) {
        }

        public ObservableList(IEnumerable<T> values) : this(new List<T>(values)) {
        }

        public ObservableList(IList<T> internalList) : base(internalList) {
            ObservationWindow = new ObservationWindow<ObservableList<T>, T, int>(this);
        }

        public ObservationWindow<ObservableList<T>, T, int> ObservationWindow { get; private set; }

        public virtual List<T> GetRange(int index, int count) {
            if (index < 0 || index >= InternalList.Count)
                throw new ArgumentOutOfRangeException("index");

            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            if (index + count > InternalList.Count)
                throw new ArgumentOutOfRangeException("count");

            var dotNetList = InternalList as List<T>;
            if (dotNetList != null) {
                return dotNetList.GetRange(index, count);
            }

            var range = new List<T>(); 
            for (var i = 0; i < count; i++) {
                range[i] = base[i];
            }

            return range;
        } 

        public override void Add(T item) {
            base.Add(item);
            ObservationWindow.NotifyItemAdded(item, this.Count - 1);
        }


        public virtual void AddRange(IEnumerable<T> collection) {
            var collectionArray = collection.ToArray();
            var dotNetList = InternalList as List<T>;
            if (dotNetList != null) {
                dotNetList.AddRange(collectionArray);
            } else {
                foreach (var item in collectionArray)
                    InternalList.Add(item);
            }
            ObservationWindow.NotifyItemsAdded(collectionArray, Enumerable.Range(InternalList.Count - collectionArray.Length, collectionArray.Length));
        }

        

        public override void Clear() {
            var items = this.ToArray();
            base.Clear();
            ObservationWindow.NotifyItemsRemoved(items, Enumerable.Range(0, items.Length));
        }

        public override void Insert(int index, T item) {
            base.Insert(index, item);
            ObservationWindow.NotifyItemAdded(item, index);
        }

        public virtual void InsertRange(int index, IEnumerable<T> items) {
            if (index < 0 || index >= InternalList.Count)
                throw new ArgumentOutOfRangeException("index");

            var itemsArray = items.ToArray();
            var dotNetList = base.InternalList as List<T>;
            if (dotNetList != null) {
                dotNetList.InsertRange(index, itemsArray);
            } else {
                foreach (var item in itemsArray.Reverse())
                    base.Insert(index, item);
            }
            ObservationWindow.NotifyItemsAdded(itemsArray, Enumerable.Range(index, itemsArray.Length));
        }

        public override bool Remove(T item) {
            var index = base.IndexOf(item);
            if (base.Remove(item)) {
                ObservationWindow.NotifyItemRemoved(item, index);
                return true;
            }
            return false;
        }

        public override void RemoveAt(int index) {
            var item = base[index];
            base.RemoveAt(index);
            ObservationWindow.NotifyItemRemoved(item, index);
        }

        public void RemoveRange(int index, int count) {
            if (index < 0 || index >= InternalList.Count)
                throw new ArgumentOutOfRangeException("index");
            
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            if (index + count > InternalList.Count)
                throw new ArgumentOutOfRangeException("count");

            var itemsList = GetRange(index, count);
            var dotNetList = InternalList as List<T>;
            if (dotNetList != null) {
                dotNetList.RemoveRange(index, count);
            } else {
                for (int i = 0; i < count; i++)
                    InternalList.RemoveAt(index);
            }
            ObservationWindow.NotifyItemsRemoved(itemsList, Enumerable.Range(index, count));
        }

        public override T this[int index] {
            get { return base[index]; }
            set {
                var hadItem = 0 <= index && index < base.Count;
                T item = hadItem ? base[index] : default(T);
                base[index] = value;
                if (hadItem)
                    ObservationWindow.NotifyItemRemoved(item, index);
                ObservationWindow.NotifyItemAdded(item, index);
            }
        }

        
    }
}
