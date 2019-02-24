//-----------------------------------------------------------------------
// <copyright file="YearAttribute.cs" company="Sphere 10 Software">
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
using System.Text.RegularExpressions;

namespace Sphere10.Framework.Web.Validators {
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class YearAttribute : DataTypeAttribute {
		private static Regex _regex = new Regex(@"^[0-9]{4}$");

		public string Regex {
			get {
				return _regex.ToString();
			}
		}

		public YearAttribute()
			: base("year") {
		}

		public override string FormatErrorMessage(string name) {
			if (ErrorMessage == null && ErrorMessageResourceName == null) {
				ErrorMessage = "The {0} field is not a valid year";
			}

			return base.FormatErrorMessage(name);
		}

		public override bool IsValid(object value) {
			if (value == null) {
				return true;
			}

			int retNum;
			var parseSuccess = int.TryParse(Convert.ToString(value), out retNum);

			return parseSuccess && retNum >= 1 && retNum <= 9999;
		}
	}
}
