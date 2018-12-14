//-----------------------------------------------------------------------
// <copyright file="WindowsPhone8.cs" company="Sphere 10 Software">
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

#if __WP8__

using System;
using System.Linq;
using System.IO;
namespace Sphere10.Framework {

    public class NonSerializedAttribute : Attribute {
    }

    public class ObfuscationAttribute : Attribute {

        public bool Exclude { get; set; }

    }


    public delegate bool TypeFilter(Type m, Object filterCriteria);

    public static class CompatibilityUtil {
        public static void AppendAllText(string filepath, string text) {
            using (var writer = File.AppendText(filepath)) {
                writer.Write(text);
                writer.Flush();
            }
        }

        public static void WriteAllBytes(string filePath, byte[] bytes) {
            using (var writer = File.AppendText(filePath)) {
                writer.Write(bytes);
                writer.Flush();
            }
        }

        public static Type[] FindInterfaces(this Type type, TypeFilter typeFilter, Object filterCriteria) {
            var result =
                from i in type.GetInterfaces()
                where typeFilter(i, filterCriteria)
                select i;

            return result.ToArray();
        }
    }
}
#endif
