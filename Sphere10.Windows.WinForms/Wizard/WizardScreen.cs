//-----------------------------------------------------------------------
// <copyright file="WizardScreen.cs" company="Sphere 10 Software">
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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sphere10.Framework;

namespace Sphere10.Windows.WinForms {
    public class                                                                                                                          WizardScreen<T> : UserControlEx, IWizardScreen<T> {
        public IWizardManager<T> Wizard { get; internal set; }

        public virtual async Task Initialize() {            
        }

        public virtual async Task OnPresent() {
            
        }

        public virtual async Task OnPrevious() {            
        }
		
        public virtual async Task OnNext() {            
        }

        public virtual async Task<Result> Validate() {
            return Result.Default;
        }

    }
}
