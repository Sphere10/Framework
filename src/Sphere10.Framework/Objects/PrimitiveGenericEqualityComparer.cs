//-----------------------------------------------------------------------
// <copyright file="PrimitiveGenericEqualityComparer.cs" company="Sphere 10 Software">
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

#if !__WP8__

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sphere10.Framework {
	public class PrimitiveGenericEqualityComparer<T> : GenericComparer, IEqualityComparer<T> {

		public bool Equals(T x, T y) {
			return Compare(x, y) == 0;
		}

		public int GetHashCode(T obj) {
			return obj.GetHashCode();
		}

	}
}
#endif
