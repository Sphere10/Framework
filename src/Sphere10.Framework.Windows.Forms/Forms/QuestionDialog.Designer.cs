//-----------------------------------------------------------------------
// <copyright file="QuestionDialog.Designer.cs" company="Sphere 10 Software">
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

namespace Sphere10.Framework.Windows.Forms {
	partial class QuestionDialog {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this._alwaysCheckBox = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxEx1)).BeginInit();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(267, 89);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(186, 89);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(105, 89);
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(24, 89);
			// 
			// _textLabel
			// 
			this._textLabel.Size = new System.Drawing.Size(292, 65);
			// 
			// _alwaysCheckBox
			// 
			this._alwaysCheckBox.AutoSize = true;
			this._alwaysCheckBox.Location = new System.Drawing.Point(12, 125);
			this._alwaysCheckBox.Name = "_alwaysCheckBox";
			this._alwaysCheckBox.Size = new System.Drawing.Size(252, 17);
			this._alwaysCheckBox.TabIndex = 6;
			this._alwaysCheckBox.Text = "Always do this for the remainder of the operation";
			this._alwaysCheckBox.UseVisualStyleBackColor = true;
			this._alwaysCheckBox.CheckedChanged += new System.EventHandler(this._alwaysCheckBox_CheckedChanged);
			// 
			// QuestionDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.ClientSize = new System.Drawing.Size(354, 154);
			this.Controls.Add(this._alwaysCheckBox);
			this.Name = "QuestionDialog";
			this.Controls.SetChildIndex(this._alwaysCheckBox, 0);
			this.Controls.SetChildIndex(this.button3, 0);
			this.Controls.SetChildIndex(this.button2, 0);
			this.Controls.SetChildIndex(this.button4, 0);
			this.Controls.SetChildIndex(this.button1, 0);
			this.Controls.SetChildIndex(this.pictureBoxEx1, 0);
			this.Controls.SetChildIndex(this._textLabel, 0);
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxEx1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox _alwaysCheckBox;
	}
}
