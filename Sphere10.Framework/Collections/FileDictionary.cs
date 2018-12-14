//-----------------------------------------------------------------------
// <copyright file="FileDictionary.cs" company="Sphere 10 Software">
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
using System.IO;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Reflection;

namespace Sphere10.Framework {

	public class FileDictionary<T1, T2> : DictionaryDecorator<T1, T2>, IPersistedDictionary<T1, T2> {
		private readonly bool _useSimplyXmlSerialization;

        public FileDictionary(bool simpleSerialization = false)
			: this(new Dictionary<T1, T2>(), simpleSerialization) {
        }

		public FileDictionary(IDictionary<T1, T2> internalDictionary, bool simpleSerialization = false)
			: base(internalDictionary)
		{
			Filename = string.Empty;
			_useSimplyXmlSerialization = simpleSerialization;
		}

        public FileDictionary(string filename, bool simpleSerialization = false) 
			: this(filename, new Dictionary<T1, T2>(), simpleSerialization) {
        }

		public FileDictionary(string filename, IDictionary<T1, T2> internalDictionary, bool simpleSerialization = false)
			: base(internalDictionary)
		{
			Debug.Assert(filename != null);
			_useSimplyXmlSerialization = simpleSerialization;
			Filename = filename;
			Load();
		}

        public string Directory {
            get {
                return Path.GetDirectoryName(Filename);
            }
        }

		public string Filename { get; protected set; }

        public virtual void CreateFileForFirstTime() {
            try {
                Tools.FileSystem.CreateBlankFile(Filename, true);
            } catch (Exception error) {
                throw new SoftwareException(
                    error,
                    "Failed to create FileDictionary for first time '{0}'. Possible security access violation",
                    Filename
                );
            }
        }

        public virtual void Load() {
            #region Pre-conditions
			Debug.Assert(Filename != null);
            #endregion
            try {
				if (File.Exists(Filename)) {

#if !__WP8__
                    var surrogate = _useSimplyXmlSerialization ?
						XmlProvider.ReadFromFile<SerializableDictionarySurrogate<T1, T2>>(Filename) :
                        XmlProvider.DeepReadFromFile<SerializableDictionarySurrogate<T1, T2>>(Filename);
#else
					var surrogate = XmlProvider.ReadFromFile<SerializableDictionarySurrogate<T1, T2>>(Filename);
#endif
                    surrogate.ToDictionary(this);
                }
            } catch (Exception error) {
                #region Attempt to recreate file
                try {
                    File.Delete(Filename);
                    CreateFileForFirstTime();
                    Save();
					if (File.Exists(Filename)) {
#if !__WP8__
						var xxx = _useSimplyXmlSerialization ?
							XmlProvider.ReadFromFile<SerializableDictionarySurrogate<T1, T2>>(Filename) :
							XmlProvider.DeepReadFromFile<SerializableDictionarySurrogate<T1, T2>>(Filename);
						if (xxx != null) {
							xxx.ToDictionary(this);
						}
#else
                        XmlProvider.ReadFromFile<SerializableDictionarySurrogate<T1, T2>>(Filename).ToDictionary(this);
#endif
                    }
                #endregion
                } catch (Exception innerError) {
                    throw new SoftwareException(
                        innerError,
                        "Failed to load and recreate FileDictionary '{0}'.",
                        Filename
                    );
                }
            }
        }

        public virtual void Save() {
            try {
				if (!File.Exists(Filename)) {
					CreateFileForFirstTime();
				}
#if !__WP8__
				if (_useSimplyXmlSerialization) 
					XmlProvider.WriteToFile(Filename, new SerializableDictionarySurrogate<T1, T2>(this));
				else
					XmlProvider.DeepWriteToFile(Filename, new SerializableDictionarySurrogate<T1, T2>(this));
#else
                XmlProvider.WriteToFile(Filename, new SerializableDictionarySurrogate<T1, T2>(this));
#endif
            } catch (Exception error) {
                throw new SoftwareException(
                    error,
                    "Failed to write FileDictionary '{0}'.",
                    Filename
                );
            }
        }

		public void Delete() {
			if (File.Exists(Filename))
				File.Delete(Filename);
		}
	}
}



	
