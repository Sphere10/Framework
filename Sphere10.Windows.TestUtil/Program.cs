//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Sphere 10 Software">
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
using System.Windows.Forms;
using Sphere10.Framework;

namespace Sphere10.Windows.TestUtil {
	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
		    var str =
		        Encoding.UTF32.GetString(
		            Enumerable
		                .Range(0, 1000)
		                .Select(z => (byte)Tools.Maths.RandomNumberGenerator.Next(0, 255))
		                .ToArray()
		            );

		    var x = str;

			System.Windows.Forms.Application.EnableVisualStyles();
			System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
			System.Windows.Forms.Application.Run(new MainForm());
		}

	}
}
