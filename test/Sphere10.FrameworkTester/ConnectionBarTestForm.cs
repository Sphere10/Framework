//-----------------------------------------------------------------------
// <copyright file="ConnectionBarTestForm.cs" company="Sphere 10 Software">
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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sphere10.Framework;
using Sphere10.Framework.Data;
using Sphere10.Framework.Windows.Forms;

namespace Sphere10.FrameworkTester {
	public partial class ConnectionBarTestForm : FormEx {
        public ConnectionBarTestForm() {
			InitializeComponent();
		}

	    protected override void PopulatePrimingData() {
	        _databaseConnectionBar.SelectedDBMSType = DBMSType.SQLServer;
	    }

	    private async void _testConnectionButton_Click(object sender, EventArgs e) {

            using (_loadingCircle.BeginAnimationScope()) {
                
                var result = await _databaseConnectionBar.TestConnection();
                _loadingCircle.StopAnimating();
                MessageBox.Show(this, 
                    result.Success ? "Success" : result.ErrorMessages.ToParagraphCase(), "Connection Test", MessageBoxButtons.OK, result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
            }
        }
	}
}
