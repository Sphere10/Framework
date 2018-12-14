//-----------------------------------------------------------------------
// <copyright file="DataTableExtensions.cs" company="Sphere 10 Software">
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
using System.Data;

namespace Sphere10.Framework.Data {
	public static class DataTableExtensions {

#if !__MOBILE__
       public static DataTable SortDataTable(DataTable table, params string[] columns) {
            if (columns.Length == 0) {
                return table;
            }

            var firstColumn = columns.First();

            var result = table.AsEnumerable().OrderBy(r => r[firstColumn]);

            foreach (var columnName in columns.Skip(1)) {
                result = result.ThenBy(r => r[columnName]);
            }

            return result.AsDataView().ToTable();
        }
#endif
		public static bool HasAutoIncrementPrimaryKey(this DataTable dataTable) {
			return dataTable.PrimaryKey.Length == 1 && dataTable.PrimaryKey[0].AutoIncrement;
		}


		public static DataColumn MakeColumnPrimaryKey(this DataTable dataTable, string columnName) {
			return dataTable.MakeColumnPrimaryKey(dataTable.Columns[columnName]);
		}

		public static DataColumn MakeColumnPrimaryKey(this DataTable dataTable, DataColumn col) {
			dataTable.PrimaryKey = Tools.Array.ConcatArrays(dataTable.PrimaryKey, new [] {col});
			return col;
		}

		public static void SetDateTimeMode(this DataTable dataTable, DataSetDateTime mode) {
			foreach (DataColumn column in dataTable.Columns)
				if (column.DataType == typeof(DateTime))
					column.DateTimeMode = mode;
		}

		public static IEnumerable<DataColumn> GetForeignKeyColumns(this DataTable dataTable) {
			return (
				from c in dataTable.Constraints.Cast<Constraint>()
				where c is ForeignKeyConstraint
				select ((ForeignKeyConstraint)c).Columns
			)
			.Cast<IEnumerable<DataColumn>>()
			.Unpartition()
			.Distinct();
		}

		public static DataRow Single(this DataTable dataTable) {
			Preconditions.CheckNotNull(dataTable, "dataTable");
			if (dataTable.Rows.Count != 1)
				throw new SoftwareException("DataTable has {0} rows", dataTable.Rows.Count);

			return dataTable.Rows[0];
		}

		public static DataRow SingleOrDefault(this DataTable dataTable) {
			Preconditions.CheckNotNull(dataTable, "dataTable");
			if (dataTable.Rows.Count > 1)
				throw new SoftwareException("DataTable has more than 1 row");

			if (dataTable.Rows.Count == 0)
				return null;

			return dataTable.Rows[0];
		}

		public static DataTable ToDataTable<T>(this IEnumerable<T> sequence, Func<T, IEnumerable<CellSpec>> typeToRow) {
			var table = new DataTable();
			var dataRows = new List<IEnumerable<CellSpec>>();
			sequence.ForEach(datum => dataRows.Add(typeToRow(datum)));

			if (dataRows.Count > 0) {
				// create columns
				dataRows[0].ForEach(r => {
					table.Columns.Add(r.ColumnName);
					var col = table.Columns[r.ColumnName];
					if (!r.ColumnVisible) {
						col.ColumnMapping = MappingType.Hidden;
					}
				});
			}

			dataRows.ForEach(dataRow => {
				var row = table.NewRow();
				dataRow.ForEach(c => row[c.ColumnName] = c.CellValue);
				table.Rows.Add(row);
			});
			return table;
		}

		/// <summary>
		/// Converts the passed in data table to a CSV-style string.
		/// </summary>
		/// <param name="table">Table to convert</param>
		/// <param name="delimiter">Delimiter used to separate fields</param>
		/// <param name="includeHeader">true - include headers<br/>
		/// false - do not include header column</param>
		/// <returns>Resulting CSV-style string</returns>
		public static string ToCSV(this DataTable table, string delimiter = ",", bool includeHeader = true) {
			var result = new StringBuilder();

			if (includeHeader) {
				table.Columns.Cast<DataColumn>().ForEach(column => {
					result.AppendFormat("{0}{1}", column.ColumnName, delimiter);
				});
				result.Remove(--result.Length, 0);
				result.Append(Environment.NewLine);
			}

			table.Rows.Cast<DataRow>().ForEach(row => {
				row.ItemArray.ForEach(item => {
					if (!(item is System.DBNull)) {
						result.Append(item.ToString().EscapeCSV());
					}
					result.Append(delimiter);
				});
				result.Remove(--result.Length, 0);
				result.Append(Environment.NewLine);
			});

			return result.ToString();
		}

	}
}
