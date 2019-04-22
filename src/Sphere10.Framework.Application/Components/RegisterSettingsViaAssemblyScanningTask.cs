//-----------------------------------------------------------------------
// <copyright file="RegisterSettingsViaAssemblyScanningTask.cs" company="Sphere 10 Software">
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
using System.Reflection;

using System.IO;
using Sphere10.Framework;

namespace Sphere10.Framework.Application {

	public class RegisterSettingsViaAssemblyScanningTask : BaseApplicationInitializeTask {

		public RegisterSettingsViaAssemblyScanningTask(IConfigurationServices configurationServices) {
			ConfigurationServices = configurationServices;
		}

		public IConfigurationServices ConfigurationServices { get; private set; }

		public override void Initialize() {
			AppDomain
				.CurrentDomain
				.GetAssemblies()
				.Where(assembly => !(assembly.FullName.StartsWith("System") || assembly.FullName.StartsWith("Microsoft") || assembly.FullName.StartsWith("mscorlib")))
				.ForEach(assembly =>
					assembly
						.GetTypes()
						.ForEach(type => {
 							if (type.IsSubclassOf(typeof(ComponentSettings))) {
								try {
									var key = ComponentSettings.DetermineKey(type);
									var settingsAttribute  = type.GetCustomAttributeOfType<RegisterSettingsAttribute>();
									ISettingsServices provider;
									switch (settingsAttribute.Scope) {
										case ComponentSettingsScope.Global:
											provider = ConfigurationServices.SystemSettings;
											break;
										case ComponentSettingsScope.User:
											provider = ConfigurationServices.UserSettings;
											break;
										default:
											throw new SoftwareException("Scope is not supported {0}", settingsAttribute.Scope);
									}
									if (!provider.ContainsKey(key)) {
										// Create component settings and save
										ComponentSettings componentSettings = Activator.CreateInstance(type) as ComponentSettings;
										componentSettings.ResetToDefault();
										componentSettings.Provider = provider;
										componentSettings.Save();
									}
								} catch (Exception typeActivationError) {
									throw new SoftwareException(typeActivationError, "Unable to process component settings class '{0}'.", type.FullName);
								}
							}
						})
				);
		}
	}

}


#endif
