//-----------------------------------------------------------------------
// <copyright file="FileSystemTool.cs" company="Sphere 10 Software">
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Sphere10.Framework;

#if __WP8__
using Windows.Security;
#endif

namespace Tools {


    public static class FileSystem {


        public static long GetFileSize(string filePath) {
            return new FileInfo(filePath).Length;
        }

        public static bool IsFileEmpty(string file) {
            Debug.Assert(File.Exists(file));
            bool retval = false;
            var fileInfo = new FileInfo(file);
            if (fileInfo.Length <= 3) {
                retval = true;
            }
            return retval;
        }

        public static string DetermineAvailableFileName(string directoryPath, string desiredFileName) {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException(directoryPath);

            // Try desired filename
            var desiredPath = Path.Combine(directoryPath, desiredFileName);
            if (!File.Exists(desiredPath))
                return desiredPath;

            // Try variants of desired filename until a free one is found
            var fileName = Path.GetFileNameWithoutExtension(desiredFileName);
            var ext = Path.GetExtension(desiredFileName);
            for (uint i = 2; i <= uint.MaxValue; i++) {
                var candidatePath = Path.Combine(directoryPath, string.Format("{0} {1}{2}", fileName, i, ext));
                if (!File.Exists(candidatePath))
                    return candidatePath;
            }

            // Weird folder! User should not be using computers.
            throw new SoftwareException("Too many files starting with '{0}' found in folder '{1}'", fileName, directoryPath);
        }

        public static void CreateBlankFile(string filename, bool createDirectories = false) {
            string dir = Path.GetDirectoryName(filename);
            Debug.Assert(dir != null, "dir != null");
            if (createDirectories)
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
#if !__WP8__
            File.WriteAllBytes(filename, new byte[0]);
#else
                CompatibilityUtil.WriteAllBytes(filename, new byte[0]);
#endif
        }

        public static void CopyFile(string sourcePath, string destPath, bool overwrite = false, bool createDirectories = false) {
            var dir = Path.GetDirectoryName(destPath);
            if (createDirectories)
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

            File.Copy(sourcePath, destPath, overwrite);
        }

        public static void CopyDirectory(string sourceDir, string destDirectory, bool copySubDirectories, bool createIfDoesNotExist = true,  bool clearIfNotEmpty = false) {
            if (!Directory.Exists(sourceDir)) {
                throw new ArgumentException("Source directory does not exist '{0}'".FormatWith(sourceDir), "sourceDir");
            }

            if (!Directory.Exists(destDirectory)) {
                if (createIfDoesNotExist) {
                    Directory.CreateDirectory(destDirectory);
                } else {
                    throw new ArgumentException("Destination directory does not exist '{0}".FormatWith(destDirectory), "destDirectory");
                }
            }

            if (clearIfNotEmpty && Directory.GetFiles(destDirectory).Length > 0) {
                DeleteAllFiles(destDirectory, true);
            }

            Directory.GetFiles(sourceDir).ForEach(f => CopyFile(f, Path.Combine(destDirectory, Path.GetFileName(f)), true));

            if (copySubDirectories) {
                Directory.GetDirectories(sourceDir).ForEach(d => CopyDirectory(d, Path.Combine(destDirectory, Tools.Paths.GetParentDirectory(d)), true, true, false));
            }
        }

        public static Task CopyDirectoryAsync(string sourceDir, string destDirectory, bool copySubDirectories, bool createIfDoesNotExist = true, bool clearIfNotEmpty = false) {
            return Task.Run(() => CopyDirectory(sourceDir, destDirectory, copySubDirectories, createIfDoesNotExist, clearIfNotEmpty));
        }

        public static void DeleteDirectory(string directory, bool ignoreIfLocked = false) {
            Action<string> deleteDirAction = ignoreIfLocked ? Tools.Lambda.ActionIgnoringExceptions<string>(Directory.Delete) : Directory.Delete;
            DeleteAllFiles(directory, true, true);
            if (Directory.GetFiles(directory).Length == 0) {
                deleteDirAction(directory);
            }
        }

        public static Task DeleteDirectoryAsync(string directory, bool ignoreIfLocked = false) {
            return Task.Run(() => DeleteDirectory(directory, ignoreIfLocked));
        }

