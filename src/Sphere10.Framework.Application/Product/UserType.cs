// <copyright file="UserType.cs" company="Sphere 10 Software">
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
using System.Reflection;
using System.Runtime.Serialization;

namespace Sphere10.Framework.Application {

    [Obfuscation(Exclude = true)]
    public enum UserType {
		System,
        HomeUser,
        SmallBusiness,
        MediumBusiness,
        Corporation
    }
}
