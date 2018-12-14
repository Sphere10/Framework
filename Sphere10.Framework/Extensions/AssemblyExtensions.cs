//-----------------------------------------------------------------------
// <copyright file="AssemblyExtensions.cs" company="Sphere 10 Software">
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
using System.Runtime.InteropServices;
#if __IOS__
using Foundation;
using UIKit;
#endif

namespace Sphere10.Framework {

	public static class AssemblyExtensions {
		static readonly string[] FrameworkPrefixes = new[]{
			"mscorlib",
			"System,",
			"System.",
			"PresentationCore,",
			"WindowsBase,",
			"App_global.asax.",
			"Microsoft.",
			"SMDiagnostics,"
		};

		public static IEnumerable<Assembly> GetNonFrameworkAssemblies(this AppDomain domain) {
			return domain.GetAssemblies().Where(x =>  !FrameworkPrefixes.Any(p => x.FullName.StartsWith(p)));
		}
        public static IEnumerable<Type> GetDerivedTypes<T>(this Assembly assembly) {
            return assembly.GetDerivedTypes(typeof(T));
        }

        public static IEnumerable<Type> GetDerivedTypes(this Assembly assembly, Type baseType) {
            return assembly.GetTypes().Where(t => t != baseType &&
                                                  baseType.IsAssignableFrom(t));
        }

#if __IOS__

        public static NSData FromResource(this Assembly assembly, string name) {
            if (name == null)
                throw new ArgumentNullException("name");
            
            using (var stream = assembly.GetManifestResourceStream(name)) {
                if (stream == null) {
                    throw new SoftwareException("No embedded resource called '{0}' was found in assembly '{1}", name, assembly.FullName);
                }

                var buffer = Marshal.AllocHGlobal((int) stream.Length);
                try {
                    if (buffer == IntPtr.Zero)
                        return null;

                    var copyBuffer = new byte[Math.Min(1024, (int) stream.Length)];
                    int n;
                    var target = buffer;
                    while ((n = stream.Read(copyBuffer, 0, copyBuffer.Length)) != 0) {
                        Marshal.Copy(copyBuffer, 0, target, n);
                        target = (IntPtr) ((int) target + n);
                    }
                    return (NSData)(NSData.FromBytes(buffer, (uint)stream.Length));
                } finally {
                    if (buffer != IntPtr.Zero)
                        Marshal.FreeHGlobal(buffer);
                }
            }
        }
#endif
    }
}
