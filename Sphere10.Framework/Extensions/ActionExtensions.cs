//-----------------------------------------------------------------------
// <copyright file="ActionExtensions.cs" company="Sphere 10 Software">
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
using System.Threading;

namespace Sphere10.Framework {
	public static class ActionExtensions {

		/// <summary>
		/// Wraps the action with a try/catch that ignores all exceptions.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <returns>Wrapped action</returns>
		public static Action IgnoringExceptions(this Action action) {
            return Tools.Lambda.ActionIgnoringExceptions(action);
		}

		/// <summary>
		/// Wraps the action with an exception handler.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <param name="exceptionHandler">The catch block.</param>
		/// <returns></returns>
		public static Action WithExceptionHandler(this Action action, Action<Exception> exceptionHandler) {
			return Tools.Lambda.ActionWithExceptionHandler(action, exceptionHandler);
		}


		/// <summary>
		/// Returns the action to run asyncronously (queued in the ThreadPool).
		/// </summary>
		/// <param name="action">The action</param>
		/// <returns>Wrapped action.</returns>
		public static Action AsAsyncronous(this Action action) {
			return Tools.Lambda.ActionAsAsyncronous(action);
		}

		/// <summary>
		/// Wraps an action with retry failover code.
		/// </summary>
		/// <param name="action">The action</param>
		/// <param name="retryCount">Number of attempts to retry upon failure</param>
		/// <param name="failAction">Action to execute when a failure occurs (e.g. could log, sleep, etc)</param>
		/// <param name="completedAction">Action to execute when action completes </param>
		/// <returns></returns>
		public static Action WithRetry(this Action action, int retryCount, Action<int, Exception> failAction = null, Action<int> completedAction = null) {
			return Tools.Lambda.ActionWithRetry(action, retryCount, failAction, completedAction);
		}


		/// <summary>
		/// Adds failover redundancy code to the action.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <param name="decideRetry">Functor to decide whether or not to retry. Parameters are attempt, Exception and returns true/false.</param>
		/// <param name="attempt">The attempt.</param>
		/// <returns>The given action wrapped with failover code.</returns>
		public static Action WithFailOver(this Action action, Func<int, Exception, bool> decideRetry, Action<int> completedAction = null, int attempt = 1) {
			return Tools.Lambda.ActionWithFailOver(action, decideRetry, completedAction, attempt);
		}


	}
}
