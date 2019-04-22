//-----------------------------------------------------------------------
// <copyright file="ApplicationLifecycleExtensions.cs" company="Sphere 10 Software">
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
using System.Threading.Tasks;
using System.Windows.Forms;
using Sphere10.Framework;
using Sphere10.Framework.Application;

namespace Sphere10.Framework.WinForms {
    public static class ApplicationLifecycleExtensions {

        public static void StartWinFormsApplication(this ApplicationLifecycle applicationLifecycle) {
            applicationLifecycle.StartFramework();
            var mainForm = ComponentRegistry.Instance.Resolve<IMainForm>();
            if (!(mainForm is Form)) {
                throw new SoftwareException("Registered IMainForm is not a WinForms Form");
            }
            if (mainForm is IBlockManager) {
                var blockManager = mainForm as IBlockManager;
                var blocks = ComponentRegistry.Instance.ResolveAll<IApplicationBlock>().OrderBy(b => ComponentRegistryExtensions.BlockPositions[b.GetType()]);
                blocks.ForEach(blockManager.RegisterBlock);
            }
            System.Windows.Forms.Application.Run(mainForm as Form);
        }

        public static void StartWinFormsApplication<TMainForm>(this ApplicationLifecycle applicationLifecycle)
            where TMainForm : class, IMainForm {
            ComponentRegistry.Instance.RegisterMainForm<TMainForm>();
            applicationLifecycle.StartWinFormsApplication();
        }

        public static void EndWinFormsApplication(this ApplicationLifecycle applicationLifecycle, out bool abort, out string abortReason) {
            applicationLifecycle.EndFramework(out abort, out abortReason);
        }
    }
}
