//-----------------------------------------------------------------------
// <copyright file="GenericEditorForm.Designer.cs" company="Sphere 10 Software">
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
    partial class GenericEditorForm {
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
            this._propertyGrid = new System.Windows.Forms.PropertyGrid();
            this._closeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _propertyGrid
            // 
            this._propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._propertyGrid.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this._propertyGrid.Location = new System.Drawing.Point(0, 0);
            this._propertyGrid.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this._propertyGrid.Name = "_propertyGrid";
            this._propertyGrid.Size = new System.Drawing.Size(818, 596);
            this._propertyGrid.TabIndex = 0;
            // 
            // _closeButton
            // 
            this._closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._closeButton.Location = new System.Drawing.Point(644, 608);
            this._closeButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this._closeButton.Name = "_closeButton";
            this._closeButton.Size = new System.Drawing.Size(150, 44);
            this._closeButton.TabIndex = 1;
            this._closeButton.Text = "&Close";
            this._closeButton.UseVisualStyleBackColor = true;
            this._closeButton.Click += new System.EventHandler(this._closeButton_Click);
            // 
            // GenericEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(818, 675);
            this.Controls.Add(this._closeButton);
            this.Controls.Add(this._propertyGrid);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "GenericEditorForm";
            this.Text = "Object Editor";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid _propertyGrid;
        private System.Windows.Forms.Button _closeButton;
    }
}
