//-----------------------------------------------------------------------
// <copyright file="SQLBulkImporter.cs" company="Sphere 10 Software">
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
using System.Reflection;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;

namespace Sphere10.Framework.Data.Linq2Sql {

	/// <summary>
	/// Used for bulk importing records into an SQL Database. Expands on <see cref="SqlBulkCopy"/> in that it provides auto-flush batching (for extremely large datasets)
	/// and type-inferencing based on LinqToSQL objects or standard objects.
	/// </summary>
	/// <typeparam name="T">Type representation of SQL table (properties match column name/type).</typeparam>
	public class SQLBulkImporter<T>  {

		public const int DEFAULT_FLUSH_THRESHOLD = 100000;

		private SqlBulkCopy _bulkCopier;
		private DataTable _entityTable;
		private int _flushThreshhold;

		/// <summary>
		/// Initializes a new instance of the <see cref="SQLBulkImporter&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="flushThreshhold">The threshhold of records in memory before it flushes to the database.</param>
		/// <remarks></remarks>
		public SQLBulkImporter(string connectionString, int flushThreshhold = DEFAULT_FLUSH_THRESHOLD) {
			_bulkCopier = new SqlBulkCopy(connectionString);
			_bulkCopier.BulkCopyTimeout = 60*10;
			_bulkCopier.DestinationTableName = GetTableName(typeof(T));
			_entityTable = CreateDataTableForLinqToSqlEntity<T>();
			_flushThreshhold= flushThreshhold;
		}

		/// <summary>
		/// Queues the specified entity for bulk insertion.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public void BulkInsertEntity(T entity) {
			lock(_entityTable) {
				_entityTable.AddEntity(entity);
				if (_entityTable.Rows.Count >= _flushThreshhold) {
					Flush();
				}
			}
		}

		/// <summary>
		/// Flushes all entities queued for bulk insertion..
		/// </summary>
		/// <remarks></remarks>
		public void Flush() {
			try {
				lock (_entityTable) {
					_bulkCopier.WriteToServer(_entityTable);
					_entityTable.Clear();
					GC.Collect();
				}
			} catch (Exception error) {
				throw new ApplicationException("Failed to bulk import data.", error);
			}
		}

		/// <summary>
		/// Completes this instance (flushes remaining entities).
		/// </summary>
		/// <remarks></remarks>
		public void Complete() {
			Flush();
		}

		#region Auxillary Methods

		private string GetTableName(Type type) {
			var tableAttr = type.GetCustomAttributes(true).FirstOrDefault( a => { return a is System.Data.Linq.Mapping.TableAttribute; });

			if (tableAttr == null) {
				throw new ApplicationException(string.Format("Supplied type {0} did not have TableAttribute", type.Name));
			}

			return tableAttr.GetPropertyValue("Name").ToString();
		}

        /// <summary>
        /// Creates a DataTable with columns matching a LinqToSql entity.
        /// </summary>
        /// <typeparam name="T">The LinqToSQL type</typeparam>
        /// <returns>A suitably spec'd empty DataTable.</returns>
        public static DataTable CreateDataTableForLinqToSqlEntity<T>() {
            DataTable dataTable = new DataTable();
            foreach (PropertyInfo pi in typeof(T).GetProperties()) {
                Type fieldType = pi.PropertyType;
                if (fieldType.IsNullable()) {
                    fieldType = Nullable.GetUnderlyingType(fieldType);
                }
                // include only DB columns
                if (pi.GetCustomAttributes(true).Any(a => a is System.Data.Linq.Mapping.ColumnAttribute)) {
                    dataTable.Columns.Add(pi.Name, fieldType);
                }
            }
            return dataTable;
        }

		#endregion

	}
}
