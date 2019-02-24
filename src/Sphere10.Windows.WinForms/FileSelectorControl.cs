//-----------------------------------------------------------------------
// <copyright file="FileSelectorControl.cs" company="Sphere 10 Software">
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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Sphere10.Common {
    public partial class FileSelectorControl : UserControl {
        public FileSelectorControl() {
            InitializeComponent();
        }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Filename {
            get {
                return _filenameTextBox.Text;
            }
            set {
                _filenameTextBox.Text = value;
            }
        }

        private void _fileSelectorButton_Click(object sender, EventArgs e) {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.CheckFileExists = true;
            dlg.CheckPathExists = true;
            dlg.Multiselect = false;
            if (dlg.ShowDialog(this) == DialogResult.OK) {
                Filename = dlg.FileName;
            }
        }


        protected override void OnEnabledChanged(EventArgs e) {
            base.OnEnabledChanged(e);
            _filenameTextBox.Enabled =
                _fileSelectorButton.Enabled = this.Enabled;
        }


    }
}
