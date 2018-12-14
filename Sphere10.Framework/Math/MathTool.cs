//-----------------------------------------------------------------------
// <copyright file="MathTool.cs" company="Sphere 10 Software">
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
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading;
#if !__MOBILE__
using Sphere10.Framework.Maths;
#endif

namespace Tools {

	public class Maths {
		public const float EPSILON_F = 0.000001f;
		public const double EPSILON_D = 0.000001d;
		public const decimal EPSILON_M = 0.000001m;
		public const double MAX_SIMPSON_RECURSION = 100;
		public const char INFINITY_SYMBOL = '∞';
		public const string INFINITY_STRING = "∞";
		public const string NEGATIVE_INFINITY_STRING = "-∞";
		public const string POSITIVE_INFINITY_STRING = "+∞";
		public const string UNDEFINED_STRING = "undef";

		static private Random _globalRandom = new Random(Environment.TickCount);

		[ThreadStatic] static private Random _localRandom;

		public static Random RandomNumberGenerator {
			get {
				if (_localRandom == null) {
					int seed;
					lock (_globalRandom) seed = _globalRandom.Next();
					_localRandom = new Random(seed);
				}
				return _localRandom;
			}
		}


		#region System.Math Proxies

		public static double Sqrt(double x) {
			return System.Math.Sqrt(x);
		}

		public static double Abs(double x) {
			return System.Math.Abs(x);
		}

		public static double Pow(double x, double y) {
			return System.Math.Pow(x, y);
		}

		public static double Round(double x, int y) {
			return System.Math.Round(x, y);
		}

		#endregion

		public static int EuclideanDistance(int x0, int y0, int x1, int y1) {
			return (int) Maths.Round(EuclideanDistance((double) x0, (double) y0, (double) x1, (double) y1), 0);
		}

		public static float EuclideanDistance(float x0, float y0, float x1, float y1) {
			return (float) EuclideanDistance((double) x0, (double) y0, (double) x1, (double) y1);
		}

		public static double EuclideanDistance(double x0, double y0, double x1, double y1) {
			return Maths.Sqrt(Maths.Pow(x1 - x0, 2) + Maths.Pow(y1 - y0, 2));
		}


		public static int ClipValue(int value, int minValue, int maxValue) {
			if (value < minValue) {
				return minValue;
			} else if (value > maxValue) {
				return maxValue;

			}
			return value;
		}

		public static uint ClipValue(uint value, uint minValue, uint maxValue) {
			if (value < minValue) {
				return minValue;
			} else if (value > maxValue) {
				return maxValue;
			}
			return value;
		}

		public static long ClipValue(long value, long minValue, long maxValue) {
			if (value < minValue) {
				return minValue;
			} else if (value > maxValue) {
				return maxValue;
			}
			return value;
		}

		public static decimal ClipValue(decimal value, decimal minValue, decimal maxValue) {
			if (value < minValue) {
				return minValue;
			} else if (value > maxValue) {
				return maxValue;
			}
			return value;
		}

		public static float ClipValue(float value, float minValue, float maxValue) {
			if (value < minValue) {
				return minValue;
			} else if (value > maxValue) {
				return maxValue;
			}
			return value;
		}

		public static double ClipValue(double value, double minValue, double maxValue) {
			if (value < minValue) {
				return minValue;
			} else if (value > maxValue) {
				return maxValue;
			}
			return value;
		}

		public static void Swap<T>(ref T fromX, ref T fromY) {
			T temp = fromX;
			fromX = fromY;
			fromY = temp;
		}

		public static bool AnyOfTheseAreUndefined(params double[] values) {
			bool retval = false;
			if (values != null) {
				if (values.Any(t => double.IsNaN(t))) {
					retval = true;
				}
			}
			return retval;
		}

		public static bool IsNumber(double d) {
			return !(double.IsNaN(d) || double.IsInfinity(d));
		}

		public static double EpsilonAdjust(double val) {
			double retval = val;
			if (System.Math.Abs(val) < EPSILON_D) {
				retval = EPSILON_D;
			}
			return retval;
		}

#if !__MOBILE__

		public static double Integrate(IFunction f, double a, double b) {
			return AdaptiveSimpsonsRule(f, a, b, 0.001);
		}

		public static double SafeIntegrate(IFunction f, double a, double b, double minRange, double maxRange) {
			return SafeAdaptiveSimpsonsRule(f, a, b, minRange, maxRange, 0.001);
		}

