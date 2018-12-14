//-----------------------------------------------------------------------
// <copyright file="CollectionGroup.cs" company="Sphere 10 Software">
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

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sphere10.Framework {
    public class CollectionGroup<TKey, TElement, TCollection> : IGrouping<TKey, TElement> where TCollection : ICollection<TElement>, new(){
        private readonly TKey _key;
        private readonly TCollection _elements;

        public CollectionGroup(TKey key, IEnumerable<TElement> elements) {
            _key = key;
            _elements = new TCollection();
            elements.ForEach(_elements.Add); 
        }

        public TKey Key { get { return _key; }  }

        public TCollection Elements { get { return _elements; } }

        public IEnumerator<TElement> GetEnumerator() {
            return _elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }

    public class CollectionGroup<TKey, TElement> : CollectionGroup<TKey, TElement, List<TElement>> {
        public CollectionGroup(TKey key, IEnumerable<TElement> elements)
            : base(key, elements) {
        }
    }
}
