//-----------------------------------------------------------------------
// <copyright file="GradientPanel.cs" company="Sphere 10 Software">
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
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using Sphere10.Framework;


namespace Sphere10.Windows.WinForms {
    public class GradientPanel : Panel {
    	
    	public GradientPanel() : this(Color.RoyalBlue, Color.LightBlue, 0) {
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

		public GradientPanel(Color fromColor, Color toColor, int angle)
            : this(fromColor, toColor, angle, null) {
        }

        public GradientPanel(Color fromColor, Color toColor, int angle, Blend blend) {
            FromColor = fromColor;
            ToColor = toColor;
            Angle = angle;
            Blend = blend;
        }

		public Blend Blend { get; set; }

    	public int Angle { get; set; }

    	public Color FromColor { get; set; }


    	public Color ToColor { get; set; }


    	protected override void OnPaintBackground(PaintEventArgs e) {
            //base.OnPaintBackground(e);
            if (FromColor != Color.Empty || ToColor != Color.Empty)
            {
				e.Graphics.GradientFillAtAngle(this.ClientRectangle, FromColor, ToColor, Angle);
				//if (FromColor == ToColor)
				//{
				//    using (Brush brush = new SolidBrush(FromColor)) {
				//        e.Graphics.FillRectangle(brush, e.ClipRectangle);
				//    }
				//}
				//else
				//{
				//    using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, FromColor, ToColor, Angle))
				//    {
				//        if (Blend != null)
				//        {
				//            brush.Blend = Blend;
				//        }

				//        e.Graphics.FillRectangle(brush, e.ClipRectangle);
				//    }
				//}
            }
        }


    }
}
