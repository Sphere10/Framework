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
using System.Reflection;
using System.Web;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Specialized;
using System.Data;
using Sphere10.Framework;

namespace Sphere10.Framework.Data.Linq2Sql {
	public static class Linq2SqlDataTableExtensions {

		/// <summary>
		/// Adds an object entity into the DataTable as a row. 
		/// </summary>
		/// <typeparam name="T">Type of object.</typeparam>
		/// <param name="dataTable">The data table.</param>
		/// <param name="entity">The entity.</param>
		/// <remarks>Entity must have properties with the same type and name as the columns in this DataTable.</remarks>
		public static void AddEntity<T>(this DataTable dataTable, T entity) {
			DataRow dataRow = dataTable.NewRow();
			foreach (DataColumn col in dataTable.Columns) {
				dataRow[col.ColumnName] = entity.GetPropertyValue(col.ColumnName) ?? DBNull.Value;
			}
			dataTable.Rows.Add(dataRow);
		}



	}
}
