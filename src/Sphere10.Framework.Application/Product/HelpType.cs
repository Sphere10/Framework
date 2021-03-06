//-----------------------------------------------------------------------
// <copyright file="HelpType.cs" company="Sphere 10 Software">
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
using System.Reflection;

namespace Sphere10.Framework.Application {

    [Obfuscation(Exclude = true)]
    public enum HelpType : uint
    {
        None = 0x00000000,
        PDF = 0x00000001,
        CHM = 0x00000010,
        RTF = 0x00000100,
        URL = 0x00001000,
    }
}
