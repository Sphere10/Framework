//-----------------------------------------------------------------------
// <copyright file="CryptoTool.cs" company="Sphere 10 Software">
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
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Security.Cryptography;
using Sphere10.Framework;

namespace Tools {


		/// <summary>
		/// This class generates and compares hashes using MD5, SHA1, SHA256, SHA384, 
		/// and SHA512 hashing algorithms. Before computing a hash, it appends a
		/// randomly generated salt to the plain text, and stores this salt appended
		/// to the result. To verify another plain text value against the given hash,
		/// this class will retrieve the salt value from the hash string and use it
		/// when computing a new hash of the plain text. Appending a salt value to
		/// the hash may not be the most efficient approach, so when using hashes in
		/// a real-life application, you may choose to store them separately. You may
		/// also opt to keep results as byte arrays instead of converting them into
		/// base64-encoded strings.
		/// </summary>
		public class Crypto {

			/// http://en.wikipedia.org/wiki/MurmurHash
			/// </summary>
			/// <param name="buffer"></param>
			/// <param name="seed"></param>
			/// <returns></returns>
			public static int MurMur3_32(byte[] buffer, int seed = 37) {
				const uint c1 = 0xcc9e2d51;
				const uint c2 = 0x1b873593;
				const int r1 = 15;
				const int r2 = 13;
				const uint m = 5;
				const uint n = 0xe6546b64;

				unchecked {
					uint hash = (uint)seed;

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
						hash = hash * m + n;

						i++;
					}

					if (remainder > 0) {
						int shift = sizeof(uint) - remainder;
						uint k = (array[i] << shift) >> shift;

						k *= c1;
						k = (k << r1) | (k >> (32 - r1)); //k = rotl32(k, r1);
						k *= c2;

						hash ^= k;
					}

					hash ^= (uint)length;

					//hash = fmix(hash);
					hash ^= hash >> 16;
					hash *= 0x85ebca6b;
					hash ^= hash >> 13;
					hash *= 0xc2b2ae35;
					hash ^= hash >> 16;

					return (int)hash;
				}
			}

			public static byte[] GenerateCryptographicallyRandomBytes(int count) {
                if (count < 0) throw new ArgumentOutOfRangeException("count", "Must be equal to or greater than 0");                    
		        var bytes = new byte[count];
		        if (count > 0) {
                    var rng = new RNGCryptoServiceProvider();
#if !__WP8__
                rng.GetNonZeroBytes(bytes);
#else
                rng.GetBytes(bytes);
#endif
            }
		        return bytes;
		    }

			public static string Hash(string plainText, SupportedHashAlgorithm hashAlgorithm, bool addSalt = true) {
				// Define min and max salt sizes.
				const int MinSaltSize = 4;
				const int MaxSaltSize = 8;

				return Hash(
                    plainText, 
                    hashAlgorithm, 
                    addSalt ? 
                        GenerateCryptographicallyRandomBytes(Tools.Maths.RandomNumberGenerator.Next(MinSaltSize, MaxSaltSize) ) :
                        null
                );
			}

			/// <summary>
			/// Generates a hash for the given plain text value and returns a
			/// base64-encoded result. Before the hash is computed, a random salt
			/// is generated and appended to the plain text. This salt is stored at
			/// the end of the hash value, so it can be used later for hash
			/// verification.
			/// </summary>
			/// <param name="plainText">
			/// Plaintext value to be hashed. The function does not check whether
			/// this parameter is null.
			/// </param>
			/// <param name="hashAlgorithm">
			/// The hash algorithm to use.
			/// </param>
			/// <param name="saltBytes">
			/// Salt bytes. 
			/// </param>
			/// <returns>
			/// Hash value formatted as a base64-encoded string.
			/// </returns>
			public static string Hash(string plainText, SupportedHashAlgorithm hashAlgorithm, byte[] saltBytes) {
				if (saltBytes == null)
					saltBytes = new byte[0];

				// Convert plain text into a byte array.
				byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

				// Allocate array, which will hold plain text and salt.
				byte[] plainTextWithSaltBytes = new byte[plainTextBytes.Length + saltBytes.Length];

				// Copy plain text bytes into resulting array.
				for (int i = 0; i < plainTextBytes.Length; i++)
					plainTextWithSaltBytes[i] = plainTextBytes[i];

				// Append salt bytes to the resulting array.
				for (int i = 0; i < saltBytes.Length; i++)
					plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

				// Because we support multiple hashing algorithms, we must define
				// hash object as a common (abstract) base class. We will specify the
				// actual hashing algorithm class later during object creation.
				HashAlgorithm hash;


				// Initialize appropriate hashing algorithm class.
				switch (hashAlgorithm) {
					case SupportedHashAlgorithm.SHA1:
						hash = new SHA1Managed();
						break;
					case SupportedHashAlgorithm.SHA256:
						hash = new SHA256Managed();
						break;
#if !__WP8__
					case SupportedHashAlgorithm.SHA384:
						hash = new SHA384Managed();
						break;
					case SupportedHashAlgorithm.SHA512:
						hash = new SHA512Managed();
						break;
					case SupportedHashAlgorithm.MD5:
						hash = new MD5CryptoServiceProvider();
						break;
#endif
					default:
						throw new SoftwareException("Failed to compute hash. Requested an unsupported hash algorithm '{0}'", hashAlgorithm);
				}

				// Compute hash value of our plain text with appended salt.
				byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);

