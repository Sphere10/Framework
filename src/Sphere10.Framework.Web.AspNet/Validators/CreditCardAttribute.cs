//-----------------------------------------------------------------------
// <copyright file="CreditCardAttribute.cs" company="Sphere 10 Software">
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
using System.Linq;

namespace Sphere10.Framework.Web {
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class CreditCardAttribute : DataTypeAttribute {
		public CreditCardAttribute()
			: base("creditcard") {
		}

		public override string FormatErrorMessage(string name) {
			if (ErrorMessage == null && ErrorMessageResourceName == null) {
				ErrorMessage = "The {0} field is not a valid credit card number.";
			}

			return base.FormatErrorMessage(name);
		}

		public override bool IsValid(object value) {
			if (value == null) {
				return true;
			}

			var ccValue = value as string;
			if (ccValue == null) {
				return false;
			}

			ccValue = ccValue.Replace("-", string.Empty);

			if (string.IsNullOrEmpty(ccValue)) return false; //Don't accept only dashes

			int checksum = 0;
			bool evenDigit = false;

			// http://www.beachnet.com/~hstiles/cardtype.html
			foreach (char digit in ccValue.Reverse()) {
				if (!Char.IsDigit(digit)) {
					return false;
				}

				int digitValue = (digit - '0') * (evenDigit ? 2 : 1);
				evenDigit = !evenDigit;

				while (digitValue > 0) {
					checksum += digitValue % 10;
					digitValue /= 10;
				}
			}

			return (checksum % 10) == 0;
		}
	}
}
