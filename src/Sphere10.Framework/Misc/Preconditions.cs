//-----------------------------------------------------------------------
// <copyright file="Preconditions.cs" company="Sphere 10 Software">
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
using System.Text;

namespace Sphere10.Framework {

	/// <summary>
	/// Helper static methods for argument/state validation.
	/// </summary>
	public static class Preconditions {
		/// <summary>
		/// Returns the given argument after checking whether it's null. This is useful for putting
		/// nullity checks in parameters which are passed to base class constructors.
		/// </summary>
		public static T CheckNotNull<T>(T argument, string paramName) where T : class {
			if (argument == null) {
				throw new ArgumentNullException(paramName);
			}
			return argument;
		}

		public static void CheckArgumentRange(string paramName, long value, long minInclusive, long maxInclusive) {
			if (value < minInclusive || value > maxInclusive) {
#if PCL
                throw new ArgumentOutOfRangeException(paramName,
                    "Value should be in range [" + minInclusive + "-" + maxInclusive + "]");
#else
				throw new ArgumentOutOfRangeException(paramName, value,
					"Value should be in range [" + minInclusive + "-" + maxInclusive + "]");
#endif
			}
		}

		// Note: this overload exists for performance reasons. It would be reasonable to call the
		// version using "long" values, but we'd incur conversions on every call. This method
		// may well be called very often.
		public static void CheckArgumentRange(string paramName, int value, int minInclusive, int maxInclusive) {
			if (value < minInclusive || value > maxInclusive) {
#if PCL
                throw new ArgumentOutOfRangeException(paramName,
                    "Value should be in range [" + minInclusive + "-" + maxInclusive + "]");
#else
				throw new ArgumentOutOfRangeException(paramName, value,
					"Value should be in range [" + minInclusive + "-" + maxInclusive + "]");
#endif
			}
		}

		public static void CheckArgument(bool expression, string parameter, string message) {
			if (!expression) {
				throw new ArgumentException(message, parameter);
			}
		}
	}
}
