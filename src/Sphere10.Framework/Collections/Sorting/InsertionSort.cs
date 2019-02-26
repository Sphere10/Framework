//-----------------------------------------------------------------------
// <copyright file="InsertionSort.cs" company="Sphere 10 Software">
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

namespace Sphere10.Framework {
	internal class InsertionSort<T> : SortStrategy<T> where T : IComparable<T> {
		public override void Execute(IList<T> list) {
			for (int i = 1; i < list.Count; i++) {
				T value = list[i];

				int j = i - 1;

				bool done = false;

				do {
					if (list[j].CompareTo(value) > 0) {
						list[j + 1] = list[j];
						j--;

						if (j < 0) {
							done = true;
						}
					} else {
						done = true;
					}
				}
				while (done == false);

				list[j + 1] = value;
			}
		}
	}
}
