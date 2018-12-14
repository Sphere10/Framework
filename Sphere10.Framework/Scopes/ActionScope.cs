//-----------------------------------------------------------------------
// <copyright file="ActionScope.cs" company="Sphere 10 Software">
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
	public class ActionScope : IDisposable {
		public ActionScope(Action beginAction = null, Action endAction = null) 
            : this(
                s => {
                    if (beginAction != null)
                        beginAction();
                },
                s => {
                    if (endAction != null)
                        endAction();
                }                   
            )
        {
		}

        public ActionScope(Action<ActionScope> beginAction = null, Action<ActionScope> endAction = null) {
            BeginAction = beginAction;
            EndAction = endAction;
            if (BeginAction != null) {
                BeginAction(this);
            }
        }

	    public Action<ActionScope> BeginAction { get; private set; }

		public Action<ActionScope> EndAction { get; private set; }


		public void Dispose() {
			if (EndAction != null) {
				EndAction(this);
			}
		}

	}

    public class ActionScope<T> : ActionScope {
        public ActionScope(Action beginAction = null, Action endAction = null)
            : base(beginAction, endAction) {

        }

        public ActionScope(Action<ActionScope<T>> beginAction = null, Action<ActionScope<T>> endAction = null) 
            : base(
                s => {
                    if (beginAction != null)
                        beginAction((ActionScope<T>)s);
                },
                s => {
                    if (endAction != null)
                        endAction((ActionScope<T>)s);
                }                   
            ) {
        }


        public T Item { get; set; }
    }
}
