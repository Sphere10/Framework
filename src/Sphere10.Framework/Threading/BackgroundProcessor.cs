//-----------------------------------------------------------------------
// <copyright file="BackgroundProcessor.cs" company="Sphere 10 Software">
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

namespace Sphere10.Framework {
	class BackgroundProcessor {

		private readonly SynchronizedCollectionEx<Action> _queue;

		public BackgroundProcessor() : this(new Collection<Action>()) {	
		}

		public BackgroundProcessor(ICollection<Action> queue) {
			_queue = new SynchronizedCollectionEx<Action>( queue );
		}


		public void QueueForExecution(Action action) {
			using(_queue.EnterWriteScope()) {
				_queue.Add(action);
				if (_queue.Count == 1)
					Tools.Threads.QueueAction(RunActions);
				
			}
		}

		private void RunActions() {
			bool hasMore;
			do {
				var actionQueue = new Queue<Action>();
				using (_queue.EnterWriteScope()) {
					if (!_queue.Any())
						return;
					foreach(var action in _queue)
						actionQueue.Enqueue(action);
					_queue.Clear();
				}
				foreach (var action in actionQueue)
					action();
				using (_queue.EnterReadScope()) {
					hasMore = _queue.Count > 0;
				}
			} while (hasMore);
		}
	}
}
