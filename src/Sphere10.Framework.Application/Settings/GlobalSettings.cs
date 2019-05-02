//-----------------------------------------------------------------------
// <copyright file="UserSettings.cs" company="Sphere 10 Software">
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

namespace Sphere10.Framework.Application {

	/// <summary>
	/// Settings that are system-wide and shared by all users.
	/// </summary>
	public static class GlobalSettings {

		private static ISettingsProvider _provider;

		public static ISettingsProvider Provider {
			get {
				CheckProvider();
				return _provider;
			}
			set => _provider = value;
		}

		public static bool Has<T>(object id = null) where T : SettingsObject, new() {
			return Provider.Has<T>(id);
		}

		public static T Get<T>(object id = null) where T : SettingsObject, new() {
			return Provider.Get<T>(id);
		}

		public static void Clear() {
			CheckProvider();
			Provider.ClearSettings();
		}

		public static ISettingsProvider CreateDefaultProvider() {
			return new DirectorySettingsProvider(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppDomain.CurrentDomain.FriendlyName));
		}

		private static void CheckProvider() {
			if (_provider == null)
				throw new SoftwareException("Global Settings Provider has not been initialized");
		}
	}
}
