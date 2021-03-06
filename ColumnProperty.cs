using System;
using System.Collections.Generic;
using System.Linq;

namespace TestVehicle
{
    class ColumnsProperties
    {
        const float columnModel = 100;

        public static List<string> GetColumnName()
        {
            List<string> columnNameProp = new List<string>();
            columnNameProp.AddRange(new string[] { "ModelId", "Модель" });
            for (int m = 0; m < 12; m++)
            {
                columnNameProp.Add(DateTime.MinValue.AddMonths(m).ToString("MMMM"));
            }
            return columnNameProp;
        }

        public static float[] GetColumnWidth()
        {
            MainWindow.dgWidth -= columnModel; // размер datagrid минус столбец Модель
            float[] columnWidthProp = new float[14];
            columnWidthProp[1] = columnModel;
            for (int m = 2; m < columnWidthProp.Count(); m++)
            {
                columnWidthProp[m] = MainWindow.dgWidth / 12;
            }
            return columnWidthProp;
        }
    }
}
