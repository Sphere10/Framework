//-----------------------------------------------------------------------
// <copyright file="FileExtensionsAttribute.cs" company="Sphere 10 Software">
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
using System.IO;
using System.Linq;

namespace Sphere10.Framework.Web {
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class FileExtensionsAttribute : DataTypeAttribute {
		public string Extensions { get; private set; }

		/// <summary>
		/// Provide the allowed file extensions, seperated via "|" (or a comma, ","), defaults to "png|jpe?g|gif" 
		/// </summary>
		public FileExtensionsAttribute(string allowedExtensions = "png,jpg,jpeg,gif")
			: base("fileextension") {
			Extensions = string.IsNullOrWhiteSpace(allowedExtensions) ? "png,jpg,jpeg,gif" : allowedExtensions.Replace("|", ",").Replace(" ", "");
		}

		public override string FormatErrorMessage(string name) {
			if (ErrorMessage == null && ErrorMessageResourceName == null) {
				ErrorMessage = "The {0} field only accepts files with the following extensions: {1}";
			}

			return String.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, Extensions);
		}

		public override bool IsValid(object value) {
			if (value == null) {
				return true;
			}

			string valueAsString = value as string;
			if (valueAsString != null) {
				return ValidateExtension(valueAsString);
			}

			return false;
		}

		private bool ValidateExtension(string fileName) {
			try {
				return Extensions.Split(',').Contains(Path.GetExtension(fileName).Replace(".", "").ToLowerInvariant());
			} catch (ArgumentException) {
				return false;
			}
		}
	}
}
