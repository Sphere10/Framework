//-----------------------------------------------------------------------
// <copyright file="DrawingTool.iOS.cs" company="Sphere 10 Software">
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

#if __IOS__
using Sphere10.Framework;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using CoreGraphics;
using Foundation;
using UIKit;


namespace Tools {

    public partial class Drawing {


        /// <summary>
        /// Save an Image as a JPeg with a given compression
        ///  Note: Filename suffix will not affect mime type which will be Jpeg.
        /// </summary>
        /// <param name="image">Image to save</param>
        /// <param name="fileName">File name to save the image as. Note: suffix will not affect mime type which will be Jpeg.</param>
        /// <param name="compression">Value between 0 and 100.</param>
        public static void SaveJpegWithCompression(UIImage image, string fileName, int compression) {
            if (!(0 <= compression && compression <= 100))
                throw new ArgumentOutOfRangeException("compression", "Must be between 0 and 100");

            using (var data = image.AsJPEG(compression/100.0f)) {
                File.WriteAllBytes(fileName, data.ToArray());
            }

        }

        /// <summary>
        /// Save an Image as a JPeg with a given compression
        ///  Note: Filename suffix will not affect mime type which will be Jpeg.
        /// </summary>
        /// <param name="image">Image to save</param>
        /// <param name="stream">The stream where the image will be saved.</param>
        /// <param name="compression">Value between 0 and 100.</param>
        public static void SaveJpegWithCompression(UIImage image, Stream stream, int compression) {
            if (!(0 <= compression && compression <= 100))
                throw new ArgumentOutOfRangeException("compression", "Must be between 0 and 100");
            using (var data = image.AsJPEG(compression/100.0f)) {
                var bytes = data.ToArray();
                stream.Write(data.ToArray(), 0, bytes.Length);
            }
        }

        public static UIImage ToUIImage(byte[] imageBuffer)
        {
            using (var imageData = NSData.FromArray(imageBuffer))
                return UIImage.LoadFromData(imageData);
        }

        public static UIImage Antialias(UIImage image)
        {
            return ResizeAndDispose(image, (CGSize)image.Size, ResizeMethod.Stretch, antiAliasing: true);
        }

        public static UIImage RemoveColor(UIImage image, UIColor color, float tolerance)
        {
            // convert color to RGBA (may be greyscale for example);
            nfloat r, g, b, a;
            color.GetRGBA(out r, out g, out b, out a);
            color = UIColor.FromRGBA(r, g, b, a);

            var imageRef = image.CGImage;

            var width = (int)imageRef.Width;
            var height = (int)imageRef.Height;
            var colorSpace = CGColorSpace.CreateDeviceRGB();

            var bytesPerPixel = 4;
            var bytesPerRow = bytesPerPixel * width;
            var bitsPerComponent = 8;
            var bitmapByteCount = bytesPerRow * height;

            byte[] rawData = new byte[bitmapByteCount];


            var context = new CGBitmapContext(
                rawData,
                width,
                height,
                bitsPerComponent,
                bytesPerRow,
                colorSpace,
                CGBitmapFlags.PremultipliedLast | CGBitmapFlags.ByteOrder32Big);

            colorSpace.Dispose();

            context.DrawImage(new CGRect(0, 0, width, height), imageRef);

            var cgColor = color.CGColor;
            var components = cgColor.Components;

            r = components[0];
            g = components[1];
            b = components[2];
            //float a = components[3]; // not needed

            r = r * 255.0f;
            g = g * 255.0f;
            b = b * 255.0f;

            var redRange = new[] {
                (nfloat) Math.Max(r - (tolerance/2.0f), 0.0f),
                (nfloat) Math.Min(r + (tolerance/2.0f), 255.0f)
            };

            var greenRange = new[] {
                (nfloat)Math.Max(g - (tolerance/2.0f), 0.0f),
                (nfloat)Math.Min(g + (tolerance/2.0f), 255.0f)
            };

            var blueRange = new[]{
                (nfloat)Math.Max(b - (tolerance/2.0f), 0.0f),
                (nfloat)Math.Min(b + (tolerance/2.0f), 255.0f)
            };

            int byteIndex = 0;

            while (byteIndex < bitmapByteCount)
            {
                var red = rawData[byteIndex];
                var green = rawData[byteIndex + 1];
                var blue = rawData[byteIndex + 2];

                if (((red >= redRange[0]) && (red <= redRange[1])) &&
                    ((green >= greenRange[0]) && (green <= greenRange[1])) &&
                    ((blue >= blueRange[0]) && (blue <= blueRange[1])))
                {
                    // make the pixel transparent
                    //
                    rawData[byteIndex] = 0;
                    rawData[byteIndex + 1] = 0;
                    rawData[byteIndex + 2] = 0;
                    rawData[byteIndex + 3] = 0;
                }

                byteIndex += 4;
            }

            var result = new UIImage(context.ToImage());
            context.Dispose();
            return result;
        }

