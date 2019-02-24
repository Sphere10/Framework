//-----------------------------------------------------------------------
// <copyright file="ActionWizardManager.cs" company="Sphere 10 Software">
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
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sphere10.Framework;

namespace Sphere10.Windows.WinForms {
    public class ActionWizardManager<T> : WizardManagerBase<T> {
        private readonly Func<T, Task<Result>> _finishFunc;
        private readonly Func<T, Result> _cancelFunc;

        public ActionWizardManager(string title, T propertyBag, IEnumerable<WizardScreen<T>> forms, Func<T, Task<Result>> finishFunc, Func<T, Result> cancelFunc = null)
            : base(title, propertyBag, forms) {
            _finishFunc = finishFunc;
            _cancelFunc = cancelFunc ?? ((x) => Result.Default);
        }

        public override Result CancelRequested() {
            return _cancelFunc(PropertyBag);
        }

        protected override async Task<Result> Finish() {
            return await _finishFunc(PropertyBag);
        }
    }

    public class ActionWizardManager : ActionWizardManager<IDictionary<string, object>> {
        public ActionWizardManager(string title, IDictionary<string, object> propertyBag, IEnumerable<WizardScreen<IDictionary<string, object>>> forms, Func<IDictionary<string, object>, Task<Result>> finishFunc, Func<IDictionary<string, object>, Result> cancelFunc = null)
            : base(title, propertyBag, forms, finishFunc, cancelFunc) {
            
        }

    }

}
