//-----------------------------------------------------------------------
// <copyright file="Statistics.cs" company="Sphere 10 Software">
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

namespace Sphere10.Framework {
    /// <summary>
    /// Summary description for Statistics.
    /// </summary>
    [Serializable]
    public class Statistics {
        private uint _count;         /* Number of items in the analysis          */
        private double _total;         /* Total of data                            */
        private double _total2;        /* Sum of sqaures of data                   */
        private double _product;       /* Product of data                          */
        private double _recip;         /* Sum of reciprocals of data               */
        private double _min;           /* Min datum (if data array not used)       */
        private double _max;           /* Min datum (if data array not used)       */

        public const double EPSILON = 0.00001;

        public Statistics() {
            Reset();
        }

        #region Properties
        public uint SampleCount => _count;

        public double Minimum => _min;

        public double Maximum => _max;

        public double Mean {
            get {
                var mean = double.NaN;
                if (SampleCount > 0) {
                    mean = Sum / SampleCount;
                }
                return mean;
            }
        }

        public double PopulationStandardDeviation {
            get {
                var stdDev = double.NaN;
                stdDev = Math.Sqrt(PopulationVariance);
                return stdDev;
            }
        }

        public double PopulationVariance {
            get {
                double variance = double.NaN;
                if (SampleCount > 0) {
                    variance = ((SampleCount * SquaredSum) - Sum * Sum) / (SampleCount * SampleCount);
                }
                return variance;
            }
        }

        public double PopulationVariationCoefficient {
            get {
                double varCoeff = double.NaN;
                if (SampleCount > 0) {
                    varCoeff = (PopulationVariance / Mean) * 100;
                }
                return varCoeff;
            }
        }

        public double GeometricMean {
            get {
                double gmean = double.NaN;
                if (SampleCount > 0) {
                    gmean = Math.Pow(Product, 1.0 / SampleCount);
                }
                return gmean;
            }
        }

        public double HarmonicMean {
            get {
                double hmean = double.NaN;
                if (SampleCount > 0) {
                    hmean = SampleCount / ReciprocalSum;
                }
                return hmean;
            }
        }

        /// <summary>
        /// Return the error (%) for the minimum datum.
        /// </summary>
        public double MinimumError {
            get {
                double error = double.NaN;
                if ((Mean * Mean) > (EPSILON * EPSILON)) {
                    error = 100.0 * (Minimum - Mean) / Mean;
                }
                return error;
            }
        }


        /// <summary>
        /// Return the error (%) for the maximum datum.
        /// </summary>
        public double MaximumError {
            get {
                double error = double.NaN;
                if ((Mean * Mean) > (EPSILON * EPSILON)) {
                    error = 100.0 * (Maximum - Mean) / Mean;
                }
                return error;
            }
        }

        public double Sum => _total;

        public double ReciprocalSum => _recip;

        public double SquaredSum => _total2;

        public double Product => _product;

        public double SampleStandardDeviation {
            get {
                double stdDev = double.NaN;
                if (SampleCount >= 2) {
                    stdDev = Math.Sqrt(SampleVariance);
                }
                return stdDev;
            }
        }

        public double SampleVariance {
            get {
                double variance = double.NaN;
                if (SampleCount > 0) {
                    variance = ((SampleCount * SquaredSum) - Sum * Sum) /
                        ((SampleCount - 1) * (SampleCount - 1));
                }
                return variance;
            }
        }

        public double SampleVariationCoefficient {
            get {
                double varCoeff = double.NaN;
                if (SampleCount >= 2) {
                    varCoeff = 100 * (SampleStandardDeviation / Mean);
                }
                return varCoeff;
            }
        }



        #endregion

        public void Reset() {
            _count = 0;
            _min = 0.0;
            _max = 0.0;
            _total = 0.0;
            _total2 = 0.0;
            _recip = 0.0;
            _product = 1.0;
        }

        public void AddDatum(double datum) {
            _count++;
            _total += datum;
            _total2 += datum * datum;
            if (double.IsNaN(_recip) || datum * datum < EPSILON * EPSILON) {
                _recip = double.NaN;
            } else {
                _recip += (1 / datum);
            }

            _product *= datum;

            if (_count == 1) {
                // first data so set _min/_max
                _min = datum;
                _max = datum;
            } else {
                // adjust _min/_max boundaries if necessary
                if (datum < Minimum) {
                    _min = datum;
                }
                if (datum > Maximum) {
                    _max = datum;
                }
            }
        }

        public void AddDatum(double datum, uint numTimes) {
            _count += numTimes;
            _total += datum * numTimes;
            _total2 += datum * datum * numTimes;
            if (double.IsNaN(_recip) || datum * datum < EPSILON * EPSILON) {
                _recip = double.NaN;
            } else {
                _recip += (1 / datum) * numTimes;
            }

            _product *= Math.Pow(datum, numTimes);

            if (_count == 1) {
                // first data so set _min/_max
                _min = datum;
                _max = datum;
            } else {
                // adjust _min/_max boundaries if necessary
                if (datum < Minimum) {
                    _min = datum;
                }
                if (datum > Maximum) {
                    _max = datum;
                }
            }
        }

        public void RemoveDatum(double datum) {
            _count--;
            _total -= datum;
            _total2 -= datum * datum;
            _recip -= 1.0 / datum;
            _product /= datum;
        }

    }
}
