//-----------------------------------------------------------------------
// <copyright file="SessionCache.cs" company="Sphere 10 Software">
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
using System.Linq;
using Sphere10.Framework.Scheduler;

namespace Sphere10.Framework {
    public class SessionCache<K,T> : CacheBase<K,T> {
        private readonly Scheduler<IJob> _cleaner; 

        public SessionCache(TimeSpan sessionTimeoutInterval) : base(CacheReapPolicy.None, ExpirationPolicy.SinceLastAccessedTime, expirationDuration: sessionTimeoutInterval) {
            _cleaner = new Scheduler<IJob>(SchedulerPolicy.DisposeWhenFinished | SchedulerPolicy.RemoveJobOnError | SchedulerPolicy.DontThrow);
            var job = JobBuilder.For(Cleanup).RunAsyncronously().RunOnce(DateTime.Now.Add(sessionTimeoutInterval)).Repeat.OnInterval(sessionTimeoutInterval).Build();
            _cleaner.AddJob(job);
            _cleaner.Start();
        }

        public void KeepAlive(K sessionID) {
            base.Get(sessionID); // refresh cache
        }

        protected override uint EstimateSize(T value) {
            return 0;
        }

        protected override T Fetch(K key) {
            throw new Exception(string.Format("Session '{0}' does not exist or has expired", key));
        }

        protected virtual void Cleanup() {
            using (base.EnterWriteScope()) {				
                var expired = base.InternalStorage.Where(kvp => IsExpired(kvp.Value)).ToArray();
                expired.ForEach(x => base.Remove(x.Key));
            }
        }

        public override void Dispose() {
            _cleaner.Stop();
            _cleaner.Dispose();
        }
    }
}
