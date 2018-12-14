//-----------------------------------------------------------------------
// <copyright file="DrawingTool.All.cs" company="Sphere 10 Software">
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

using Sphere10.Framework;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Tools {

    public partial class Drawing {

        /// <summary>
        /// Converts a string to a color
        /// </summary>
        /// <param name="col">The string to be converted</param>
        /// <returns>The converted Color</returns>
        public static Color ConvertStringToColor(string col) {
            if (col == null) {
                return Color.Empty;
            }

            string[] s = col.Split(new char[] {':'});

            if (s.Length != 4) {
                return Color.Empty;
            }

            return Color.FromArgb(Int32.Parse(s[0]), Int32.Parse(s[1]), Int32.Parse(s[2]), Int32.Parse(s[3]));
        }

        /// <summary>
        /// Interpolate the specified number of times between start and end color
        /// </summary>
        /// <param name="p_StartColor"></param>
        /// <param name="p_EndColor"></param>
        /// <param name="p_NumberOfGradients"></param>
        /// <returns></returns>
        public static Color[] CalculateColorGradient(Color p_StartColor, Color p_EndColor, int p_NumberOfGradients) {
            if (p_NumberOfGradients < 2)
                throw new ArgumentException("Invalid Number of gradients, must be 2 or more");
            Color[] l_Colors = new Color[p_NumberOfGradients];
            l_Colors[0] = p_StartColor;
            l_Colors[l_Colors.Length - 1] = p_EndColor;

            float l_IncrementA = ((float) (p_EndColor.A - p_StartColor.A))/(float) p_NumberOfGradients;
            float l_IncrementR = ((float) (p_EndColor.R - p_StartColor.R))/(float) p_NumberOfGradients;
            float l_IncrementG = ((float) (p_EndColor.G - p_StartColor.G))/(float) p_NumberOfGradients;
            float l_IncrementB = ((float) (p_EndColor.B - p_StartColor.B))/(float) p_NumberOfGradients;

            for (int i = 1; i < (l_Colors.Length - 1); i++) {
                l_Colors[i] = Color.FromArgb((int) (p_StartColor.A + l_IncrementA*(float) i),
                    (int) (p_StartColor.R + l_IncrementR*(float) i),
                    (int) (p_StartColor.G + l_IncrementG*(float) i),
                    (int) (p_StartColor.B + l_IncrementB*(float) i));
            }

            return l_Colors;
        }

        /// <summary>
        /// Calculate a darker or lighter color using the source specified.
        /// A light of 1 is White, a light of -1 is black. All the other values are an interpolation from the source color.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="light"></param>
        /// <returns></returns>
        public static Color CalculateLightDarkColor(Color source, float light)
        {
            if (light == 0)
                return source;
            if (light > 1 || light < -1)
                throw new ArgumentException("Must be between 1 and -1", "light");

            float _IncrementR, _IncrementG, _IncrementB;

            if (light < 0)
            {
                _IncrementR = ((float)(source.R)) / (float)100;
                _IncrementG = ((float)(source.G)) / (float)100;
                _IncrementB = ((float)(source.B)) / (float)100;
            }
            else
            {
                _IncrementR = ((float)(255 - source.R)) / (float)100;
                _IncrementG = ((float)(255 - source.G)) / (float)100;
                _IncrementB = ((float)(255 - source.B)) / (float)100;
            }

            int newR, newG, newB;

            newR = source.R + (int)(_IncrementR * light * (float)100);
            newG = source.G + (int)(_IncrementG * light * (float)100);
            newB = source.B + (int)(_IncrementB * light * (float)100);

            if (newR > 255)
                newR = 255;
            else if (newR < 0)
                newR = 0;
            if (newG > 255)
                newG = 255;
            else if (newG < 0)
                newG = 0;
            if (newB > 255)
                newB = 255;
            else if (newB < 0)
                newB = 0;

            return Color.FromArgb(source.A, newR, newG, newB);
        }

        /// <summary>
        /// Calculate the middle color between the start and the end color.
        /// </summary>
        /// <param name="p_StartColor"></param>
        /// <param name="p_EndColor"></param>
        /// <returns></returns>
        public static Color CalculateMiddleColor(Color p_StartColor, Color p_EndColor) {
            return CalculateColorGradient(p_StartColor, p_EndColor, 3)[1];
        }



        public static string ConvertColorToString(Color color) {
            return color.ToARGBString();
        }


        public static Color RandomColor(bool solid = true) {
            return Color.FromArgb(solid ? 255 : Tools.Maths.RandomNumberGenerator.Next(256), Tools.Maths.RandomNumberGenerator.Next(256), Tools.Maths.RandomNumberGenerator.Next(256));
        }
    }
}
