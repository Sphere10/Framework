//-----------------------------------------------------------------------
// <copyright file="EncryptedAttribute.cs" company="Sphere 10 Software">
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
using System.IO;
using System.Reflection;
using System.Text;



namespace Sphere10.Framework {
	public class EncryptedAttribute : Attribute {
		[Obfuscation]
		private const string SharedSecret = "92748AC3-7B85-4B56-8672-FBB616022BA0";

		public EncryptedAttribute() {
			Policy = EncryptionSaltPolicy.Custom;
			Pepper = string.Empty;
		}

		public EncryptionSaltPolicy Policy { get; set; }

		public string Pepper { get; set; }

		public virtual string Encrypt(string value) {
			var salt = CalculateSalt() ?? string.Empty;
			var pepper = Pepper ?? string.Empty;
			return Tools.Crypto.EncryptStringAES(value, SharedSecret, salt + pepper);
		}

		public virtual string Decrypt(string value) {
			var salt = CalculateSalt() ?? string.Empty;
			var pepper = Pepper ?? string.Empty;
			return Tools.Crypto.DecryptStringAES(value, SharedSecret, salt + pepper);
		}

		protected string CalculateSalt() {
			var salt = new StringBuilder();

			if (Policy.HasFlag(EncryptionSaltPolicy.None))
				salt.Append(string.Empty);

			if (Policy.HasFlag(EncryptionSaltPolicy.MACAddress)) {
				salt.Append(GetMacAddress());
			}

#if !__WP8__
			if (Policy.HasFlag(EncryptionSaltPolicy.ExecutableFileCreationTime)) {
				salt.Append(GetExecutableFileCreationTime());
			}

			if (Policy.HasFlag(EncryptionSaltPolicy.MachineName)) {
				salt.Append(GetMachineName());
			}

			if (Policy.HasFlag(EncryptionSaltPolicy.UserName)) {
				salt.Append(GetUserName());
			}
#endif

			if (Policy.HasFlag(EncryptionSaltPolicy.OperatingSystemVersion)) {
				salt.Append(GetOperatingSystemVersion());
			}

			if (Policy.HasFlag(EncryptionSaltPolicy.ProcessorCount)) {
				salt.Append(GetProcessorCount());
			}

			if (Policy.HasFlag(EncryptionSaltPolicy.Custom)) {
				salt.Append(GetCustomSaltValue());
			}

			return salt.ToString();
		}

		protected virtual string GetMacAddress() {
			return Tools.Network.GetMacAddress();
		}

#if !__WP8__
		protected virtual string GetExecutableFileCreationTime() {
			return string.Format("{0:yyyy-MM-dd HH:mm:ss fff}",  File.GetCreationTimeUtc(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName));
		}


		protected virtual string GetMachineName() {
			return Environment.MachineName;	
		}

		protected virtual string GetUserName() {
			return Environment.UserName;
		}
#endif
		protected virtual string GetOperatingSystemVersion() {
			return Environment.OSVersion.ToString();
		}

		protected virtual string GetProcessorCount() {
			return Environment.ProcessorCount.ToString();
		}

		protected virtual string GetCustomSaltValue() {
			return Pepper ?? string.Empty;
		}
		
	}

}
