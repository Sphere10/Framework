//-----------------------------------------------------------------------
// <copyright file="ObservableDictionary.cs" company="Sphere 10 Software">
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

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Sphere10.Framework {
    public class ObservableDictionary<TKey, TValue> : DictionaryDecorator<TKey, TValue>, IObservableCollection<ObservableDictionary<TKey, TValue>, TValue, TKey> {


        public ObservableDictionary() : this(new Dictionary<TKey, TValue>()) {
        }

        public ObservableDictionary(IDictionary<TKey, TValue> internalDictionary) : base(internalDictionary) {
            ObservationWindow = new ObservationWindow<ObservableDictionary<TKey, TValue>, TValue, TKey>(this);
        }

        public ObservationWindow<ObservableDictionary<TKey, TValue>, TValue, TKey> ObservationWindow { get; private set; }

        public override void Add(TKey key, TValue value) {

            base.Add(key, value);
            ObservationWindow.NotifyItemAdded(value, key);
        }

        public override void Clear() {
            var items = this.ToArray();
            base.Clear();
            ObservationWindow.NotifyItemsRemoved(items.Select(i => i.Value), items.Select(i => i.Key));
        }

        public override void Add(KeyValuePair<TKey, TValue> item) {
            base.Add(item);
            ObservationWindow.NotifyItemAdded(item.Value, item.Key);
        }

        public override bool Remove(TKey item) {
            var location = item;
            TValue val;
            base.TryGetValue(item, out val);
            if (base.Remove(item)) {
                ObservationWindow.NotifyItemRemoved(val, location);
                return true;
            }
            return false;
        }

        public override bool Remove(KeyValuePair<TKey, TValue> item) {
            if (base.Remove(item)) {
                ObservationWindow.NotifyItemRemoved(item.Value, item.Key);
                return true;
            }
            return false;
        }

        public override TValue this[TKey key] {
            get { return base[key]; }
            set {
                TValue oldValue;
                var hadValue = base.TryGetValue(key, out oldValue);
                base[key] = value;
                if (hadValue)
                    ObservationWindow.NotifyItemRemoved(oldValue, key);
                ObservationWindow.NotifyItemAdded(value, key);
            }
        }

    
    }
}
