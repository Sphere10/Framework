//-----------------------------------------------------------------------
// <copyright file="ImageResult.cs" company="Sphere 10 Software">
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
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;

namespace Sphere10.Framework.Web {
	/// <summary>
	/// Streams an Image into the output stream
	/// </summary>
	public class ImageResult : ActionResult {

		public Image Image { get; set; }

		public ImageFormat Format { get; set; }

		public HttpCacheability Cacheability { get; set; }


		
		public ImageResult(Image image) : this (image, ImageFormat.Jpeg)	{
		}

		public ImageResult(Image image, ImageFormat format) : this (image, format, HttpCacheability.ServerAndPrivate)	{
		}


		/// <summary>
		/// Stream as a jpeg
		/// </summary>
		/// <param name="image"></param>
		public ImageResult(Image image, ImageFormat imageFormat,  HttpCacheability cacheability) {
			this.Image = image;
			this.Format = imageFormat;
			this.Cacheability = cacheability;
		}


		public override void ExecuteResult(ControllerContext context) {
			try {
				HttpResponseBase response = context.HttpContext.Response;
				if (Format == ImageFormat.Jpeg) {
					response.ContentType = "image/jpeg";
				} else if (Format == ImageFormat.Png) {
					response.ContentType = "image/png";
				} else if (Format == ImageFormat.Gif) {
					response.ContentType = "image/gif";
				} else {
					throw (new Exception("Unsupported Image Format."));
				}
				response.Cache.SetCacheability(Cacheability);
				//response.Cache.SetLastModified()???
				Image.Save(response.OutputStream, Format);
			} finally {
				Image.Dispose();
			}
		}

	}
}
