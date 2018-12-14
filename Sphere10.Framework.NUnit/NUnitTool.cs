//-----------------------------------------------------------------------
// <copyright file="NUnitTool.cs" company="Sphere 10 Software">
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
using System.Threading.Tasks;
using NUnit.Framework;
using Sphere10.Framework;

namespace Tools
{
    public static class NUnitTool {

        public static void AssertSame2DArrays<T>(IEnumerable<IEnumerable<T>> expectedRows, IEnumerable<IEnumerable<T>> actualRows,  string actualName = "Actual", string expectedName = "Expected") {
            var expectedRowsArr = expectedRows as T[][] ?? expectedRows.Select(i => i as T[] ?? i.ToArray()).ToArray();
            var actualRowsArr = actualRows as T[][] ?? actualRows.Select(i => i as T[] ?? i.ToArray()).ToArray();

            var preText = string.Format("{0}{1}{2}{0}", Environment.NewLine, Convert2DArrayToString(expectedName, expectedRowsArr), Convert2DArrayToString(actualName, actualRowsArr));

            Assert.AreEqual(expectedRowsArr.Count(), actualRowsArr.Count(), "{4}{0} has {1} row(s) but {2} has {3} row(s)", actualName, expectedRowsArr.Count(), expectedName, actualRowsArr.Count(), preText);
            foreach (var rowExpectation in expectedRowsArr.WithDescriptions().ZipWith(actualRowsArr, Tuple.Create)) {
                Assert.AreEqual(rowExpectation.Item1.Item.Count(), rowExpectation.Item2.Count(), "{5}{0} row {1} had {2} column(s) but {3} row {1} had {4} column(s)", expectedName, rowExpectation.Item1.Index, rowExpectation.Item1.Item.Count(), actualName, rowExpectation.Item2.Count(), preText);
                foreach (var colExpectation in rowExpectation.Item1.Item.WithDescriptions().ZipWith(rowExpectation.Item2, Tuple.Create)) {
                    Assert.AreEqual(colExpectation.Item1.Item, colExpectation.Item2, "{6}{0} row {1} col {2} had value {3} but {4} row {1} col {2} had value {5}", expectedName, rowExpectation.Item1.Index, colExpectation.Item1.Index, colExpectation.Item1.Item, actualName, colExpectation.Item2, preText);
                }
            }
        }

        public static string Convert2DArrayToString<T>(string header, IEnumerable<IEnumerable<T>> arr2D) {
            var textBuilder = new StringBuilder();
            textBuilder.AppendLine("{0}:",header);
            foreach (var row in arr2D) {
                textBuilder.AppendLine("\t{0}", row.ToDelimittedString(",\t"));
            }
            return textBuilder.ToString();
        }

        public static void AssertApproxEqual(System.DateTime expected, System.DateTime actual, TimeSpan? tolerance = null, string errorMessage = null) {
            var approxEqual = expected.ApproxEqual(actual, tolerance);
            if (!approxEqual)
                Assert.Fail(errorMessage ?? "Dates not approximately equal.{0}Expected: {1:yyyy-MM-dd HH:mm:ss.fff}{0}Actual: {2:yyyy-MM-dd HH:mm:ss.fff}", Environment.NewLine, expected, actual);
        }

        public static void IsEmpty<T>(IEnumerable<T> collection, string message = null) {
#if __MOBILE__
            Assert.AreEqual(0, collection.Count(), message ?? "Collection is not empty.");
#else
            if (!string.IsNullOrWhiteSpace(message))
                Assert.IsEmpty(collection, message);                
            else
                Assert.IsEmpty(collection);
#endif
        }

        public static void IsNotEmpty<T>(IEnumerable<T> collection, string message = null) {
#if __MOBILE__
            Assert.AreNotEqual(0, collection.Count(), message ??  "Collection is empty.");
#else
            if (!string.IsNullOrWhiteSpace(message))
                Assert.IsNotEmpty(collection, message);
            else
                Assert.IsNotEmpty(collection);

#endif

        }


        public static void Print<T>(IEnumerable<T> items) {
            foreach (var x in items.WithDescriptions()) {
                if (!x.Description.HasFlag(EnumeratedItemDescription.First))
                    Console.Write(", ");
                Console.Write(x.Item);
                if (x.Description.HasFlag(EnumeratedItemDescription.Last))
                    Console.WriteLine();
            }
        }
    }
}
