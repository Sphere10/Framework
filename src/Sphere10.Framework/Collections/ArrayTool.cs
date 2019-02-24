//-----------------------------------------------------------------------
// <copyright file="ArrayTool.cs" company="Sphere 10 Software">
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
using System.Linq;

namespace Tools {

    public static class Array {

        public static T[] ConcatArrays<T>(T[] arr1, T[] arr2) {
            //Contract.Requires(arr1 != null);
            //Contract.Requires(arr2 != null);
            //Contract.Ensures(Contract.Result<T[]>() != null);
            //Contract.Ensures(Contract.Result<T[]>().Length == arr1.Length + arr2.Length);
            return arr1.Concat(arr2).ToArray();
        }

        public static T[] ConcatArrays<T>(T[] arr1, T item) {
            return ConcatArrays(arr1, new[] {item});
        }

        public static T RandomElement<T>(T[] arr) {
            if (arr.Length == 0)
                throw new ArgumentException("array is empty", nameof(arr));

            return arr[Tools.Maths.RandomNumberGenerator.Next(0, arr.Length - 1)];
        }
    }
}
