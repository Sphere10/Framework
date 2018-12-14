//-----------------------------------------------------------------------
// <copyright file="CacheTests.cs" company="Sphere 10 Software">
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
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Sphere10.Windows;

namespace Sphere10.Framework.UnitTests {

    [TestFixture]
    public class CacheTests {

        [Test]
        public void BulkTest_Simple_1() {
            var cache = new BulkFetchActionCache<int, string>(
                () => new Dictionary<int, string>() {
                    {1, "one"}, {2, "two"}, {3, "three"}
                });
           Assert.AreEqual(new [] { "one", "two", "three"}, cache.GetAllCachedValues().ToArray());
        }

		[Test]
        public void ExpirationTest_Simple_1() {
            var val = "first";
            var cache = new ActionCache<int, string>(
                (x) => val,
                reapStrategy: CacheReapPolicy.LeastUsed,
                expirationStrategy: ExpirationPolicy.SinceFetchedTime,
                expirationDuration: TimeSpan.FromMilliseconds(100)
            );      
            Assert.AreEqual("first", cache[1]);
            val = "second";
            Assert.AreEqual("first", cache[1]);
            Assert.AreEqual("first", cache[1]);
            Thread.Sleep(111);          
            Assert.AreEqual("second", cache[1]);
            Assert.AreEqual("second", cache[1]);
            Assert.AreEqual("second", cache[1]);   
        }

        [Test]
        public void SizeTest_Simple_1() {
            var cache = new ActionCache<int, string>(
                (x) => x.ToString(),
                reapStrategy: CacheReapPolicy.LeastUsed,
                expirationStrategy: ExpirationPolicy.SinceFetchedTime,
                expirationDuration: TimeSpan.FromMilliseconds(100),
                sizeEstimator: uint.Parse,
                maxCapacity: 100
            );

            Assert.Throws<SoftwareException>(() => { var x = cache[101]; });
            Assert.AreEqual("98", cache[98]);
            Assert.AreEqual(1, cache.GetCachedItems().Count);
            Assert.AreEqual("2", cache[2]);
            Assert.AreEqual(2, cache.GetCachedItems().Count);
            Assert.AreEqual("1", cache[1]);
            Assert.AreEqual(2, cache.GetCachedItems().Count);  // should have purged first item
            Assert.AreEqual(new [] { "1", "2"}, cache.GetAllCachedValues().ToArray());
            Assert.AreEqual("100", cache[100]);
            Assert.AreEqual(1, cache.GetCachedItems().Count);  // should have purged everything 
        }

		[Test]
		public void ContainsCachedItem_1() {
			var called = false;
			var cache = new ActionCache<int, string>(
				(x) => {
					if (x != 1)
						throw new Exception("test only allows key with value 1");
					if (called)
						throw new Exception("item 1 has been requested more than once");
					called = true;
					return "value";
				},
				reapStrategy: CacheReapPolicy.LeastUsed,
				expirationStrategy: ExpirationPolicy.SinceFetchedTime,
				expirationDuration: TimeSpan.FromMilliseconds(100)
			);
			Assert.IsFalse(cache.ContainsCachedItem(1));
			var val = cache[1];
			Assert.IsTrue(cache.ContainsCachedItem(1));
			Thread.Sleep(111);
			Assert.IsFalse(cache.ContainsCachedItem(1));
		}
    }
}
