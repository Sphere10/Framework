//-----------------------------------------------------------------------
// <copyright file="PasswordDialog.cs" company="Sphere 10 Software">
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
using Sphere10.Framework;

namespace Sphere10.Framework.Windows.Forms {

	public partial class PasswordDialog : Form {
		public const int MaxTextLength = 5000;
		private readonly Func<string, IEnumerable<string>> _policyValidator;

		public PasswordDialog() : this("Designer Title", "Designer mode text") {
		}

		public PasswordDialog(string title, string text, Func<string, IEnumerable<string>> policyValidator = null) {
			InitializeComponent();
			this.Text = title;
			_textLabel.Text = text;
			_policyValidator = policyValidator;
		}

		public string Password { get; protected set; }

		#region Methods

		protected virtual bool ValidatePassword() {
			const int rtfHeightPerLine = 15;
			const int maxErrorsWithoutScrollbar = 10;
			var errors = new List<string>();
			var passwordText = _passwordTextBox.Text;
			var repeatText = _repeatTextBox.Text;

			if (string.IsNullOrEmpty(passwordText)) {
				errors.Add("Password cannot be empty");
			}

			if (passwordText != repeatText) {
				errors.Add("Passwords do not match");
			}

			if (_policyValidator != null) {
				errors.AddRange(_policyValidator.Invoke(passwordText));
			}
			
			var extraHeight = rtfHeightPerLine * errors.Count().ClipTo(0, maxErrorsWithoutScrollbar) - _errorRichTextBox.Height;
			this.Size = new Size(this.Size.Width, (int)(this.Size.Height + extraHeight));
			if (errors.Any()) {
				_errorRichTextBox.Lines = errors.ToArray();
				return false;
			}
			return true;
		}

		#endregion

		#region Static Methods

		public static DialogResult Show(IWin32Window owner, string title, string text, out string password, Func<string, IEnumerable<string>> policyValidator = null) {
			var dialog = new PasswordDialog(title, text, policyValidator);
			var startPosition = owner != null ? FormStartPosition.CenterParent : FormStartPosition.WindowsDefaultLocation;
			dialog.StartPosition = startPosition;
			dialog.ShowDialog(owner);
			password = dialog.Password;
			return dialog.DialogResult;
		}

		#endregion

		#region Event Handlers

		private void _okButton_Click(object sender, EventArgs e) {
			try {
				if (ValidatePassword()) {
					this.DialogResult = DialogResult.OK;
					Password = _passwordTextBox.Text;
					Close();
				}
			} catch (Exception error) {
				ExceptionDialog.Show(this, error);
			}
		}

		private void _cancelButton_Click(object sender, EventArgs e) {
			try {
				this.DialogResult = DialogResult.Cancel;
				Password = null;
				Close();
			} catch (Exception error) {
				ExceptionDialog.Show(this, error);
			}
		}

		private void _showPasswordCheckBox_CheckedChanged(object sender, EventArgs e) {
			try {
				_passwordTextBox.PasswordChar =
					_repeatTextBox.PasswordChar =
					_hidePasswordCheckBox.Checked ? '*' : (char)0;
			} catch (Exception error) {
				ExceptionDialog.Show(this, error);
			}
		}

		#endregion

	}

}
