//-----------------------------------------------------------------------
// <copyright file="IConfigurationServices.cs" company="Sphere 10 Software">
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

namespace Sphere10.Application {
	
	public delegate void ConfigurationChangedEventHandler();

	public interface IConfigurationServices  {

		event ConfigurationChangedEventHandler ConfigurationChanged;

		void FireConfigurationChangedEvent();

		ISettingsServices UserSettings { get; }

		ISettingsServices SystemSettings { get; }

		ComponentSettings GetComponentSettings(Type componentSettingsType);

	}
}

#endif

