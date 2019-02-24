//-----------------------------------------------------------------------
// <copyright file="MinAttribute.cs" company="Sphere 10 Software">
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
using System.Globalization;

namespace Sphere10.Framework.Web.Validators {
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class MinAttribute : DataTypeAttribute {
		public object Min { get { return _min; } }

		private readonly double _min;

		public MinAttribute(int min)
			: base("min") {
			_min = min;
		}

		public MinAttribute(double min)
			: base("min") {
			_min = min;
		}

		public override string FormatErrorMessage(string name) {
			if (ErrorMessage == null && ErrorMessageResourceName == null) {
				ErrorMessage = "The field {0} must be greater than or equal to {1}";
			}

			return String.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, _min);
		}

		public override bool IsValid(object value) {
			if (value == null) return true;

			double valueAsDouble;

			var isDouble = double.TryParse(Convert.ToString(value), out valueAsDouble);

			return isDouble && valueAsDouble >= _min;
		}
	}
}
