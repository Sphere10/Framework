//-----------------------------------------------------------------------
// <copyright file="StandardAboutBox.Designer.cs" company="Sphere 10 Software">
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
    partial class StandardAboutBox {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            this.okButton = new System.Windows.Forms.Button();
            this.applicationBanner1 = new Sphere10.Framework.Windows.Forms.ApplicationBanner();
            this._label1 = new System.Windows.Forms.Label();
            this._label2 = new System.Windows.Forms.Label();
            this._label3 = new System.Windows.Forms.Label();
            this._label4 = new System.Windows.Forms.Label();
            this._link1 = new System.Windows.Forms.LinkLabel();
            this._label5 = new System.Windows.Forms.Label();
            this._label6 = new System.Windows.Forms.Label();
            this._label7 = new System.Windows.Forms.Label();
            this._label8 = new System.Windows.Forms.Label();
            this._label9 = new System.Windows.Forms.Label();
            this._label10 = new System.Windows.Forms.Label();
            this._label11 = new System.Windows.Forms.Label();
            this._label12 = new System.Windows.Forms.Label();
            this._productExpirationDetailsControl = new Sphere10.Framework.Windows.Forms.ProductExpirationDetailsControl();
            this._enterNewProductKey = new System.Windows.Forms.Button();
            this._companyNumberLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.okButton.Location = new System.Drawing.Point(353, 254);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(115, 23);
            this.okButton.TabIndex = 31;
            this.okButton.Text = "&OK";
            // 
            // applicationBanner1
            // 
            this.applicationBanner1.AutoDetectChildStateChanges = false;
            this.applicationBanner1.CompanyName = "{CompanyName}";
            this.applicationBanner1.Dock = System.Windows.Forms.DockStyle.Top;
            this.applicationBanner1.FromColor = System.Drawing.Color.RoyalBlue;
            this.applicationBanner1.Location = new System.Drawing.Point(0, 0);
            this.applicationBanner1.Name = "applicationBanner1";
            this.applicationBanner1.Size = new System.Drawing.Size(479, 96);
            this.applicationBanner1.TabIndex = 32;
            this.applicationBanner1.Title = "{ProductName}";
            this.applicationBanner1.ToColor = System.Drawing.Color.LightBlue;
            this.applicationBanner1.Version = "Version {ProductVersion}";
            // 
            // _label1
            // 
            this._label1.AutoSize = true;
            this._label1.Location = new System.Drawing.Point(12, 114);
            this._label1.Name = "_label1";
            this._label1.Size = new System.Drawing.Size(87, 13);
            this._label1.TabIndex = 33;
            this._label1.Text = "{CompanyName}";
            // 
            // _label2
            // 
            this._label2.AutoSize = true;
            this._label2.Location = new System.Drawing.Point(12, 140);
            this._label2.Name = "_label2";
            this._label2.Size = new System.Drawing.Size(80, 13);
            this._label2.TabIndex = 34;
            this._label2.Text = "{ProductName}";
            // 
            // _label3
            // 
            this._label3.AutoSize = true;
            this._label3.Location = new System.Drawing.Point(12, 153);
            this._label3.Name = "_label3";
            this._label3.Size = new System.Drawing.Size(149, 13);
            this._label3.TabIndex = 35;
            this._label3.Text = "Version {ProductLongVersion}";
            // 
            // _label4
            // 
            this._label4.AutoSize = true;
            this._label4.Location = new System.Drawing.Point(12, 166);
            this._label4.Name = "_label4";
            this._label4.Size = new System.Drawing.Size(90, 13);
            this._label4.TabIndex = 36;
            this._label4.Text = "{CopyrightNotice}";
            // 
            // _link1
            // 
            this._link1.AutoSize = true;
            this._link1.Location = new System.Drawing.Point(12, 179);
            this._link1.Name = "_link1";
            this._link1.Size = new System.Drawing.Size(72, 13);
            this._link1.TabIndex = 37;
            this._link1.TabStop = true;
            this._link1.Text = "{CompanyUrl}";
            this._link1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._productLink_LinkClicked);
            // 
            // _label5
            // 
            this._label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._label5.AutoSize = true;
            this._label5.Location = new System.Drawing.Point(350, 114);
            this._label5.Name = "_label5";
            this._label5.Size = new System.Drawing.Size(55, 13);
            this._label5.TabIndex = 38;
            this._label5.Text = "First used:";
            // 
            // _label6
            // 
            this._label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._label6.AutoSize = true;
            this._label6.Location = new System.Drawing.Point(346, 127);
            this._label6.Name = "_label6";
            this._label6.Size = new System.Drawing.Size(59, 13);
            this._label6.TabIndex = 39;
            this._label6.Text = "Total uses:";
            // 
            // _label7
            // 
            this._label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._label7.AutoSize = true;
            this._label7.Location = new System.Drawing.Point(277, 140);
            this._label7.Name = "_label7";
            this._label7.Size = new System.Drawing.Size(128, 13);
            this._label7.TabIndex = 40;
            this._label7.Text = "First used by current user:";
            // 
            // _label8
            // 
            this._label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._label8.AutoSize = true;
            this._label8.Location = new System.Drawing.Point(273, 153);
            this._label8.Name = "_label8";
            this._label8.Size = new System.Drawing.Size(132, 13);
            this._label8.TabIndex = 41;
            this._label8.Text = "Total uses by current user:";
            // 
            // _label9
            // 
            this._label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._label9.AutoSize = true;
            this._label9.Location = new System.Drawing.Point(411, 114);
            this._label9.Name = "_label9";
            this._label9.Size = new System.Drawing.Size(150, 13);
            this._label9.TabIndex = 42;
            this._label9.Text = "{FirstUsedDateBySystemUTC}";
            // 
            // _label10
            // 
            this._label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._label10.AutoSize = true;
            this._label10.Location = new System.Drawing.Point(411, 127);
            this._label10.Name = "_label10";
            this._label10.Size = new System.Drawing.Size(133, 13);
            this._label10.TabIndex = 43;
            this._label10.Text = "{NumberOfUsesBySystem}";
            // 
            // _label11
            // 
            this._label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._label11.AutoSize = true;
            this._label11.Location = new System.Drawing.Point(411, 140);
            this._label11.Name = "_label11";
            this._label11.Size = new System.Drawing.Size(138, 13);
            this._label11.TabIndex = 44;
            this._label11.Text = "{FirstUsedDateByUserUTC}";
            // 
            // _label12
            // 
            this._label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._label12.AutoSize = true;
            this._label12.Location = new System.Drawing.Point(411, 153);
            this._label12.Name = "_label12";
            this._label12.Size = new System.Drawing.Size(121, 13);
            this._label12.TabIndex = 45;
            this._label12.Text = "{NumberOfUsesByUser}";
            // 
            // _productExpirationDetailsControl
            // 
            this._productExpirationDetailsControl.AutoDetectChildStateChanges = false;
            this._productExpirationDetailsControl.Location = new System.Drawing.Point(15, 207);
            this._productExpirationDetailsControl.Name = "_productExpirationDetailsControl";
            this._productExpirationDetailsControl.Size = new System.Drawing.Size(453, 41);
            this._productExpirationDetailsControl.TabIndex = 46;
            // 
            // _enterNewProductKey
            // 
            this._enterNewProductKey.Location = new System.Drawing.Point(12, 254);
            this._enterNewProductKey.Name = "_enterNewProductKey";
            this._enterNewProductKey.Size = new System.Drawing.Size(115, 23);
            this._enterNewProductKey.TabIndex = 47;
            this._enterNewProductKey.Text = "Change Product Key";
            this._enterNewProductKey.UseVisualStyleBackColor = true;
            this._enterNewProductKey.Click += new System.EventHandler(this._enterNewProductKey_Click);
            // 
            // _companyNumberLabel
            // 
            this._companyNumberLabel.AutoSize = true;
            this._companyNumberLabel.Location = new System.Drawing.Point(12, 127);
            this._companyNumberLabel.Name = "_companyNumberLabel";
            this._companyNumberLabel.Size = new System.Drawing.Size(96, 13);
            this._companyNumberLabel.TabIndex = 48;
            this._companyNumberLabel.Text = "{CompanyNumber}";
            // 
            // StandardAboutBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 289);
            this.Controls.Add(this._companyNumberLabel);
            this.Controls.Add(this._enterNewProductKey);
            this.Controls.Add(this._productExpirationDetailsControl);
            this.Controls.Add(this._label12);
            this.Controls.Add(this._label11);
            this.Controls.Add(this._label10);
            this.Controls.Add(this._label9);
            this.Controls.Add(this._label8);
            this.Controls.Add(this._label7);
            this.Controls.Add(this._label6);
            this.Controls.Add(this._label5);
            this.Controls.Add(this._link1);
            this.Controls.Add(this._label4);
            this.Controls.Add(this._label3);
            this.Controls.Add(this._label2);
            this.Controls.Add(this._label1);
            this.Controls.Add(this.applicationBanner1);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StandardAboutBox";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About {ProductName}";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

		protected System.Windows.Forms.Button okButton;
		protected ApplicationBanner applicationBanner1;
		protected System.Windows.Forms.Label _label1;
		protected System.Windows.Forms.Label _label2;
		protected System.Windows.Forms.Label _label3;
		protected System.Windows.Forms.Label _label4;
		protected System.Windows.Forms.LinkLabel _link1;
		protected System.Windows.Forms.Label _label5;
		protected System.Windows.Forms.Label _label6;
		protected System.Windows.Forms.Label _label7;
		protected System.Windows.Forms.Label _label8;
		protected System.Windows.Forms.Label _label9;
		protected System.Windows.Forms.Label _label10;
		protected System.Windows.Forms.Label _label11;
		protected System.Windows.Forms.Label _label12;
		protected ProductExpirationDetailsControl _productExpirationDetailsControl;
		protected System.Windows.Forms.Button _enterNewProductKey;
        protected System.Windows.Forms.Label _companyNumberLabel;


	}
}
