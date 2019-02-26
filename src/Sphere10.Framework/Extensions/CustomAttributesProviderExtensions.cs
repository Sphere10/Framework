//-----------------------------------------------------------------------
// <copyright file="CustomAttributesProviderExtensions.cs" company="Sphere 10 Software">
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
using System.Reflection;

namespace Sphere10.Framework {

	public static class CustomAttributesProviderExtensions {

		public static IEnumerable<T> GetCustomAttributesOfType<T>(this ICustomAttributeProvider attributesProvider, bool inherited = false) where T : Attribute {
			return
				from attribute in attributesProvider.GetCustomAttributes(inherited)
				where attribute is T
				select (T)attribute;
		}

		public static T GetCustomAttributeOfType<T>(this ICustomAttributeProvider attributesProvider, bool inherited = false) where T : Attribute {
			var attributes = attributesProvider.GetCustomAttributesOfType<T>(inherited);
			if (attributes.Count() != 1) {
				throw new SoftwareException("{0} did not contain (or contained more than one) attribute {1}", attributesProvider.ToString(), typeof(T).Name);
			}
			return attributes.Single();
		}

	}
}
