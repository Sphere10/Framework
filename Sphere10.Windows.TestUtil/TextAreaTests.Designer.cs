//-----------------------------------------------------------------------
// <copyright file="TextAreaTests.Designer.cs" company="Sphere 10 Software">
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

namespace Sphere10.Framework.WinForms.TestUtil {
    partial class TextAreaTests {
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
            this._standardTextBox = new System.Windows.Forms.TextBox();
            this._fillStandardButton = new System.Windows.Forms.Button();
            this._fillLockedButton = new System.Windows.Forms.Button();
            this._genOnlyButton = new System.Windows.Forms.Button();
            this._fillLockedAsyncButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _standardTextBox
            // 
            this._standardTextBox.Location = new System.Drawing.Point(12, 41);
            this._standardTextBox.Multiline = true;
            this._standardTextBox.Name = "_standardTextBox";
            this._standardTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._standardTextBox.Size = new System.Drawing.Size(505, 224);
            this._standardTextBox.TabIndex = 0;
            // 
            // _fillStandardButton
            // 
            this._fillStandardButton.Location = new System.Drawing.Point(12, 12);
            this._fillStandardButton.Name = "_fillStandardButton";
            this._fillStandardButton.Size = new System.Drawing.Size(110, 23);
            this._fillStandardButton.TabIndex = 2;
            this._fillStandardButton.Text = "Fill Standard";
            this._fillStandardButton.UseVisualStyleBackColor = true;
            this._fillStandardButton.Click += new System.EventHandler(this._fillStandardButton_Click);
            // 
            // _fillLockedButton
            // 
            this._fillLockedButton.Location = new System.Drawing.Point(128, 12);
            this._fillLockedButton.Name = "_fillLockedButton";
            this._fillLockedButton.Size = new System.Drawing.Size(75, 23);
            this._fillLockedButton.TabIndex = 3;
            this._fillLockedButton.Text = "Fill Locked";
            this._fillLockedButton.UseVisualStyleBackColor = true;
            this._fillLockedButton.Click += new System.EventHandler(this._fillLockedButton_Click);
            // 
            // _genOnlyButton
            // 
            this._genOnlyButton.Location = new System.Drawing.Point(442, 12);
            this._genOnlyButton.Name = "_genOnlyButton";
            this._genOnlyButton.Size = new System.Drawing.Size(75, 23);
            this._genOnlyButton.TabIndex = 4;
            this._genOnlyButton.Text = "Generate only";
            this._genOnlyButton.UseVisualStyleBackColor = true;
            this._genOnlyButton.Click += new System.EventHandler(this._genOnlyButton_Click);
            // 
            // _fillLockedAsyncButton
            // 
            this._fillLockedAsyncButton.Location = new System.Drawing.Point(209, 12);
            this._fillLockedAsyncButton.Name = "_fillLockedAsyncButton";
            this._fillLockedAsyncButton.Size = new System.Drawing.Size(109, 23);
            this._fillLockedAsyncButton.TabIndex = 5;
            this._fillLockedAsyncButton.Text = "Fill Locked Async";
            this._fillLockedAsyncButton.UseVisualStyleBackColor = true;
            this._fillLockedAsyncButton.Click += new System.EventHandler(this._fillLockedAsyncButton_Click);
            // 
            // TextAreaTests
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(935, 277);
            this.Controls.Add(this._fillLockedAsyncButton);
            this.Controls.Add(this._genOnlyButton);
            this.Controls.Add(this._fillLockedButton);
            this.Controls.Add(this._fillStandardButton);
            this.Controls.Add(this._standardTextBox);
            this.Name = "TextAreaTests";
            this.Text = "MiscTestForm";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.TextBox _standardTextBox;
        private System.Windows.Forms.Button _fillStandardButton;
        private System.Windows.Forms.Button _fillLockedButton;
        private System.Windows.Forms.Button _genOnlyButton;
        private System.Windows.Forms.Button _fillLockedAsyncButton;



    }
}
