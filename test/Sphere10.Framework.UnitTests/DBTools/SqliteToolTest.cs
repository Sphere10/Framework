//-----------------------------------------------------------------------
// <copyright file="SqliteToolTest.cs" company="Sphere 10 Software">
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
using NUnit.Framework;
using System.IO;
using Sphere10.Framework.Data;

namespace Sphere10.Framework.UnitTests {

    [TestFixture]
    public class SqliteToolTest {
        [Test]
        public void ExistsByFilePath_False() {
            Assert.IsFalse(Tools.Sqlite.ExistsByFilePath(GenerateTempFilename()));
        }

        [Test]
        public void ExistsByFilePath_True() {
            var path = GenerateTempFilename();
            try {
                var dac1 = Tools.Sqlite.Create(path);
                Assert.IsTrue(Tools.Sqlite.ExistsByFilePath(path));
            } finally {
                if (File.Exists(path))
                    File.Delete(path);
            }
        }


        [Test]
        public void Create() {
            var path = GenerateTempFilename();
            try {
                var dac1 = Tools.Sqlite.Create(path);
                System.Console.WriteLine("File Size is " + Tools.FileSystem.GetFileSize(path));
                Assert.IsTrue(File.Exists(path));
            } finally {
                if (File.Exists(path))
                    File.Delete(path);
            }
        }

        [Test]
        public void Create_Skip() {
            var path = GenerateTempFilename();
            try {
                var dac1 = Tools.Sqlite.Create(path);
                Assert.DoesNotThrow( () => Tools.Sqlite.Create(path, existsPolicy: AlreadyExistsPolicy.Skip));
                Assert.IsTrue(File.Exists(path));
            } finally {
                if (File.Exists(path))
                    File.Delete(path);
            }
        }

        [Test]
        public void Create_Overwrite() {
            var path = GenerateTempFilename();
            try {
                var dac1 = Tools.Sqlite.Create(path);
                Assert.DoesNotThrow(() => Tools.Sqlite.Create(path, existsPolicy: AlreadyExistsPolicy.Overwrite));
                Assert.IsTrue(File.Exists(path));
            } finally {
                if (File.Exists(path))
                    File.Delete(path);
            }
        }

        [Test]
        public void Create_Throws() {
            var path = GenerateTempFilename();
            try {
                var dac1 = Tools.Sqlite.Create(path);
                Assert.Catch<Exception>(() => Tools.Sqlite.Create(path));
                Assert.IsTrue(File.Exists(path));
            } finally {
                if (File.Exists(path))
                    File.Delete(path);
            }
        }

        [Test]
        public void Create_Password() {
            var path = GenerateTempFilename();
            try {
                var dac1 = Tools.Sqlite.Create(path, "password");
                Assert.Catch<Exception>( () => {
                    using (var x = Tools.Sqlite.Open(path, "wrong password").BeginScope(true)) {
                        var y = ((SqliteDAC) x.DAC).SelectSqliteMaster();
                    }
                });
                Assert.DoesNotThrow(() => {
                    using (var x = Tools.Sqlite.Open(path, "password").BeginScope(true)) {
                        var y = ((SqliteDAC)x.DAC).SelectSqliteMaster();
                    }
                });
            } finally {
                if (File.Exists(path))
                    File.Delete(path);
            }
        }

        [Test]
        public void Create_PageSize_4096() {
            var path = GenerateTempFilename();
            try {
                var dac1 = Tools.Sqlite.Create(path, pageSize: 4096);
                var pageSize = dac1.ExecuteScalar<long>("PRAGMA page_size;");
                Assert.AreEqual(4096, pageSize);
            } finally {
                if (File.Exists(path))
                    File.Delete(path);
            }
        }

        [Test]
        public void Create_PageSize_32768() {
            var path = GenerateTempFilename();
            try {
                var dac1 = Tools.Sqlite.Create(path, pageSize:32768);
                var pageSize = dac1.ExecuteScalar<long>("PRAGMA page_size;");
                Assert.AreEqual(32768, pageSize);
            } finally {
                if (File.Exists(path))
                    File.Delete(path);
            }
        }

        [Test]
        public void Drop() {
            var path = GenerateTempFilename();
            try {
                var dac = Tools.Sqlite.Create(path);
                Assert.IsTrue(Tools.Sqlite.ExistsByFilePath(path));
                Tools.Sqlite.Drop(path);
                Assert.IsFalse(Tools.Sqlite.ExistsByFilePath(path));
            } finally {
                if (File.Exists(path))
                    File.Delete(path);
            }
        }

        [Test]
        public void Drop_Throws() {
            Assert.Catch<Exception>( () => Tools.Sqlite.Drop(GenerateTempFilename()));
        }

        [Test]
        public void Drop_NoThrows() {
            Assert.DoesNotThrow(() => Tools.Sqlite.Drop(GenerateTempFilename(), false));
        }

        protected string GenerateTempFilename() {
            string path = Path.GetTempFileName();
            File.Delete(path);
            return path;
        }

    }

}
