//-----------------------------------------------------------------------
// <copyright file="RandomExtensions.cs" company="Sphere 10 Software">
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
	public static class RandomExtensions {
		public static int NextIn(this Random random, int minInclusive, int maxInclusive) {
			if (minInclusive == int.MinValue && maxInclusive == int.MaxValue) {
				return random.Next();
			} else if (minInclusive > int.MinValue && maxInclusive == int.MaxValue) {
				return random.Next(minInclusive - 1, maxInclusive) + 1;
			} else {
				return random.Next(minInclusive, maxInclusive + 1);
			}
		}

	}
}
