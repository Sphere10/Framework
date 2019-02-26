//-----------------------------------------------------------------------
// <copyright file="DebugTextWriter.cs" company="Sphere 10 Software">
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
	/// TextWriter which outputs to Debug output.
	/// </summary>
	public class DebugTextWriter : BaseTextWriter {


		protected override void InternalWrite(string value) {
#if __IOS__ || __WP8__
            System.Diagnostics.Debug.WriteLine(value);
#else
            System.Diagnostics.Debug.Write(value);
#endif
                                                            }
	}
}