        public static UIImage Crop(UIImage image, CGRect section)
        {
            UIGraphics.BeginImageContext(section.Size);
            var context = UIGraphics.GetCurrentContext();
            context.ClipToRect(new CGRect(0, 0, section.Width, section.Height));
            var drawRectangle = new CGRect(-section.X, -section.Y, image.Size.Width, image.Size.Height);
            image.Draw(drawRectangle);
            var croppedImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return croppedImage;
        }

        public static UIImage Resize(
            UIImage sourceImage,
            CGSize requestedSize,
            ResizeMethod resizeMethod = ResizeMethod.Stretch,
            ResizeAlignment resizeAlignment = ResizeAlignment.CenterCenter,
            bool antiAliasing = true,
            CGInterpolationQuality interpolationQuality = CGInterpolationQuality.High,
            UIColor paddingColor = null)
        {

            if (paddingColor == null)
                paddingColor = UIColor.Clear;

            var sourceImageSize = (CGSize)sourceImage.Size;
            var scaleWidth = ((float)requestedSize.Width / (float)sourceImageSize.Width);
            var scaleHeight = ((float)requestedSize.Height / (float)sourceImageSize.Height);
            CGSize destImageSize;
            CGRect sourceBlitRect = CGRect.Empty;
            CGRect destBlitRect = CGRect.Empty;
            bool alignSourceBlitRect = false;
            bool alignDestBlitRect = false;
            switch (resizeMethod)
            {
                case ResizeMethod.AspectFit:
                    sourceBlitRect = new CGRect(CGPoint.Empty, sourceImageSize);
                    scaleWidth = Math.Min(scaleWidth, scaleHeight);
                    scaleHeight = Math.Min(scaleWidth, scaleHeight);
                    destBlitRect.Width = sourceImageSize.Width * scaleWidth;
                    destBlitRect.Height = sourceImageSize.Height * scaleHeight;
                    destImageSize = destBlitRect.Size;

                    break;
                case ResizeMethod.AspectFitPadded:
                    sourceBlitRect = new CGRect(CGPoint.Empty, sourceImageSize);
                    scaleWidth = Math.Min(scaleWidth, scaleHeight);
                    scaleHeight = Math.Min(scaleWidth, scaleHeight);
                    destBlitRect.Width = sourceImageSize.Width * scaleWidth;
                    destBlitRect.Height = sourceImageSize.Height * scaleHeight;
                    destImageSize = requestedSize;
                    alignDestBlitRect = true;
                    break;
                case ResizeMethod.AspectFill:
                    var sourceAspect = sourceImageSize.Width / sourceImageSize.Height;
                    var destAspect = requestedSize.Width / requestedSize.Height;
                    if (destAspect > sourceAspect)
                    {
                        sourceBlitRect = new CGRect(0, 0, sourceImageSize.Width, sourceImageSize.Width / destAspect);
                        alignSourceBlitRect = true;
                    }
                    else if (destAspect < sourceAspect)
                    {
                        sourceBlitRect = new CGRect(0, 0, sourceImageSize.Height * destAspect, sourceImageSize.Height);
                        alignSourceBlitRect = true;
                    }
                    else
                    {
                        sourceBlitRect = new CGRect(CGPoint.Empty, sourceImageSize);
                    }
                    destBlitRect = new CGRect(CGPoint.Empty, requestedSize);
                    destImageSize = requestedSize;
                    break;
                case ResizeMethod.Stretch:
                default:
                    sourceBlitRect = new CGRect(CGPoint.Empty, sourceImageSize);
                    destBlitRect.Width = sourceImageSize.Width * scaleWidth;
                    destBlitRect.Height = sourceImageSize.Height * scaleHeight;
                    destImageSize = requestedSize;
                    break;
            }

            if (alignDestBlitRect)
            {
                switch (resizeAlignment)
                {
                    case ResizeAlignment.TopLeft:
                        destBlitRect.Offset(0, 0);
                        break;
                    case ResizeAlignment.TopCenter:
                        destBlitRect.Offset((destImageSize.Width - destBlitRect.Width) / 2, 0);
                        break;
                    case ResizeAlignment.TopRight:
                        destBlitRect.Offset(destImageSize.Width - destBlitRect.Width, 0);
                        break;
                    case ResizeAlignment.CenterLeft:
                        destBlitRect.Offset(0, (destImageSize.Height - destBlitRect.Height) / 2);
                        break;
                    case ResizeAlignment.CenterCenter:
                        destBlitRect.Offset((destImageSize.Width - destBlitRect.Width) / 2, (destImageSize.Height - destBlitRect.Height) / 2);
                        break;
                    case ResizeAlignment.CenterRight:
                        destBlitRect.Offset((destImageSize.Width - destBlitRect.Width), (destImageSize.Height - destBlitRect.Height) / 2);
                        break;
                    case ResizeAlignment.BottomLeft:
                        destBlitRect.Offset(0, (destImageSize.Height - destBlitRect.Height));
                        break;
                    case ResizeAlignment.BottomCenter:
                        destBlitRect.Offset((destImageSize.Width - destBlitRect.Width) / 2, destImageSize.Height - destBlitRect.Height);
                        break;
                    case ResizeAlignment.BottomRight:
                        destBlitRect.Offset(destImageSize.Width - destBlitRect.Width, destImageSize.Height - destBlitRect.Height);
                        break;
                    default:
                        break;
                }
            }

            if (alignSourceBlitRect)
            {
                switch (resizeAlignment)
                {
                    case ResizeAlignment.TopLeft:
                        sourceBlitRect.Offset(0, 0);
                        break;
                    case ResizeAlignment.TopCenter:
                        sourceBlitRect.Offset((sourceImageSize.Width - sourceBlitRect.Width) / 2, 0);
                        break;
                    case ResizeAlignment.TopRight:
                        sourceBlitRect.Offset(sourceImageSize.Width - sourceBlitRect.Width, 0);
                        break;
                    case ResizeAlignment.CenterLeft:
                        sourceBlitRect.Offset(0, (sourceImageSize.Height - sourceBlitRect.Height) / 2);
                        break;
                    case ResizeAlignment.CenterCenter:
                        sourceBlitRect.Offset((sourceImageSize.Width - sourceBlitRect.Width) / 2, (sourceImageSize.Height - sourceBlitRect.Height) / 2);
                        break;
                    case ResizeAlignment.CenterRight:
                        sourceBlitRect.Offset((sourceImageSize.Width - sourceBlitRect.Width), (sourceImageSize.Height - sourceBlitRect.Height) / 2);
                        break;
                    case ResizeAlignment.BottomLeft:
                        sourceBlitRect.Offset(0, sourceImageSize.Height - sourceBlitRect.Height);
                        break;
                    case ResizeAlignment.BottomCenter:
                        sourceBlitRect.Offset((sourceImageSize.Width - sourceBlitRect.Width) / 2, sourceImageSize.Height - sourceBlitRect.Height);
                        break;
                    case ResizeAlignment.BottomRight:
                        sourceBlitRect.Offset((sourceImageSize.Width - sourceBlitRect.Width), sourceImageSize.Height - sourceBlitRect.Height);
                        break;
                    default:
                        break;
                }
            }

            var imageToUse = alignSourceBlitRect ? Crop(sourceImage, sourceBlitRect) : sourceImage;
            UIGraphics.BeginImageContext(destImageSize);
            var context = UIGraphics.GetCurrentContext();
            //context.TranslateCTM(0, destImageSize.Height);
            //context.ScaleCTM(1f, -1f);
            context.InterpolationQuality = interpolationQuality;
            context.SetAllowsAntialiasing(antiAliasing);
            context.SetFillColor(paddingColor.CGColor);
            context.FillRect(new CGRect(CGPoint.Empty, destImageSize));
            //context.DrawImage(destBlitRect, imageToUse.CGImage);
            imageToUse.Draw(destBlitRect);
            UIImage resizedImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            if (imageToUse != sourceImage)
            {
                imageToUse.Dispose();
            }

            return resizedImage;

        }

