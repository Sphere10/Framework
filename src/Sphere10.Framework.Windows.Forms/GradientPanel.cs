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

namespace Sphere10.Common {
    public class GradientPanel : Panel {
        private Color _fromColor;
        private Color _toColor;
        private float _angle;
        private Blend _blend;

        public Blend Blend {
            get { return _blend; }
            set { _blend = value; }
        }

        public GradientPanel() : this(Color.RoyalBlue, Color.LightBlue, 0) {
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        public GradientPanel(Color fromColor, Color toColor, float angle)
            : this(fromColor, toColor, angle, null) {
        }

        public GradientPanel(Color fromColor, Color toColor, float angle, Blend blend) {
            FromColor = fromColor;
            ToColor = toColor;
            Angle = angle;
            Blend = blend;
        }

        public float Angle
        {
          get { return _angle; }
          set { _angle = value; }
        }

        public Color FromColor {
            get { return _fromColor; }
            set { _fromColor = value; }
        }


        public Color ToColor {
            get { return _toColor; }
            set { _toColor = value; }
        }


        protected override void OnPaintBackground(PaintEventArgs e) {
            //base.OnPaintBackground(e);
            
            if (FromColor != Color.Empty || ToColor != Color.Empty)
            {
                if (FromColor == ToColor)
                {
                    using (Brush brush = new SolidBrush(FromColor)) {
                        e.Graphics.FillRectangle(brush, e.ClipRectangle);
                    }
                }
                else
                {
                    using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, FromColor, ToColor, Angle))
                    {
                        if (Blend != null)
                        {
                            brush.Blend = Blend;
                        }

                        e.Graphics.FillRectangle(brush, e.ClipRectangle);
                    }
                }
            }
        }


    }
}
