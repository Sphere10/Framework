//-----------------------------------------------------------------------
// <copyright file="FlagsCheckedBoxListTestForm.cs" company="Sphere 10 Software">
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sphere10.Framework;
using Sphere10.Framework.Windows.Forms;

namespace Sphere10.FrameworkTester {
	// There is an issue dealing with flags that have value '0'. How is such a flag to be interpreted?
	public partial class FlagsCheckedBoxListTestForm : Form {
		private readonly TextWriter _textWriter;

		public FlagsCheckedBoxListTestForm() {
			InitializeComponent();
			_flagsCheckedListBox.EnumType = typeof(Test1Enum);
			var selectedEnum = _flagsCheckedListBox.SelectedEnum;
			_textWriter = new TextBoxWriter(_outputTextBox);
			_textWriter.WriteLine("Initial value: {0}", selectedEnum != null ? ((Test1Enum)selectedEnum).ToString() : string.Empty);
		}

		private void _flagsCheckedListBox_SelectedValueChanged(object sender, EventArgs e) {
			var selectedEnum = _flagsCheckedListBox.SelectedEnum;
			_textWriter.WriteLine("Selected Value Changed: {0}", selectedEnum != null ? ((Test1Enum)selectedEnum).ToString() : string.Empty);
		}

		private void _flagsCheckedListBox_SelectedFlagChanged(FlagsCheckedListBox arg) {
			var selectedEnum = _flagsCheckedListBox.SelectedEnum;
			_textWriter.WriteLine("Selected Flag Changed: {0}", selectedEnum != null ? ((Test1Enum)selectedEnum).ToString() : string.Empty);
		}

		[Flags]
		private enum Test1Enum {
			Zero = 0,
			Flag1 = 1 << 0,
			Flag2 = 1 << 1,
			Flag3 = 1 << 2,
			[Description("Flags 1 & 2")]
			Flag1And2 = Flag1 | Flag2,
			All = Flag1 | Flag2 | Flag3,
			Default = Flag1 | Flag3,
			//[Description("All (with Zero)")]
			//SuperAll = All | Zero
		}

	}
}
