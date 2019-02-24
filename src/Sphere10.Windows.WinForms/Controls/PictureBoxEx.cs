//-----------------------------------------------------------------------
// <copyright file="PictureBoxEx.cs" company="Sphere 10 Software">
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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sphere10.Windows.WinForms {
	public class PictureBoxEx : PictureBox {
		private SystemIconType _systemIcon;

		public PictureBoxEx() {
			SystemIcon = SystemIconType.None;
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);

		}

		protected override void OnSizeChanged(EventArgs e) {
			base.OnSizeChanged(e);
			SetSystemImage();
		}

		public SystemIconType SystemIcon {
			get {
				return _systemIcon;
			}
			set {
				_systemIcon = value;
				if (_systemIcon != SystemIconType.None)
					SetSystemImage();
			}
		}

		private void SetSystemImage() {
			Icon icon;
			switch(_systemIcon) {
				case SystemIconType.None:
					icon = null;
					break;
				case SystemIconType.Application:
					icon = new Icon(SystemIcons.Application, Size);
					break;
				case SystemIconType.Asterisk:
					icon = new Icon(SystemIcons.Asterisk, Size);
					break;
				case SystemIconType.Error:
					icon = new Icon(SystemIcons.Error, Size);
					break;
				case SystemIconType.Exclamation:
					icon = new Icon(SystemIcons.Exclamation, Size);
					break;
				case SystemIconType.Hand:
					icon = new Icon(SystemIcons.Hand, Size);
					break;
				case SystemIconType.Information:
					icon = new Icon(SystemIcons.Information, Size);
					break;
				case SystemIconType.Question:
					icon = new Icon(SystemIcons.Question, Size);
					break;
				case SystemIconType.Warning:
					icon = new Icon(SystemIcons.Warning, Size);
					break;
				case SystemIconType.WinLogo:
					icon = new Icon(SystemIcons.WinLogo, Size);
					break;
				case SystemIconType.Shield:
					icon = new Icon(SystemIcons.Shield, Size);
					break;
				default:
					throw new Exception(string.Format("Unsupported SystemIconType '{0}'", _systemIcon));

			}
			if (icon != null)
				this.Image = icon.ToBitmap();

		}

	}

}


