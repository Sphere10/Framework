//-----------------------------------------------------------------------
// <copyright file="RectangleFExtensions.cs" company="Sphere 10 Software">
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

using System.Drawing;
#if !__IOS__ && !__ANDROID__
using System.Drawing.Drawing2D;
#endif

namespace Sphere10.Framework {

	public static class RectangleFFExtensions {

		#region Positions & Dimensions

		public static PointF TopLeft(this RectangleF RectangleF) {
			return new PointF(RectangleF.Left, RectangleF.Top);
		}

		public static PointF TopRight(this RectangleF RectangleF) {
			return new PointF(RectangleF.Right, RectangleF.Top);
		}

		public static PointF TopCenter(this RectangleF rect) {
			return new PointF(rect.CenterX(), rect.Top);
		}

		public static PointF BottomLeft(this RectangleF RectangleF) {
			return new PointF(RectangleF.Left, RectangleF.Bottom);
		}

		public static PointF BottomCenter(this RectangleF rect) {
			return new PointF(rect.CenterX(), rect.Bottom);
		}

		public static PointF BottomRight(this RectangleF RectangleF) {
			return new PointF(RectangleF.Right, RectangleF.Bottom);
		}

		public static float CenterX(this RectangleF rect) {
			return rect.Left + rect.Width / 2;
		}

		public static float CenterY(this RectangleF rect) {
			return rect.Top + rect.Height / 2;
		}

		public static PointF CenterLeft(this RectangleF rect) {
			return new PointF(rect.Left, rect.CenterY());
		}

		public static PointF CenterCenter(this RectangleF rect) {
			return new PointF(rect.CenterX(), rect.CenterY());
		}

		public static PointF CenterRight(this RectangleF rect) {
			return new PointF(rect.Right, rect.CenterY());
		}

		public static float AbsoluteWidth(this RectangleF orgRect) {
			return orgRect.X + orgRect.Width;
		}
		public static float AbsoluteHeight(this RectangleF orgRect) {
			return orgRect.Y + orgRect.Height;
		}

		#endregion

		#region Intersections

		public static RectangleF IntersectWith(this RectangleF rect, float x, float y, float width, float height) {
			return rect.IntersectWith(new RectangleF(x, y, width, height));
		}

		public static RectangleF IntersectWith(this RectangleF rect, RectangleF other) {
			return RectangleF.Intersect(rect, other);
		}

		#endregion

		#region Setting Position & Dimensions

		public static RectangleF SetLocation(this RectangleF orgRect, PointF PointF) {
			orgRect.Location = PointF;
			return orgRect;
		}

		public static RectangleF SetLocation(this RectangleF orgRect, float x, float y) {
			orgRect.Location = new PointF(x, y);
			return orgRect;
		}
		public static RectangleF SetLocation(this RectangleF orgRect, SizeF PointF) {
			orgRect.Location = new PointF(PointF.Width, PointF.Height);
			return orgRect;
		}

		public static RectangleF SetSizeF(this RectangleF orgRect, SizeF SizeF) {
			orgRect.Size = SizeF;
			return orgRect;
		}

		public static RectangleF SetSizeF(this RectangleF orgRect, float width, float height) {
			orgRect.Size = new SizeF(width, height); ;
			return orgRect;
		}

		public static RectangleF SetHeight(this RectangleF orgRect, float height) {
			orgRect.Height = height;
			return orgRect;
		}

		public static RectangleF SetWidth(this RectangleF orgRect, float width) {
			orgRect.Width = width;
			return orgRect;
		}

		#endregion

		#region Translations & Expansions

		public static RectangleF Translate(this RectangleF orgRect, float x, float y) {
			orgRect.X += x;
			orgRect.Y += y;
			return orgRect;
		}

		public static RectangleF Translate(this RectangleF orgRect, PointF PointF) {
			orgRect.Location = orgRect.Location.Add(PointF);
			return orgRect;
		}

