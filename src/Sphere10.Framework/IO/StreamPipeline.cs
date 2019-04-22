//-----------------------------------------------------------------------
// <copyright file="StreamPipeline.cs" company="Sphere 10 Software">
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
using System.Threading;

namespace Sphere10.Framework {
	public class StreamPipeline : IDisposable {
		private readonly ManualResetEvent _lastStagedFinished;
		private readonly Action<Stream, Stream>[] _filters;
		private List<BlockingStream> _blockingStreams;

		public StreamPipeline(params Action<Stream, Stream>[] filters) {
			if (filters == null) throw new ArgumentNullException("filters");
			if (filters.Length == 0 || Array.IndexOf(filters, null) >= 0)
				throw new ArgumentException("filters");

			_filters = filters;

			_blockingStreams = new List<BlockingStream>(_filters.Length - 1);
			for (var i = 0; i < filters.Length - 1; i++) {
				_blockingStreams.Add(new BlockingStream());
			}
			_lastStagedFinished = new ManualResetEvent(false);
		}

		public void Run(Stream input, Stream output) {
			if (_blockingStreams == null)
				throw new ObjectDisposedException(GetType().Name);
			if (input == null) throw new ArgumentNullException("input");
			if (!input.CanRead) throw new ArgumentException("input");
			if (output == null) throw new ArgumentNullException("output");
			if (!output.CanWrite) throw new ArgumentException("output");

			var errors = new SynchronizedList<Exception>();
			ThreadStart lastStage = null;
			for (var i = 0; i < _filters.Length; i++) {
				var stageInput = i == 0 ? input : _blockingStreams[i - 1];
				var stageOutput =
					i == _filters.Length - 1 ? output : _blockingStreams[i];
				var filter = _filters[i];
				ThreadStart stage = delegate {
					try {
						filter(stageInput, stageOutput);
						var blockingStream = stageOutput as BlockingStream;
						if (blockingStream != null) blockingStream.SetEndOfStream();
					} catch (Exception error) {
						errors.Add(error);
						var blockingStream = stageOutput as BlockingStream;
						if (blockingStream != null) blockingStream.SetEndOfStreamDueToFailure();
					}
				};
				if (i < _filters.Length - 1) {
					var t = new Thread(stage) { IsBackground = true };
					t.Start();
				} else lastStage = stage;
			}
			if (!errors.Any()) {
				try {
					lastStage();
				} catch (Exception error) {
					errors.Add(error);
				}
			}
			if (errors.Any()) {
				throw new AggregateException(errors);
			}
		}

		public void Dispose() {
			if (_blockingStreams == null) return;
			foreach (var stream in _blockingStreams) {
				stream.Dispose();
			}
			_blockingStreams = null;
		}
	}

}
