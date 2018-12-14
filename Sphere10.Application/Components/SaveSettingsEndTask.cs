//-----------------------------------------------------------------------
// <copyright file="SaveSettingsEndTask.cs" company="Sphere 10 Software">
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
using Sphere10.Framework;


namespace Sphere10.Application {
	
	
	public class SaveSettingsEndTask :  IApplicationEndTask {

		public SaveSettingsEndTask(IConfigurationServices configurationServices) {
			ConfigurationServices = configurationServices;
		}

		public IConfigurationServices ConfigurationServices { get; set; }


		public Result End() {
			Result result = Result.Default;
			try {
				ConfigurationServices.UserSettings.Persist();
				ConfigurationServices.SystemSettings.Persist();
			} catch (Exception error) {
				result.AddException(error);
			}
			return result;
		}
	}
}


#endif
