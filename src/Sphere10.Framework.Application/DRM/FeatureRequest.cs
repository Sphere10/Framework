//-----------------------------------------------------------------------
// <copyright file="FeatureRequest.cs" company="Sphere 10 Software">
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
using System.Configuration;
using System.Xml.Serialization;
using System.Reflection;
using System.Runtime.Serialization;

namespace Sphere10.Framework.Application {

    [Obfuscation(Exclude = true)]
    public class FeatureRequest : ClientRequest {
        public FeatureRequest() : base () {
        }

        public FeatureRequest(UserType userType, string sender, ProductInformation senderProductInformation, string featureDescription)
            : base(userType, sender, senderProductInformation, featureDescription) {
        }
        
    }
   
}
