//-----------------------------------------------------------------------
// <copyright file="ActionTextWriterDecorator.cs" company="Sphere 10 Software">
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


	public sealed class ActionTextWriterDecorator : TextWriterDecorator {

		private readonly Func<string, string> valueMutator;

		public ActionTextWriterDecorator(Func<string, string> valueMutator, TextWriter internalTextWrtier) : base(internalTextWrtier) {
			if (valueMutator == null) {
				throw new ArgumentNullException("valueMutator");
			}
			this.valueMutator = valueMutator;
		}

		protected override string DecorateText(string text) {
			return valueMutator(text);
		}

	}

}