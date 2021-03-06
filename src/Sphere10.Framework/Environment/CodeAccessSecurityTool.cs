//-----------------------------------------------------------------------
// <copyright file="CodeAccessSecurityTool.cs" company="Sphere 10 Software">
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
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace Tools {
	public static class CodeAccessSecurity {
		public static bool HasUnrestrictedFeatureSet => AppDomain.CurrentDomain.IsFullyTrusted;
	}
}
