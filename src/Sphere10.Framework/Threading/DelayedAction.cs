//-----------------------------------------------------------------------
// <copyright file="DelayedAction.cs" company="Sphere 10 Software">
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

namespace Sphere10.Framework.Threading {
	public class DelayedAction {
		private readonly object _threadLock;
		private readonly Action _action;
		private readonly TimeSpan _waitDuration;
		private DateTime _lastPing;
		private WaitState _waitState;

		public DelayedAction(Action action, TimeSpan durationToWaitTillExecution) {
			_threadLock = new object();
			_action = action;
			_waitDuration = durationToWaitTillExecution;
			_waitState = WaitState.Idle;
		}
		
		public void Ping() {
			lock (_threadLock) {
				_lastPing = DateTime.Now;
				if (_waitState == WaitState.Idle) {
					_waitState = WaitState.Waiting;
					Tools.Threads.QueueAction(ExecuteAction);
				}
			}
		}

		private void ExecuteAction() {
			TimeSpan durationToSleep = TimeSpan.Zero;
			lock(_threadLock) {
				if (_lastPing.TimeElapsed() < _waitDuration) {
					durationToSleep = _waitDuration.Subtract(DateTime.Now.Subtract(_lastPing));
					if (durationToSleep.TotalMilliseconds < 0)
						durationToSleep = TimeSpan.Zero;
				}
			}

			if (durationToSleep == TimeSpan.Zero) {
				try {
					_action();
				} finally {
					_waitState = WaitState.Idle;
				}
			} else {
				System.Threading.Thread.Sleep(durationToSleep);
				Tools.Threads.QueueAction(ExecuteAction);
			}
		}

		private enum WaitState {
			Idle,
			Waiting,
		}
	}


}