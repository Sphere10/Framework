//-----------------------------------------------------------------------
// <copyright file="GlobalSettings.cs" company="Sphere 10 Software">
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
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Sphere10.Framework {
	public static class GlobalSettings {

		static GlobalSettings() {
#if __IOS__
            Provider = new UserDefaultsSettingsProvider();
#elif __ANDROID__
			Provider = new DirectorySettingsProvider(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppDomain.CurrentDomain.FriendlyName));
#elif __WP8__
            Provider = new WP8SettingsProvider();
#else
			Provider = new DirectorySettingsProvider(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppDomain.CurrentDomain.FriendlyName));
#endif
		}

		public static ISettingsProvider Provider { get; set; }


		public static bool Has<T>(object id = null) where T : SettingsObject, new() {
			return Provider.Has<T>(id);
		}

		public static T Get<T>(object id = null) where T : SettingsObject, new() {
			return Provider.Get<T>(id);
		}

		public static void Clear() {
			Provider.ClearSettings();
		}
	}
}
