//-----------------------------------------------------------------------
// <copyright file="StreamDecorator.cs" company="Sphere 10 Software">
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
using System.Threading;
using System.Threading.Tasks;

namespace Sphere10.Framework {
    public abstract class StreamDecorator : Stream {

        private readonly Stream _decoratedStream;

        protected StreamDecorator(Stream stream) {
            _decoratedStream = stream;
        }

        protected Stream InnerStream { get {  return _decoratedStream; } }

        public override void Flush() {
            _decoratedStream.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin) {
            return _decoratedStream.Seek(offset, origin);
        }

        public override void SetLength(long value) {
            _decoratedStream.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count) {
            return _decoratedStream.Read(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count) {
            _decoratedStream.Write(buffer, offset, count);
        }

        public override bool CanRead {
            get { return _decoratedStream.CanRead; }
        }

        public override bool CanSeek {
            get { return _decoratedStream.CanSeek; }
        }

        public override bool CanWrite {
            get { return _decoratedStream.CanWrite; }
        }

        public override long Length {
            get { return _decoratedStream.Length; }
        }

        public override long Position {
            get { return _decoratedStream.Position; }
            set { _decoratedStream.Position = value; }
        }

        protected override void Dispose(bool disposing) {
            _decoratedStream.Dispose();
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state) {
            return _decoratedStream.BeginRead(buffer, offset, count, callback, state);
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state) {
            return _decoratedStream.BeginWrite(buffer, offset, count, callback, state);
        }

        public override bool CanTimeout {
            get { return _decoratedStream.CanTimeout; }
        }

        public override void Close() {
            _decoratedStream.Close();
        }

        public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken) {
            return _decoratedStream.CopyToAsync(destination, bufferSize, cancellationToken);
        }

        public override int EndRead(IAsyncResult asyncResult) {
            return _decoratedStream.EndRead(asyncResult);
        }

        public override void EndWrite(IAsyncResult asyncResult) {
            _decoratedStream.EndWrite(asyncResult);
        }

        public override bool Equals(object obj) {
            return _decoratedStream.Equals(obj);
        }

        public override Task FlushAsync(CancellationToken cancellationToken) {
            return _decoratedStream.FlushAsync(cancellationToken);
        }

        public override int GetHashCode() {
            return _decoratedStream.GetHashCode();
        }

#if !__MOBILE__
        public override object InitializeLifetimeService() {
            return _decoratedStream.InitializeLifetimeService();
        }
#endif

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) {
            return _decoratedStream.ReadAsync(buffer, offset, count, cancellationToken);
        }

        public override int ReadByte() {
            return _decoratedStream.ReadByte();
        }

        public override int ReadTimeout {
            get { return _decoratedStream.ReadTimeout; }
            set { _decoratedStream.ReadTimeout = value; }
        }

        public override string ToString() {
            return _decoratedStream.ToString();
        }

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) {
            return _decoratedStream.WriteAsync(buffer, offset, count, cancellationToken);
        }

        public override void WriteByte(byte value) {
            _decoratedStream.WriteByte(value);
        }

        public override int WriteTimeout {
            get { return _decoratedStream.WriteTimeout; }
            set { _decoratedStream.WriteTimeout = value; }
        }

#if !__WP8__
        protected override WaitHandle CreateWaitHandle() {
            throw new NotSupportedException();
        }

        protected override void ObjectInvariant() {
            throw new NotSupportedException();
        }
#endif
    }
}
