//-----------------------------------------------------------------------
// <copyright file="FieldAccessor.cs" company="Sphere 10 Software">
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
using System.Linq.Expressions;
using Sphere10.Framework;

namespace Sphere10.Framework.FastReflection {

    public class FieldAccessor {
        private Func<object, object> _mGetter;

        public FieldInfo FieldInfo { get; private set; }

        public FieldAccessor(FieldInfo fieldInfo) {
            this.FieldInfo = fieldInfo;
            this.InitializeGet(fieldInfo);
            //this.InitializeSet(propertyInfo);
        }

        private void InitializeGet(FieldInfo fieldInfo) {
            // target: (object)(((TInstance)instance).Field)

            // preparing parameter, object type
            var instance = Expression.Parameter(typeof(object), "instance");

            // non-instance for static method, or ((TInstance)instance)
            var instanceCast = fieldInfo.IsStatic ? null :
                Expression.Convert(instance, fieldInfo.ReflectedType);

            // ((TInstance)instance).Property
            var fieldAccess = Expression.Field(instanceCast, fieldInfo);

            // (object)(((TInstance)instance).Property)
            var castFieldValue = Expression.Convert(fieldAccess, typeof(object));

            // Lambda expression
            var lambda = Expression.Lambda<Func<object, object>>(castFieldValue, instance);

            this._mGetter = lambda.Compile();
        }

        public object GetValue(object instance) {
            return this._mGetter(instance);
        }

    }
}
