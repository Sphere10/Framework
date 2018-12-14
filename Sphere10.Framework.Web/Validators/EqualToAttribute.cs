//-----------------------------------------------------------------------
// <copyright file="EqualToAttribute.cs" company="Sphere 10 Software">
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
using System.Linq;
using System.Reflection;

namespace Sphere10.Framework.Web.Validators {
	/// <summary>
	/// Validates that the property has the same value as the given 'otherProperty' 
	/// </summary>
	/// <remarks>
	/// From Mvc3 Futures
	/// </remarks>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class EqualToAttribute : ValidationAttribute {
		public EqualToAttribute(string otherProperty) {
			if (otherProperty == null) {
				throw new ArgumentNullException("otherProperty");
			}
			OtherProperty = otherProperty;
			OtherPropertyDisplayName = null;
		}

		public string OtherProperty { get; private set; }

		public string OtherPropertyDisplayName { get; set; }

		public override string FormatErrorMessage(string name) {
			if (ErrorMessage == null && ErrorMessageResourceName == null) {
				ErrorMessage = "'{0}' and '{1}' do not match.";
			}

			var otherPropertyDisplayName = OtherPropertyDisplayName ?? OtherProperty;

			return String.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, otherPropertyDisplayName);
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
			var memberNames = new[] { validationContext.MemberName };

			PropertyInfo otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);
			if (otherPropertyInfo == null) {
				return new ValidationResult(String.Format(CultureInfo.CurrentCulture, "Could not find a property named {0}.", OtherProperty), memberNames);
			}

			var displayAttribute =
                otherPropertyInfo.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;

			if (displayAttribute != null && !string.IsNullOrWhiteSpace(displayAttribute.Name)) {
				OtherPropertyDisplayName = displayAttribute.Name;
			}

			object otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);
			if (!Equals(value, otherPropertyValue)) {
				return new ValidationResult(FormatErrorMessage(validationContext.DisplayName), memberNames);
			}
			return null;
		}
	}
}
