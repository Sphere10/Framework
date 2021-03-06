//-----------------------------------------------------------------------
// <copyright file="TestArtificialKeysForm.cs" company="Sphere 10 Software">
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

namespace Sphere10.FrameworkTester {
	public partial class TestArtificialKeysForm : Form {
		public TestArtificialKeysForm() {
			InitializeComponent();
		}

		private void _testButton_Click(object sender, EventArgs e) {
			try {
				var artificialKeys =
					#region Create test object
					 new ArtificialKeys {
						 Tables = new[] {
							new ArtificialKeys.Table() {
								Name = "Table1",
                      			PrimaryKey = new ArtificialKeys.PrimaryKey {
									Name = "PK1",
									AutoIncrement = true,
									Columns = new [] {
										new ArtificialKeys.Column {
											Name = "ID"
										}
									}
								},
                      			ForeignKeys = new[] {
									new ArtificialKeys.ForeignKey {
										Name = "PK1",
										ReferenceTable = "Table2",
										Columns = new [] {
											new ArtificialKeys.Column {
												Name = "ID"
											}
										}
									}
								},
								UniqueConstraints = new[] {
									new ArtificialKeys.UniqueConstraint {
										Name = "UC1",
	                             		Columns = new [] {
		                             		new ArtificialKeys.Column {
												Name = "X"			                             		                          
											},	                 
		                             		new ArtificialKeys.Column {
			                             		Name = "Y"                 
											},	                 

										}
									}                   
								},
							},
							new ArtificialKeys.Table() {
								Name = "Table2",
                      			PrimaryKey = new ArtificialKeys.PrimaryKey {
									Name = "PK1",
									Sequence = "Sequence1",
									Columns = new [] {
										new ArtificialKeys.Column {
											Name = "A"
										},
										new ArtificialKeys.Column {
											Name = "B"
										},
										new ArtificialKeys.Column {
											Name = "C"
										}
									}
								}
							}
						}
					};
					#endregion

				var serialized = XmlProvider.WriteToString(artificialKeys);

				var deserialized = XmlProvider.ReadFromString<ArtificialKeys>(serialized);

				textBox1.Clear();
				textBox1.AppendText(serialized);

				var reserialized = XmlProvider.WriteToString(deserialized);
				if (reserialized != serialized ) {
					textBox1.AppendText("Deserialization did not match - " + Environment.NewLine);
					textBox1.AppendText(reserialized);
				}
			} catch(Exception error) {
				textBox1.Clear();
				textBox1.AppendText(error.ToDiagnosticString());
			}
		}
	}
}
//<ArtificialKeys>
//    <Table name = "Table1">
//        <PrimaryKey name="PK1" autoIncrement="true">
//            <Column name="ID"/>
//        </PrimaryKey>
//	
//        <ForeignKey name="FK1" foreignKeyTable="Table2">
//            <Column name="A" linksTo="U">
//            <Column name="B" linksTo="V">
//            <Column name="C" linksTo="W">
//        </ForeignKey>
//
//        <UniqueConstraint name="UC1">
//            <Column>X</Column>
//            <Column>Y</Column>
//        </UniqueConstraint>
//    </Table>
//
//    <Table name="Table2">
//        <PrimaryKey name="PK1" sequence="Sequence1">
//            <Column name="A"/>
//        </PrimaryKey>
//    </Table>
//
//</AritificialKeys>
