//-----------------------------------------------------------------------
// <copyright file="ImageResizeForm.cs" company="Sphere 10 Software">
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sphere10.Framework;
using Sphere10.Framework.Windows.Forms;

namespace Sphere10.FrameworkTester {
    public partial class ImageResizeForm : Form {
        private readonly PictureBoxEx[,] Boxes;
        public ImageResizeForm() {
            InitializeComponent();
            Boxes = new[,] {
                {_pictureBoxEx11, _pictureBoxEx12, _pictureBoxEx13, _pictureBoxEx14, _pictureBoxEx15, _pictureBoxEx16, _pictureBoxEx17, _pictureBoxEx18, _pictureBoxEx19},
                {_pictureBoxEx21, _pictureBoxEx22, _pictureBoxEx23, _pictureBoxEx24, _pictureBoxEx25, _pictureBoxEx26, _pictureBoxEx27, _pictureBoxEx28, _pictureBoxEx29},
                {_pictureBoxEx31, _pictureBoxEx32, _pictureBoxEx33, _pictureBoxEx34, _pictureBoxEx35, _pictureBoxEx36, _pictureBoxEx37, _pictureBoxEx38, _pictureBoxEx39},
                {_pictureBoxEx41, _pictureBoxEx42, _pictureBoxEx43, _pictureBoxEx44, _pictureBoxEx45, _pictureBoxEx46, _pictureBoxEx47, _pictureBoxEx48, _pictureBoxEx49}
            };
        }

        private void _pathSelectorControl_FilenameSelected(object sender, EventArgs e) {
            RedrawBoxes();
        }



        private void RedrawBoxes() {
            if (!File.Exists(_pathSelectorControl.Path)) {
                return;
            }

            var original = Image.FromFile(_pathSelectorControl.Path);

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 9; j++)
                    Boxes[i, j].Image = original.Resize(
                        _pictureBoxEx11.Size,
                        (ResizeMethod) (i + 1),
                        (ResizeAlignment) (j + 1),
                        paddingColor: Color.Transparent
                        );
        }

        private void ResizeBoxes(int width, int height) {
            var sampleBox = Boxes[0, 0];
            var deltaWidth = width - sampleBox.Width;
            var deltaHeight = height - sampleBox.Height;

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 9; j++) {
                    var currLocation = Boxes[i, j].Location;
                    Boxes[i, j].Location = new Point(currLocation.X + j*deltaWidth, currLocation.Y + i*deltaHeight);
                    Boxes[i, j].Width += deltaWidth;
                    Boxes[i, j].Height += deltaHeight;
                }
        }

        private void _heightNumeric_ValueChanged(object sender, EventArgs e) {
            ResizeBoxes((int)_widthNumeric.Value, (int)_heightNumeric.Value);
            RedrawBoxes();
        }

        private void _widthNumeric_ValueChanged(object sender, EventArgs e) {
            ResizeBoxes((int)_widthNumeric.Value, (int)_heightNumeric.Value);
            RedrawBoxes();
        }


    }
}


        //StretchToFit,
        //CropAndZoomToFit,
        //PreserveAspectRatioWithPadding,
        //PreserveAspectRatioWithoutPadding,
