//-----------------------------------------------------------------------
// <copyright file="ObservationWindow.cs" company="Sphere 10 Software">
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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Sphere10.Framework {
    public sealed class ObservationWindow<TCollection, TItem, TLocation> where TCollection : IObservableCollection<TCollection, TItem, TLocation> {
        public event EventHandlerEx<TCollection, TItem, TLocation> ItemAdded;
        public event EventHandlerEx<TCollection, TItem, TLocation> ItemRemoved;
        public event EventHandlerEx<TCollection, TItem, TLocation> ItemChanged;
        public event EventHandlerEx<TCollection, IEnumerable<ObservableCollectionItem<TItem, TLocation>>> Changed;

        private readonly TCollection _collection;


        public void RemoveAllListeners() {
            ItemAdded = null;
            ItemRemoved = null;
            ItemChanged = null;
            Changed = null;
        }

        public ObservationWindow(TCollection collection) {
            this._collection = collection;
        }

        internal void NotifyItemAdded(TItem item, TLocation location) {
            NotifySingleEvent(ObservableCollectionItemEventType.Added, item, location);
        }


        internal void NotifyItemRemoved(TItem item, TLocation location) {
            NotifySingleEvent(ObservableCollectionItemEventType.Removed, item, location);
        }

        internal void NotifyItemChanged(TItem item, TLocation location) {
            NotifySingleEvent(ObservableCollectionItemEventType.Changed, item, location);
        }

        internal void NotifyItemsAdded(IEnumerable<TItem> items, IEnumerable<TLocation> locations) {
            NotifyManyEvents(ObservableCollectionItemEventType.Added, items, locations);
        }

        internal void NotifyItemsRemoved(IEnumerable<TItem> items, IEnumerable<TLocation> locations) {
            NotifyManyEvents(ObservableCollectionItemEventType.Removed, items, locations);
        }

        internal void NotifyItemsChanged(IEnumerable<TItem> items, IEnumerable<TLocation> locations) {
            NotifyManyEvents(ObservableCollectionItemEventType.Changed, items, locations);
        }

        internal void NotifySingleEvent(ObservableCollectionItemEventType eventType, TItem item, TLocation location) {
            Notify(new[] { new ObservableCollectionItem<TItem, TLocation>(eventType, item, location) });
        }

        internal void NotifyManyEvents(ObservableCollectionItemEventType eventType, IEnumerable<TItem> items, IEnumerable<TLocation> locations) {
            Notify(items.ZipWith(locations, (item, location) => new ObservableCollectionItem<TItem, TLocation>(eventType, item, location)));
        }

        internal void Notify(IEnumerable<ObservableCollectionItem<TItem, TLocation>> events) {
            var eventsArray = events as ObservableCollectionItem<TItem, TLocation>[] ?? events.ToArray();
            NotifyChangedInternal(eventsArray);
            foreach (var @event in eventsArray) {
                switch (@event.EventType) {
                    case ObservableCollectionItemEventType.Added:
                        NotifyItemAddedInternal(@event.Item, @event.Location);
                        break;
                    case ObservableCollectionItemEventType.Removed:
                        NotifyItemRemovedInternal(@event.Item, @event.Location);
                        break;
                    case ObservableCollectionItemEventType.Changed:
                        NotifyItemChangedInternal(@event.Item, @event.Location);
                        break;
                }
            }
        }



        private void NotifyItemAddedInternal(TItem item, TLocation location) {
            if (ItemAdded != null)
                ItemAdded(_collection, item, location);
            
        }

        private void NotifyItemRemovedInternal(TItem item, TLocation location) {
            if (ItemRemoved != null)
                ItemRemoved(_collection, item, location);


        }


        private void NotifyItemChangedInternal(TItem item, TLocation location) {
            if (ItemChanged != null)
                ItemChanged(_collection, item, location);


        }


        private void NotifyChangedInternal(IEnumerable<ObservableCollectionItem<TItem, TLocation>> changeEvents ) {
            if (Changed != null)
                Changed(_collection, changeEvents);


        }


    }
}