		public static double Derivative(IFunction f, double x) {
			return (f.Eval(x + EPSILON_D) - f.Eval(x))/EPSILON_D;
		}


		public static double ArcLength(IFunction f, double a, double b) {
			return ArcLength(f, Metric.Euclidean, a, b);
		}

		public static double ArcLength(IFunction f, Metric metric, double a, double b) {

			return Integrate(ConstructArcLengthDerivativeForFunction(f, metric), a, b);
		}

		public static double SafeArcLength(IFunction f, double a, double b, double minRange, double maxRange) {
			return SafeArcLength(f, a, b, Metric.Euclidean, minRange, maxRange);
		}

		public static double SafeArcLength(IFunction f, double a, double b, Metric metric, double minRange, double maxRange) {
			return SafeIntegrate(ConstructArcLengthDerivativeForFunction(f, metric), a, b, minRange, maxRange);
		}
#endif

		public static void Rotate2D(double x, double y, double angle, out double x_bar, out double y_bar) {
			double c = System.Math.Cos(angle);
			double s = System.Math.Sin(angle);
			x_bar = c*x - s*y;
			y_bar = s*x + c*y;
		}

		public static void HyperbolicRotate2D(double x, double y, double angle, out double x_bar, out double y_bar) {
			double c = System.Math.Cosh(angle);
			double s = System.Math.Sinh(angle);
			x_bar = c*x - s*y;
			y_bar = s*x + c*y;
		}


#if !__MOBILE__
		public static IFunction ConstructArcLengthDerivativeForFunction(IFunction sourceFuntion, Metric metric) {
			switch (metric) {
				case Metric.Euclidean:
					return new ArcLengthDerivativeFunction(sourceFuntion);
				case Metric.SpacelikeMinkowski:
					return new SpacelikeMinkowskiDerivativeFunction(sourceFuntion);
				case Metric.TimelikeMinkowski:
					return new TimelikeMinkowskiDerivativeFunction(sourceFuntion);
				default:
					throw new ApplicationException(string.Format("Unsupported metric {0}", metric));
			}
		}

#endif

		public static double Asinh(double x) {
			return System.Math.Log(x + System.Math.Sqrt(x*x + 1));
		}

		public static double Acosh(double x) {
			return System.Math.Log(x + System.Math.Sqrt(x*x - 1));
		}

		public static double Atanh(double x) {
			if (System.Math.Abs(x - 1.0) < EPSILON_D) {
				int breakpoint = 1;
			}
			return 0.5*System.Math.Log((1 + x)/(1 - x));
		}

#if !__MOBILE__

		#region Integration auxillary functions

		/// <summary>
		/// http://en.wikipedia.org/wiki/Adaptive_Simpson%27s_method
		/// </summary>
		/// <param name="f"></param>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		private static double SimpsonsRule(IFunction f, double a, double b) {
			double c = (a + b)/2.0;
			double h3 = System.Math.Abs(b - a)/6.0;
			return h3*(f.Eval(a) + 4.0*f.Eval(c) + f.Eval(b));
		}

		private static double RecursiveAdaptiveSimpsonsRule(IFunction f, double a, double b, double eps, double sum) {
			double c = (a + b)/2.0;
			double left = SimpsonsRule(f, a, c);
			double right = SimpsonsRule(f, c, b);
			if (System.Math.Abs(left + right - sum) <= 15*eps || !IsNumber(sum)) {
				return left + right + (left + right - sum)/15;
			}
			return RecursiveAdaptiveSimpsonsRule(f, a, c, eps/2, left) + RecursiveAdaptiveSimpsonsRule(f, c, b, eps/2, right);
		}

		/// <summary>
		/// Takes the simpson rule over the entire interval, and of the two half-intervals.
		/// If the sum of half-intervals exceed tolerance it indicates that we need to subdivide the intervals.
		/// </summary>
		/// <param name="f"></param>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="eps"></param>
		/// <returns></returns>
		private static double AdaptiveSimpsonsRule(IFunction f, double a, double b, double eps) {
			return RecursiveAdaptiveSimpsonsRule(f, a, b, eps, SimpsonsRule(f, a, b));
		}

		private static double SafeSimpsonsRule(IFunction f, double a, double b, double minRange, double maxRange, ref bool encounteredDiscontinuity) {
			double result = 0.0;
			double c = (a + b)/2.0;
			double f_a = f.Eval(a);
			double f_b = f.Eval(b);
			double f_c = f.Eval(c);
			if (IsIn(f_a, minRange, maxRange) && IsIn(f_b, minRange, maxRange) && IsIn(f_c, minRange, maxRange)) {
				encounteredDiscontinuity = false;
				var h3 = Maths.Abs(b - a)/6.0;
				result = h3*(f_a + 4.0*f_c + f_b);
			} else {
				encounteredDiscontinuity = true;
			}
			return result;
		}

