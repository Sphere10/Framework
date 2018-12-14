//-----------------------------------------------------------------------
// <copyright file="DecoratedLogger.cs" company="Sphere 10 Software">
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

namespace Sphere10.Framework {
	public class DecoratedLogger : ILogger {

		private readonly ILogger _decoratedLogger;

		public DecoratedLogger(ILogger decoratedLogger) {
			_decoratedLogger = decoratedLogger;
		}

        public LogOptions Options { get { return _decoratedLogger.Options; } set { _decoratedLogger.Options = value; } }

	    public virtual void Debug(string message, params object[] formatOptions) {
			_decoratedLogger.Debug(message, formatOptions);	
		}

		public virtual void Info(string message, params object[] formatOptions) {
			_decoratedLogger.Info(message, formatOptions);
		}

		public virtual void Warning(string message, params object[] formatOptions) {
			_decoratedLogger.Warning(message, formatOptions);
		}

		public virtual void Error(string message, params object[] formatOptions) {
			_decoratedLogger.Error(message, formatOptions);
		}
	}
}
