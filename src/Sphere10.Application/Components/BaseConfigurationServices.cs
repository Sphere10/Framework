//-----------------------------------------------------------------------
// <copyright file="BaseConfigurationServices.cs" company="Sphere 10 Software">
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

#if !__MOBILE__
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using Sphere10.Framework;

namespace Sphere10.Application {

	public abstract class BaseConfigurationServices : IConfigurationServices {
		public event ConfigurationChangedEventHandler ConfigurationChanged;
		private IDictionary<string, ComponentSettings> _componentSettingsCache;


		public BaseConfigurationServices() {
			_componentSettingsCache = new Dictionary<string, ComponentSettings>();
		}

		protected virtual void OnConfigurationChanged() {
		}

		public void FireConfigurationChangedEvent() {
			OnConfigurationChanged();
			if (ConfigurationChanged != null) {
				ConfigurationChanged();
			}
		}

		public abstract ISettingsServices UserSettings { get; }

		public abstract ISettingsServices SystemSettings { get; }


		public ComponentSettings GetComponentSettings(Type componentSettingsType)  {
			ComponentSettings componentSettings;
			if (!_componentSettingsCache.TryGetValue(componentSettingsType.FullName, out componentSettings)) {
				lock (this) {
					if (!_componentSettingsCache.TryGetValue(componentSettingsType.FullName, out componentSettings)) {
#region Get the component settings from the provider
						var registrationAttribute = componentSettingsType.GetCustomAttributeOfType<RegisterSettingsAttribute>();
						ISettingsServices provider;
						switch (registrationAttribute.Scope) {
							case ComponentSettingsScope.Global:
								provider = SystemSettings;
								break;
							case ComponentSettingsScope.User:
								provider = UserSettings;
								break;
							default:
								throw new SoftwareException("Unsupported Component Settings Scope '{0}'", registrationAttribute.Scope);
						}
						componentSettings = provider[componentSettingsType.FullName] as ComponentSettings;
						componentSettings.Provider = provider;
						// Cache it permanently
						_componentSettingsCache[componentSettingsType.FullName] = componentSettings;
#endregion
					}
				}
			}
			return componentSettings;
		}
	}

}


#endif
