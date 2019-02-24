//-----------------------------------------------------------------------
// <copyright file="TextWriterLogger.cs" company="Sphere 10 Software">
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

namespace Sphere10.Framework {


	/// <summary>
	/// Logger which simply appends a file.
	/// </summary>
	/// <remarks></remarks>
	public class TextWriterLogger : ILogger {

		private readonly TextWriter _writer;


		/// <summary>
		/// Can enable/disable logging levels
		/// </summary>
		public LogOptions Options { get; set; }

	    /// <summary>
	    /// Initializes a new instance of the <see cref="T:System.Object"/> class.
	    /// </summary>
	    /// <remarks></remarks>
	    public TextWriterLogger()
			: this(new DebugTextWriter()) {
		}

	    /// <summary>
	    /// Initializes a new instance of the <see cref="TextWriterLogger"/> class.
	    /// </summary>
	    /// <param name="writer">The writer.</param>
	    /// <remarks></remarks>
	    public TextWriterLogger(TextWriter writer) {
		    _writer = writer;
		    this.Options = LogOptions.DebugBuildDefaults;
		}

		/// <summary>
		/// Logs a debug message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="formatOptions">The format options (if any)</param>
		/// <remarks></remarks>
		public void Debug(string message, params object[] formatOptions) {
			if (Options.HasFlag(LogOptions.DebugEnabled)) {
                LogMessage(_writer, message, formatOptions);
			}
		}

		/// <summary>
		/// Logs an information message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="formatOptions">The format options (if any)</param>
		/// <remarks></remarks>
		public void Info(string message, params object[] formatOptions) {
		    if (Options.HasFlag(LogOptions.InfoEnabled)) {
                LogMessage(_writer, message, formatOptions);
		    }
		}

		/// <summary>
		/// Logs a warning message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="formatOptions">The format options (if any)</param>
		/// <remarks></remarks>
		public void Warning(string message, params object[] formatOptions) {
		    if (Options.HasFlag(LogOptions.WarningEnabled)) {
                LogMessage(_writer, message, formatOptions);
		    }
		}

		/// <summary>
		/// Logs an error message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="formatOptions">The format options (if any)</param>
		/// <remarks></remarks>
		public void Error(string message, params object[] formatOptions) {
		    if (Options.HasFlag(LogOptions.ErrorEnabled)) {
                LogMessage(_writer, message, formatOptions);
		    }
		}

	    /// <summary>
	    /// Logs the message.
	    /// </summary>
	    /// <param name="writer">The writer.</param>
	    /// <param name="message">The message.</param>
	    /// <param name="formatOptions">The format options.</param>
	    /// <remarks></remarks>
	    protected virtual void LogMessage(TextWriter writer, string message, params object[] formatOptions) {
	        try {
	            writer.WriteLine(formatOptions == null ? message : string.Format(message, formatOptions));
	        } catch (Exception error) {
	            // errors do not propagate outside logging framework
	        }
	    }

	}
}
