//-----------------------------------------------------------------------
// <copyright file="IncrementUsageByOneTask.cs" company="Sphere 10 Software">
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



	public class IncrementUsageByOneTask : IApplicationInitializeTask {


		public IncrementUsageByOneTask(IProductUsageServices productUsageServices) {
			ProductUsageServices = productUsageServices;
		}

		public IProductUsageServices ProductUsageServices { get; private set; }

		public void Initialize() {
			ProductUsageServices.IncrementUsageByOne();
		}

		// Critical to run this early on
		public int Sequence {
			get { return 1; }
		}
	}
}

