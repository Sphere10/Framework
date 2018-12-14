//-----------------------------------------------------------------------
// <copyright file="FileStore.cs" company="Sphere 10 Software">
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

#if !__WP8__
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

#if !__MOBILE__
using System.Security.AccessControl;
#endif

namespace Sphere10.Framework {

	public class FileStore : Disposable {
		private const string DefaultFileStoreDictionaryFileName = "__FileStoreRegistry.xml";
		private readonly SyncronizedDictionary<IPersistedDictionary<string, string>, string, string> _fileRegistry;

		public FileStore() : this(Path.GetTempPath()) {
		}

		public FileStore(string baseDirectory) 
			: this (baseDirectory, FileStorePersistencePolicy.Perist) {
		}

		public FileStore(string baseDirectory, FileStorePersistencePolicy persistencePolicy) 
			: this (baseDirectory, new FileDictionary<string, string>(Path.Combine(baseDirectory, DefaultFileStoreDictionaryFileName), true), persistencePolicy) {
		}

		public FileStore(string baseDirectory, IPersistedDictionary<string, string> fileRegistry, FileStorePersistencePolicy persistencePolicy = FileStorePersistencePolicy.Perist) {
			PersistencePolicy = persistencePolicy;
			if (!Directory.Exists(baseDirectory)) {
				Directory.CreateDirectory(baseDirectory);
			}
			_fileRegistry = new SyncronizedDictionary<IPersistedDictionary<string, string>, string, string>(fileRegistry);
			using (_fileRegistry.EnterWriteScope()) {
				_fileRegistry.InternalDictionary.Load();

				// Delete files not cleaned up since last use
				if (_fileRegistry.Any() && persistencePolicy == FileStorePersistencePolicy.DeleteOnDispose) {
					_fileRegistry.Values.ForEach(file => Tools.Exceptions.ExecuteIgnoringException(() => File.Delete(file)));
					_fileRegistry.Clear();
					_fileRegistry.InternalDictionary.Save();
				}
			}
			BaseDirectory = baseDirectory;
		}

		#region Properties

		public IEnumerable<string> FileAliases { get { return _fileRegistry.Values; } }

		public FileStorePersistencePolicy PersistencePolicy { get; set; }

		protected string BaseDirectory { get; set; }

		#endregion

		#region Methods

		public virtual string GetFilePath(string fileAlias) {
			using (_fileRegistry.EnterWriteScope()) {
				if (!ContainsAlias(fileAlias))
					throw new SoftwareException("No file '{0}' has been registered in the file store", fileAlias);
				return Path.Combine(BaseDirectory, _fileRegistry[fileAlias]);
			}
		}

        public virtual bool ContainsAlias(string alias) {
			return _fileRegistry.ContainsKey(alias);
		}

		public virtual string NewFile() {
			var fileAlias = Guid.NewGuid().ToStrictAlphaString();
			RegisterFile(fileAlias);
			return fileAlias;
		}

        public virtual void RegisterFile(string fileAlias) {
			RegisterMany(new[] { fileAlias });
		}

        public virtual void RegisterMany(IEnumerable<string> fileAliases) {
	        using (_fileRegistry.EnterWriteScope()) {
		        if (fileAliases.Any(alias => alias == DefaultFileStoreDictionaryFileName)) {
			        throw new SoftwareException("Registering a file with alias '{0}' is prohibited", DefaultFileStoreDictionaryFileName);
		        }
		        foreach (var fileAlias in fileAliases) {
			        if (!ContainsAlias(fileAlias)) {
				        _fileRegistry[fileAlias] = GenerateInternalFilePath(fileAlias);
						File.WriteAllBytes(GetFilePath(fileAlias), new byte[0]);				        
			        }
		        }
		        _fileRegistry.InternalDictionary.Save();
	        }
        }

        public virtual void Delete(string fileAlias) {
			DeleteMany(new[] { fileAlias });
		}

        public virtual void DeleteMany(IEnumerable<string> fileAliases) {
	        using (_fileRegistry.EnterWriteScope()) {
		        var exceptions = new List<Exception>();
		        foreach (var file in fileAliases) {
			        try {
				        File.Delete(GetFilePath(file));
				        if (_fileRegistry.ContainsKey(file))
					        _fileRegistry.Remove(_fileRegistry[file]);
			        } catch (Exception error) {
				        exceptions.Add(error);
			        } 
		        }
				_fileRegistry.InternalDictionary.Save();		     
		        if (exceptions.Count > 0)
			        throw new AggregateException(exceptions);
	        }
        }

        public virtual void Clear() {
	        using (_fileRegistry.EnterWriteScope()) {
		        DeleteMany(_fileRegistry.Keys);
		        _fileRegistry.InternalDictionary.Delete();
	        }
        }

		#endregion

		#region File Access Methods

		public FileInfo GetInfo(string fileAlias) {
			return new FileInfo(GetFilePath(fileAlias));
		}

		public FileStream Open(string fileAlias, FileMode mode) {
			return File.Open(GetFilePath(fileAlias), mode);
		}

		public FileStream Open(string fileAlias, FileMode mode, FileAccess access) {
			return File.Open(GetFilePath(fileAlias), mode, access);
		}

		public FileStream Open(string fileAlias, FileMode mode, FileAccess access, FileShare share) {
			return File.Open(GetFilePath(fileAlias), mode, access, share);
		}

