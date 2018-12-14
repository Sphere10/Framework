//-----------------------------------------------------------------------
// <copyright file="ByteArrayExtensions.cs" company="Sphere 10 Software">
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
using System.IO;
using System.Runtime.Serialization;
#if !__WP8__
using System.Runtime.Serialization.Formatters.Binary;
#endif
using System.Diagnostics;

namespace Sphere10.Framework {

	public static class ByteArrayExtensions {

	    public static bool SequenceEquals(this byte[] source, byte[] dest) {
	        return ByteArrayComparer.Instance.Compare(source, dest) == 0;
	    }


        public static int GetHashCodeSimple(this byte[] buffer) {
	        const int CONSTANT = 17;
	        unchecked {
	            int hashCode = 37;

	            ReinterpretArray common = new ReinterpretArray();
	            common.AsByteArray = buffer;
	            int[] array = common.AsInt32Array;

	            int length = buffer.Length;
	            int remainder = length & 3;
	            int len = length >> 2;

	            int i = 0;

	            while (i < len) {
	                hashCode = CONSTANT*hashCode + array[i];
	                i++;
	            }

	            if (remainder > 0) {
	                int shift = sizeof (uint) - remainder;
	                hashCode = CONSTANT*hashCode + ((array[i] << shift) >> shift);
	            }

	            return hashCode;
	        }
	    }

	    /// http://en.wikipedia.org/wiki/MurmurHash
	    /// </summary>
	    /// <param name="buffer"></param>
	    /// <param name="seed"></param>
	    /// <returns></returns>
	    public static int GetMurMurHash3(this byte[] buffer, int seed = 37) {
	        const uint c1 = 0xcc9e2d51;
	        const uint c2 = 0x1b873593;
	        const int r1 = 15;
	        const int r2 = 13;
	        const uint m = 5;
	        const uint n = 0xe6546b64;

	        unchecked {


	            uint hash = (uint) seed;

	            ReinterpretArray common = new ReinterpretArray();
	            common.AsByteArray = buffer;
	            uint[] array = common.AsUInt32Array;

	            int length = buffer.Length;
	            int remainder = length & 3;
	            int len = length >> 2;

	            int i = 0;

	            while (i < len) {
	                uint k = array[i];

	                k *= c1;
	                k = (k << r1) | (k >> (32 - r1)); //k = rotl32(k, r1);
	                k *= c2;

	                hash ^= k;
	                hash = (hash << r2) | (hash >> (32 - r2)); //hash = rotl32(hash, r2);
	                hash = hash*m + n;

	                i++;
	            }

	            if (remainder > 0) {
	                int shift = sizeof (uint) - remainder;
	                uint k = (array[i] << shift) >> shift;

	                k *= c1;
	                k = (k << r1) | (k >> (32 - r1)); //k = rotl32(k, r1);
	                k *= c2;

	                hash ^= k;
	            }

	            hash ^= (uint) length;

	            //hash = fmix(hash);
	            hash ^= hash >> 16;
	            hash *= 0x85ebca6b;
	            hash ^= hash >> 13;
	            hash *= 0xc2b2ae35;
	            hash ^= hash >> 16;

	            return (int) hash;
	        }
	    }

        public static byte[] SubArray(this byte[] buffer, int offset, int length) {
            byte[] middle = new byte[length];
            Buffer.BlockCopy(buffer, offset, middle, 0, length);
            return middle;
        }

        public static byte[] Left(this byte[] buffer, int length) {
            return buffer.SubArray(0, length);
        }

        public static byte[] Right(this byte[] buffer, int length) {
            return buffer.SubArray(buffer.Length - length, length);
        }

        public static int GetBit(this byte[] map, int bitIndex) {
            return (map[bitIndex >> 3] >> (bitIndex & 7)) & 1;
        }

        public static void SetBit(this byte[] map, int bitIndex, int value) {
            int bitMask = 1 << (bitIndex & 7);
            if (value != 0)
                map[bitIndex >> 3] |= (byte)bitMask;
            else
                map[bitIndex >> 3] &= (byte)(~bitMask);
        }

        public static string ToBase62(this byte[] buffer) {
	        return Base62Converter.ToBase62String(buffer);
	    }
        public static BinaryReader AsReader(this byte[] buffer, EndianBitConverter bitConverter) {
            return new BinaryReader(new MemoryStream(buffer));
        }

        public static EndianBinaryReader AsEndianReader(this byte[] buffer, EndianBitConverter bitConverter) {
	        return new EndianBinaryReader(bitConverter, new MemoryStream(buffer));
	    }

#if !__WP8__
		public static string ToASCIIString(this byte[] asciiByteArray) {
			var enc = new ASCIIEncoding();
			return enc.GetString(asciiByteArray);
		}
#endif

		public static string ToHexString(this byte[] byteArray, bool ommit_0x = false) {
			if (byteArray.Length == 0)
				return string.Empty;

			var hexBuilder = new StringBuilder(byteArray.Length * 2);
			if (!ommit_0x)
				hexBuilder.Append("0x");

			foreach (var @byte in byteArray)
				hexBuilder.AppendFormat("{0:x2}", @byte);

			return hexBuilder.ToString();
		}

#if !__WP8__
		/// <summary>
		/// Converts a byte array to an object
		/// </summary>
		/// <param name="bytes">The array of bytes to be converted</param>
		/// <returns>An object that represents the byte array</returns>
		public static object DeserializeToObject(this byte[] bytes) {
			if (bytes.Length == 0) {
				return null;
			}

			using (var stream = new MemoryStream(bytes)) {
				stream.Position = 0;
				var formatter = new BinaryFormatter();
				var obj = formatter.Deserialize(stream);
				stream.Close();
				return obj;
			}
		}
#endif

		public static byte[] Xor(this byte[] left, byte[] right) {
#warning Should auto-wrap the right array
			#region Pre-conditions
			Debug.Assert(right.Length >= left.Length);
			if (!(right.Length >= left.Length)) {
				throw new ArgumentOutOfRangeException("right", "Parameter must be less than or equal to the size of source array");
			}
			#endregion
			var result = new List<byte>();
			for (var i = 0; i < left.Length; i++) {
				result.Add((byte)(left[i] ^ right[i]));
			}
			return result.ToArray();
		}
	}
}
