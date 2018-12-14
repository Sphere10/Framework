//-----------------------------------------------------------------------
// <copyright file="NetworkTool.cs" company="Sphere 10 Software">
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
using System.Net.NetworkInformation;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Specialized;
using Sphere10.Framework;

#if __WP8__
using Windows.Networking;
using Windows.Networking.Connectivity;
#endif

namespace Tools {


		public static class Network {

			public static IPAddress GetNetworkAddress() {
#if __ANDROID__
				try {

					var networkInterfaces = Java.Net.NetworkInterface.NetworkInterfaces;
					while (networkInterfaces.HasMoreElements) {
						var netInterface = (Java.Net.NetworkInterface)networkInterfaces.NextElement();
						var ipAddresses = netInterface.InetAddresses;
						while (ipAddresses.HasMoreElements) {
							var inetAddress = (Java.Net.InetAddress)ipAddresses.NextElement();
							if (inetAddress.IsLoopbackAddress)
								continue;
							return new IPAddress(inetAddress.GetAddress());
						}
					}
				} catch (SocketException ex) {
					SystemLog.Exception(ex);
				}
				return null;
#elif __WP8__
                return IPAddress.None;
#else
				foreach (var netInterface in NetworkInterface.GetAllNetworkInterfaces()) { 
					switch(netInterface.NetworkInterfaceType) {
						case NetworkInterfaceType.Loopback:
						case NetworkInterfaceType.Tunnel:
						case NetworkInterfaceType.Unknown:
							continue;
					}
					foreach (var addrInfo in netInterface.GetIPProperties().UnicastAddresses) { 
						if (addrInfo.Address.AddressFamily == AddressFamily.InterNetwork) { 
							var ipAddress = addrInfo.Address; // use ipAddress as needed ... 
							return ipAddress;
						} 
					} 
				}
				return null;
#endif
			}

		

			/// <summary>
			/// Finds the MAC address of the NIC with maximum speed.
			/// </summary>
			/// <returns>The MAC address.</returns>
			public static string GetMacAddress() {
#if __ANDROID__
				try {
					var networkInterfaces = Java.Net.NetworkInterface.NetworkInterfaces;
					while (networkInterfaces.HasMoreElements) {
						var netInterface = (Java.Net.NetworkInterface)networkInterfaces.NextElement();
						//netInterface.GetHardwareAddress
						var ipAddresses = netInterface.InetAddresses;
						while (ipAddresses.HasMoreElements) {
							var inetAddress = (Java.Net.InetAddress)ipAddresses.NextElement();
							if (inetAddress.IsLoopbackAddress)
								continue;
							return ToMacAddressString(inetAddress.GetAddress());
						}
					}
				} catch (SocketException ex) {
					SystemLog.Exception(ex);
				}
					return null;
#elif __WP8__
                return "unknown";
#else
					foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces()) {
						switch(nic.NetworkInterfaceType) {
							case NetworkInterfaceType.Loopback:
							case NetworkInterfaceType.Tunnel:
							case NetworkInterfaceType.Unknown:
								continue;
						}
						foreach (var addrInfo in nic.GetIPProperties().UnicastAddresses) { 
							if (addrInfo.Address.AddressFamily == AddressFamily.InterNetwork) { 
								var ipAddress = addrInfo.Address; // use ipAddress as needed ... 
								return ToMacAddressString(nic.GetPhysicalAddress().GetAddressBytes());
							} 
						} 
					}
 					return null;
#endif
			}

			public static string ToMacAddressString(byte[] bytes) {
				var stringBuilder = new StringBuilder();
				for (int i = 0; i < bytes.Length; i++) {
					// Display the physical address in hexadecimal.
					stringBuilder.AppendFormat("{0}", bytes[i].ToString("X2"));
					// Insert a hyphen after each byte, unless we are at the end of the
					// address.
					if (i != bytes.Length - 1) {
						stringBuilder.AppendFormat("-");
					}
				}
				return stringBuilder.ToString();
			}

		}

	}


