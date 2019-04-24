//-----------------------------------------------------------------------
// <copyright file="SoundPlayerEx.cs" company="Sphere 10 Software">
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
using System.Text;
using System.Runtime.InteropServices;

namespace Sphere10.Framework.Windows.Forms {


	public class SoundPlayerEx : IDisposable {
		private byte[] bytesToPlay = null;
		public byte[] BytesToPlay {
			get { return bytesToPlay; }
			set {
				FreeHandle();
				bytesToPlay = value;
			}
		}

		GCHandle? gcHandle = null;
		public SoundPlayerEx(System.IO.Stream stream) {
			LoadStream(stream);
		}

		public SoundPlayerEx() {
		}

		public void LoadStream(System.IO.Stream stream) {
			byte[] bytesToPlay = new byte[stream.Length];
			stream.Read(bytesToPlay, 0, (int)stream.Length);
			this.BytesToPlay = bytesToPlay;
		}

		public void PlaySync() {
			FreeHandle();
			NativeMethods.SoundFlags flags = NativeMethods.SoundFlags.SND_SYNC;
			flags |= NativeMethods.SoundFlags.SND_MEMORY;
			NativeMethods.PlaySound(BytesToPlay, (UIntPtr)0, (uint)flags);
		}

		public void PlaySync(NativeMethods.SoundFlags flags) {
			FreeHandle();
			flags |= NativeMethods.SoundFlags.SND_SYNC;
			flags |= NativeMethods.SoundFlags.SND_MEMORY;
			NativeMethods.PlaySound(BytesToPlay, (UIntPtr)0, (uint)flags);
		}

		public void PlayASync() {
			PlayASync(NativeMethods.SoundFlags.SND_ASYNC);
		}
		
		public void PlayASync(NativeMethods.SoundFlags flags) {
			FreeHandle();
			flags |= NativeMethods.SoundFlags.SND_ASYNC;
			if (BytesToPlay != null) {
				gcHandle = GCHandle.Alloc(BytesToPlay, GCHandleType.Pinned);
				flags |= NativeMethods.SoundFlags.SND_MEMORY;
				NativeMethods.PlaySound(gcHandle.Value.AddrOfPinnedObject(), (UIntPtr)0, (uint)flags);
			} else {
				NativeMethods.PlaySound((byte[])null, (UIntPtr)0, (uint)flags);
			}
		}

		private void FreeHandle() {
			NativeMethods.PlaySound((byte[])null, (UIntPtr)0, (uint)0);
			if (gcHandle != null) {
				gcHandle.Value.Free();
				gcHandle = null;
			}
		}
		#region IDisposable Members

		public void Dispose() {
			Dispose(true);
		}

		private void Dispose(bool disposing) {
			BytesToPlay = null;
			if (disposing) {
				GC.SuppressFinalize(this);
			}
		}

		~SoundPlayerEx() {
			Dispose(false);
		}

		#endregion
	}

	public static class NativeMethods {
		[DllImport("winmm.dll", SetLastError = true)]
		public static extern bool PlaySound(string pszSound,
		 System.UIntPtr hmod, uint fdwSound);

		[DllImport("winmm.dll", SetLastError = true)]
		public static extern bool PlaySound(byte[] ptrToSound,
		 System.UIntPtr hmod, uint fdwSound);

		[DllImport("winmm.dll", SetLastError = true)]
		public static extern bool PlaySound(IntPtr ptrToSound,
		 System.UIntPtr hmod, uint fdwSound);

		[Flags]
		public enum SoundFlags : int {
			SND_SYNC = 0x0000,            // play synchronously (default)
			SND_ASYNC = 0x0001,        // play asynchronously
			SND_NODEFAULT = 0x0002,        // silence (!default) if sound not found
			SND_MEMORY = 0x0004,        // pszSound points to a memory file
			SND_LOOP = 0x0008,            // loop the sound until next sndPlaySound
			SND_NOSTOP = 0x0010,        // don't stop any currently playing sound
			SND_NOWAIT = 0x00002000,        // don't wait if the driver is busy
			SND_ALIAS = 0x00010000,        // name is a registry alias
			SND_ALIAS_ID = 0x00110000,        // alias is a predefined id
			SND_FILENAME = 0x00020000,        // name is file name
		}
	}

	//public class SoundPlayerEx : IDisposable {

	//    [DllImport("winmm.dll", SetLastError = true)]
	//    public static extern bool PlaySound(byte[] ptrToSound, UIntPtr hmod, uint fdwSound);

	//    [DllImport("winmm.dll", SetLastError = true)]
	//    public static extern bool PlaySound(IntPtr ptrToSound, UIntPtr hmod, uint fdwSound);

	//    private GCHandle? _gcHandle = null;
	//    private byte[] _bytesToPlay = null;

	//    private byte[] BytesToPlay {
	//        get { return _bytesToPlay; }
	//        set {
	//            FreeHandle();
	//            _bytesToPlay = value;
	//        }
	//    }

	//    public void PlaySound(System.IO.Stream stream) {
	//        PlaySound(stream, SoundFlags.SND_MEMORY | SoundFlags.SND_ASYNC);
	//    }

	//    public void PlaySound(System.IO.Stream stream,
	//                          SoundFlags flags) {
	//        LoadStream(stream);
	//        flags |= SoundFlags.SND_ASYNC;
	//        flags |= SoundFlags.SND_MEMORY;

	//        if (BytesToPlay != null) {
	//            _gcHandle = GCHandle.Alloc(BytesToPlay, GCHandleType.Pinned);
	//            PlaySound(_gcHandle.Value.AddrOfPinnedObject(), (UIntPtr) 0, (uint) flags);
	//        }
	//        else {
	//            PlaySound((byte[]) null, (UIntPtr) 0, (uint) flags);
	//        }
	//    }

	//    private void LoadStream(System.IO.Stream stream) {
	//        if (stream != null) {
	//            byte[] bytesToPlay = new byte[stream.Length];
	//            stream.Read(bytesToPlay, 0, (int) stream.Length);
	//            BytesToPlay = bytesToPlay;
	//        }
	//        else {
	//            BytesToPlay = null;
	//        }
	//    }

	//    private void FreeHandle() {
	//        if (_gcHandle != null) {
	//            PlaySound((byte[]) null, (UIntPtr) 0, (uint) 0);
	//            _gcHandle.Value.Free();
	//            _gcHandle = null;
	//        }
	//    }

	//    public void Dispose() {
	//        FreeHandle();
	//    }

	//    [Flags]
	//    public enum SoundFlags : int {
	//        SND_SYNC = 0x0000, // play synchronously (default)
	//        SND_ASYNC = 0x0001, // play asynchronously
	//        SND_NODEFAULT = 0x0002, // silence (!default) if sound not found
	//        SND_MEMORY = 0x0004, // pszSound points to a memory file
	//        SND_LOOP = 0x0008, // loop the sound until next sndPlaySound
	//        SND_NOSTOP = 0x0010, // don't stop any currently playing sound
	//        SND_NOWAIT = 0x00002000, // don't wait if the driver is busy
	//        SND_ALIAS = 0x00010000, // name is a registry alias
	//        SND_ALIAS_ID = 0x00110000, // alias is a predefined id
	//        SND_FILENAME = 0x00020000, // name is file name
	//    }
	//}
}
