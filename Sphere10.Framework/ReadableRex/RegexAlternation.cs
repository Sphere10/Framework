//-----------------------------------------------------------------------
// <copyright file="RegexAlternation.cs" company="Sphere 10 Software">
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
    public class RegexAlternation {
        readonly RegexPattern _precedingRegexPattern;
        internal RegexAlternation(RegexPattern precedingRegexPattern) {
            _precedingRegexPattern = precedingRegexPattern;
        }

        public RegexPattern Either(RegexPattern firstOption, RegexPattern secondOption) {
            return _precedingRegexPattern.RegEx($"({firstOption}|{secondOption})");
        }

        public RegexPattern If(RegexPattern matched, RegexPattern then, RegexPattern otherwise) {
            return _precedingRegexPattern.RegEx($"(?(?={matched}){then}|{otherwise})");
        }

        public RegexPattern If(string namedGroupToMatch, RegexPattern then, RegexPattern otherwise) {
            return _precedingRegexPattern.RegEx($"(?({namedGroupToMatch}){then}|{otherwise})");
        }

        public RegexPattern If(int unnamedCaptureToMatch, RegexPattern then, RegexPattern otherwise) {
            return _precedingRegexPattern.RegEx($"(?({unnamedCaptureToMatch}){then}|{otherwise})");
        }
    }
}
