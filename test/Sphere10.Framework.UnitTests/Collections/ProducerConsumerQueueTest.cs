//-----------------------------------------------------------------------
// <copyright file="ProducerConsumerQueueTest.cs" company="Sphere 10 Software">
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
using System.Text;
using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Sphere10.Framework;
using Sphere10.Framework.Maths.Compiler;

namespace Sphere10.Framework.UnitTests {

    [TestFixture]
    public class ProducerConsumerQueueTest {

        [Test]
        public async Task Simple() {
            using (var queue = new ProducerConsumerQueue<string>(10)) {
                await queue.PutAsync("Hello World!");
                Assert.AreEqual(1, queue.Count);
                var r = await queue.TakeManyAsync(1);
                Assert.AreEqual(0, queue.Count);
                Assert.AreEqual(1, r.Length);
                Assert.AreEqual("Hello World!", r[0]);
            }
        }
        [Test]
        public async Task Complex_1() {
            var expected = Enumerable.Range(0, 1000).ToArray();
            var result = new List<int>();
            var @lock = new object();
            var counter = 0;

            var putManyCounter = 0;
            var takeManyCounter = 0;

            using (var queue = new ProducerConsumerQueue<int>(10)) {

                Func<string, Task> produceAction = async (string name) => {
                    while (counter < 1000) {
                        //await Task.Delay(10);
                        var localProduction = new List<int>();
                        lock (@lock) {
                            var numToProduce = Tools.Maths.RandomNumberGenerator.Next(1, 10);
                            for (var i = 0; i < numToProduce; i++) {
                                if (counter == 1000)
                                    break;
                                localProduction.Add(counter++);
                            }
                        }
                        await queue.PutManyAsync(localProduction);
                    }
                };

                Func<string, Task> consumeAction = async (string name) => {
                    while (!queue.HasFinishedProducing) {
                        //await Task.Delay(10);
                        var numToConsume = Tools.Maths.RandomNumberGenerator.Next(1, 10);
                        var consumption = await queue.TakeManyAsync(numToConsume);
                        result.AddRange(consumption);
                    }
                };

                Func<Task> produceTask = async () => {
                    await Task.WhenAll(produceAction("Producer 1"));
                    queue.FinishedProducing();
                };

                Func<Task> consumeTask = async () => {
                    await Task.WhenAll(consumeAction("Consumer 1"));
                    queue.FinishedConsuming();
                };

                await Task.WhenAll(produceTask(), consumeTask());
                Tools.NUnitTool.Print(result);
                Assert.AreEqual(expected, result);

            }
        }


        [Test]
        public async Task Complex_2() {
            var expected = Enumerable.Range(0, 1000).ToArray();
            var result = new List<int>();
            var @lock = new object();
            var counter = 0;

            using (var queue = new ProducerConsumerQueue<int>(10)) {

                Func<string, Task> produceAction = async (string name) => {
                    while (counter < 1000) {
                        //await Task.Delay(10);
                        var localProduction = new List<int>();
                        lock (@lock) {
                            var numToProduce = Tools.Maths.RandomNumberGenerator.Next(1, 10);
                            for (var i = 0; i < numToProduce; i++) {
                                if (counter == 1000)
                                    break;
                                localProduction.Add(counter++);
                            }
                        }
                        await queue.PutManyAsync(localProduction);
                    }
                };

                Func<string, Task> consumeAction = async (string name) => {
                    while (!queue.HasFinishedProducing) {
                        //await Task.Delay(10);
                        var numToConsume = Tools.Maths.RandomNumberGenerator.Next(1, 10);
                        var consumption = await queue.TakeManyAsync(numToConsume);
                        result.AddRange(consumption);
                    }
                };

                var producers = new[] {
                    produceAction("Producer 1"),
                    produceAction("Producer 2"),
                    produceAction("Producer 3"),
                    produceAction("Producer 4"),
                    produceAction("Producer 5"),
                };

                var consumers = new[] {
                    consumeAction("Consumer 1"),
                    consumeAction("Consumer 2"),
                    consumeAction("Consumer 3"),         
                };

                Func<Task> produceTask = async () => {
                    await Task.WhenAll(producers);
                    queue.FinishedProducing();
                };

                Func<Task> consumeTask = async () => {
                    await Task.WhenAll(consumers);
                    queue.FinishedConsuming();
                };

                await Task.WhenAll(produceTask(), consumeTask());

                Tools.NUnitTool.Print(result);
                Assert.AreEqual(expected, result.OrderBy(x => x).ToArray());

            }
        }

    }

}
