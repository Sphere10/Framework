//-----------------------------------------------------------------------
// <copyright file="StandardConfigurationServices.cs" company="Sphere 10 Software">
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

	
	public class StandardConfigurationServices : BaseConfigurationServices  {

		public StandardConfigurationServices() {
			ResolveSettingsImplementations();
		}

		public ISettingsServices UserSettingsImpl { get; private set; }
		public ISettingsServices SystemSettingsImpl { get; private set; }


		public virtual void ResolveSettingsImplementations() {
			UserSettingsImpl = ComponentRegistry.Instance.Resolve<ISettingsServices>("UserSettings");
			SystemSettingsImpl = ComponentRegistry.Instance.Resolve<ISettingsServices>("SystemSettings");
		}

		public override ISettingsServices UserSettings {
			get { return UserSettingsImpl; }
		}


		public override ISettingsServices SystemSettings {
			get { return SystemSettingsImpl; }
		}

	}
}


#endif
