//-----------------------------------------------------------------------
// <copyright file="UpdateScope.cs" company="Sphere 10 Software">
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

namespace Sphere10.Windows.WinForms {
    internal class UpdateScope : IDisposable {
        private readonly bool _saveToDataSource;
        public UpdateScope(IUpdatable updatable, bool saveToDatasource) {
            _saveToDataSource = saveToDatasource;
            if (!updatable.Updating) {
                Updatable = updatable;
                Updatable.Updating = true;
            }
        }

        
        public IUpdatable Updatable { get; set; }

        public void Dispose() {
            if (Updatable != null) {
                Updatable.Updating = false;
                Updatable.NotifyStateChangedEvent(_saveToDataSource);
            }
        }

    }


}
