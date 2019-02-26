//-----------------------------------------------------------------------
// <copyright file="AppSettingAttribute.cs" company="Sphere 10 Software">
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
using System.Configuration;
using System.Linq;
using System.Text;

namespace Sphere10.Framework {
	public class AppSettingAttribute : DefaultValueAttribute {

		public AppSettingAttribute(string key) : base(GetAppSetting(key)) {
			Key = key;
		}

		public string Key { get; private set; }

		public override object Value {
			get {
				return GetAppSetting(Key);
			}
		}

		private static string GetAppSetting(string key) {
			return ConfigurationManager.AppSettings[key];
		}

	}
}

