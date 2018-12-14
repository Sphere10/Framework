//-----------------------------------------------------------------------
// <copyright file="HttpContextLifetimeProvider.cs" company="Sphere 10 Software">
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
using System.Web;

namespace Sphere10.Framework.Web {
    public class HttpContextLifetimeProvider : TinyIoCContainer.ITinyIoCObjectLifetimeProvider {
        private readonly string _keyName = String.Format("TinyIoC.HttpContext.{0}", Guid.NewGuid());

        public object GetObject() {
            return HttpContext.Current.Items[_keyName];
        }

        public void SetObject(object value) {
            HttpContext.Current.Items[_keyName] = value;
        }

        public void ReleaseObject() {
            var item = GetObject() as IDisposable;
            item?.Dispose();
            SetObject(null);
        }
    }
}
