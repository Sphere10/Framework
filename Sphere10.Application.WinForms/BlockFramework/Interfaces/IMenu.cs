//-----------------------------------------------------------------------
// <copyright file="IMenu.cs" company="Sphere 10 Software">
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
using System.Drawing;
using System.Windows.Forms;
using Sphere10.Application;

namespace Sphere10.Application.WinForms {

    public interface IMenu : IDisposable {
        IApplicationBlock Parent { get; set; }

        string Text { get; }

        IMenuItem[] Items { get; }

        Image Image32x32 { get; }

        bool ShowInMenuStrip { get; }

    }
}
