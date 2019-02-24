//-----------------------------------------------------------------------
// <copyright file="SerializableKeyValuePair.cs" company="Sphere 10 Software">
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
using System.Runtime.InteropServices;
using System.Text;
namespace Sphere10.Framework {

	[Serializable, StructLayout(LayoutKind.Sequential)]
	public struct SerializableKeyValuePair<TKey, TValue> {
		private TKey _key;
		private TValue _value;

		public SerializableKeyValuePair(KeyValuePair<TKey, TValue> keyValuePair)
			: this(keyValuePair.Key, keyValuePair.Value) {
		}

		public SerializableKeyValuePair(TKey key, TValue value) {
			_key = key;
			_value = value;
		}
		public override string ToString() {
			StringBuilder builder1 = new StringBuilder();
			builder1.Append('[');
			if (Key != null) {
				builder1.Append(Key);
			}
			builder1.Append(", ");
			if (Value != null) {
				builder1.Append(Value);
			}
			builder1.Append(']');
			return builder1.ToString();
		}
		/// <summary>
		/// Gets the Value in the Key/Value Pair
		/// </summary>
		public TValue Value {
			get {
				return _value;
			}
			set {
				_value = value;
			}
		}
		/// <summary>
		/// Gets the Key in the Key/Value pair
		/// </summary>
		public TKey Key {
			get {
				return this._key;
			}
			set {
				this._key = value;
			}
		}
	}
}
