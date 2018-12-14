//-----------------------------------------------------------------------
// <copyright file="TextAreaTests.cs" company="Sphere 10 Software">
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
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;
using Sphere10.Framework;
using Sphere10.Framework.Data;
using Sphere10.Windows;
using Sphere10.Windows.WinForms;

namespace Sphere10.Framework.WinForms.TestUtil {
	public partial class TextAreaTests : Form {
        public TextAreaTests() {
			InitializeComponent();
		}
        private void _fillStandardButton_Click(object sender, EventArgs e) {
            try {
                _standardTextBox.Clear();
                var start = DateTime.Now;
                long genSize = 0;
                while (genSize < 1000000) {
                    var randomString = Tools.Text.GenerateRandomString(50) + Environment.NewLine;
                    genSize += randomString.Length*sizeof (char);
                    _standardTextBox.AppendText(randomString);
                }
                DialogEx.Show(this, SystemIconType.Information, "Results", "{0} bytes took {1} seconds".FormatWith(genSize, DateTime.Now.Subtract(start).TotalSeconds));
            } catch (Exception error) {
                ExceptionDialog.Show(error);
            }
        }

        private void _fillLockedButton_Click(object sender, EventArgs e) {
            //try {
            //    _standardTextBox.Clear();
            //    var start = DateTime.Now;
            //    long genSize = 0;
            //    if (!WinAPI.USER32.LockWindowUpdate(_lockTextBox.Handle)) {
            //        throw new Exception("Unable to lock");
            //    }
            //    while (genSize < 1000000) {
            //        var randomString = Tools.Text.GenerateRandomString(50) + Environment.NewLine;
            //        genSize += randomString.Length * sizeof(char);
            //        _lockTextBox.AppendText(randomString);
            //    }
            //    if (!WinAPI.USER32.LockWindowUpdate(IntPtr.Zero)) {
            //        throw new Exception("Unable to unlock");
            //    }

            //    DialogEx.Show(this, SystemIconType.Information, "Results", "{0} bytes took {1} seconds".FormatWith(genSize, DateTime.Now.Subtract(start).TotalSeconds));
            //} catch (Exception error) {
            //    ExceptionDialog.Show(error);
            //}
        }

        private void _genOnlyButton_Click(object sender, EventArgs e) {
            try {
                _standardTextBox.Clear();
                var start = DateTime.Now;
                long genSize = 0;
                while (genSize < 1000000) {
                    var randomString = Tools.Text.GenerateRandomString(50) + Environment.NewLine;
                    genSize += randomString.Length * sizeof(char);
                }
                DialogEx.Show(this, SystemIconType.Information, "Results", "{0} bytes took {1} seconds".FormatWith(genSize, DateTime.Now.Subtract(start).TotalSeconds));
            } catch (Exception error) {
                ExceptionDialog.Show(error);
            }
        }

        private void _fillLockedAsyncButton_Click(object sender, EventArgs e) {
            try {
                //_standardTextBox.Clear();
                //var start = DateTime.Now;
                //long genSize = 0;
                //if (!WinAPI.USER32.LockWindowUpdate(_lockTextBox.Handle)) {
                //    throw new Exception("Unable to lock");
                //}
                //while (genSize < 1000000) {                    
                //    var randomString = Tools.Text.GenerateRandomString(50) + Environment.NewLine;
                //    genSize += randomString.Length * sizeof(char);
                //    _lockTextBox.AppendText(randomString);
                //}
                //if (!WinAPI.USER32.LockWindowUpdate(IntPtr.Zero)) {
                //    throw new Exception("Unable to unlock");
                //}

                //DialogEx.Show(this, SystemIconType.Information, "Results", "{0} bytes took {1} seconds".FormatWith(genSize, DateTime.Now.Subtract(start).TotalSeconds));
            } catch (Exception error) {
                ExceptionDialog.Show(error);
            }
        }

    }
}
