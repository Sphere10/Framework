//-----------------------------------------------------------------------
// <copyright file="ExecuteScriptForm.cs" company="Sphere 10 Software">
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
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sphere10.Framework;
using Sphere10.Framework.Data;

namespace Sphere10.Framework.Windows.Forms {
	public partial class ExecuteScriptForm : Form {
		public ExecuteScriptForm(IDAC dac, ISQLBuilder script) {
			InitializeComponent();
			_scriptTextBox.Text = script.ToString();
			_scriptTextBox.FocusAtEnd();
			_databaseConnectionStringLabel.Text = dac.ConnectionString;
			DAC = dac;
			Script = script;
			DialogResult = DialogResult.Cancel;
		}

		public IDAC DAC { get; private set; }

		public ISQLBuilder Script { get; private set; }


		private async void _executeScriptButton_Click(object sender, EventArgs e) {
			try {
			    using (LoadingCircle.EnterAnimationScope(this, disableControls:true)) {
			       await Task.Run( () =>  DAC.ExecuteBatch(Script));
			    }
			    this.DialogResult = DialogResult.OK;

				Close();
			} catch (Exception error) {
				ExceptionDialog.Show("Error", error);
			} finally {
			}
		}

		private void _copyToClipboardButton_Click(object sender, EventArgs e) {
			Clipboard.SetText(_scriptTextBox.Text);
		}


	}
}
