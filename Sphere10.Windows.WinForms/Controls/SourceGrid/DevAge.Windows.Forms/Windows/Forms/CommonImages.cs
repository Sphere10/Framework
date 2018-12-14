//-----------------------------------------------------------------------
// <copyright file="CommonImages.cs" company="Sphere 10 Software">
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
using System.Drawing;

namespace DevAge.Windows.Forms
{
	public class Resources
	{
        static Resources()
        {
            using (System.IO.MemoryStream mem = new System.IO.MemoryStream(Properties.Resources.CURSOR_Right))
            {
                mRightArrow = new System.Windows.Forms.Cursor(mem);
            }
            using (System.IO.MemoryStream mem = new System.IO.MemoryStream(Properties.Resources.CURSOR_Left))
            {
                mLeftArrow = new System.Windows.Forms.Cursor(mem);
            }
        }

        public static Icon IconSortDown
		{
			get{return Properties.Resources.IEW_SortDown;}
		}
        public static Icon IconSortUp
		{
            get { return Properties.Resources.IEW_SortUp; }
		}

        private static System.Windows.Forms.Cursor mRightArrow;
        public static System.Windows.Forms.Cursor CursorRightArrow
        {
            get { return mRightArrow; }
        }
        private static System.Windows.Forms.Cursor mLeftArrow;
        public static System.Windows.Forms.Cursor CursorLeftArrow
        {
            get { return mLeftArrow; }
        }
	}
}
