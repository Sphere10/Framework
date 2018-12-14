//-----------------------------------------------------------------------
// <copyright file="IActiveApplicationMonitor.cs" company="Sphere 10 Software">
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

#if !__MOBILE__
using System;
using System.Reflection;
using System.Threading;
using System.Diagnostics;


namespace Sphere10.Framework  {

	public interface IActiveApplicationMonitor  {
		event EventHandler<ApplicationChangedEvent> ApplicationChanged;

		string GetActiveApplicationName();
	}
}
#endif
