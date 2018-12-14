//-----------------------------------------------------------------------
// <copyright file="ConsoleTextWriter.cs" company="Sphere 10 Software">
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
	/// TextWriter which outputs to Console.
	/// </summary>
	public class ConsoleTextWriter : BaseTextWriter {

		protected override void InternalWrite(string value) {
#if !__WP8__
			System.Console.Write(value);
#else
            System.Diagnostics.Debug.WriteLine(value);
#endif
		}

	}

}
