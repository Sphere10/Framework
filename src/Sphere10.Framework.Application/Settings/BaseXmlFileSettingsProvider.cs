//-----------------------------------------------------------------------
// <copyright file="BaseXmlFileSettingsProvider.cs" company="Sphere 10 Software">
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


namespace Sphere10.Framework.Application {


	public class BaseXMLFileSettingsProvider : XMLFileSettingsProvider {

		public BaseXMLFileSettingsProvider(IProductInformationServices productInformationServices, Environment.SpecialFolder specialFolder, string fileName) 
			: base (
				string.Format(
					"{1}{0}{2}{0}{3}{0}{4}",
					Path.DirectorySeparatorChar,
					Environment.GetFolderPath(specialFolder),
					productInformationServices.ProductInformation.CompanyName,
					productInformationServices.ProductInformation.ProductName,
					fileName
				)
			) {
			ProductInformationServices = productInformationServices;
		}

		public IProductInformationServices ProductInformationServices { get; private set; }

		
	}
}
#endif
