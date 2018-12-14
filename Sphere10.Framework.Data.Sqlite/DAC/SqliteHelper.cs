//-----------------------------------------------------------------------
// <copyright file="SqliteHelper.cs" company="Sphere 10 Software">
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
#if __IOS__ || __ANDROID__
using Mono.Data.Sqlite;
#else
using System.Data.SQLite;
#endif

namespace Sphere10.Framework.Data {
    public static class SqliteHelper {


        public static SQLiteJournalModeEnum Convert(SqliteJournalMode journalMode) {
            switch (journalMode) {
                case SqliteJournalMode.Delete:
                    return SQLiteJournalModeEnum.Delete;
                case SqliteJournalMode.Off:
                    return SQLiteJournalModeEnum.Off;
                case SqliteJournalMode.Persist:
                    return SQLiteJournalModeEnum.Persist;
#if !__IOS__ && !__ANDROID__
                case SqliteJournalMode.Memory:
                    return SQLiteJournalModeEnum.Memory;
                case SqliteJournalMode.Truncate:
                    return SQLiteJournalModeEnum.Truncate;
                case SqliteJournalMode.Wal:
                    return SQLiteJournalModeEnum.Wal;
                case SqliteJournalMode.Default:
                default:
                    return SQLiteJournalModeEnum.Default;
#else
                default:
                    return SQLiteJournalModeEnum.Delete;
#endif
            }
        }


        public static SqliteJournalMode Convert(SQLiteJournalModeEnum journalMode) {
            switch (journalMode) {
                case SQLiteJournalModeEnum.Delete:
                    return SqliteJournalMode.Delete;
                case SQLiteJournalModeEnum.Off:
                    return SqliteJournalMode.Off;
                case SQLiteJournalModeEnum.Persist:
                    return SqliteJournalMode.Persist;
#if !__IOS__ && !__ANDROID__
                case SQLiteJournalModeEnum.Memory:
                    return SqliteJournalMode.Memory;
                case SQLiteJournalModeEnum.Truncate:
                    return SqliteJournalMode.Truncate;
                case SQLiteJournalModeEnum.Wal:
                    return SqliteJournalMode.Wal;
                case SQLiteJournalModeEnum.Default:
                default:
                    return SqliteJournalMode.Default;
#else
                default:
                    return SqliteJournalMode.Delete;
#endif
            }
        }


        public static SynchronizationModes Convert(SqliteSyncMode syncMode) {
            switch (syncMode) {
                case SqliteSyncMode.Full:
                    return SynchronizationModes.Full;
                case SqliteSyncMode.Off:
                    return SynchronizationModes.Off;
                case SqliteSyncMode.Normal:
                default:
                    return SynchronizationModes.Normal;
            }
        }

        public static SqliteSyncMode Convert(SynchronizationModes syncMode) {
            switch (syncMode) {
                case SynchronizationModes.Full:
                    return SqliteSyncMode.Full;
                case SynchronizationModes.Off:
                    return SqliteSyncMode.Off;
                case SynchronizationModes.Normal:
                default:
                    return SqliteSyncMode.Normal;
            }
        }


    }
}
