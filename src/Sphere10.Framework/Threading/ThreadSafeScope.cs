//-----------------------------------------------------------------------
// <copyright file="ThreadSafeScope.cs" company="Sphere 10 Software">
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

namespace Sphere10.Framework {

	public class ThreadSafeScope : IDisposable {
		private readonly IThreadSafeObject _owner;
		private readonly bool _write;
	    private readonly Action _additionalDisposeAction;

		public ThreadSafeScope(IThreadSafeObject owner, bool write, Action additionalDisposeAction = null) {
			_owner = owner;
			_write = write;
		    _additionalDisposeAction = additionalDisposeAction;
			if (_write)
				_owner.ThreadLock.EnterWriteLock();
			else
				_owner.ThreadLock.EnterReadLock();
		}

		public virtual void Dispose() {
			if (_write)
				_owner.ThreadLock.ExitWriteLock();
			else
				_owner.ThreadLock.ExitReadLock();
		    if (_additionalDisposeAction != null)
		        _additionalDisposeAction();
		}

	}

}
