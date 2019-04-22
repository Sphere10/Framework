//-----------------------------------------------------------------------
// <copyright file="DateAttribute.cs" company="Sphere 10 Software">
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
using System.ComponentModel.DataAnnotations;

namespace Sphere10.Framework.Web {
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class DateAttribute : DataTypeAttribute {
		public DateAttribute()
			: base(DataType.Date) {
		}

		public override string FormatErrorMessage(string name) {
			if (ErrorMessage == null && ErrorMessageResourceName == null) {
				ErrorMessage = "The field {0} is not a valid date.";
			}

			return base.FormatErrorMessage(name);
		}

		public override bool IsValid(object value) {
			if (value == null) return true;

			DateTime retDate;

			return DateTime.TryParse(Convert.ToString(value), out retDate);
		}
	}
}
