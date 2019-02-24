//-----------------------------------------------------------------------
// <copyright file="ILogger.cs" company="Sphere 10 Software">
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

namespace Sphere10.Framework{

	/// <summary>
	/// Loggers are used to process debug, information, warning and error messages.
	/// </summary>
	/// <remarks></remarks>
	public interface ILogger {

        LogOptions Options { get; set; }

		/// <summary>
		/// Logs a debug message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="formatOptions">The format options (if any)</param>
		void Debug(string message, params object[] formatOptions);

		/// <summary>
		/// Logs an information message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="formatOptions">The format options (if any)</param>
		void Info(string message, params object[] formatOptions);

		/// <summary>
		/// Logs a warning message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="formatOptions">The format options (if any)</param>
		void Warning(string message, params object[] formatOptions);

		/// <summary>
		/// Logs an error message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="formatOptions">The format options (if any)</param>
		void Error(string message, params object[] formatOptions);

	}

	public static class ILoggerExtensions {

		public static void LogException(this ILogger logger, Exception exception) {
			logger.Error(exception.ToDiagnosticString());
		}

        public static IDisposable LogDuration(this ILogger logger, string messagePrefix) {
            var start = DateTime.Now;
            return new ActionScope(
                Tools.Lambda.NoOp,
                () => logger.Debug("{0} ({1} ms)", messagePrefix ?? string.Empty, (long)DateTime.Now.Subtract(start).TotalMilliseconds)
            );
        }
	}
}
