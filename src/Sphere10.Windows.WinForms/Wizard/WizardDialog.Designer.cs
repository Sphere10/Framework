//-----------------------------------------------------------------------
// <copyright file="WizardDialog.Designer.cs" company="Sphere 10 Software">
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
    partial class WizardDialog<T> {
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
            this._nextButton = new System.Windows.Forms.Button();
            this._previousButton = new System.Windows.Forms.Button();
            this.line1 = new DevAge.Windows.Forms.Line();
            this._contentPanel = new System.Windows.Forms.Panel();
            this.loadingCircle1 = new Sphere10.Framework.Windows.Forms.LoadingCircle();
            this.SuspendLayout();
            // 
            // _nextButton
            // 
            this._nextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._nextButton.Location = new System.Drawing.Point(997, 772);
            this._nextButton.Name = "_nextButton";
            this._nextButton.Size = new System.Drawing.Size(175, 47);
            this._nextButton.TabIndex = 0;
            this._nextButton.Text = "&Next";
            this._nextButton.UseVisualStyleBackColor = true;
            this._nextButton.Click += new System.EventHandler(this._nextButton_Click);
            // 
            // _previousButton
            // 
            this._previousButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._previousButton.Location = new System.Drawing.Point(12, 772);
            this._previousButton.Name = "_previousButton";
            this._previousButton.Size = new System.Drawing.Size(175, 47);
            this._previousButton.TabIndex = 2;
            this._previousButton.Text = "&Previous";
            this._previousButton.UseVisualStyleBackColor = true;
            this._previousButton.Click += new System.EventHandler(this._previousButton_Click);
            // 
            // line1
            // 
            this.line1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.line1.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this.line1.FirstColor = System.Drawing.SystemColors.ControlDark;
            this.line1.LineStyle = DevAge.Windows.Forms.LineStyle.Horizontal;
            this.line1.Location = new System.Drawing.Point(1, 754);
            this.line1.Name = "line1";
            this.line1.SecondColor = System.Drawing.SystemColors.ControlLightLight;
            this.line1.Size = new System.Drawing.Size(1188, 2);
            this.line1.TabIndex = 3;
            this.line1.TabStop = false;
            // 
            // _contentPanel
            // 
            this._contentPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._contentPanel.Location = new System.Drawing.Point(12, 12);
            this._contentPanel.Name = "_contentPanel";
            this._contentPanel.Size = new System.Drawing.Size(1160, 736);
            this._contentPanel.TabIndex = 4;
            // 
            // loadingCircle1
            // 
            this.loadingCircle1.Active = false;
            this.loadingCircle1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.loadingCircle1.BackColor = System.Drawing.Color.Transparent;
            this.loadingCircle1.Color = System.Drawing.Color.DarkGray;
            this.loadingCircle1.InnerCircleRadius = 9;
            this.loadingCircle1.Location = new System.Drawing.Point(939, 772);
            this.loadingCircle1.Name = "loadingCircle1";
            this.loadingCircle1.NumberSpoke = 12;
            this.loadingCircle1.OuterCircleRadius = 20;
            this.loadingCircle1.RotationSpeed = 100;
            this.loadingCircle1.Size = new System.Drawing.Size(52, 47);
            this.loadingCircle1.SpokeThickness = 2;
            this.loadingCircle1.StylePreset = Sphere10.Framework.Windows.Forms.LoadingCircle.StylePresets.MacOSX;
            this.loadingCircle1.TabIndex = 5;
            this.loadingCircle1.Text = "_loadingCircle";
            this.loadingCircle1.Visible = false;
            // 
            // WizardDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 831);
            this.Controls.Add(this.loadingCircle1);
            this.Controls.Add(this._contentPanel);
            this.Controls.Add(this.line1);
            this.Controls.Add(this._previousButton);
            this.Controls.Add(this._nextButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "WizardDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "WizardDialog";
            this.ResumeLayout(false);

        }

        #endregion
        private DevAge.Windows.Forms.Line line1;
        private System.Windows.Forms.Panel _contentPanel;
        internal System.Windows.Forms.Button _nextButton;
        internal System.Windows.Forms.Button _previousButton;
        private LoadingCircle loadingCircle1;
    }
}
