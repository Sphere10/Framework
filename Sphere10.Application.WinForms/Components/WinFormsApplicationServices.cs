//-----------------------------------------------------------------------
// <copyright file="WinFormsApplicationServices.cs" company="Sphere 10 Software">
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
using System.Linq;
using System.Text;
using Sphere10.Application;
using System.Drawing;
using Sphere10.Framework;

namespace Sphere10.Application.WinForms {

	public class WinFormsApplicationServices : ApplicationServices, IWinFormsApplicationServices{
	    private IApplicationIconProvider ApplicationIconProvider { get { return ComponentRegistry.Instance.Resolve<IApplicationIconProvider>(); } }

		public Icon ApplicationIcon {
			get { return ApplicationIconProvider.ApplicationIcon; }
		}
	}
}


