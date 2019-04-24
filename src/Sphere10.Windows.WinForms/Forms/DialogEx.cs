//-----------------------------------------------------------------------
// <copyright file="DialogEx.cs" company="Sphere 10 Software">
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
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sphere10.Framework.Windows.Forms {

	public partial class DialogEx : Form {

		public const int MaxTextLength = 5000;

		public DialogEx() : this(SystemIconType.None, string.Empty, "OK") {
		}

		protected DialogEx(SystemIconType iconType, string title, string text, params string[] buttonNames) {
			if (title == null)
				throw new ArgumentNullException("title");
			if (text == null)
				throw new ArgumentNullException("text");
			if (buttonNames.Length == 0)
				buttonNames = new[] { "OK" };

			if (buttonNames.Length > 5)
				throw new ArgumentOutOfRangeException("buttonNames", "Cannot have more than 5 buttons");

			// clip the text if its too large
			if (text.Length > MaxTextLength)
				text = text.Substring(0, MaxTextLength);
	
			InitializeComponent();

			this.Text = title;

			// determine size of form based on label/text
			if (!_textLabel.CanTextFit(text)) {
				var oldSize = _textLabel.Size;
				var newSize = _textLabel.FitSize(text, oldSize.Width);

				// we resize the form, not the label since the label is anchored to the form
				var heightDiff = newSize.Height - oldSize.Height;
				this.Height += (int)Math.Round(heightDiff);
			}
			_textLabel.Text = text;

			for (var i = 0; i < buttonNames.Length; i++) {
				switch (i) {
					case 0:
						button1.Visible = true;
						button1.Text = buttonNames[i];
						break;
					case 1:
						button2.Visible = true;
						button2.Text = buttonNames[i];
						break;
					case 2:
						button3.Visible = true;
						button3.Text = buttonNames[i];
						break;
					case 3:
						button4.Visible = true;
						button4.Text = buttonNames[i];
						break;
				}
			}

			for (var i = buttonNames.Length; i < 4; i++) {
				switch (i) {
					case 0:
						button1.Visible = false;
						break;
					case 1:
						button2.Visible = false;
						break;
					case 2:
						button3.Visible = false;
						break;
					case 3:
						button4.Visible = false;
						break;
				}
			}
			
		}

		public static DialogExResult Show(SystemIconType iconType, string title, string text, params string[] buttonNames) {
			var dialog = new DialogEx(iconType, title, text, buttonNames);
			dialog.ShowDialog();
			return dialog.DialogResult;
		}

		public static DialogExResult Show(IWin32Window owner, SystemIconType iconType, string title, string text, params string[] buttonNames) {
			var dialog = new DialogEx(iconType, title, text, buttonNames);
			dialog.StartPosition = FormStartPosition.CenterParent;
			dialog.ShowDialog(owner);
			return dialog.DialogResult;
		}


		public new DialogExResult DialogResult { get; private set; }

		private void CloseWithResult(DialogExResult result) {
			DialogResult = result;
			Close();
		}

		private void button1_Click(object sender, EventArgs e) {
			CloseWithResult(DialogExResult.Button1);
		}

		private void button2_Click(object sender, EventArgs e) {
			CloseWithResult(DialogExResult.Button2);
		}

		private void button3_Click(object sender, EventArgs e) {
			CloseWithResult(DialogExResult.Button3);
		}

		private void button4_Click(object sender, EventArgs e) {
			CloseWithResult(DialogExResult.Button4);
		}
	
	}

	public enum DialogExResult {
		Button1,
		Button2,
		Button3,
		Button4,
		Button5
	}
}
