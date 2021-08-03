using System;
using System.Collections.Generic;
using System.Windows;
using AP = Microsoft.Office.Interop.Excel;
using Dt = System.Data;

namespace TestVehicle
{
    class ExportExcel
    {
        public ExportExcel(Dt.DataTable dataTable, int markerLine)
        {
            try
            {
                AP.Application ExcelApp = new AP.Application();
                AP.Workbook ExcelWorkBook;
                AP.Worksheet ExcelWorkSheet;
                List<string> list = ColumnsProperties.GetColumnName();
                ExcelWorkBook = ExcelApp.Workbooks.Add(Type.Missing);
                ExcelWorkSheet = (AP.Worksheet)ExcelWorkBook.Worksheets.get_Item(1);
                ExcelApp.Columns.ColumnWidth = 15;
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

                for (int rows = 0; rows < dataTable.Rows.Count; rows++)
                {
                    for (int columns = 1; columns < dataTable.Columns.Count; columns++)
                    {
                        if (columns > 1)
                        {
                            if (dataTable.Rows[rows][columns] != DBNull.Value)
                                if (Convert.ToInt32(dataTable.Rows[rows][columns]) > markerLine)
                                    (ExcelWorkSheet.Cells[rows + 2, columns] as AP.Range).Interior.Color = 245;
                            //j + 2, первая строка отведена для шапки!
                            if (string.IsNullOrEmpty(dataTable.Rows[rows][columns].ToString()))
                            {
                                dataTable.Rows[rows][columns] = "0";
                            }
                        }
                        ExcelWorkSheet.Cells[rows + 2, columns] = dataTable.Rows[rows][columns];
                    }
                }

                ExcelApp.Visible = true;
                ExcelApp.DisplayFullScreen = false;
                ExcelApp.WindowState = AP.XlWindowState.xlMaximized;
                ExcelApp.UserControl = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }
    }
}