				// Create array which will hold hash and original salt bytes.
				byte[] hashWithSaltBytes = new byte[hashBytes.Length +
													saltBytes.Length];

				// Copy hash bytes into resulting array.
				for (int i = 0; i < hashBytes.Length; i++)
					hashWithSaltBytes[i] = hashBytes[i];

				// Append salt bytes to the result.
				for (int i = 0; i < saltBytes.Length; i++)
					hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

				// Convert result into a base64-encoded string.
				string hashValue = Convert.ToBase64String(hashWithSaltBytes);

				// Return the result.
				return hashValue;
			}

			/// <summary>
			/// Compares a hash of the specified plain text value to a given hash
			/// value. Plain text is hashed with the same salt value as the original
			/// hash.
			/// </summary>
			/// <param name="plainText">
			/// Plain text to be verified against the specified hash. The function
			/// does not check whether this parameter is null.
			/// </param>
			/// <param name="hashAlgorithm">
			/// Name of the hash algorithm. Allowed values are: "MD5", "SHA1", 
			/// "SHA256", "SHA384", and "SHA512" (if any other value is specified,
			/// MD5 hashing algorithm will be used). This value is case-insensitive.
			/// </param>
			/// <param name="hashValue">
			/// Base64-encoded hash value produced by ComputeHash function. This value
			/// includes the original salt appended to it.
			/// </param>
			/// <returns>
			/// If computed hash mathes the specified hash the function the return
			/// value is true; otherwise, the function returns false.
			/// </returns>
			/// <remarks>The salt is retrieved by getting the excess bytes off the end of the hashValue, since each hash algorithm provides fixed length hash.</remarks>
			public static bool VerifyHash(string plainText, SupportedHashAlgorithm hashAlgorithm, string hashValue) {
				// Convert base64-encoded hash value into a byte array.
				byte[] hashWithSaltBytes = Convert.FromBase64String(hashValue);

				// We must know size of hash (without salt).
				int hashSizeInBits, hashSizeInBytes;

				// Size of hash is based on the specified algorithm.
				switch (hashAlgorithm) {
					case SupportedHashAlgorithm.SHA1:
						hashSizeInBits = 160;
						break;

					case SupportedHashAlgorithm.SHA256:
						hashSizeInBits = 256;
						break;
#if !__WP8__
					case SupportedHashAlgorithm.SHA384:
						hashSizeInBits = 384;
						break;

					case SupportedHashAlgorithm.SHA512:
						hashSizeInBits = 512;
						break;
					case SupportedHashAlgorithm.MD5:
						hashSizeInBits = 128;
						break;
#endif
					default:
						throw new SoftwareException("Failed to verify hash. Requested an unsupported hash algorithm '{0}'", hashAlgorithm);
						break;
				}

				// Convert size of hash from bits to bytes.
				hashSizeInBytes = hashSizeInBits / 8;

				// Make sure that the specified hash value is long enough.
				if (hashWithSaltBytes.Length < hashSizeInBytes)
					return false;

				// Allocate array to hold original salt bytes retrieved from hash.
				byte[] saltBytes = new byte[hashWithSaltBytes.Length -
											hashSizeInBytes];

				// Copy salt from the end of the hash to the new array.
				for (int i = 0; i < saltBytes.Length; i++)
					saltBytes[i] = hashWithSaltBytes[hashSizeInBytes + i];

				// Compute a new hash string.
				string expectedHashString = Hash(plainText, hashAlgorithm, saltBytes);

				// If the computed hash matches the specified hash,
				// the plain text value must be correct.
				return (hashValue == expectedHashString);
			}

			//private static byte[] _salt = Encoding.ASCII.GetBytes("o6806642kbM7c5");

			public static string EncryptStringAES(string plainText, string sharedSecret, string salt) {
				return EncryptStringAES(plainText, sharedSecret, Encoding.UTF8.GetBytes(salt));
			}

			/// <summary>
			/// Encrypt the given string using AES.  The string can be decrypted using 
			/// DecryptStringAES().  The sharedSecret parameters must match.
			/// </summary>
			/// <param name="plainText">The text to encrypt.</param>
			/// <param name="sharedSecret">A password used to generate a key for encryption.</param>
			public static string EncryptStringAES(string plainText, string sharedSecret, byte[] salt) {
				if (String.IsNullOrEmpty(plainText))
					throw new ArgumentNullException("plainText");
				if (String.IsNullOrEmpty(sharedSecret))
					throw new ArgumentNullException("sharedSecret");

				string outStr = null;                       // Encrypted string to return
				AesManaged aesAlg = null;              // RijndaelManaged object used to encrypt the data.

				try {
					// generate the key from the shared secret and the salt
					Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, salt);

					// Create a RijndaelManaged object
                    aesAlg = new AesManaged();
					aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

					// Create a decryptor to perform the stream transform.
					ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

					// Create the streams used for encryption.
					using (MemoryStream msEncrypt = new MemoryStream()) {
						// prepend the IV
						msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
						msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
						using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write)) {
							using (StreamWriter swEncrypt = new StreamWriter(csEncrypt)) {
								//Write all data to the stream.
								swEncrypt.Write(plainText);
							}
						}
						outStr = Convert.ToBase64String(msEncrypt.ToArray());
					}
				} finally {
					// Clear the RijndaelManaged object.
					if (aesAlg != null)
						aesAlg.Clear();
				}

				// Return the encrypted bytes from the memory stream.
				return outStr;
			}

			public static string DecryptStringAES(string cipherText, string sharedSecret, string salt) {
				return DecryptStringAES(cipherText, sharedSecret, Encoding.UTF8.GetBytes(salt));
			}

			/// <summary>
			/// Decrypt the given string.  Assumes the string was encrypted using 
			/// EncryptStringAES(), using an identical sharedSecret.
			/// </summary>
			/// <param name="cipherText">The text to decrypt.</param>
			/// <param name="sharedSecret">A password used to generate a key for decryption.</param>
			public static string DecryptStringAES(string cipherText, string sharedSecret, byte[] salt) {
				if (String.IsNullOrEmpty(cipherText))
					throw new ArgumentNullException("cipherText");
				if (String.IsNullOrEmpty(sharedSecret))
					throw new ArgumentNullException("sharedSecret");

				// Declare the RijndaelManaged object
				// used to decrypt the data.
				AesManaged aesAlg = null;
                
				// Declare the string used to hold
				// the decrypted text.
				string plaintext = null;

				try {
					// generate the key from the shared secret and the salt
					Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, salt);

					// Create the streams used for decryption.                
					byte[] bytes = Convert.FromBase64String(cipherText);
					using (MemoryStream msDecrypt = new MemoryStream(bytes)) {
						// Create a RijndaelManaged object
						// with the specified key and IV.
                        aesAlg = new AesManaged();
						aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
						// Get the initialization vector from the encrypted stream
						aesAlg.IV = ReadByteArray(msDecrypt);
						// Create a decrytor to perform the stream transform.
						ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
						using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read)) {
							using (StreamReader srDecrypt = new StreamReader(csDecrypt))

								// Read the decrypted bytes from the decrypting stream
								// and place them in a string.
								plaintext = srDecrypt.ReadToEnd();
						}
					}
				} finally {
					// Clear the RijndaelManaged object.
					if (aesAlg != null)
						aesAlg.Clear();
				}

				return plaintext;
			}

			private static byte[] ReadByteArray(Stream s) {
				byte[] rawLength = new byte[sizeof(int)];
				if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length) {
					throw new SystemException("Stream did not contain properly formatted byte array");
				}

				byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
				if (s.Read(buffer, 0, buffer.Length) != buffer.Length) {
					throw new SystemException("Did not read byte array properly");
				}

				return buffer;
			}

			public static void EncryptStream(Stream input, Stream output, SymmetricAlgorithm symmetricAlgorithm) {
				// prepend IV
				output.Write(symmetricAlgorithm.IV, 0, symmetricAlgorithm.IV.Length);
				using (var transform = symmetricAlgorithm.CreateEncryptor())
				using (var encryptor = new CryptoStream(output, transform, CryptoStreamMode.Write)) {
					Tools.Streams.RouteStream(input, encryptor);
				}
			}

			public static void DecryptStream(Stream input, Stream output, SymmetricAlgorithm symmetricAlgorithm) {
				// read IV
				var iv = new byte[symmetricAlgorithm.BlockSize / 8];
				var numRead = input.Read(iv, 0, iv.Length);
				if (numRead != iv.Length)
					throw new Exception("Unable to contiguously read intialization vector from input stream");
				symmetricAlgorithm.IV = iv;
				using (var transform = symmetricAlgorithm.CreateDecryptor())
				using (var decryptor = new CryptoStream(input, transform, CryptoStreamMode.Read))
					Tools.Streams.RouteStream(decryptor, output);
			}

			public static SymmetricAlgorithm PrepareSymmetricAlgorithm<TSymmetricAlgorithm>(string password, byte[] salt = null, PaddingMode paddingMode = PaddingMode.PKCS7, CipherMode cipherMode = CipherMode.CBC) where TSymmetricAlgorithm : SymmetricAlgorithm, new() {
				if (salt == null)
					salt = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };

				var key = new Rfc2898DeriveBytes(password, salt);
#if __WP8__
                var algorithm = new TSymmetricAlgorithm();
			    
                
#else
                var algorithm = new TSymmetricAlgorithm {
					Padding = paddingMode,
					Mode = cipherMode,
				};
#endif
				algorithm.Key = key.GetBytes(algorithm.KeySize / 8);
				algorithm.IV = (from i in Enumerable.Range(0, algorithm.BlockSize / 8) select (byte)Maths.RandomNumberGenerator.Next(0, 255)).ToArray();
				return algorithm;
			}
		}
	}