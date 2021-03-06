//-----------------------------------------------------------------------
// <copyright file="FunctionDerivative.cs" company="Sphere 10 Software">
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
    public class FunctionDerivative : IFunction {
        private IFunction _function = null;

        public FunctionDerivative(IFunction function) {
            _function = function;
        }

        public double Eval(double x) {
            return Tools.MathPlus.Derivative(_function, x);
        }
    }
}
#endif
