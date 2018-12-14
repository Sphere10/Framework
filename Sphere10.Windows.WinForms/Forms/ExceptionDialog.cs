//-----------------------------------------------------------------------
// <copyright file="ExceptionDialog.cs" company="Sphere 10 Software">
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
using System.Text;
using System.Windows.Forms;
using Sphere10.Framework;

namespace Sphere10.Windows.WinForms {
	public partial class ExceptionDialog : Form {

		public ExceptionDialog() : this(string.Empty, new Exception()) {
			
		}

		public ExceptionDialog(string title, Exception error) {
			InitializeComponent();
			this.Text = title;
			Error = error;
			var text = error.ToDisplayString();
			// determine size of form based on label/text
			if (!_textLabel.CanTextFit(text)) {
				var oldSize = _textLabel.Size;
				var newSize = _textLabel.FitSize(text, oldSize.Width);

				// we resize the form, not the label since the label is anchored to the form
				var heightDiff = newSize.Height - oldSize.Height;
				this.Height += (int)Math.Round(heightDiff);
			}
			_textLabel.Text = text;

		}

		public static void Show(Exception error) {
			Show("Error", error);
		}


		public static void Show(string title, Exception error) {
			Show(null, title, error);
		}

		public static void Show(IWin32Window owner,  Exception error) {
			Show(owner, "Error", error);
		}

		public static void Show(IWin32Window owner, string title, Exception error) {
		    if (Application.OpenForms.Count > 0) {
		        Application.OpenForms[0].InvokeEx(() => {
		            var form = new ExceptionDialog(title, error);
		            form.ShowDialog(owner);
		        });
		    } else {
                var form = new ExceptionDialog(title, error);
                form.ShowDialog(owner);
		    }
		}


		private Exception Error { get; set; }

		private void _viewDetailButton_Click(object sender, EventArgs e) {
			var detailForm = new TextEditorForm(Error.ToDiagnosticString());
			detailForm.ShowDialog(this);
		}

		private void _closeButton_Click(object sender, EventArgs e) {
			Close();
		}


	}
}
