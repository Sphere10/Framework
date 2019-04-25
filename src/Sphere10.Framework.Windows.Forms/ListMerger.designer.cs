//-----------------------------------------------------------------------
// <copyright file="ListMerger.designer.cs" company="Sphere 10 Software">
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
    partial class ListMerger {
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
            this._rightHeaderLabel = new System.Windows.Forms.Label();
            this._moveRightButton = new System.Windows.Forms.Button();
            this._moveLeftButton = new System.Windows.Forms.Button();
            this._leftHeaderLabel = new System.Windows.Forms.Label();
            this._rightListBox = new System.Windows.Forms.ListBox();
            this._leftListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // _rightHeaderLabel
            // 
            this._rightHeaderLabel.AutoEllipsis = true;
            this._rightHeaderLabel.Location = new System.Drawing.Point(217, 0);
            this._rightHeaderLabel.Name = "_rightHeaderLabel";
            this._rightHeaderLabel.Size = new System.Drawing.Size(183, 13);
            this._rightHeaderLabel.TabIndex = 29;
            this._rightHeaderLabel.Text = "_rightHeaderLabel";
            this._rightHeaderLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // _moveRightButton
            // 
            this._moveRightButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this._moveRightButton.Image = global::Sphere10.Common.Properties.Resources.RArrow;
            this._moveRightButton.Location = new System.Drawing.Point(188, 82);
            this._moveRightButton.Name = "_moveRightButton";
            this._moveRightButton.Size = new System.Drawing.Size(24, 24);
            this._moveRightButton.TabIndex = 3;
            this._moveRightButton.UseVisualStyleBackColor = true;
            this._moveRightButton.Click += new System.EventHandler(this._moveRightButton_Click);
            // 
            // _moveLeftButton
            // 
            this._moveLeftButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this._moveLeftButton.Image = global::Sphere10.Common.Properties.Resources.LArrow;
            this._moveLeftButton.Location = new System.Drawing.Point(188, 52);
            this._moveLeftButton.Name = "_moveLeftButton";
            this._moveLeftButton.Size = new System.Drawing.Size(24, 24);
            this._moveLeftButton.TabIndex = 2;
            this._moveLeftButton.UseVisualStyleBackColor = true;
            this._moveLeftButton.Click += new System.EventHandler(this._moveLeftButton_Click);
            // 
            // _leftHeaderLabel
            // 
            this._leftHeaderLabel.AutoEllipsis = true;
            this._leftHeaderLabel.Location = new System.Drawing.Point(0, 0);
            this._leftHeaderLabel.Name = "_leftHeaderLabel";
            this._leftHeaderLabel.Size = new System.Drawing.Size(181, 13);
            this._leftHeaderLabel.TabIndex = 25;
            this._leftHeaderLabel.Text = "_leftHeaderLabel";
            this._leftHeaderLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // _rightListBox
            // 
            this._rightListBox.FormattingEnabled = true;
            this._rightListBox.Location = new System.Drawing.Point(217, 14);
            this._rightListBox.Name = "_rightListBox";
            this._rightListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this._rightListBox.Size = new System.Drawing.Size(183, 134);
            this._rightListBox.TabIndex = 4;
            this._rightListBox.SelectedIndexChanged += new System.EventHandler(this._rightListBox_SelectedIndexChanged);
            // 
            // _leftListBox
            // 
            this._leftListBox.FormattingEnabled = true;
            this._leftListBox.Location = new System.Drawing.Point(0, 14);
            this._leftListBox.Name = "_leftListBox";
            this._leftListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this._leftListBox.Size = new System.Drawing.Size(183, 134);
            this._leftListBox.TabIndex = 1;
            this._leftListBox.SelectedIndexChanged += new System.EventHandler(this._leftListBox_SelectedIndexChanged);
            // 
            // ListMerger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._rightHeaderLabel);
            this.Controls.Add(this._moveRightButton);
            this.Controls.Add(this._moveLeftButton);
            this.Controls.Add(this._leftHeaderLabel);
            this.Controls.Add(this._rightListBox);
            this.Controls.Add(this._leftListBox);
            this.Name = "ListMerger";
            this.Size = new System.Drawing.Size(401, 154);
            this.Resize += new System.EventHandler(this.ListMerger_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label _rightHeaderLabel;
        private System.Windows.Forms.Button _moveRightButton;
        private System.Windows.Forms.Button _moveLeftButton;
        private System.Windows.Forms.Label _leftHeaderLabel;
        private System.Windows.Forms.ListBox _rightListBox;
        private System.Windows.Forms.ListBox _leftListBox;



    }
}
