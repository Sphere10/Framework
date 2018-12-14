//-----------------------------------------------------------------------
// <copyright file="GST.cs" company="Sphere 10 Software">
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

namespace Sphere10.Framework.Misc {
    /// <summary>
    /// Simple GST class to execute gst related function
    /// </summary>
    public static class GST {

        public static double AddGST(int cents) {
            return Math.Round((double)cents * 1.1);
        }


        public static double RemoveGST(int cents) {
            return Math.Round((double)cents / 1.1);
        }

    }
}
