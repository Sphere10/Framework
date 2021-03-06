//-----------------------------------------------------------------------
// <copyright file="IBlockManager.cs" company="Sphere 10 Software">
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
using Sphere10.Framework.Application;

namespace Sphere10.Framework.Windows.Forms {

    public interface IBlockManager {
        void RegisterBlock(IApplicationBlock plugin);
        void UnregisterBlock(IApplicationBlock plugin);
        bool IsBlockRegistered(IApplicationBlock plugin);
        IEnumerable<IApplicationBlock> RegisteredBlocks { get; }
        void ExecuteMenuItem(IMenuItem menuItem);

    }

}
