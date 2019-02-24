//-----------------------------------------------------------------------
// <copyright file="SortStrategyFactory.cs" company="Sphere 10 Software">
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

namespace Sphere10.Framework {
	internal class SortStrategyFactory {
		public static SortStrategy<T> GetSortStrategy<T>(SortType sortType) where T : IComparable<T> {
			switch (sortType) {
				case SortType.BubbleSort:
					return new BubbleSort<T>();

				case SortType.InsertionSort:
					return new InsertionSort<T>();

				case SortType.SelectionSort:
					return new SelectionSort<T>();

				case SortType.QuickSort:
					return new QuickSort<T>();

				default:
					throw new NotSupportedException("SortType not supported.");
			}
		}
	}
}
