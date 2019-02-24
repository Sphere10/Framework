//-----------------------------------------------------------------------
// <copyright file="MemoryTool.cs" company="Sphere 10 Software">
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
using Sphere10.Framework;

namespace Tools {
    public static class Memory {

        public static double ConvertMemoryMetric(double quanity, MemoryMetric fromMetric, MemoryMetric toMetric) {
            double fromBaseFactor;
            var fromBaseMetric = ToBaseMetric(fromMetric, out fromBaseFactor);

            double toBaseFactor;
            var toBaseMetric = ToBaseMetric(toMetric, out toBaseFactor);

            double conversionFactor;
            switch (fromBaseMetric) {
                case MemoryMetric.Bit:
                    switch (toBaseMetric) {
                        case MemoryMetric.Bit:
                            conversionFactor = 1;
                            break;
                        case MemoryMetric.Byte:
                            conversionFactor = 1/8D;
                            break;
                        default:
                            throw new NotSupportedException(fromBaseMetric.ToString());
                    }

                    break;
                case MemoryMetric.Byte:
                    switch (toBaseMetric) {
                        case MemoryMetric.Bit:
                            conversionFactor = 8D;
                            break;
                        case MemoryMetric.Byte:
                            conversionFactor = 1;
                            break;
                        default:
                            throw new NotSupportedException(fromBaseMetric.ToString());
                    }
                    break;
                default:
                    throw new NotSupportedException(fromBaseMetric.ToString());
            }

            return (fromBaseFactor*quanity*conversionFactor)/toBaseFactor;
        }


        public static MemoryMetric ToBaseMetric(MemoryMetric metric, out double factor) {
            switch (metric) {
                case MemoryMetric.Bit:
                    factor = 1;
                    return MemoryMetric.Bit;
                case MemoryMetric.Kilobit:
                    factor = 1E3;
                    return MemoryMetric.Bit;
                case MemoryMetric.Megabit:
                    factor = 1E6;
                    return MemoryMetric.Bit;
                case MemoryMetric.Gigabit:
                    factor = 1E9;
                    return MemoryMetric.Bit;
                case MemoryMetric.Terrabit:
                    factor = 1E12;
                    return MemoryMetric.Bit;
                case MemoryMetric.Petabit:
                    factor = 1E15D;
                    return MemoryMetric.Bit;
                case MemoryMetric.Byte:
                    factor = 1;
                    return MemoryMetric.Byte;
                case MemoryMetric.Kilobyte:
                    factor = 1E3;
                    return MemoryMetric.Byte;
                case MemoryMetric.Megabyte:
                    factor = 1E6;
                    return MemoryMetric.Byte;
                case MemoryMetric.Gigabyte:
                    factor = 1E9;
                    return MemoryMetric.Byte;
                case MemoryMetric.Terrabyte:
                    factor = 1E12;
                    return MemoryMetric.Byte;
                case MemoryMetric.PetaByte:
                    factor = 1E15;
                    return MemoryMetric.Byte;
                default:
                    throw new NotSupportedException(metric.ToString());

            }

        }
    }
}

