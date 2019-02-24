//-----------------------------------------------------------------------
// <copyright file="Tool.cs" company="Sphere 10 Software">
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
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading;

namespace Sphere10.Common {
	public partial class Tool {

		public const float EPSILON = 0.0001f;


		public static int EuclideanDistance(int x0, int y0, int x1, int y1) {
			return (int)Math.Round(EuclideanDistance((double)x0, (double)y0, (double)x1, (double)y1),0);
		}

		public static float EuclideanDistance(float x0, float y0, float x1, float y1) {
			return (float)EuclideanDistance((double)x0, (double)y0, (double)x1, (double)y1);
		}

		public static double EuclideanDistance(double x0, double y0, double x1, double y1) {
			return Math.Sqrt(Math.Pow(x1 - x0, 2) + Math.Pow(y1 - y0, 2));
		}
	}
}
