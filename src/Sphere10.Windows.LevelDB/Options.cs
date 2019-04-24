//-----------------------------------------------------------------------
// <copyright file="Options.cs" company="Sphere 10 Software">
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

namespace Sphere10.Framework.Windows.LevelDB
{
    /// <summary>
    /// Options to control the behavior of a database (passed to Open)
    /// 
    /// the setter methods for InfoLogger, Env, and Cache only "safe to clean up guarantee". Do not
    /// use Option object if throws.
    /// </summary>
    public class Options : LevelDBHandle
    {
        Env EnvValue { get; set; }
        Cache CacheValue { get; set; }
        Comparator ComparatorValue { get; set; }

        public Options()
        {
            Handle = LevelDBInterop.leveldb_options_create();
        }

        /// <summary>
        /// If true, the database will be created if it is missing.
        /// </summary>
        public bool CreateIfMissing
        {
            set { LevelDBInterop.leveldb_options_set_create_if_missing(Handle, value ? (byte)1 : (byte)0); }
        }

        /// <summary>
        /// If true, an error is raised if the database already exists.
        /// </summary>
        public bool ErrorIfExists
        {
            set { LevelDBInterop.leveldb_options_set_error_if_exists(Handle, value ? (byte)1 : (byte)0); }
        }

        /// <summary>
        /// If true, the implementation will do aggressive checking of the
        /// data it is processing and will stop early if it detects any
        /// errors.  This may have unforeseen ramifications: for example, a
        /// corruption of one DB entry may cause a large number of entries to
        /// become unreadable or for the entire DB to become unopenable.
        /// </summary>
        public bool ParanoidChecks
        {
            set { LevelDBInterop.leveldb_options_set_paranoid_checks(Handle, value ? (byte)1 : (byte)0); }
        }

        /// <summary>
        /// Use the specified Env object to interact with the environment,
        /// e.g. to read/write files, schedule background work, etc.
        /// </summary>
        public Env Env
        {
            get { return EnvValue; }
            set
            {
                LevelDBInterop.leveldb_options_set_env(Handle, value.Handle);
                this.EnvValue = value;
            }
        }

        // Any internal progress/error information generated by the db will
        // be written to info_log if it is non-NULL, or to a file stored
        // in the same directory as the DB contents if info_log is NULL.

        /// <summary>
        /// Amount of data to build up in memory (backed by an unsorted log
        /// on disk) before converting to a sorted on-disk file.
        ///
        /// Larger values increase performance, especially during bulk loads.
        /// Up to two write buffers may be held in memory at the same time,
        /// so you may wish to adjust this parameter to control memory usage.
        /// Also, a larger write buffer will result in a longer recovery time
        /// the next time the database is opened.
        ///
        /// Default: 4MB
        /// </summary>
        public long WriteBufferSize
        {
            set { LevelDBInterop.leveldb_options_set_write_buffer_size(Handle, value); }
        }

        /// <summary>
        /// Number of open files that can be used by the DB.  You may need to
        /// increase this if your database has a large working set (budget
        /// one open file per 2MB of working set).
        ///
        /// Default: 1000
        /// </summary>
        public int MaxOpenFiles
        {
            set { LevelDBInterop.leveldb_options_set_max_open_files(Handle, value); }
        }

        /// <summary>
        /// Control over blocks (user data is stored in a set of blocks, and
        /// a block is the unit of reading from disk).
        ///
        /// If not set, leveldb will automatically create and use an 8MB internal cache.
        /// </summary>
        public Cache Cache
        {
            get { return CacheValue; }
            set
            {
                LevelDBInterop.leveldb_options_set_cache(Handle, value.Handle);
                this.CacheValue = value;
            }
        }

        public Comparator Comparator
        {
            get { return ComparatorValue; }
            set
            {
                LevelDBInterop.leveldb_options_set_comparator(Handle, value.Handle);
                this.ComparatorValue = value;
            }
        }

        /// <summary>
        /// Approximate size of user data packed per block.  Note that the
        /// block size specified here corresponds to uncompressed data.  The
        /// actual size of the unit read from disk may be smaller if
        /// compression is enabled.  This parameter can be changed dynamically.
        ///
        /// Default: 4K
        /// </summary>
        public long BlockSize
        {
            set { LevelDBInterop.leveldb_options_set_block_size(Handle, value); }
        }

        /// <summary>
        /// Number of keys between restart points for delta encoding of keys.
        /// This parameter can be changed dynamically.  
        /// Most clients should leave this parameter alone.
        ///
        /// Default: 16
        /// </summary>
        public int RestartInterval
        {
            set { LevelDBInterop.leveldb_options_set_block_restart_interval(Handle, value); }
        }

        /// <summary>
        /// Compress blocks using the specified compression algorithm.  
        /// This parameter can be changed dynamically.
        ///
        /// Default: kSnappyCompression, which gives lightweight but fast compression.
        ///
        /// Typical speeds of kSnappyCompression on an Intel(R) Core(TM)2 2.4GHz:
        ///    ~200-500MB/s compression
        ///    ~400-800MB/s decompression
        /// Note that these speeds are significantly faster than most
        /// persistent storage speeds, and therefore it is typically never
        /// worth switching to kNoCompression.  Even if the input data is
        /// incompressible, the kSnappyCompression implementation will
        /// efficiently detect that and will switch to uncompressed mode.
        /// </summary>
        public CompressionLevel CompressionLevel
        {
            set { LevelDBInterop.leveldb_options_set_compression(Handle, (int)value); }
        }

        protected override void FreeUnManagedObjects()
        {
            LevelDBInterop.leveldb_options_destroy(Handle);
        }
    }
}
