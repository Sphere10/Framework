//-----------------------------------------------------------------------
// <copyright file="MergeableStatusStrip.cs" company="Sphere 10 Software">
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
using System.ComponentModel;
using System.Windows.Forms;

namespace Sphere10.Windows.WinForms {

	/// <summary>
	/// This component allows you to modify a base forms ToolStrip. 
	/// Microsoft has intentionally added this limitation due to the complexities it could not overcome. This component simply works around
	/// those limitations by allowing you to merge a (hidden) tool strip on the sub form with the readonly inherited base form.
	/// </summary>
	public class MergeableStatusStrip : StatusStrip {
		private ToolStripVisualInheritanceFixer _fixer;

		public MergeableStatusStrip() {
			_fixer = new ToolStripVisualInheritanceFixer();
			_fixer.ToolStrip = this;
		}

		protected override void InitLayout() {
			base.InitLayout();
			Visible = false;
		}


		/// <summary>
		/// The base form's menu strip.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Description("Set this to the base forms StatusStrip. If you can't find it, make sure it's modifer is set to to 'protected' or greater.")]
		[Category("Behavior")]
		public StatusStrip InheritedToolStrip { 
			get {
				return _fixer.InheritedToolStrip as StatusStrip;
			}
			set {
				_fixer.InheritedToolStrip = (value as StatusStrip);
			}
		}

	}
}
