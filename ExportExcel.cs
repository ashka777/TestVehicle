using Microsoft.Office.Interop.Excel;
using System;
using Dt = System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Globalization;

namespace TestVehicle
{
    class ExportExcel
    {
        public ExportExcel(Dt.DataTable dataTable)
        {
            int columns = 1;
            Application ExcelApp = new Application();
            Workbook ExcelWorkBook;
            Worksheet ExcelWorkSheet;
           
            ExcelWorkBook = ExcelApp.Workbooks.Add(Type.Missing);
            ExcelWorkSheet = (Worksheet)ExcelWorkBook.Worksheets.get_Item(1);
            ExcelApp.Columns.ColumnWidth = 15;
            List<string> list = ColumnProperty.GetColumnName();
            ExcelApp.Cells[1, 1] = list[1];
            ExcelApp.Cells[1, 2] = list[2];
            ExcelApp.Cells[1, 3] = list[3];
            ExcelApp.Cells[1, 4] = list[4];
            ExcelApp.Cells[1, 5] = list[5];
            ExcelApp.Cells[1, 6] = list[6];
            ExcelApp.Cells[1, 7] = list[7];
            ExcelApp.Cells[1, 8] = list[8];
            ExcelApp.Cells[1, 9] = list[9];
            ExcelApp.Cells[1, 10] = list[10];
            ExcelApp.Cells[1, 11] = list[11];
            ExcelApp.Cells[1, 12] = list[12]; 
            ExcelApp.Cells[1, 13] = list[13];
            
            //foreach (var item in list)
            //{
            //ExcelApp.Cells[1, columns] = item[columns].ToString(new CultureInfo("ru-RU"));
            //columns++;
            // if (columns > 3) break;
            //}



            int rows = 0;
            for (rows = 0; rows< dataTable.Rows.Count; rows++)
            {
                for (columns = 1; columns < dataTable.Columns.Count; columns++)
                {
                    if(columns>1) 
                        if (dataTable.Rows[rows][columns] != DBNull.Value)
                        //var integer = Convert.ToInt32(dataTable.Rows[rows][columns]);
                    if (Convert.ToInt32(dataTable.Rows[rows][columns]) > 25000000)
                        (ExcelWorkSheet.Cells[rows+2,columns] as Microsoft.Office.Interop.Excel.Range).Interior.Color= 245;
                    //j + 2, потому что первая строка отведена для подписей столбцов!
                    ExcelWorkSheet.Cells[rows + 2, columns] = dataTable.Rows[rows][columns];
                }
                //columns++;
            }
            ExcelApp.Visible = true;
            ExcelApp.UserControl = true;
        }
    }
}
