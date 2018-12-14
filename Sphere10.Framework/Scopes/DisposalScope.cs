//-----------------------------------------------------------------------
// <copyright file="DisposalScope.cs" company="Sphere 10 Software">
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

namespace Sphere10.Framework {


    /// <summary>
    /// A scope that carries with it a collection of disposable items. If not salvaged, the items are
    /// disposed at the end of the DisposalScope.
    /// </summary>
    public class DisposalScope : IDisposable {
        private readonly IDisposable[] _disposals;
        public DisposalScope(params IDisposable[] disposals) {
            _disposals = disposals;
        }

        public IDisposable Salvage(int index) {
            var item = _disposals[index];
            _disposals[index] = default(IDisposable);
            return item;
        }

        public void Dispose() {
            foreach (var toxin in _disposals) {
                if (toxin != null)
                    Tools.Exceptions.ExecuteIgnoringException(toxin.Dispose);
            }
        }
    }
}
