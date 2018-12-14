//-----------------------------------------------------------------------
// <copyright file="RegexRange.cs" company="Sphere 10 Software">
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

namespace Sphere10.Framework {
    public class RegexRange {
        public static RegexPattern Of(char from, char to) {
            return new RegexPattern(from + "-" + to);
        }
        public static RegexPattern Of(int from, int to) {
            return new RegexPattern(from + "-" + to);
        }
        public static RegexPattern AnyLetter => new RegexPattern("a-zA-Z");
        public static RegexPattern AnyLowercaseLetter => new RegexPattern("a-z");
        public static RegexPattern AnyUppercaseLetter => new RegexPattern("A-Z");
    }
}
