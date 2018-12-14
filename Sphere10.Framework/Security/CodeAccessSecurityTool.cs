//-----------------------------------------------------------------------
// <copyright file="CodeAccessSecurityTool.cs" company="Sphere 10 Software">
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
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace Tools {
		public static class CodeAccessSecurity {
			private static volatile bool _unrestrictedFeatureSet = false;
			private static volatile bool _determinedUnrestrictedFeatureSet = false;
			private static readonly object _threadLock = new object();

			public static bool HasUnrestrictedFeatureSet {
				get {
#if __IOS__
                    return false;
#elif __ANDROID__
					return false;
#elif __WP8__
                    return false;
#else
					if (!_determinedUnrestrictedFeatureSet)
						lock (_threadLock) {
							if (!_determinedUnrestrictedFeatureSet) {
								_unrestrictedFeatureSet = AppDomain.CurrentDomain.ApplicationTrust == null || AppDomain.CurrentDomain.ApplicationTrust.DefaultGrantSet.PermissionSet.IsUnrestricted();
								_determinedUnrestrictedFeatureSet = true;
							}
						}
					return _unrestrictedFeatureSet;
#endif
				}
			}

		}
	}

