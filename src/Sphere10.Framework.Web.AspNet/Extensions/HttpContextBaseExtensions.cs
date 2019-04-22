//-----------------------------------------------------------------------
// <copyright file="HttpContextBaseExtensions.cs" company="Sphere 10 Software">
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
using System.Web;

namespace Sphere10.Framework.Web
{
    public static class HttpContextBaseExtensions
    {
          public static ICollection<UserMessage> GetUserMessages(this HttpContextBase context)
          {
              return context.ApplicationInstance.Context.GetUserMessages();
          }

    }
}
