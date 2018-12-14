//-----------------------------------------------------------------------
// <copyright file="SessionCacheTests.cs" company="Sphere 10 Software">
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
using NUnit.Framework;

namespace Sphere10.Framework.UnitTests {

    [TestFixture]
    public class SessionCacheTests {

        [Test]
        public void Simple_1() {
            var cache = new SessionCache<int, string>(TimeSpan.FromMilliseconds(100));
            cache.Set(1, "one");
            Thread.Sleep(300);
            Assert.AreEqual(0, cache.GetCachedItems().Count);
        }

        [Test]
        public void Simple_2() {
            var cache = new SessionCache<int, string>(TimeSpan.FromMilliseconds(100));
            cache.Set(1, "one");
            cache.Set(2, "two");
            cache.Set(3, "three");
            cache.Set(4, "four");
            cache.Set(5, "five");
            cache.Set(6, "six");
            Assert.AreEqual(6, cache.GetCachedItems().Count);
            Thread.Sleep(300);
            Assert.AreEqual(0, cache.GetCachedItems().Count);
        }


        [Test]
        public void Complex_1() {
            var cache = new SessionCache<int, string>(TimeSpan.FromMilliseconds(150));
            cache.Set(1, "one");
            cache.Set(2, "two");
            cache.Set(3, "three");
            cache.Set(4, "four");
            cache.Set(5, "five");
            cache.Set(6, "six");
            Assert.AreEqual(6, cache.GetCachedItems().Count);
            for (int i = 0; i < 6; i++) {
                Thread.Sleep(100);
                cache.KeepAlive(1);
                cache.KeepAlive(2);
                cache.KeepAlive(3);
            }
            var remaining = cache.GetCachedItems();
            Assert.AreEqual(3, remaining.Count);
            Assert.IsTrue(remaining.ContainsKey(1));
            Assert.IsTrue(remaining.ContainsKey(2));
            Assert.IsTrue(remaining.ContainsKey(3));
        }

        [Test]
        public void Complex_2() {
            var cache = new SessionCache<int, string>(TimeSpan.FromMilliseconds(150));
            cache.Set(1, "one");
            cache.Set(2, "two");
            cache.Set(3, "three");
            cache.Set(4, "four");
            cache.Set(5, "five");
            cache.Set(6, "six");
            Assert.AreEqual(6, cache.GetCachedItems().Count);
            for (int i = 0; i < 6; i++) {
                Thread.Sleep(100);
                cache.KeepAlive(1);
                cache.KeepAlive(2);
                cache.KeepAlive(3);
            }
            cache.Remove(2);
            cache.Remove(3);
            var remaining = cache.GetCachedItems();
            Assert.AreEqual(1, remaining.Count);
            Assert.IsTrue(remaining.ContainsKey(1));
        }


        [Test]
        public void SessionDisposed() {
            var disposed = false;
            var cache = new SessionCache<int, IDisposable>(TimeSpan.FromMilliseconds(100));
            cache.ItemRemoved += (i, item) => item.Dispose();
            cache.Set(1, Tools.Scope.ExecuteOnDispose(() => disposed = true));
            Thread.Sleep(300);
            Assert.IsTrue(disposed);
        }

        [Test]
        public void SessionDisposedOnFlush() {
            var disposed = false;
            var cache = new SessionCache<int, IDisposable>(TimeSpan.FromMilliseconds(100));
            cache.ItemRemoved += (i, item) => item.Dispose();
            cache.Set(1, Tools.Scope.ExecuteOnDispose(() => disposed = true));
            cache.Flush();
            Assert.IsTrue(disposed);
        }


        [Test]
        public void Throws_1() {
            var cache = new SessionCache<int, string>(TimeSpan.FromMilliseconds(100));
            cache.Set(1, "one");
            Assert.AreEqual("one", cache[1]);
            Thread.Sleep(300);
            Assert.Throws<Exception>(() => {
                var x = cache[1];
            } );
        }


        [Test]
        public void DoesNotExpire() {
            var cache = new SessionCache<int, string>(TimeSpan.FromMilliseconds(100));
            cache.Set(1, "one");
            DateTime start = DateTime.Now;
            while (DateTime.Now.Subtract(start).TotalSeconds <= 1.0D) {
                Assert.AreEqual("one", cache[1]);
                System.Threading.Thread.Sleep(50);
            }
            Assert.AreEqual("one", cache[1]);

        }

    }
}
