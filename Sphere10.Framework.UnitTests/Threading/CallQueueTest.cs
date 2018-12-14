//-----------------------------------------------------------------------
// <copyright file="CallQueueTest.cs" company="Sphere 10 Software">
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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.IO;
using System.Threading;
using Sphere10.Framework;
using Sphere10.Framework.Maths.Compiler;
using Sphere10.Framework.Threading;

namespace Sphere10.Framework.UnitTests {

    [TestFixture]
    public class CallQueueTest {

        [Test]
        public void Single() {
            var result = 0;
            var callQueue = new CallQueue();
            using (callQueue.BlockUntil(() => true)) {
                result = 1;
            }
            Assert.AreEqual(1, result);
        }

        [Test]
        public void TwoThreads() {
            int state = 0;
            var callQueue = new CallQueue();
            var result = new List<int>();
            var start = false;
            Action<int, Func<bool>> concurrentAction = (int x, Func<bool> pred) => {
                using (callQueue.BlockUntil(pred)) {
                    result.Add(x);
                }
            };
            var thread1 = new Thread(() => concurrentAction(1, () => Volatile.Read(ref start)));
            var thread2 = new Thread(() => concurrentAction(2, () => true));
            
            thread1.Start();
            Thread.Sleep(1);
            thread2.Start();
            Thread.Sleep(100);

            Volatile.Write(ref start, true);
            callQueue.Pulse();
            Thread.Sleep(200);
            Assert.AreEqual(new int[] {1,2}, result.ToArray());
        }

        
        
        [Test]
        public void PulseInMiddle() {
            var callQueue = new CallQueue();
            var result = new List<int>();
            var start = false;    
            Action<int, Func<bool>> concurrentAction = (int x, Func<bool> pred) => {
                using (callQueue.BlockUntil(pred)) {
                    Thread.Sleep(x*10);
                    result.Add(x);
                    if (x == 2)
                        Volatile.Write(ref start, false);
                }
            };
            var thread1 = new Thread(() => concurrentAction(1, () => Volatile.Read(ref start)));
            var thread2 = new Thread(() => concurrentAction(2, () => true));
            var thread3 = new Thread(() => concurrentAction(3, () => Volatile.Read(ref start)));
            var thread4 = new Thread(() => concurrentAction(4, () => true));

            thread1.Start();
            Thread.Sleep(1);
            thread2.Start();
            Thread.Sleep(1);
            thread3.Start();
            Thread.Sleep(1);
            thread4.Start();
            Thread.Sleep(1);

            // Everything should be blocked
            Thread.Sleep(100);
            Assert.AreEqual(Enumerable.Empty<int>(), result);
            
            // Allow first two threads in
            Volatile.Write(ref start, true);            
            callQueue.Pulse();
            Thread.Sleep(100);
            Assert.AreEqual(new[] { 1, 2 }, result.ToArray());
            
            // Allow last two threads in
            Volatile.Write(ref start, true);
            callQueue.Pulse();
            Thread.Sleep(100);
            Assert.AreEqual(new[] { 1, 2, 3, 4 }, result.ToArray());

        }

        
        [Test]
        public void MultipleThreads() {
            int state = 0;
            var callQueue = new CallQueue();
            var result = new List<int>();
            var start = false;    
            Action<int, Func<bool>> concurrentAction = (int x, Func<bool> pred) => {
                using (callQueue.BlockUntil(pred)) {
                    Thread.Sleep(x);
                    result.Add(x);
                }
            };
            var threads = new Thread[100];
            for (var i = 0; i < 100; i++) {
                if (i == 0) {
                    var i1 = i;
                    threads[i] = new Thread(() => concurrentAction(i1 + 1, () => Volatile.Read(ref start)));
                } else {
                    var i1 = i;
                    threads[i] = new Thread(() => concurrentAction(i1 + 1, () => true));
                }
            }

            foreach (var thread in threads) {
                thread.Start();
                Thread.Sleep(1);
            }

            Thread.Sleep(100);
            Assert.AreEqual(Enumerable.Empty<int>(), result);


            Volatile.Write(ref start, true);
            callQueue.Pulse();
            while (result.Count < 100) {
                Thread.Sleep(100);
            }
            Tools.NUnitTool.Print(result);
            Assert.AreEqual(Enumerable.Range(1,100) , result);
        }
    }

}
