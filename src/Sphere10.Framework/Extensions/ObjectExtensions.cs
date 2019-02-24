//-----------------------------------------------------------------------
// <copyright file="ObjectExtensions.cs" company="Sphere 10 Software">
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
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.IO;
#if !__WP8__
using System.Runtime.Serialization.Formatters.Binary;
#endif
using System.Runtime.Serialization;
using Sphere10.Framework.FastReflection;


namespace Sphere10.Framework {
	public static class ObjectExtensions {

		public static T As<T>(this object obj) {
			return Tools.Object.ChangeType<T>(obj);
		}

#if !__MOBILE__

		public static object GetPropertyValue(this object obj, string propertyName) {
			return obj.GetType().GetProperty(propertyName).FastGetValue(obj);
		}

		public static T GetPropertyValue<T>(this object obj, string propertyName) {
			var val = GetPropertyValue(obj, propertyName);
			return val == null ? default(T) : Tools.Object.ChangeType<T>(val);
		}

		public static void SetPropertyValue(this object obj, string propertyName, object value) {
			obj.GetType().GetProperty(propertyName).FastSetValue(obj, value);
		}


		/// <summary>
		/// Returns a _private_ Property Value from a given Object. Uses Reflection.
		/// Throws a ArgumentOutOfRangeException if the Property is not found.
		/// </summary>
		/// <typeparam name="T">Type of the Property</typeparam>
		/// <param name="obj">Object from where the Property Value is returned</param>
		/// <param name="propName">Propertyname as string.</param>
		/// <returns>PropertyValue</returns>
		public static T GetPrivatePropertyValue<T>(this object obj, string propName) {
			if (obj == null) throw new ArgumentNullException("obj");
			PropertyInfo pi = obj.GetType().GetProperty(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			if (pi == null) throw new ArgumentOutOfRangeException("propName", string.Format("Property {0} was not found in Type {1}", propName, obj.GetType().FullName));
			return Tools.Object.ChangeType<T>(pi.GetValue(obj, null));
		}

		/// <summary>
		/// Returns a private Property Value from a given Object. Uses Reflection.
		/// Throws a ArgumentOutOfRangeException if the Property is not found.
		/// </summary>
		/// <typeparam name="T">Type of the Property</typeparam>
		/// <param name="obj">Object from where the Property Value is returned</param>
		/// <param name="propName">Propertyname as string.</param>
		/// <returns>PropertyValue</returns>
		public static T GetPrivateFieldValue<T>(this object obj, string propName) {
			if (obj == null) throw new ArgumentNullException("obj");
			Type t = obj.GetType();
			FieldInfo fi = null;
			while (fi == null && t != null) {
				fi = t.GetField(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				t = t.BaseType;
			}
			if (fi == null) throw new ArgumentOutOfRangeException("propName", string.Format("Field {0} was not found in Type {1}", propName, obj.GetType().FullName));
			return Tools.Object.ChangeType<T>(fi.GetValue(obj));
		}

		/// <summary>
		/// Sets a _private_ Property Value from a given Object. Uses Reflection.
		/// Throws a ArgumentOutOfRangeException if the Property is not found.
		/// </summary>
		/// <typeparam name="T">Type of the Property</typeparam>
		/// <param name="obj">Object from where the Property Value is set</param>
		/// <param name="propName">Propertyname as string.</param>
		/// <param name="val">Value to set.</param>
		/// <returns>PropertyValue</returns>
		public static void SetPrivatePropertyValue<T>(this object obj, string propName, T val) {
			Type t = obj.GetType();
			if (t.GetProperty(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) == null)
				throw new ArgumentOutOfRangeException("propName", string.Format("Property {0} was not found in Type {1}", propName, obj.GetType().FullName));
			t.InvokeMember(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance, null, obj, new object[] { val });
		}

		/// <summary>
		/// Set a private Property Value on a given Object. Uses Reflection.
		/// </summary>
		/// <typeparam name="T">Type of the Property</typeparam>
		/// <param name="obj">Object from where the Property Value is returned</param>
		/// <param name="propName">Propertyname as string.</param>
		/// <param name="val">the value to set</param>
		/// <exception cref="ArgumentOutOfRangeException">if the Property is not found</exception>
		public static void SetPrivateFieldValue<T>(this object obj, string propName, T val) {
			if (obj == null) throw new ArgumentNullException("obj");
			Type t = obj.GetType();
			FieldInfo fi = null;
			while (fi == null && t != null) {
				fi = t.GetField(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				t = t.BaseType;
			}
			if (fi == null) throw new ArgumentOutOfRangeException("propName", string.Format("Field {0} was not found in Type {1}", propName, obj.GetType().FullName));
			fi.SetValue(obj, val);
		}

		public static object InvokeMethod(this object obj, string methodName, params object[] parameters) {
			return
				obj
				.GetType()
				.GetMethod(
					methodName,
					BindingFlags.Instance |
					BindingFlags.Public |
					BindingFlags.NonPublic |
					BindingFlags.Static |
					BindingFlags.FlattenHierarchy
				)
				.FastInvoke(obj, parameters);
		}

		public static T InvokeMethod<T>(this object obj, string methodName, params object[] parameters) {
			var val = InvokeMethod(obj, methodName, parameters);
			return val == null ? default(T) : Tools.Object.ChangeType<T>(val);
		}
#endif

		public static void SetDefaultValues(this object obj) {
			Tools.Object.SetDefaultValues(obj);
		}

#if !__WP8__
		/// <summary>
		/// Converts an object to a byte array
		/// </summary>
		/// <param name="obj">The object to be converted</param>
		/// <returns>A byte array that contains the converted object</returns>
		public static byte[] SerializeToByteArray(this object obj) {
			if (obj == null) {
				return new byte[0];
			}

			using (var stream = new MemoryStream()) {
				var formatter = new BinaryFormatter();
				formatter.Serialize(stream, obj);
				var bytes = stream.ToArray();
				stream.Flush();
				stream.Close();
				return bytes;
			}
		}
#endif

		public static string ToSQLString(this object obj) {
			if (obj == null || obj == DBNull.Value)
				return "NULL";

			var value = string.Empty;
			TypeSwitch.Do(obj,
				TypeSwitch.Case<string>(s => value = string.Format("'{0}'", s.EscapeSQL())),
				TypeSwitch.Case<Guid>(g => value = "'" + g.ToString().ToUpper() + "'"),
				TypeSwitch.Case<char[]>(s => value = string.Format("'{0}'", s.ToString())),
				TypeSwitch.Case<DateTime>(d => value = string.Format("'{0:yyyy-MM-dd HH:mm:ss.fff}'", d)),
				TypeSwitch.Case<byte[]>(b => value = string.Format("'{0}'", b.ToHexString())),
				TypeSwitch.Case<char>(x => value = string.Format("'{0}'", x)),
				TypeSwitch.Case<bool>(b => value = string.Format("{0}", b ? 1 : 0)),
				TypeSwitch.Case<byte>(x => value = string.Format("{0}", x)),
				TypeSwitch.Case<sbyte>(x => value = string.Format("{0}", x)),
				TypeSwitch.Case<short>(x => value = string.Format("{0}", x)),
				TypeSwitch.Case<ushort>(x => value = string.Format("{0}", x)),
				TypeSwitch.Case<int>(x => value = string.Format("{0}", x)),
				TypeSwitch.Case<uint>(x => value = string.Format("{0}", x)),
				TypeSwitch.Case<long>(x => value = string.Format("{0}", x)),
				TypeSwitch.Case<ulong>(x => value = string.Format("{0}", x)),
				TypeSwitch.Case<float>(x => value = string.Format("{0}", x)),
				TypeSwitch.Case<double>(x => value = string.Format("{0}", x)),
				TypeSwitch.Case<decimal>(x => value = string.Format("{0}", x)),
				TypeSwitch.Default(() => value = string.Format("'{0}'", value))
			);
			return value;
		}

	}
}
