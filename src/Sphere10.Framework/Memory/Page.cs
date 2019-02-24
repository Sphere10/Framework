//-----------------------------------------------------------------------
// <copyright file="Page.cs" company="Sphere 10 Software">
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
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;


namespace Sphere10.Framework {

    public class Page<T> : IDisposable {
        private readonly string _filePath;

        private List<T> Items { get; set; }
       
        public Page(int pageNumber, int startIndex) {
            PageNumber = pageNumber;
            StartIndex = startIndex;
            Size = 0;
            Count = 0;
            Items = new List<T>();
            _filePath = Path.Combine(Path.GetTempPath(), "_LC" + Guid.NewGuid().ToStrictAlphaString().ToUpperInvariant() + ".DAT");
            Loaded = true;
        }

        public static Page<T> First {
            get {
                var p = new Page<T>(0, 0);
                return p;
            }
        }

        public int PageNumber { get; private set; }

        public int StartIndex { get; private set; }

        public int EndIndex {
            get { return StartIndex + Count - 1; }
        }

        public int Count { get; private set; }
        public int Size { get; private set; }

        public bool Loaded { get; private set; }

        public void Dispose() {
            Unload(false);
            if (File.Exists(_filePath)) 
              Tools.Exceptions.ExecuteIgnoringException(() => File.Delete(_filePath));            
        }

        internal T GetItem(int index) {
            if (index < StartIndex || index > EndIndex)
                throw new SoftwareException("Index out of bounds. Requested {0} bounds were {1} - {2}", index, StartIndex, EndIndex);

            return Items[index - StartIndex];
        }

        internal void AddItem(T item, int size) {
            Items.Add(item);
            Count++;
            Size += size;
        }

        internal void Save() {
            var formatter = new BinaryFormatter();
            using (var writeStream = File.Open(_filePath, FileMode.OpenOrCreate, FileAccess.Write)) {
                formatter.Serialize(writeStream, Items);
            }
        }

        internal void Load() {
            if (!File.Exists(_filePath))
                throw new SoftwareException("Unable to load page as the file '{0}' did not exist", _filePath);
            var formatter = new BinaryFormatter();
            using (var readStream = File.OpenRead(_filePath)) {
                Items = (List<T>) formatter.Deserialize(readStream);
            }
            Loaded = true;
        }

        internal void Unload(bool performGC = true) {
            Items.Clear();
            Loaded = false;
            if (performGC) {
                GC.Collect();
                GC.WaitForFullGCComplete();
                GC.WaitForPendingFinalizers();
            }
        }
    }
}