		public void SetCreationTime(string fileAlias, DateTime creationTime) {
			File.SetCreationTime(GetFilePath(fileAlias), creationTime);
		}

		public void SetCreationTimeUtc(string fileAlias, DateTime creationTimeUtc) {
			File.SetCreationTimeUtc(GetFilePath(fileAlias), creationTimeUtc);
		}

		public DateTime GetCreationTime(string fileAlias) {
			return File.GetCreationTime(GetFilePath(fileAlias));
		}

		public DateTime GetCreationTimeUtc(string fileAlias) {
			return File.GetCreationTimeUtc(GetFilePath(fileAlias));
		}

		public void SetLastAccessTime(string fileAlias, DateTime lastAccessTime) {
			File.SetLastAccessTime(GetFilePath(fileAlias), lastAccessTime);
		}

		public void SetLastAccessTimeUtc(string fileAlias, DateTime lastAccessTimeUtc) {
			File.SetLastAccessTimeUtc(GetFilePath(fileAlias), lastAccessTimeUtc);
		}

		public DateTime GetLastAccessTime(string fileAlias) {
			return File.GetLastAccessTime(GetFilePath(fileAlias));
		}

		public DateTime GetLastAccessTimeUtc(string fileAlias) {
			return File.GetLastAccessTimeUtc(GetFilePath(fileAlias));
		}

		public void SetLastWriteTime(string fileAlias, DateTime lastWriteTime) {
			File.SetLastAccessTime(GetFilePath(fileAlias), lastWriteTime);
		}

		public void SetLastWriteTimeUtc(string fileAlias, DateTime lastWriteTimeUtc) {
			File.SetLastWriteTimeUtc(GetFilePath(fileAlias), lastWriteTimeUtc);
		}

		public DateTime GetLastWriteTime(string fileAlias) {
			return File.GetLastAccessTime(GetFilePath(fileAlias));
		}

		public DateTime GetLastWriteTimeUtc(string fileAlias) {
			return File.GetLastWriteTimeUtc(GetFilePath(fileAlias));
		}

		public FileAttributes GetAttributes(string fileAlias) {
			return File.GetAttributes(GetFilePath(fileAlias));
		}

		public void SetAttributes(string fileAlias, FileAttributes fileAttributes) {
			File.SetAttributes(GetFilePath(fileAlias), fileAttributes);
		}

#if !__MOBILE__
		public FileSecurity GetAccessControl(string fileAlias) {
			return File.GetAccessControl(GetFilePath(fileAlias));
		}

		public FileSecurity GetAccessControl(string fileAlias, AccessControlSections includeSections) {
			return File.GetAccessControl(GetFilePath(fileAlias), includeSections);
		}

		public void SetAccessControl(string fileAlias, FileSecurity fileSecurity) {
			File.SetAccessControl(GetFilePath(fileAlias), fileSecurity);
		}
#endif
		public FileStream OpenRead(string fileAlias) {
			return File.OpenRead(GetFilePath(fileAlias));
		}

		public FileStream OpenWrite(string fileAlias) {
			return File.OpenWrite(GetFilePath(fileAlias));
		}

		public string ReadAllText(string fileAlias) {
			return File.ReadAllText(GetFilePath(fileAlias));
		}

		public string ReadAllText(string fileAlias, Encoding encoding) {
			return File.ReadAllText(GetFilePath(fileAlias), encoding);
		}

		public void WriteAllText(string fileAlias, string contents) {
			File.WriteAllText(GetFilePath(fileAlias), contents);
		}

		public void WriteAllText(string fileAlias, string contents, Encoding encoding) {
			File.WriteAllText(GetFilePath(fileAlias), contents, encoding);
		}

		public byte[] ReadAllBytes(string fileAlias) {
			return File.ReadAllBytes(GetFilePath(fileAlias));
		}

		public void WriteAllBytes(string fileAlias, byte[] bytes) {
			File.WriteAllBytes(GetFilePath(fileAlias), bytes);
		}

		public void AppendAllBytes(string fileAlias, byte[] bytes) {
			Tools.FileSystem.AppendAllBytes(GetFilePath(fileAlias), bytes);
		}

		public string[] ReadAllLines(string fileAlias) {
			return File.ReadAllLines(GetFilePath(fileAlias));
		}

		public string[] ReadAllLines(string fileAlias, Encoding encoding) {
			return File.ReadAllLines(GetFilePath(fileAlias), encoding);
		}

		public void WriteAllLines(string fileAlias, string[] contents) {
			File.WriteAllLines(GetFilePath(fileAlias), contents);
		}

		public void WriteAllLines(string fileAlias, string[] contents, Encoding encoding) {
			File.WriteAllLines(GetFilePath(fileAlias), contents, encoding);
		}

		public void AppendAllText(string fileAlias, string contents) {
			File.AppendAllText(GetFilePath(fileAlias), contents);
		}

		public void AppendAllText(string fileAlias, string contents, Encoding encoding) {
			File.AppendAllText(GetFilePath(fileAlias), contents, encoding);
		}

		#endregion

		#region Internal Methods

		protected virtual string GenerateInternalFilePath(string fileAlias) {
			return  Guid.NewGuid().ToStrictAlphaString().ToLowerInvariant() + ".file";
		}

		protected override void FreeManagedResources() {
			if (PersistencePolicy == FileStorePersistencePolicy.DeleteOnDispose) {
				Clear();
			}
		}

		#endregion

	}
}

#endif
