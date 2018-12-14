//-----------------------------------------------------------------------
// <copyright file="StandardLicenseKeyProvider.cs" company="Sphere 10 Software">
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
using System.Diagnostics;
using Sphere10.Framework;


namespace Sphere10.Application {
	public class StandardLicenseKeyProvider : ILicenseKeyServices {

		public const string LicenseKeySettingName = "LicenseKey";
		public const string LicenseOverrideCommandSettingName = "LicenseOverrideCommand";

		public StandardLicenseKeyProvider(IConfigurationServices configurationServices) {
			ConfigurationServices = configurationServices;
			AssemblyAttributesManager = new AssemblyAttributesManager();
		}

		public IConfigurationServices ConfigurationServices { get; private set; }

		public AssemblyAttributesManager AssemblyAttributesManager { get; private set; }

		public bool HasDefaultLicenseKey() {

			return AssemblyAttributesManager.HasAssemblyDefaultProductKey();
		}

		public string GetDefaultLicenseKey() {
			AssemblyAttributesManager assemblyAttibutes = new AssemblyAttributesManager();
			return assemblyAttibutes.GetAssemblyDefaultProductKey();
		}

		public bool HasRegisteredLicenseKey() {
			return ConfigurationServices.SystemSettings.ContainsKey(LicenseKeySettingName);
		}

		public string GetRegisteredLicenseKey() {
			Debug.Assert(HasRegisteredLicenseKey());
			if (!HasRegisteredLicenseKey()) {
				throw new SoftwareException("No license key has been registered with this product");
			}
			return (string)ConfigurationServices.SystemSettings[LicenseKeySettingName];
		}

		public void SetLicenseKey(string key) {
			if (string.IsNullOrEmpty(key)) {
				throw new SoftwareException("Unable to set license key to empty string");
			}
			ConfigurationServices.SystemSettings[LicenseKeySettingName] = key;
			ConfigurationServices.SystemSettings.Persist();

		}

		public void RemoveRegisteredLicenseKey() {
			if (HasRegisteredLicenseKey()) {
				ConfigurationServices.SystemSettings.Remove(LicenseKeySettingName);
				ConfigurationServices.SystemSettings.Persist();
			}
		}

		public bool HasLicenseOverrideCommand() {
			return ConfigurationServices.SystemSettings.ContainsKey(LicenseOverrideCommandSettingName);
		}

		public ProductLicenseCommand GetLicenseOverrideCommand() {
			Debug.Assert(HasLicenseOverrideCommand());
			return (ProductLicenseCommand)ConfigurationServices.SystemSettings[LicenseOverrideCommandSettingName];
		}

		public void SetLicenseOverrideCommand(ProductLicenseCommand value) {
			ConfigurationServices.SystemSettings[LicenseOverrideCommandSettingName] = value;
			ConfigurationServices.SystemSettings.Persist();
		}

		public void RemoveLicenseOverrideCommand() {
			ConfigurationServices.SystemSettings.Remove(LicenseOverrideCommandSettingName);
			ConfigurationServices.SystemSettings.Persist();
		}


	}
}

#endif
