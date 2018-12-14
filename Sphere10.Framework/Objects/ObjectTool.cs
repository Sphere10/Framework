//-----------------------------------------------------------------------
// <copyright file="ObjectTool.cs" company="Sphere 10 Software">
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

#define USE_FAST_REFLECTION
#if __IOS__
#undef USE_FAST_REFLECTION
#endif

using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Sphere10.Framework;

#if USE_FAST_REFLECTION
using Sphere10.Framework.FastReflection;
#endif

namespace Tools {

    public static class Object {

        public static object Create(string typeName, params object[] args) {
            return TypeActivator.Create(typeName, args);
        }

        public static object Create(Type targetType, params object[] args) {
            return TypeActivator.Create(targetType, args);
        }

        public static Type ResolveType(string fullName) {
            return TypeResolver.Resolve(fullName);
        }


        public static object ChangeType(object value, Type targetType) {
            return TypeChanger.ChangeType(value, targetType);
        }

        public static T ChangeType<T>(object value) {
			return TypeChanger.ChangeType<T>(value);
        }

        public static object SanitizeObject(object obj) {
            return TypeChanger.SanitizeObject(obj);
        }


        public static T Clone<T>(T obj, bool deepClone = false, IEnumerable<Type> dontClone = null) {
            return (T) CloneObject(obj, deepClone, dontClone);
        }

        public static object CloneObject(object obj, bool deepClone = false, IEnumerable<Type> dontClone = null) {
            IObjectCloner cloner;
#if __MOBILE__
            if (deepClone)
                throw new SoftwareException("Deep copying not supoprted in mobile platforms");
            cloner = new MobileCompatibleObjectCloner();
#else
            cloner = deepClone ? (IObjectCloner) new DeepObjectCloner(dontClone) : new ShallowObjectCloner();
#endif
            return cloner.Clone(obj);
        }

        public static void CopyMembers(object source, object dest, bool deepCopy = false) {
            IObjectCloner cloner;
#if __MOBILE__
            if (deepCopy)
                throw new SoftwareException("Deep copying not supoprted in mobile platforms");
            cloner = new MobileCompatibleObjectCloner();
#else
            cloner = deepCopy ? (IObjectCloner) new DeepObjectCloner() : new ShallowObjectCloner();
#endif
            cloner.Copy(source, dest);
        }

        public static bool Compare(object obj1, object obj2) {
            var comparer = new DeepObjectComparer();
            return comparer.Equals(obj1, obj2);
        }


        public static void DecryptMembers(object obj) {
            ObjectEncryptor.DecryptMembers(obj);
        }

        public static void EncryptMembers(object obj) {
            ObjectEncryptor.EncryptMembers(obj);
        }


        public static int CombineHashCodes(int hashCode1, int hashCode2) {
            unchecked {
                var hash = 17;
                hash = hash * 31 + hashCode1;
                hash = hash * 31 + hashCode2;
                return hash;
            }
        }

        public static void SetDefaultValues(object obj) {
            ObjectWithDefaultValues.SetDefaults(obj);
        }

	}
}

