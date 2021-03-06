//-----------------------------------------------------------------------
// <copyright file="ILicenseServices.cs" company="Sphere 10 Software">
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
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Reflection;

namespace Sphere10.Framework.Application {


    /// <summary>
    /// Provides all the licensing services for the application. 
    /// </summary>
    public interface ILicenseServices {

		void RegisterLicenseKey(string key);

		void RegisterLicenseOverrideCommand(ProductLicenseCommand command);

		void RemoveLicenseOverrideCommand();

		LicenseInformation LicenseInformation { get; }

	}
		
}

#endif
