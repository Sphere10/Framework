//-----------------------------------------------------------------------
// <copyright file="LargeCollectionTests.cs" company="Sphere 10 Software">
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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;
using NUnit.Framework.Constraints;
using Sphere10.Framework;
using Sphere10.Framework.Maths.Compiler;

namespace Sphere10.Framework.UnitTests {

    [TestFixture]
    public class LargeCollectionTests {

        [Test]
        public void TestSinglePage() {
            using (var collection = new LargeCollection<string>(100, str => str.Length*sizeof (char))) {

                collection.Add("01234567890123456789012345678901234567890123456789");

                var pages = collection.Pages.ToArray();
                Assert.AreEqual(1, pages.Length);
                Assert.AreEqual(0, pages[0].StartIndex);
                Assert.AreEqual(0, pages[0].EndIndex);
                Assert.AreEqual(1, pages[0].Count);
                Assert.AreEqual(100, pages[0].Size);
            }
        }

        [Test]
        public void TestSinglePage2() {
            using (var collection = new LargeCollection<string>(100, str => str.Length*sizeof (char))) {

                collection.Add("0123456789");
                collection.Add("0123456789");
                collection.Add("0123456789");
                collection.Add("0123456789");
                collection.Add("0123456789");


                Assert.AreEqual(5, collection.Count);
                var pages = collection.Pages.ToArray();
                Assert.AreEqual(1, pages.Length);

                // Page 0
                Assert.AreEqual(0, pages[0].PageNumber);

                Assert.AreEqual(0, pages[0].StartIndex);
                Assert.AreEqual(4, pages[0].EndIndex);
                Assert.AreEqual(5, pages[0].Count);
                Assert.AreEqual(100, pages[0].Size);
            }
        }


        [Test]
        public void TestTwoPages1() {
            var pageLoads = new List<int>();
            var pageUnloads = new List<int>();
            using (var collection = new LargeCollection<string>(100, str => str.Length*sizeof (char))) {
                collection.PageLoaded += (largeCollection, page) => pageLoads.Add(page.PageNumber);
                collection.PageUnloaded += (largeCollection, page) => pageUnloads.Add(page.PageNumber);
                collection.Add("01234567890123456789012345678901234567890123456789");
                collection.Add("0123456789012345678901234567890123456789012345678");

                Assert.AreEqual(2, collection.Count);
                var pages = collection.Pages.ToArray();
                Assert.AreEqual(2, pages.Length);

                // Page 0
                Assert.AreEqual(0, pages[0].PageNumber);
                Assert.AreEqual(0, pages[0].StartIndex);
                Assert.AreEqual(0, pages[0].EndIndex);
                Assert.AreEqual(1, pages[0].Count);
                Assert.AreEqual(100, pages[0].Size);

                // Page 1
                Assert.AreEqual(1, pages[1].PageNumber);
                Assert.AreEqual(1, pages[1].StartIndex);
                Assert.AreEqual(1, pages[1].EndIndex);
                Assert.AreEqual(1, pages[1].Count);
                Assert.AreEqual(98, pages[1].Size);

                // Page Swaps
                Assert.AreEqual(1, pageLoads.Count);
                Assert.AreEqual(1, pageUnloads.Count);

                Assert.AreEqual(0, pageUnloads[0]);
                Assert.AreEqual(1, pageLoads[0]);
            }
        }


        [Test]
        public void TestTwoPages2() {
            var pageLoads = new List<int>();
            var pageUnloads = new List<int>();
            using (var collection = new LargeCollection<string>(100, str => str.Length * sizeof(char))) {
                collection.PageLoaded += (largeCollection, page) => pageLoads.Add(page.PageNumber);
                collection.PageUnloaded += (largeCollection, page) => pageUnloads.Add(page.PageNumber);
                collection.Add("0123456789");
                collection.Add("0123456789");
                collection.Add("0123456789");
                collection.Add("0123456789");
                collection.Add("0123456789");
                collection.Add("0123456789");
                collection.Add("0123456789");
                collection.Add("0123456789");
                collection.Add("0123456789");
                collection.Add("012345678");

                Assert.AreEqual(10, collection.Count);
                var pages = collection.Pages.ToArray();
                Assert.AreEqual(2, pages.Length);

                // Page 0
                Assert.AreEqual(0, pages[0].PageNumber);
                Assert.AreEqual(0, pages[0].StartIndex);
                Assert.AreEqual(4, pages[0].EndIndex);
                Assert.AreEqual(5, pages[0].Count);
                Assert.AreEqual(100, pages[0].Size);

                // Page 1
                Assert.AreEqual(1, pages[1].PageNumber);
                Assert.AreEqual(5, pages[1].StartIndex);
                Assert.AreEqual(9, pages[1].EndIndex);
                Assert.AreEqual(5, pages[1].Count);
                Assert.AreEqual(98, pages[1].Size);


                // Page Swaps
                Assert.AreEqual(1, pageLoads.Count);
                Assert.AreEqual(1, pageUnloads.Count);
                Assert.AreEqual(0, pageUnloads[0]);
                Assert.AreEqual(1, pageLoads[0]);
            }
        }


