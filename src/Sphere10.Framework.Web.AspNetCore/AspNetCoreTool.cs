//-----------------------------------------------------------------------
// <copyright file="WebTool.cs" company="Sphere 10 Software">
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
using System.Linq;
using System.Globalization;
using Sphere10.Framework;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sphere10.Framework.Web;

namespace Tools {

	public static partial class AspNetCore {

		public static SelectList ToSelectList<TEnum>(TEnum defaultSelection)
			where TEnum : struct, IComparable, IFormattable, IConvertible {
			var values = from TEnum e in System.Enum.GetValues(typeof(TEnum))
			             select new {Id = e, Name = e.ToString(CultureInfo.InvariantCulture)};
			return new SelectList(values, "Id", "Name", defaultSelection);
		}

	}
}
