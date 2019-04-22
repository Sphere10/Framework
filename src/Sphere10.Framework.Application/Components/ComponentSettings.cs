//-----------------------------------------------------------------------
// <copyright file="ComponentSettings.cs" company="Sphere 10 Software">
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
using System.ComponentModel;

using System.IO;
using Sphere10.Framework;

namespace Sphere10.Framework.Application {

    [Obfuscation(Exclude = true)]
	public abstract class ComponentSettings {

		public ISettingsServices Provider { get; set; }

		public void ResetToDefault() {
			foreach (var property in this.GetType().GetProperties(BindingFlags.Instance |  BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.FlattenHierarchy | BindingFlags.Public)) {
				try {
					var defaultAttrs = property.GetCustomAttributesOfType<DefaultValueAttribute>(true).ToArray();
					if (defaultAttrs.Length > 0) {
						this.SetPropertyValue(property.Name, defaultAttrs[0].Value );
					}
				} catch (Exception error) {
					throw new SoftwareException(error, "ComponentSetting property '{0}' cannot be assigned to its default value", property.Name);
				}
			}
		}

		public void Save() {
			var key = DetermineKey(GetType());
			if (!Provider.ContainsKey(key)) {
				Provider[key] = this;
			}
			Provider.Persist();
		}


		internal static string DetermineKey(Type type) {
			return type.FullName;
		}

	}
}