		private static double SafeRecursiveAdaptiveSimpsonsRule(IFunction f, double a, double b, double minRange, double maxRange, double eps, double sum) {
			var encounteredDiscontinuity = false;
			var c = (a + b)/2.0;
			var left = SafeSimpsonsRule(f, a, c, minRange, maxRange, ref encounteredDiscontinuity);
			if (encounteredDiscontinuity) {
				return double.NaN;
			}

			var right = SafeSimpsonsRule(f, c, b, minRange, maxRange, ref encounteredDiscontinuity);
			if (encounteredDiscontinuity) {
				return double.NaN;
			}

			if (Maths.Abs(left + right - sum) <= 15*eps || !IsNumber(sum)) {
				return left + right + (left + right - sum)/15;
			}
			return SafeRecursiveAdaptiveSimpsonsRule(f, a, c, minRange, maxRange, eps/2, left) + SafeRecursiveAdaptiveSimpsonsRule(f, c, b, minRange, maxRange, eps/2, right);
		}

		private static double SafeAdaptiveSimpsonsRule(IFunction f, double a, double b, double minRange, double maxRange, double eps) {
			return SafeRecursiveAdaptiveSimpsonsRule(f, a, b, minRange, maxRange, eps, SimpsonsRule(f, a, b));
		}



		#endregion

#endif

		/// <summary>
		/// Same as ceiling except but also increments perfect integer parameters.
		/// </summary>
		/// <param name="arcLength"></param>
		/// <returns></returns>
		public static double NextInt(double x) {
			double y = System.Math.Ceiling(x);
			if (System.Math.Abs(y - x) < EPSILON_D) {
				y += 1.0;
			}
			return y;
		}

		public static bool IsIn(double x, double lowerBound, double upperBound) {
			if (x < lowerBound || x > upperBound) {
				return false;
			}
			return true;
		}

		#region String parsing

		public static bool TryParseDoubleNiceString(string str, out double val) {
			bool retval = false;
			if (str == INFINITY_STRING || str == POSITIVE_INFINITY_STRING) {
				val = double.PositiveInfinity;
				retval = true;
			} else if (str == NEGATIVE_INFINITY_STRING) {
				val = double.NegativeInfinity;
				retval = true;
			} else if (str == UNDEFINED_STRING) {
				val = double.NaN;
				retval = true;
			} else {
				retval = double.TryParse(str, out val);
			}
			return retval;
		}

		public static string DoubleToNiceString(double val) {
			string retval = val.ToString();
			if (!IsNumber(val)) {
				if (double.IsNaN(val)) {
					retval = UNDEFINED_STRING;
				} else {
					retval = double.IsNegativeInfinity(val) ? NEGATIVE_INFINITY_STRING : POSITIVE_INFINITY_STRING;
				}
			}
			return retval;
		}

		public static string DoubleToShortNiceString(double val) {
			string retval = string.Format("{0:N}", val);
			if (!IsNumber(val)) {
				if (double.IsNaN(val)) {
					retval = UNDEFINED_STRING;
				} else {
					retval = double.IsNegativeInfinity(val) ? NEGATIVE_INFINITY_STRING : POSITIVE_INFINITY_STRING;
				}
			}
			return retval;
		}


		#endregion




		public static bool Gamble(double winProbability) {
			return RandomNumberGenerator.Next(1, 1000001)/1000000.0D <= winProbability;
		}

		public static int GambleOdds(params double[] pieChances) {
			if (Math.Abs(pieChances.Sum() - 1.0D) > EPSILON_D)
				throw new ArgumentException("Must sum to 1.0D", nameof(pieChances));

			var draw = (double) RandomNumberGenerator.Next(1, 1000001)/1000000.0D;
			for (var i = 0; i < pieChances.Length; i++) {
				var start = i > 0 ? pieChances.Take(i - 1).Sum() : 0D;
				var end = start + pieChances[i];
				if (start <= draw && draw <= end)
					return i;
			}
			throw new Exception("Should not happen");
		}


		#region Min/Max

		public static T Min<T>(params T[] values) {
			return values.Min();
		}

		public static T Max<T>(params T[] values) {
			return values.Max();
		}

		#endregion
	}

}

