//-----------------------------------------------------------------------
// <copyright file="WebLogger.cs" company="Sphere 10 Software">
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

namespace Sphere10.Framework.Web {
	/// <summary>
	/// Provides web-wide logging functionality for a web application.
	/// </summary>
	/// <remarks></remarks>
	public class WebLogger : ILogger {
		public const string ApplicationKey = "CDA2B6EEE5EA48BEA4CB57930496AAF5_Logger";
		private static readonly ILogger _logger;

		public static ILogger GetWebLogger() {
			try {
				System.Web.HttpContext.Current.Application.Lock();
				var instance = System.Web.HttpContext.Current.Application[ApplicationKey];
				if (instance == null) {
					System.Web.HttpContext.Current.Application[ApplicationKey] = new TimestampLogger( new RollingFileLogger()); 
				}
				return (ILogger) System.Web.HttpContext.Current.Application[ApplicationKey];
			} finally {
				System.Web.HttpContext.Current.Application.UnLock();
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Object"/> class.
		/// </summary>
		/// <remarks></remarks>
		static WebLogger() {
			_logger = GetWebLogger();
		}

		/// <summary>
		/// Logs a debug message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="formatOptions">The format options (if any)</param>
		public static void LogDebug(string message, params object[] formatOptions) {
			_logger.Debug(message, formatOptions);
		}

		/// <summary>
		/// Logs an information message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="formatOptions">The format options (if any)</param>
		public static void LogInfo(string message, params object[] formatOptions) {
			_logger.Info(message, formatOptions);
		}

		/// <summary>
		/// Logs a warning message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="formatOptions">The format options (if any)</param>
		public static void LogWarning(string message, params object[] formatOptions) {
			_logger.Warning(message, formatOptions);
		}

		/// <summary>
		/// Logs an error message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="formatOptions">The format options (if any)</param>
		public static void LogError(string message, params object[] formatOptions) {
			_logger.Error(message, formatOptions);
		}

		public static void LogException(Exception exception) {
			_logger.LogException(exception);
		}


		/// <summary>
		/// The internal logger.
		/// </summary>
		public static ILogger Logger => _logger;


		#region  ILogger Implementation

		public LogOptions Options {
			get { return WebLogger.Logger.Options; }
			set { WebLogger.Logger.Options = value; }
		}

		public void Debug(string message, params object[] formatOptions) {
			global::Sphere10.Framework.Web.WebLogger.LogDebug(message, formatOptions);
		}

		public void Info(string message, params object[] formatOptions) {
			global::Sphere10.Framework.Web.WebLogger.LogInfo(message, formatOptions);
		}

		public void Warning(string message, params object[] formatOptions) {
			global::Sphere10.Framework.Web.WebLogger.LogWarning(message, formatOptions);
		}

		public void Error(string message, params object[] formatOptions) {
			global::Sphere10.Framework.Web.WebLogger.LogError(message, formatOptions);
		}

		#endregion
	}
}