		public static RectangleF TranslateNegative(this RectangleF orgRect, float x, float y) {
			orgRect.X -= x;
			orgRect.Y -= y;
			return orgRect;
		}

		public static RectangleF TranslateNegative(this RectangleF orgRect, PointF PointF) {
			orgRect.Location = orgRect.Location.Subtract(PointF);
			return orgRect;
		}

		public static RectangleF ExpandBy(this RectangleF orgRect, SizeF SizeF) {
			orgRect.Size = orgRect.Size.ExpandBy(SizeF);
			return orgRect;
		}

		public static RectangleF ExpandBy(this RectangleF orgRect, float width, float height) {
			orgRect.Width += width;
			orgRect.Height += height;
			return orgRect;
		}

		public static RectangleF ShrinkBy(this RectangleF orgRect, float width, float height) {
			orgRect.Width -= width;
			orgRect.Height -= height;
			return orgRect;
		}

		public static RectangleF ShrinkBy(this RectangleF orgRect, SizeF SizeF) {
			orgRect.Size = orgRect.Size.ShrinkBy(SizeF);
			return orgRect;
		}

		public static RectangleF TranslateAndExpandBy(this RectangleF orgRect, PointF PointF, SizeF SizeF) {
			orgRect.Location = orgRect.Location.Add(PointF);
			orgRect.Size = orgRect.Size.ExpandBy(SizeF);
			return orgRect;
		}

		public static RectangleF TranslateAndExpandBy(this RectangleF orgRect, float x, float y, float width, float height) {
			orgRect.X += x;
			orgRect.Y += y;
			orgRect.Width += width;
			orgRect.Height += height;
			return orgRect;
		}

		public static RectangleF TranslateNegativeAndShrinkBy(this RectangleF orgRect, float x, float y, float width, float height) {
			orgRect.X -= x;
			orgRect.Y -= y;
			orgRect.Width -= width;
			orgRect.Height -= height;
			return orgRect;
		}

		public static RectangleF TranslateNegativeAndShrinkBy(this RectangleF orgRect, PointF PointF, SizeF SizeF) {
			orgRect.Location = orgRect.Location.Subtract(PointF);
			orgRect.Size = orgRect.Size.ShrinkBy(SizeF);
			return orgRect;
		}

		public static RectangleF AsWideRectangleF(this RectangleF rect) {
			bool isLong = rect.Height > rect.Width;
			return new RectangleF(
				rect.X,
				rect.Y,
				isLong ? rect.Height : rect.Width,
				isLong ? rect.Width : rect.Height
			);
		}

		public static RectangleF AsLongRectangleF(this RectangleF rect) {
			bool isWide = rect.Width > rect.Height;
			return new RectangleF(
				rect.X,
				rect.Y,
				isWide ? rect.Height : rect.Width,
				isWide ? rect.Width : rect.Height
			);
		}

		#endregion

		#region Conversions

		public static Rectangle ToRectangle(this RectangleF rectangle) {
			return new Rectangle(rectangle.Location.ToPoint(), rectangle.Size.ToSize());
		}

		#endregion

		#region Misc

#if !__IOS__ && !__ANDROID__
		public static GraphicsPath GetRoundPath(this RectangleF r, float depth) {
			GraphicsPath graphPath = new GraphicsPath();
			graphPath.AddArc(r.X, r.Y, depth, depth, 180, 90);
			graphPath.AddArc(r.X + r.Width - depth, r.Y, depth, depth, 270, 90);
			graphPath.AddArc(r.X + r.Width - depth, r.Y + r.Height - depth, depth, depth, 0, 90);
			graphPath.AddArc(r.X, r.Y + r.Height - depth, depth, depth, 90, 90);
			graphPath.AddLine(r.X, r.Y + r.Height - depth, r.X, r.Y + depth / 2);
			return graphPath;
		}
#endif

		#endregion
	}

}
