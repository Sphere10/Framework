//-----------------------------------------------------------------------
// <copyright file="IntegerAttribute.cs" company="Sphere 10 Software">
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

namespace Sphere10.Framework.Web.Validators {
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class IntegerAttribute : DataTypeAttribute {
		public IntegerAttribute()
			: base("integer") {
		}

		public override string FormatErrorMessage(string name) {
			if (ErrorMessage == null && ErrorMessageResourceName == null) {
				ErrorMessage = "The field {0} should be a positive or negative non-decimal number.";
			}

			return base.FormatErrorMessage(name);
		}

		public override bool IsValid(object value) {
			if (value == null) return true;

			int retNum;

			return int.TryParse(Convert.ToString(value), out retNum);
		}
	}
}