        [Test]
        public void TestPageSwaps() {
            var pageLoads = new List<int>();
            var pageUnloads = new List<int>();
            using (var collection = new LargeCollection<string>(40, str => str.Length * sizeof(char))) {
                collection.PageLoaded += (largeCollection, page) => pageLoads.Add(page.PageNumber);
                collection.PageUnloaded += (largeCollection, page) => pageUnloads.Add(page.PageNumber);
                collection.Add("012345"); // page 0
                collection.Add("6789"); // page 0
                collection.Add("01"); // page 0
                collection.Add("2345678"); // page 0  (38 bytes)
                collection.Add("0123456789"); // page 1
                collection.Add("0123456789"); // page 1
                collection.Add("0123456789"); // page 2
                collection.Add("0123456789"); // page 2
                collection.Add("0123456789"); // page 3
                collection.Add("0123456789"); // page 3
                collection.Add("0123456789"); // page 4
                collection.Add("0123456789"); // page 4

                for (int i = 0; i < collection.Count; i++) {
                    var item = collection[i];
                }

                // Page Swaps - should be 9, 4 on create, 5 on access
                Assert.AreEqual(9, pageUnloads.Count);
                Assert.AreEqual(9, pageLoads.Count);
                Assert.AreEqual(0, pageUnloads[0]);
                Assert.AreEqual(1, pageLoads[0]);
                Assert.AreEqual(1, pageUnloads[1]);
                Assert.AreEqual(2, pageLoads[1]);
                Assert.AreEqual(2, pageUnloads[2]);
                Assert.AreEqual(3, pageLoads[2]);
                Assert.AreEqual(3, pageUnloads[3]);
                Assert.AreEqual(4, pageLoads[3]);
                Assert.AreEqual(4, pageUnloads[4]);
                Assert.AreEqual(0, pageLoads[4]);
                Assert.AreEqual(0, pageUnloads[5]);
                Assert.AreEqual(1, pageLoads[5]);
                Assert.AreEqual(1, pageUnloads[6]);
                Assert.AreEqual(2, pageLoads[6]);
                Assert.AreEqual(2, pageUnloads[7]);
                Assert.AreEqual(3, pageLoads[7]);
                Assert.AreEqual(3, pageUnloads[8]);
                Assert.AreEqual(4, pageLoads[8]);
            }
        }



        [Test]
        public void TestEmpty() {
            using (var collection = new LargeCollection<string>(40, str => str.Length * sizeof(char))) {
                Assert.AreEqual(1, collection.PageCount);
                Assert.AreEqual(0, collection.Count);
            }
        }

        [Test]
        public void TestEmptyItems() {
            using (var collection = new LargeCollection<string>(1, str => str.Length * sizeof(char))) {
                collection.Add("");
                collection.Add("");
                collection.Add("");
                Assert.AreEqual(1, collection.PageCount);
                Assert.AreEqual(3, collection.Count);
            }
        }


        [Test]
        public void TestIteratorEmpty() {
            using (var collection = new LargeCollection<string>(40, str => str.Length*sizeof (char))) {
                foreach (var item in collection) {
                    var xxx = 1;
                }
            }
        }

