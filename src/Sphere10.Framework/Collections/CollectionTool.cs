//-----------------------------------------------------------------------
// <copyright file="CollectionTool.cs" company="Sphere 10 Software">
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

namespace Tools {

		public static class Collection {

			public static IEnumerable<T> IgnoreNulls<T>(params T[] values ) {
				return values.Where(v => v != null);
			}

			public static T[] GenerateArray<T>(int num, Func<int, T> generator) {
				var arr = new T[num];
				for (int i = 0; i < num; i++)
					arr[i] = generator(i);
				return arr;
			}

		    public static IEnumerable<T> Generate<T>(Func<T> generator) {
		        while (true)
		            yield return generator();
		    }



            public static bool ValidIndex<T>(IEnumerable<T> collection, int index) {
                if (index < 0)
                    return false;
                return index < collection.Count();
            }
        }

	
}
