//-----------------------------------------------------------------------
// <copyright file="SerializableDictionarySurrogate2.cs" company="Sphere 10 Software">
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
using System.Xml.Serialization;
using System.Diagnostics;
using System.Reflection;

namespace Sphere10.Framework {
    
    [Obfuscation(Exclude = true)]
    [XmlRoot("DicionarySurrogate")]
    public class SerializableDictionarySurrogate2<TKey, TValue> {
 
        public SerializableDictionarySurrogate2() {
			Items = new SerializableKeyValuePair<TKey, TValue>[0];
        }

		public SerializableDictionarySurrogate2(IDictionary<TKey, TValue> parent)
            : this() {
            FromDictionary(parent);
        }

		[XmlElement]
		public SerializableKeyValuePair<TKey, TValue>[] Items;


        public void FromDictionary(IDictionary<TKey, TValue> parent) {
			if (parent != null) {
				Items = parent.Select(kv => new SerializableKeyValuePair<TKey, TValue>(kv)).ToArray();
			} else {
				Items = new SerializableKeyValuePair<TKey, TValue>[0];
			}
        }

        public Dictionary<TKey, TValue> ToDictionary() {
			return ToDictionary(new Dictionary<TKey, TValue>()) as Dictionary<TKey, TValue>;
        }

        public IDictionary<TKey, TValue> ToDictionary(IDictionary<TKey, TValue> dictionaryToPopulate) {
			if (Items != null) {
				foreach (var skv in Items) {
					dictionaryToPopulate.Add(new KeyValuePair<TKey, TValue>(skv.Key, skv.Value));
				}
			}
			return dictionaryToPopulate;
        }
    }

}