        [Test]
        public void TestIterator1() {
            var data = new [] { "012345", "6789", "01", "2345678", "0123456789", "0123456789", "0123456789", "0123456789", "0123456789", "0123456789", "0123456789", "0123456789" };
            var pageLoads = new List<int>();
            var pageUnloads = new List<int>();
            using (var collection = new LargeCollection<string>(40, str => str.Length * sizeof(char))) {
                collection.PageLoaded += (largeCollection, page) => pageLoads.Add(page.PageNumber);
                collection.PageUnloaded += (largeCollection, page) => pageUnloads.Add(page.PageNumber);
                collection.Add(data[0]); // page 0
                collection.Add(data[1]); // page 0
                collection.Add(data[2]); // page 0
                collection.Add(data[3]); // page 0  (38 bytes)
                collection.Add(data[4]); // page 1
                collection.Add(data[5]); // page 1
                collection.Add(data[6]); // page 2
                collection.Add(data[7]); // page 2
                collection.Add(data[8]); // page 3
                collection.Add(data[9]); // page 3
                collection.Add(data[10]); // page 4
                collection.Add(data[11]); // page 4

                foreach (var item in collection.WithDescriptions()) {
                    Assert.AreEqual(item.Item, data[item.Index]);
                }

                // Page Swaps - should be 9, 4 on create, 5 on access
                Assert.AreEqual(9, pageUnloads.Count);
                Assert.AreEqual(9, pageLoads.Count);
                Assert.AreEqual(0, pageUnloads[0]);
                Assert.AreEqual(1, pageLoads[0]);
                Assert.AreEqual(1, pageUnloads[1]);
                Assert.AreEqual(2, pageLoads[1]);
                Assert.AreEqual(2, pageUnloads[2]);
                Assert.AreEqual(3, pageLoads[2]);
                Assert.AreEqual(3, pageUnloads[3]);
                Assert.AreEqual(4, pageLoads[3]);
                Assert.AreEqual(4, pageUnloads[4]);
                Assert.AreEqual(0, pageLoads[4]);
                Assert.AreEqual(0, pageUnloads[5]);
                Assert.AreEqual(1, pageLoads[5]);
                Assert.AreEqual(1, pageUnloads[6]);
				Assert.AreEqual(2, pageLoads[6]);
                Assert.AreEqual(2, pageUnloads[7]);
                Assert.AreEqual(3, pageLoads[7]);
                Assert.AreEqual(3, pageUnloads[8]);
                Assert.AreEqual(4, pageLoads[8]);
            }
        }


        [Test]
        public void TestIterator2() {
            var data = new[] { "012345", "6789", "01", "2345678", "0123456789", "0123456789", "0123456789", "0123456789", "0123456789", "0123456789", "0123456789", "0123456789" };
            var pageLoads = new List<int>();
            var pageUnloads = new List<int>();
            using (var collection = new LargeCollection<string>(40, str => str.Length * sizeof(char))) {
                collection.PageLoaded += (largeCollection, page) => pageLoads.Add(page.PageNumber);
                collection.PageUnloaded += (largeCollection, page) => pageUnloads.Add(page.PageNumber);
                collection.Add(data[0]); // page 0
                collection.Add(data[1]); // page 0
                collection.Add(data[2]); // page 0
                collection.Add(data[3]); // page 0  (38 bytes)
                collection.Add(data[4]); // page 1
                collection.Add(data[5]); // page 1
                collection.Add(data[6]); // page 2
                collection.Add(data[7]); // page 2
                collection.Add(data[8]); // page 3
                collection.Add(data[9]); // page 3
                collection.Add(data[10]); // page 4
                collection.Add(data[11]); // page 4

                foreach (var item in collection.WithDescriptions()) {
                    Assert.AreEqual(item.Item, data[item.Index]);
                }

                foreach (var item in collection.WithDescriptions()) {
                    Assert.AreEqual(item.Item, data[item.Index]);
                }

                // Page Swaps - should be 14, 4 on create, 10 on access
                Assert.AreEqual(14, pageUnloads.Count);
                Assert.AreEqual(14, pageLoads.Count);
                Assert.AreEqual(0, pageUnloads[0]);
                Assert.AreEqual(1, pageLoads[0]);
                Assert.AreEqual(1, pageUnloads[1]);
                Assert.AreEqual(2, pageLoads[1]);
                Assert.AreEqual(2, pageUnloads[2]);
                Assert.AreEqual(3, pageLoads[2]);
                Assert.AreEqual(3, pageUnloads[3]);
                Assert.AreEqual(4, pageLoads[3]);
                Assert.AreEqual(4, pageUnloads[4]);
                Assert.AreEqual(0, pageLoads[4]);
                Assert.AreEqual(0, pageUnloads[5]);
                Assert.AreEqual(1, pageLoads[5]);
                Assert.AreEqual(1, pageUnloads[6]);
                Assert.AreEqual(2, pageLoads[6]);
                Assert.AreEqual(2, pageUnloads[7]);
                Assert.AreEqual(3, pageLoads[7]);
                Assert.AreEqual(3, pageUnloads[8]);
                Assert.AreEqual(4, pageLoads[8]);
                Assert.AreEqual(4, pageUnloads[4]);
                Assert.AreEqual(0, pageLoads[4]);
                Assert.AreEqual(0, pageUnloads[5]);
                Assert.AreEqual(1, pageLoads[5]);
                Assert.AreEqual(1, pageUnloads[6]);
                Assert.AreEqual(2, pageLoads[6]);
                Assert.AreEqual(2, pageUnloads[7]);
                Assert.AreEqual(3, pageLoads[7]);
                Assert.AreEqual(3, pageUnloads[8]);
                Assert.AreEqual(4, pageLoads[8]);
            }
        }