        public static void DeleteAllFiles(string directory, bool deleteSubDirectories = true, bool ignoreIfLocked = false) {
            Action<string> deleteFileAction = ignoreIfLocked ? Tools.Lambda.ActionIgnoringExceptions<string>(File.Delete) : File.Delete;
            Action<string> deleteDirAction = ignoreIfLocked ? Tools.Lambda.ActionIgnoringExceptions<string>(Directory.Delete) : Directory.Delete;
            foreach (var file in Directory.GetFiles(directory))
                deleteFileAction(file);

            if (deleteSubDirectories) {
                foreach (var subDirectory in Directory.GetDirectories(directory)) {
                    DeleteAllFiles(subDirectory, true, ignoreIfLocked);
                    deleteDirAction(subDirectory);
                }
            }
        }

        public static Task DeleteAllFilesAsync(string directory, bool deleteSubDirectories = true, bool ignoreIfLocked = false) {
            return Task.Run(() => DeleteAllFiles(directory, deleteSubDirectories, ignoreIfLocked));
        }

        public static void AppendAllBytes(string path, byte[] bytes) {
            if (path == null)
                throw new ArgumentNullException("path");

            if (bytes == null)
                throw new ArgumentNullException("bytes");

            using (var stream = new FileStream(path, FileMode.Append)) {
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        public static byte[] GetFilePart(string filePath, long offset, int fetchSize) {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found", filePath);

            var fileInfo = new FileInfo(filePath);
            if (offset > fileInfo.Length)
                throw new ArgumentOutOfRangeException("offset", "Offset '{0}' larger than file size '{1}'".FormatWith(offset, fileInfo.Length));

            using (var reader = File.OpenRead(filePath)) {
                reader.Seek(offset, SeekOrigin.Begin);
                var bytes = new byte[fetchSize];
                var bytesRead = reader.Read(bytes, 0, fetchSize);
                System.Array.Resize(ref bytes, bytesRead);
                return bytes;
            }
        }

        public static void CompressFile(string sourcePath, string destPath, string password = null) {
            CompressFile<AesManaged>(sourcePath, destPath, password);
        }

        public static void DecompressFile(string sourcePath, string destPath, string password = null) {
            DecompressFile<AesManaged>(sourcePath, destPath, password);
        }

        public static void CompressFile<TSymmetricAlgorithm>(string sourcePath, string destPath, string password = null, PaddingMode paddingMode = PaddingMode.PKCS7, CipherMode cipherMode = CipherMode.CBC) where TSymmetricAlgorithm : SymmetricAlgorithm, new() {
            var hasPassword = !string.IsNullOrEmpty(password);
            Action<Stream, Stream> compressor = Tools.Streams.GZipCompress;
            Action<Stream, Stream> encryptor = (source, dest) => Tools.Streams.Encrypt<TSymmetricAlgorithm>(source, dest, password, null, paddingMode, cipherMode);
            Action<Stream, Stream> noop = (source, dest) => Tools.Streams.RouteStream(source, dest);
            using (var sourceStream = File.OpenRead(sourcePath))
            using (var destStream = File.OpenWrite(destPath))
            using (var streamPipeline = new StreamPipeline(compressor, hasPassword ? encryptor : noop)) {
                streamPipeline.Run(sourceStream, destStream);
            }
        }

        public static void DecompressFile<TSymmetricAlgorithm>(string sourcePath, string destPath, string password = null, PaddingMode paddingMode = PaddingMode.PKCS7, CipherMode cipherMode = CipherMode.CBC) where TSymmetricAlgorithm : SymmetricAlgorithm, new() {
            var hasPassword = !string.IsNullOrEmpty(password);
            Action<Stream, Stream> decryptor = (source, dest) => Tools.Streams.Decrypt<TSymmetricAlgorithm>(source, dest, password, null, paddingMode, cipherMode);
            Action<Stream, Stream> decompressor = Tools.Streams.GZipDecompress;
            Action<Stream, Stream> noop = (source, dest) => Tools.Streams.RouteStream(source, dest);
            using (var sourceStream = File.OpenRead(sourcePath))
            using (var destStream = File.OpenWrite(destPath))
            using (var streamPipeline = new StreamPipeline(hasPassword ? decryptor : noop, decompressor))
                streamPipeline.Run(sourceStream, destStream);
        }


        public static string GetTempEmptyDirectory(bool create = true) {
            var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToStrictAlphaString());
            if (create)
                Directory.CreateDirectory(path);
            return path;

        }

        public static string[] GetFiles(string directory) {
#if __WP8__
            //if (string.IsNullOrWhiteSpace(searchPattern))
            //    searchPattern = "*";
            return Directory.GetFiles(directory, "*");
#else
            return Directory.EnumerateFiles(directory).Select(Path.GetFileName).ToArray(); 
#endif
        }
        public static bool DirectoryContainsFiles(string directory, params string[] filenames) {
            var files = new HashSet<string>(GetFiles(directory));
            return filenames.All(files.Contains);
        }

    }
}

