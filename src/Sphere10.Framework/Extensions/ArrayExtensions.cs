//-----------------------------------------------------------------------
// <copyright file="ArrayExtensions.cs" company="Sphere 10 Software">
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
using System.IO;
using System.Runtime.Serialization;

using System.Diagnostics;

namespace Sphere10.Framework {


    public static class ArrayExtensions {
        public static T[] Copy<T>(this T[] array) {
            T[] array2 = new T[array.Length];
            Array.Copy(array, array2, array.Length);

            return array2;
        }

        public static void InsertionSort<T>(this T[] array, int index, int count, IComparer<T> comparer) {
            int limit = index + count;
            for (int i = index + 1; i < limit; i++) {
                var item = array[i];

                int j = i - 1;
                while (comparer.Compare(array[j], item) > 0) {
                    array[j + 1] = array[j];
                    j--;
                    if (j < index)
                        break;
                }

                array[j + 1] = item;
            }
        }

        public static void InsertionSort<T>(this T[] array, IComparer<T> comparer) {
            InsertionSort<T>(array, 0, array.Length, comparer);
        }

        public static T[] SubArray<T>(this T[] buffer, int offset, int length) {
            T[] middle = new T[length];
            Array.Copy(buffer, offset, middle, 0, length);
            return middle;
        }

        public static T[] Left<T>(this T[] buffer, int length) {
            return buffer.SubArray(0, length);
        }

        public static T[] Right<T>(this T[] buffer, int length) {
            return buffer.SubArray(buffer.Length - length, length);
        }

        public static string ToString<T>(this T[] array, string separator) {
            return "{" + String.Join<T>(separator, array) + "}";
        }


  
    }
}
