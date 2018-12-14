//-----------------------------------------------------------------------
// <copyright file="EnumTool.cs" company="Sphere 10 Software">
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
using System.Text;
using Sphere10.Framework;

namespace Tools {

	public static class Enums {

		public static bool HasDescription(System.Enum value) {
			var result = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true).Length > 0;
			return result;
		}

		public static string GetDescription(System.Enum value) {
			var field = value.GetType().GetField(value.ToString());
			var descriptions = field.GetCustomAttributesOfType<DescriptionAttribute>().ToArray();
			return !descriptions.Any() ? value.ToString() : descriptions.First().Description;
		}

		public static object GetDefaultValue(System.Enum value) {
			var field = value.GetType().GetField(value.ToString());
			var values = field.GetCustomAttributesOfType<DefaultValueAttribute>().ToArray();
			return !values.Any() ? value.ToString() : values.First().Value;
		}

		public static T GetValueFromDescription<T>(string description) {
			return (T) GetValueFromDescription(typeof(T), description);
		}

		public static object GetValueFromDescription(Type type, string description) {
			if (!type.IsEnum) throw new InvalidOperationException();
			foreach (var field in type.GetFields()) {
				var attribute = Attribute.GetCustomAttribute(field,
					typeof(DescriptionAttribute)) as DescriptionAttribute;
				if (attribute != null) {
					if (attribute.Description == description)
						return field.GetValue(null);
				} else {
					if (field.Name == description)
						return field.GetValue(null);
				}
			}
			throw new ArgumentException("Not found.", "description");
		}

	}
}

