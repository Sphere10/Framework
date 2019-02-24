////-----------------------------------------------------------------------
//// <copyright file="CsvResult.cs" company="Sphere 10 Software">
////
//// Copyright (c) Sphere 10 Software. All rights reserved. (http://www.sphere10.com)
////
//// Distributed under the MIT software license, see the accompanying file
//// LICENSE or visit http://www.opensource.org/licenses/mit-license.php.
////
//// <author>Herman Schoenfeld</author>
//// <date>2018</date>
//// </copyright>
////-----------------------------------------------------------------------

//using System;
//using System.Data;
//using System.IO;
//using System.Text;
//using System.Web;
//using System.Web.Mvc;

//namespace Sphere10.Framework.Web  {

//	public sealed class CsvResult : FileResult {
//		private readonly DataTable _dataTable;

//		public CsvResult(DataTable dataTable, string fileDownloadName = "Data.csv")
//			: base("text/csv" ) {
//			_dataTable = dataTable;
//			base.FileDownloadName = fileDownloadName;
//		}

//		protected override void WriteFile(HttpResponseBase response) {
//			var outputStream = response.OutputStream;
//			using (var memoryStream = new MemoryStream()) {
//				WriteDataTable(memoryStream);
//				outputStream.Write(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
//			}
//		}

//		private void WriteDataTable(Stream stream) {
//			var streamWriter = new StreamWriter(stream, Encoding.Default);

//			WriteHeaderLine(streamWriter);
//			streamWriter.WriteLine();
//			WriteDataLines(streamWriter);

//			streamWriter.Flush();
//		}

//		private void WriteHeaderLine(StreamWriter streamWriter) {
//			foreach (DataColumn dataColumn in _dataTable.Columns) {
//				WriteValue(streamWriter, dataColumn.ColumnName);
//			}
//		}

//		private void WriteDataLines(StreamWriter streamWriter) {
//			foreach (DataRow dataRow in _dataTable.Rows) {
//				foreach (DataColumn dataColumn in _dataTable.Columns) {
//					WriteValue(streamWriter, dataRow[dataColumn.ColumnName].ToString());
//				}
//				streamWriter.WriteLine();
//			}
//		}


//		private static void WriteValue(StreamWriter writer, String value) {
//			writer.Write("\"");
//			writer.Write(value.Replace("\"", "\"\""));
//			writer.Write("\",");
//		}
//	}

//}
