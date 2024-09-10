using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ContactCenter.Web
{
    public static class ExcelHelperExtensions
    {
        public static FileStreamResult ToExcelFileResult(this PageModel page, DataTable dataTable, string fileName)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet1");
            ws.Cells["A1"].LoadFromDataTable(dataTable, true);
            var ms = new MemoryStream();
            pck.SaveAs(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return page.File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{fileName}.xlsx");
        }

        public static FileStreamResult ToExcelFileResult(this PageModel page, List<ExpandoObject> rows, string fileName)
            => page.ToExcelFileResult(rows.ToDataTable(), fileName);

        public static DataTable ToDataTable(this List<ExpandoObject> objArr)
        {
            if (objArr == null || objArr.Count == 0) return null;
            DataTable table = new DataTable();
            var student_tmp = objArr[0];

            table.Columns.AddRange(student_tmp.Select(field => new DataColumn(field.Key, field.Value?.GetType() ?? typeof(string))).ToArray());

            int fieldCount = student_tmp.Count();
            objArr.All(data =>
            {
                table.Rows.Add(Enumerable.Range(0, fieldCount).Select(index => data.ToList()[index].Value).ToArray());
                return true;
            });
            return table;
        }

        public static ExpandoObject ToExpando(this object anonymousObject)
        {
            IDictionary<string, object> anonymousDictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(anonymousObject);
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (var item in anonymousDictionary)
                expando.Add(item);
            return (ExpandoObject)expando;
        }
    }
}
