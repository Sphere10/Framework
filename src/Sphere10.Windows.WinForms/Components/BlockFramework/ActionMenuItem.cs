//-----------------------------------------------------------------------
// <copyright file="ActionMenuItem.cs" company="Sphere 10 Software">
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
using System.Text;
using System.Drawing;

namespace Sphere10.Framework.WinForms {
    public class ActionMenuItem : MenuItem, ILinkMenuItem
    {
		public ActionMenuItem(Action onClick)
			: this(string.Empty, onClick) {
        }

		public ActionMenuItem(string text, Action onClick) {
			Text = text;
			OnClick = onClick;
        }

		public ActionMenuItem(string text, Image image16x16, Action OnClick)
            : this(text, image16x16, true, true, false) {
        }

		public ActionMenuItem(
			string text,
			Image image16x16,
			bool showOnExplorerBar = true,
			bool showOnToolBar = true,
			bool executeOnLoad = false
		) : base(image16x16, showOnExplorerBar, showOnToolBar, executeOnLoad)  {
			Text = text;
        }

		public Action OnClick { get; set; }

        public virtual string Text { get; set; }

        public virtual void OnSelect() {
        }

        public override void Dispose() {
            base.Dispose();
        }

    }
}
