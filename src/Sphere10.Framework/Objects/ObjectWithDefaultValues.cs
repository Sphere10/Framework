//-----------------------------------------------------------------------
// <copyright file="ObjectWithDefaultValues.cs" company="Sphere 10 Software">
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
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sphere10.Framework {
	public abstract class ObjectWithDefaultValues : object {
	    protected ObjectWithDefaultValues()
			: this(true) {
		}

	    protected ObjectWithDefaultValues(bool setDefaultValues) {
			if (setDefaultValues)
				RestoreDefaultValues();
		}

		public virtual void RestoreDefaultValues() {
			// Default implementation is to use 
			this.SetDefaultValues(); 
		}


	    public static void SetDefaults(object obj) {
            var bindingFlags = BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.SetField;
			// HS: Removed 2019-02-19, .NET Standard 2.0 assume has unrestricted
		    //if (Tools.CodeAccessSecurity.HasUnrestrictedFeatureSet)
		    //    bindingFlags |= BindingFlags.NonPublic;
		    bindingFlags |= BindingFlags.NonPublic;

			foreach (var f in obj.GetType().GetFields(bindingFlags)) {
                foreach (var attr in f.GetCustomAttributesOfType<DefaultValueAttribute>()) {
                    var dv = (DefaultValueAttribute)attr;
                    f.SetValue(obj, Tools.Object.ChangeType(dv.Value, f.FieldType));
                }
            }

            bindingFlags = BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.SetProperty;
		    // HS: Removed 2019-02-19, .NET Standard 2.0 assume has unrestricted
		    //if (Tools.CodeAccessSecurity.HasUnrestrictedFeatureSet)
		    //    bindingFlags |= BindingFlags.NonPublic;
		    bindingFlags |= BindingFlags.NonPublic;

			foreach (var p in obj.GetType().GetProperties(bindingFlags)) {
                if (p.GetIndexParameters().Length != 0)
                    continue;

                if (!p.CanWrite)
                    continue;

                foreach (var attr in p.GetCustomAttributesOfType<DefaultValueAttribute>()) {
                    var dv = (DefaultValueAttribute)attr;
#if USE_FAST_REFLECTION
                    p.FastSetValue(obj, ChangeType(dv.Value, p.PropertyType)); // using FastReflection lib
#else
                    p.SetValue(obj, TypeChanger.ChangeType(dv.Value, p.PropertyType), null);		// using normal reflection
#endif
                }
            }
        }
	}
}
