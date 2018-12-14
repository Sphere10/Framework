//-----------------------------------------------------------------------
// <copyright file="SpacelikeMinkowskiDerivativeFunction.cs" company="Sphere 10 Software">
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

#if !__MOBILE__
using System;
using System.Collections.Generic;
using System.Text;
using Sphere10.Framework.Maths;

namespace Sphere10.Framework.Maths {
    public class SpacelikeMinkowskiDerivativeFunction : IFunction {
                private IFunction _derivativeFunction = null;

        public SpacelikeMinkowskiDerivativeFunction(IFunction function) {
            _derivativeFunction = new FunctionDerivative(function);
        }

        public double Eval(double x) {
			return System.Math.Sqrt(-1 + System.Math.Pow(_derivativeFunction.Eval(x), 2));

        }
    }
}
#endif
