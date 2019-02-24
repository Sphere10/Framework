//-----------------------------------------------------------------------
// <copyright file="BinaryFormatter.cs" company="Sphere 10 Software">
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
using System.IO;

namespace System.Runtime.Serialization.Formatters.Binary
{
    public sealed class BinaryFormatter
    {
        public object Deserialize(Stream serializationStream)
        {
            throw new PlatformNotSupportedException("PCL");
        }
        public void Serialize(Stream serializationStream, object graph)
        {
            throw new PlatformNotSupportedException("PCL");
        }        
    }
}

#endif