        [Test]
        public void TestIteratorOverChangedCollection() {
            using (var collection = new LargeCollection<string>(40, str => str.Length*sizeof (char))) {
                collection.Add("10");
                var thrown = false;
                try {
                    foreach (var item in collection) {
                        var xxx = 1;
                        collection.Add("20");
                    }
                } catch (Exception error) {
                    thrown = true;
                }
                Assert.IsTrue(thrown, "Exception was not thrown");
            }
        }

        [Test]
        public void TestRandomAccess() {
            using (var collection = new LargeCollection<string>(50000, str => str.Length*sizeof (char))) {
                collection.PageLoaded += (largeCollection, page) => {
                    System.Console.WriteLine("Page Loaded: {0}\t\t{1}", page.PageNumber, largeCollection.PageCount);
                };
                collection.PageUnloaded += (largeCollection, page) => {
                    System.Console.WriteLine("Page Unloaded: {0}\t\t{1}", page.PageNumber, largeCollection.PageCount);
                };
                for (var i = 0; i < 10000; i++) {
                    collection.Add(Tools.Text.GenerateRandomString(Tools.Maths.RandomNumberGenerator.Next(0, 100)));
                }

                Assert.AreEqual(10000, collection.Count);

                for (var i = 0; i < 300; i++) {
                    var str = collection[Tools.Maths.RandomNumberGenerator.Next(0, 10000 - 1)];
                }
            }
        }



        [Test]
        public void TestGrowWhilstRandomAccess() {
            using (var collection = new LargeCollection<string>(5000, str => str.Length*sizeof (char))) {
                collection.PageLoaded += (largeCollection, page) => {
                    System.Console.WriteLine("Page Loaded: {0}\t\t{1}", page.PageNumber, largeCollection.PageCount);
                };
                collection.PageUnloaded += (largeCollection, page) => {
                    System.Console.WriteLine("Page Unloaded: {0}\t\t{1}", page.PageNumber, largeCollection.PageCount);
                };


                for (var i = 0; i < 100; i++) {
                    collection.Add(Tools.Text.GenerateRandomString(Tools.Maths.RandomNumberGenerator.Next(100, 1000)));
                    for (var j = 0; j < 3; j++) {
                        var str = collection[Tools.Maths.RandomNumberGenerator.Next(0, collection.Count - 1)];
                    }
                }
            }
        }

        [Test]
        public void TestLinq() {
            using (var collection = new LargeCollection<string>(50000, str => str.Length*sizeof (char))) {
                collection.PageLoaded += (largeCollection, page) => {
                    System.Console.WriteLine("Page Loaded: {0}\t\t{1}", page.PageNumber, largeCollection.PageCount);
                };
                collection.PageUnloaded += (largeCollection, page) => {
                    System.Console.WriteLine("Page Unloaded: {0}\t\t{1}", page.PageNumber, largeCollection.PageCount);
                };

                for (int i = 0; i < 100000; i++) {
                    collection.Add(i.ToString());
                }
                var xxx = collection
                    .Where(s => s.StartsWith("1"))
                    .Union(collection.Where(s => s.StartsWith("2")))
                    .Reverse();

                foreach (var val in xxx) {
                    Assert.IsTrue(val.StartsWith("1") || val.StartsWith("2"));
                }

            }

            
        }



    }

}
