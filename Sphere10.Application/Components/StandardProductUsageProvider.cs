//-----------------------------------------------------------------------
// <copyright file="StandardProductUsageProvider.cs" company="Sphere 10 Software">
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
using System.Reflection;
using System.Security.Policy;

namespace Sphere10.Application {

	public class StandardProductUsageProvider : IProductUsageServices {
		private const string FirstTimeExecutedByUserKey = "FirstTimeExecutedByUser";
		private const string NumberOfUsesByUserKey = "NumberOfUsesByUser";
		private const string FirstTimeExecutedBySystemKey = "FirstTimeExecutedBySystem";
		private const string NumberOfUsesBySystemKey = "NumberOfUsesBySystem";


		public StandardProductUsageProvider(IConfigurationServices configurationServices) {
			ConfigurationServices = configurationServices;
		}

		public IConfigurationServices ConfigurationServices { get; set; }

		public ProductUsageInformation ProductUsageInformation {
			get {
				return new ProductUsageInformation {
					DaysUsedBySystem = (int)Math.Ceiling(DateTime.UtcNow.Subtract(this.FirstUTCTimeExecutedBySystem).TotalDays),
					DaysUsedByUser = (int)Math.Ceiling(DateTime.UtcNow.Subtract(this.FirstUTCTimeExecutedByUser).TotalDays),
					FirstUsedDateBySystemUTC = this.FirstUTCTimeExecutedBySystem,
					FirstUsedDateByUserUTC = this.FirstUTCTimeExecutedByUser,
					NumberOfUsesBySystem = NumberOfUsesBySystem,
					NumberOfUsesByUser = NumberOfUsesByUser
				};
			}
		}

		protected virtual DateTime FirstUTCTimeExecutedByUser {
			get {
				return (DateTime)ConfigurationServices.UserSettings[FirstTimeExecutedByUserKey];
			}
			set {
				ConfigurationServices.UserSettings[FirstTimeExecutedByUserKey] = value;
			}
		}

		protected virtual int NumberOfUsesByUser {
			get {
				return (int)ConfigurationServices.UserSettings[NumberOfUsesByUserKey];
			}
			set {
				ConfigurationServices.UserSettings[NumberOfUsesByUserKey] = value;
			}
		}

		protected virtual DateTime FirstUTCTimeExecutedBySystem {
			get {
				return (DateTime)ConfigurationServices.SystemSettings[FirstTimeExecutedBySystemKey];
			}
			set {
				ConfigurationServices.SystemSettings[FirstTimeExecutedBySystemKey] = value;
			}
		}

		protected virtual int NumberOfUsesBySystem {
			get {
				return (int)ConfigurationServices.SystemSettings[NumberOfUsesBySystemKey];
			}
			set {
				ConfigurationServices.SystemSettings[NumberOfUsesBySystemKey] = value;
			}
		}

		public void IncrementUsageByOne() {
			NumberOfUsesBySystem++;
			NumberOfUsesByUser++;
			ConfigurationServices.SystemSettings.Persist();
			ConfigurationServices.UserSettings.Persist();
		}

		/// <summary>
		/// This is an implementation-specific initialization task for <see cref="StandardProductUsageProvider"/>.
		/// It will always run, but it will do nothing if the user overrides the standard <see cref="IProductUsageServices"/> provider.
		/// </summary>
		public class Initializer : IApplicationInitializeTask {

			public Initializer(IProductUsageServices productUsageServices, IConfigurationServices configurationServices) {
				ProductUsageServices = productUsageServices;
				ConfigurationServices = configurationServices;
			}

			public IProductUsageServices ProductUsageServices { get; private set; }

			public IConfigurationServices ConfigurationServices { get; private set; }

			public void Initialize() {
				// We only intialize the class if it is the implementation we expect, as it is possible the user has overriden this implementation and 
				// thus we do not care to initialize it
				if (ProductUsageServices is StandardProductUsageProvider) {
					var standardProductUsageProvider = (StandardProductUsageProvider) ProductUsageServices;
					bool modifiedUserSettings = false;
					bool modifiedGlobalSettings = false;

					// Set the keys if they don't exist
					if (!ConfigurationServices.SystemSettings.ContainsKey(StandardProductUsageProvider.FirstTimeExecutedBySystemKey)) {
						standardProductUsageProvider.FirstUTCTimeExecutedBySystem = DateTime.UtcNow;
						standardProductUsageProvider.NumberOfUsesBySystem = 0;
						modifiedGlobalSettings = true;
					}
					if (!ConfigurationServices.SystemSettings.ContainsKey(StandardProductUsageProvider.NumberOfUsesBySystemKey)) {
						standardProductUsageProvider.NumberOfUsesBySystem = 0;
						modifiedGlobalSettings = true;
					}
					if (!ConfigurationServices.UserSettings.ContainsKey(StandardProductUsageProvider.FirstTimeExecutedByUserKey)) {
						standardProductUsageProvider.FirstUTCTimeExecutedByUser = DateTime.UtcNow;
						standardProductUsageProvider.NumberOfUsesByUser = 0;
						modifiedUserSettings = true;
					}
					if (!ConfigurationServices.UserSettings.ContainsKey(StandardProductUsageProvider.NumberOfUsesByUserKey)) {
						standardProductUsageProvider.NumberOfUsesByUser = 0;
						modifiedUserSettings = true;
					}

					if (modifiedGlobalSettings) {
						ConfigurationServices.SystemSettings.Persist();
					}
					if (modifiedUserSettings) {
						ConfigurationServices.UserSettings.Persist();
					}
				}
			}

			// Critical that this component is run in the very beginning
			public int Sequence {
				get { return 0; }
			}
		}


	}
}

#endif
