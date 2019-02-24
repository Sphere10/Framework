//-----------------------------------------------------------------------
// <copyright file="ActionDisposable.cs" company="Sphere 10 Software">
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

namespace Sphere10.Framework {

  
    public sealed class ActionDisposable : Disposable {
		private readonly Action _disposeAction;

	    public ActionDisposable(Action disposeAction) {
			this._disposeAction = disposeAction;
	    }

        protected override void FreeManagedResources() {
			if (_disposeAction != null)
				_disposeAction();
        }
 
    }
}
