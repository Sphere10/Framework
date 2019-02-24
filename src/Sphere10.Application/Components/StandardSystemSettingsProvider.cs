//-----------------------------------------------------------------------
// <copyright file="StandardSystemSettingsProvider.cs" company="Sphere 10 Software">
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


namespace Sphere10.Application {


	public class StandardSystemSettingsProvider : BaseXMLFileSettingsProvider {

		public StandardSystemSettingsProvider(IProductInformationServices productInformationServices)
			: base(productInformationServices, Environment.SpecialFolder.CommonApplicationData,	"ApplicationSettings.xml") {
		}

	}
}

#endif
