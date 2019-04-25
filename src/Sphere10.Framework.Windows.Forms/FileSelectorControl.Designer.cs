//-----------------------------------------------------------------------
// <copyright file="FileSelectorControl.Designer.cs" company="Sphere 10 Software">
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

namespace Sphere10.Common {
    partial class FileSelectorControl {
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this._fileSelectorButton = new System.Windows.Forms.Button();
            this._filenameTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _fileSelectorButton
            // 
            this._fileSelectorButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._fileSelectorButton.Location = new System.Drawing.Point(229, -1);
            this._fileSelectorButton.Name = "_fileSelectorButton";
            this._fileSelectorButton.Size = new System.Drawing.Size(31, 20);
            this._fileSelectorButton.TabIndex = 5;
            this._fileSelectorButton.Text = "...";
            this._fileSelectorButton.UseVisualStyleBackColor = true;
            this._fileSelectorButton.Click += new System.EventHandler(this._fileSelectorButton_Click);
            // 
            // _filenameTextBox
            // 
            this._filenameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._filenameTextBox.Location = new System.Drawing.Point(-422, 0);
            this._filenameTextBox.Name = "_filenameTextBox";
            this._filenameTextBox.Size = new System.Drawing.Size(645, 20);
            this._filenameTextBox.TabIndex = 4;
            // 
            // FileSelectorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._fileSelectorButton);
            this.Controls.Add(this._filenameTextBox);
            this.Name = "FileSelectorControl";
            this.Size = new System.Drawing.Size(260, 20);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _fileSelectorButton;
        private System.Windows.Forms.TextBox _filenameTextBox;
    }
}
