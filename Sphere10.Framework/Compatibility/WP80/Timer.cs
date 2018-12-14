//-----------------------------------------------------------------------
// <copyright file="Timer.cs" company="Sphere 10 Software">
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

#if __WP8__
namespace System.Timers {
    using System.ComponentModel;

    public delegate void ElapsedEventHandler(object sender, ElapsedEventArgs e);

    public sealed class Timer : IDisposable {

        public event ElapsedEventHandler Elapsed
        {
            add { throw new PlatformNotSupportedException("PCL"); }
            remove { throw new PlatformNotSupportedException("PCL"); }
        }

        public bool Enabled { get; set; }

        public double Interval { get; set; }

        public ISynchronizeInvoke SynchronizingObject { get; set; }
        public bool	 AutoReset { get; set; }

        public void Dispose() {
            throw new PlatformNotSupportedException("PCL");
        }

        public void Start() {
            throw new PlatformNotSupportedException("PCL");
        }

        public void Stop() {
            throw new PlatformNotSupportedException("PCL");
        }
    }
}
#endif
