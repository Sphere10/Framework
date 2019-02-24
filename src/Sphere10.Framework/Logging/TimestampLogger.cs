//-----------------------------------------------------------------------
// <copyright file="TimestampLogger.cs" company="Sphere 10 Software">
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

	public class TimestampLogger : DecoratedLogger {

		public const string DefaultDateFormat = "yyyy-MM-dd HH:mm:ss";

		private string _dateFormat;

		public TimestampLogger(ILogger decoratedLogger, string dateFormat = null) : base(decoratedLogger) {
			_dateFormat = dateFormat ?? DefaultDateFormat;
		}

		public string TimestampFormat { get { return _dateFormat; } set { _dateFormat = value; } }

		public override void Debug(string message, params object[] formatOptions) {
			base.Debug(GetTimestamp() + message, formatOptions);
		}

		public override void Info(string message, params object[] formatOptions) {
			base.Info(GetTimestamp() + message, formatOptions);
		}

		public override void Warning(string message, params object[] formatOptions) {
			base.Warning(GetTimestamp() + message, formatOptions);
		}

		public override void Error(string message, params object[] formatOptions) {
			base.Error(GetTimestamp() + message, formatOptions);
		}

		private string GetTimestamp() {
			return string.Format("{0:"+ _dateFormat + "}: ", DateTime.Now);
		}


	}	
}
