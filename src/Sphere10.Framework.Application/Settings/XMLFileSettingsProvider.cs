//-----------------------------------------------------------------------
// <copyright file="XMLFileSettingsProvider.cs" company="Sphere 10 Software">
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
using System.Text;
using System.IO;
using System.Reflection;
using Sphere10.Framework;


namespace Sphere10.Framework.Application {

	public class XMLFileSettingsProvider : FileDictionary<string, object>, ISettingsServices {

		public XMLFileSettingsProvider(string filePath)
			: base(filePath) {
		}

		public void Persist() {
			base.Save();
		}

		public void Reload() {
			base.Load();
		}

		public T GetComponentSettings<T>() where T : ComponentSettings, new() {
			string key = typeof(T).FullName;
			if (this.ContainsKey(key)) {
				return this[key] as T;
			} else {
				T settings = new T();
				settings.Provider = this;
				settings.ResetToDefault();
				this[key] = settings;
				this.Persist();
				return settings;
			}
		}
	}
}
#endif
