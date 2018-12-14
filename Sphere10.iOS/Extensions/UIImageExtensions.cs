//-----------------------------------------------------------------------
// <copyright file="UIImageExtensions.cs" company="Sphere 10 Software">
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
using System.IO;
using UIKit;
using Foundation;
using CoreGraphics;
using CoreGraphics;
using Sphere10.Framework;

namespace Sphere10.iOS {
    public static class UIImageExtensions {

        public static UIImage ToUIImage(this byte[] imageBuffer) {
            return Tools.Drawing.ToUIImage(imageBuffer);
        }

        public static UIImage Antialias(this UIImage image) {
            return Tools.Drawing.Antialias(image);
        }

        public static UIImage RemoveColor(this UIImage image, UIColor color, float tolerance) {
            return Tools.Drawing.RemoveColor(image, color, tolerance);
        }

        public static UIImage Crop(this UIImage image, CGRect section) {
            return Tools.Drawing.Crop(image, section);
        }

        public static UIImage Resize(
            this UIImage sourceImage,
            CGSize requestedSize,
            ResizeMethod resizeMethod = ResizeMethod.Stretch,
            ResizeAlignment resizeAlignment = ResizeAlignment.CenterCenter,
            bool antiAliasing = true,
            CGInterpolationQuality interpolationQuality = CGInterpolationQuality.High,
            UIColor paddingColor = null) {
            return Tools.Drawing.Resize(sourceImage, requestedSize, resizeMethod, resizeAlignment, antiAliasing, interpolationQuality, paddingColor);
        }

        public static UIImage ResizeAndDispose(
            this UIImage image,
            CGSize requestedSize,
            ResizeMethod resizeStrategy = ResizeMethod.Stretch,
            ResizeAlignment resizeAlignment = ResizeAlignment.CenterCenter,
            bool antiAliasing = true,
            CGInterpolationQuality interpolationQuality = CGInterpolationQuality.High,
            UIColor paddingColor = null) {
            return Tools.Drawing.ResizeAndDispose(image, requestedSize, resizeStrategy, resizeAlignment, antiAliasing, interpolationQuality, paddingColor);
        }


        public static UIImage Zoom(this UIImage image, float zoomFactor) {
            return image.Resize(
                new CGSize(image.Size.Width*zoomFactor, image.Size.Height*zoomFactor)
                );
        }

        public static UIImage ZoomAndDispose(this UIImage image, float zoomFactor) {
            using (var sourceImage = image) {
                return sourceImage.Zoom(zoomFactor);
            }
        }

        public static UIImage FocusZoom(
            this UIImage image,
            float zoomFactor,
            int focusX,
            int focusY,
            bool antiAliasing = true,
            CGInterpolationQuality interpolationQuality = CGInterpolationQuality.High,
            UIColor paddingColor = null) {

            if (paddingColor == null)
                paddingColor = UIColor.Clear;

            if (Math.Abs(zoomFactor - 0.0f) < Tools.Maths.EPSILON_F) {
                throw new ArgumentOutOfRangeException("zoomFactor", string.Format("Must not be 0 (epsilon tested with {0})", Tools.Maths.EPSILON_F));
            }
            var sourceHeight = (float) image.Size.Height/zoomFactor;
            var sourceWidth = (float) image.Size.Width/zoomFactor;
            var startX = (float) focusX - sourceWidth/2;
            var startY = (float) focusY - sourceHeight/2;

            var sourceRectangle = new CGRect(
                startX,
                startY,
                sourceWidth,
                sourceHeight
                )
                .IntersectWith(0, 0, image.Size.Width, image.Size.Height);
            var xClippedRatio = sourceRectangle.Width/sourceWidth;
            var yClippedRatio = sourceRectangle.Height/sourceHeight;

            var destRectangle = new CGRect(CGPoint.Empty, image.Size);
            destRectangle.Width *= xClippedRatio;
            destRectangle.Height *= yClippedRatio;

            UIImage result;
            using (var sourceImage = image.Crop(sourceRectangle)) {
                UIGraphics.BeginImageContext((CGSize)image.Size);
                var context = UIGraphics.GetCurrentContext();
                context.TranslateCTM(0, image.Size.Height);
                context.ScaleCTM(1f, -1f);
                context.InterpolationQuality = interpolationQuality;
                context.SetAllowsAntialiasing(antiAliasing);
                context.SetFillColor(paddingColor.CGColor);
                context.FillRect(new CGRect(CGPoint.Empty, (CGSize)image.Size));
                context.DrawImage(destRectangle, sourceImage.CGImage);
                result = UIGraphics.GetImageFromCurrentImageContext();
                UIGraphics.EndImageContext();
            }

            return result;
        }

        public static UIImage FocusZoomAndDispose(
            this UIImage image,
            float zoomFactor,
            int focusX,
            int focusY,
            bool antiAliasing = true,
            CGInterpolationQuality interpolationQuality = CGInterpolationQuality.High,
            UIColor paddingColor = null) {
            using (var sourceImage = image) {
                return sourceImage.FocusZoom(zoomFactor, focusX, focusY);
            }
        }

        /// <summary>
        /// Save an Image as a JPeg with a given compression
        /// Note: Filename suffix will not affect mime type which will be Jpeg.
        /// </summary>
        /// <param name="image">This image</param>
        /// <param name="fileName">File name to save the image as. Note: suffix will not affect mime type which will be Jpeg.</param>
        /// <param name="compression">Value between 0 and 100.</param>
        public static void SaveAsJpegWithCompression(this UIImage image, string fileName, int compression) {
            Tools.Drawing.SaveJpegWithCompression(image, fileName, compression);
        }

        /// <summary>
        /// Save an Image as a JPeg with a given compression
        /// Note: Filename suffix will not affect mime type which will be Jpeg.
        /// </summary>
        /// <param name="image">This image</param>
        /// <param name="stream">The stream where the image will be saved.</param>
        /// <param name="compression">Value between 0 and 100.</param>
        public static void SaveAsJpegWithCompression(this UIImage image, Stream stream, int compression) {
            Tools.Drawing.SaveJpegWithCompression(image, stream, compression);
        }

    }

}

