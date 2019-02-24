//-----------------------------------------------------------------------
// <copyright file="CuitAttribute.cs" company="Sphere 10 Software">
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
	public class CuitAttribute : DataTypeAttribute {
		private static Regex _regex = new Regex(@"^[0-9]{2}-?[0-9]{8}-?[0-9]$");

		public string Regex {
			get {
				return _regex.ToString();
			}
		}

		public CuitAttribute()
			: base("cuit") {
		}

		public override string FormatErrorMessage(string name) {
			if (ErrorMessage == null && ErrorMessageResourceName == null) {
				ErrorMessage = "The {0} field is not a valid CUIT number.";
			}

			return base.FormatErrorMessage(name);
		}

		public override bool IsValid(object value) {
			if (value == null) {
				return true;
			}

			var valueAsString = value as string;
			return valueAsString != null && _regex.Match(valueAsString).Length > 0;
		}
	}
}
