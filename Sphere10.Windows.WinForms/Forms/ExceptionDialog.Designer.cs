//-----------------------------------------------------------------------
// <copyright file="ExceptionDialog.Designer.cs" company="Sphere 10 Software">
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

namespace Sphere10.Windows.WinForms {
	partial class ExceptionDialog {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExceptionDialog));
			this._viewDetailButton = new System.Windows.Forms.Button();
			this._closeButton = new System.Windows.Forms.Button();
			this._textLabel = new System.Windows.Forms.Label();
			this.pictureBoxEx1 = new Sphere10.Windows.WinForms.PictureBoxEx();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxEx1)).BeginInit();
			this.SuspendLayout();
			// 
			// _viewDetailButton
			// 
			this._viewDetailButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._viewDetailButton.Location = new System.Drawing.Point(239, 145);
			this._viewDetailButton.Name = "_viewDetailButton";
			this._viewDetailButton.Size = new System.Drawing.Size(75, 23);
			this._viewDetailButton.TabIndex = 9;
			this._viewDetailButton.Text = "&Detail";
			this._viewDetailButton.UseVisualStyleBackColor = true;
			this._viewDetailButton.Click += new System.EventHandler(this._viewDetailButton_Click);
			// 
			// _closeButton
			// 
			this._closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._closeButton.Location = new System.Drawing.Point(320, 145);
			this._closeButton.Name = "_closeButton";
			this._closeButton.Size = new System.Drawing.Size(75, 23);
			this._closeButton.TabIndex = 8;
			this._closeButton.Text = "&Close";
			this._closeButton.UseVisualStyleBackColor = true;
			this._closeButton.Click += new System.EventHandler(this._closeButton_Click);
			// 
			// _textLabel
			// 
			this._textLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._textLabel.AutoEllipsis = true;
			this._textLabel.Location = new System.Drawing.Point(50, 11);
			this._textLabel.Name = "_textLabel";
			this._textLabel.Size = new System.Drawing.Size(346, 131);
			this._textLabel.TabIndex = 7;
			this._textLabel.Text = "_text";
			// 
			// pictureBoxEx1
			// 
			this.pictureBoxEx1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxEx1.Image")));
			this.pictureBoxEx1.Location = new System.Drawing.Point(1, 11);
			this.pictureBoxEx1.Name = "pictureBoxEx1";
			this.pictureBoxEx1.Size = new System.Drawing.Size(32, 32);
			this.pictureBoxEx1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBoxEx1.SystemIcon = Sphere10.Windows.WinForms.SystemIconType.Error;
			this.pictureBoxEx1.TabIndex = 6;
			this.pictureBoxEx1.TabStop = false;
			// 
			// ExceptionDialog
			// 
			this.AcceptButton = this._viewDetailButton;
			this.CancelButton = this._closeButton;
			this.ClientSize = new System.Drawing.Size(408, 180);
			this.Controls.Add(this._viewDetailButton);
			this.Controls.Add(this._closeButton);
			this.Controls.Add(this._textLabel);
			this.Controls.Add(this.pictureBoxEx1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(555, 900);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(300, 131);
			this.Name = "ExceptionDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.TopMost = true;
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxEx1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button _viewDetailButton;
		private System.Windows.Forms.Button _closeButton;
		private System.Windows.Forms.Label _textLabel;
		private PictureBoxEx pictureBoxEx1;
	}
}
