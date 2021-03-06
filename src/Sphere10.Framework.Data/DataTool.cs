//-----------------------------------------------------------------------
// <copyright file="DataTool.cs" company="Sphere 10 Software">
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
using System.Data;
using Sphere10.Framework;
using System.Reflection;
using Sphere10.Framework.Data;
using Sphere10.Framework.Data.Csv;

namespace Tools {
	
	public static partial class Data {

		public static readonly System.DateTime SQLDateTimeMinValue = new System.DateTime(1753, 1, 1);
		public static readonly System.DateTime SQLDateTimeMaxValue = new System.DateTime(9999, 12, 31);

	    public static DataTable ReadCsv(string filename, bool hasHeaders) {
	        using (var streamReader = new StreamReader(filename))
	            return ReadCsv(streamReader, hasHeaders);
	    }

	    public static DataTable ReadCsv(StreamReader stream, bool hasHeaders) {
	        using (var csvReader = new CsvReader(stream, hasHeaders)) {
	            return csvReader.ToDataTable();
	        }
	    }


	    public static DataTable CreateDataTable(IEnumerable<CellSpec> colSpecs) {
			var table = new DataTable();
			// create columns
			colSpecs.ForEach(c => {
				table.Columns.Add(c.ColumnName);
				var col = table.Columns[c.ColumnName];
				if (!c.ColumnVisible) {
					col.ColumnMapping = MappingType.Hidden;
				}
			});
			return table;
		}



		/// <summary>
		/// Creates a DataTable with columns matching the properties of the given entity.
		/// </summary>
		/// <typeparam name="T">The LinqToSQL type</typeparam>
		/// <returns>A suitably spec'd empty DataTable.</returns>
		public static DataTable CreateDataTableForType<T>() {
			var dataTable = new DataTable();
			foreach (PropertyInfo pi in typeof(T).GetProperties()) {
				Type fieldType = pi.PropertyType;
				if (fieldType.IsNullable()) {
					fieldType = Nullable.GetUnderlyingType(fieldType);
				}
				dataTable.Columns.Add(pi.Name, fieldType);
			}
			return dataTable;
		}
	}
}