        public static UIImage ResizeAndDispose(
            UIImage image,
            CGSize requestedSize,
            ResizeMethod resizeStrategy = ResizeMethod.Stretch,
            ResizeAlignment resizeAlignment = ResizeAlignment.CenterCenter,
            bool antiAliasing = true,
            CGInterpolationQuality interpolationQuality = CGInterpolationQuality.High,
            UIColor paddingColor = null)
        {
            using (var sourceImage = image)
            {
                return Resize(sourceImage, requestedSize, resizeStrategy, resizeAlignment, antiAliasing, interpolationQuality, paddingColor);
            }
        }


        public static UIImage Zoom(UIImage image, float zoomFactor)
        {
            return Resize(image,new CGSize(image.Size.Width * zoomFactor, image.Size.Height * zoomFactor));
        }

        public static UIImage ZoomAndDispose(UIImage image, float zoomFactor)
        {
            using (var sourceImage = image)
            {
                return Zoom(sourceImage, zoomFactor);
            }
        }

        public static UIImage FocusZoom(
            UIImage image,
            float zoomFactor,
            int focusX,
            int focusY,
            bool antiAliasing = true,
            CGInterpolationQuality interpolationQuality = CGInterpolationQuality.High,
            UIColor paddingColor = null)
        {

            if (paddingColor == null)
                paddingColor = UIColor.Clear;

            if (Math.Abs(zoomFactor - 0.0f) < Tools.Maths.EPSILON_F)
            {
                throw new ArgumentOutOfRangeException("zoomFactor", string.Format("Must not be 0 (epsilon tested with {0})", Tools.Maths.EPSILON_F));
            }
            var sourceHeight = (float)image.Size.Height / zoomFactor;
            var sourceWidth = (float)image.Size.Width / zoomFactor;
            var startX = (float)focusX - sourceWidth / 2;
            var startY = (float)focusY - sourceHeight / 2;

            var sourceRectangle = new CGRect(
                startX,
                startY,
                sourceWidth,
                sourceHeight
                )
                .IntersectWith(0, 0, image.Size.Width, image.Size.Height);
            var xClippedRatio = sourceRectangle.Width / sourceWidth;
            var yClippedRatio = sourceRectangle.Height / sourceHeight;

            var destRectangle = new CGRect(CGPoint.Empty, image.Size);
            destRectangle.Width *= xClippedRatio;
            destRectangle.Height *= yClippedRatio;

            UIImage result;
            using (var sourceImage = Crop(image,sourceRectangle))
            {
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
            UIImage image,
            float zoomFactor,
            int focusX,
            int focusY,
            bool antiAliasing = true,
            CGInterpolationQuality interpolationQuality = CGInterpolationQuality.High,
            UIColor paddingColor = null)
        {
            using (var sourceImage = image)
            {
                return FocusZoom(sourceImage, zoomFactor, focusX, focusY);
            }
        }

    }
}


#endif
