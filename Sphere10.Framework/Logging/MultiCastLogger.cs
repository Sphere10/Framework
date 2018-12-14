//-----------------------------------------------------------------------
// <copyright file="MultiCastLogger.cs" company="Sphere 10 Software">
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
using System.Threading.Tasks;

namespace Sphere10.Framework{

	public class MulticastLogger : ILogger {

		private readonly SyncronizedList<ILogger> _loggers;

		public MulticastLogger()
			: this(new List<ILogger>()) {
		    Options = LogOptions.DebugBuildDefaults;
		}


	    public LogOptions Options {
	        get { throw new NotSupportedException("Options can only be set in MultiCastLogger"); }
	        set {
	            using (_loggers.EnterWriteScope()) {
	                _loggers.ForEach(l => l.Options = value);
	            }
	        }
	    }

	    public MulticastLogger(params ILogger[] loggers)
			: this(new List<ILogger>(loggers)) {
		}

		public MulticastLogger(IEnumerable<ILogger> loggers)
			: this(new List<ILogger>(loggers)) {
		}

		public MulticastLogger(IList<ILogger> loggers) {
			_loggers = new SyncronizedList<ILogger>();
			_loggers.AddRange(loggers);
		}

		public void Add(ILogger logger) {
			_loggers.Add(logger);
		}

		public bool Remove(ILogger logger) {
			return _loggers.Remove(logger);
		}

		public void Clear() {
			_loggers.Clear();
		}


	    public void Debug(string message, params object[] formatOptions) {
	        using (_loggers.EnterReadScope())
	            Parallel.ForEach(_loggers, (logger) => {
	                try {
	                    logger.Debug(message, formatOptions);
	                } catch {
	                    // ignored
	                }
	            });
	    }

	    public void Info(string message, params object[] formatOptions) {
            using (_loggers.EnterReadScope())
                Parallel.ForEach(_loggers, (logger) => {
                    try {
                        logger.Info(message, formatOptions);
                    } catch {
                        // ignored
                    }
                });
	    }

	    public void Warning(string message, params object[] formatOptions) {
            using (_loggers.EnterReadScope())
                Parallel.ForEach(_loggers, (logger) => {
                    try {
                        logger.Warning(message, formatOptions);
                    } catch {
                        // ignored
                    }
                });
	    }

	    public void Error(string message, params object[] formatOptions) {
            using (_loggers.EnterReadScope())
                Parallel.ForEach(_loggers, (logger) => {
                    try {
                        logger.Error(message, formatOptions);
                    } catch {
                        // ignored
                    }
                });
	    }

	}
}
