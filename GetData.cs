using System;
using System.Data;
using System.Data.SqlClient;

namespace TestVehicle
{
    class GetData:MainWindow
    {
        internal DataTable GetAllModels(string selectYear, int selectModelId)
        {
           error = 0;
            try
            {
                string selectSQL = "WITH cte as (SELECT ord.ModelId, br.Name +' '+ mod.Name Model, ord.TotalSum Total, ord.InsDate Date " +
                        "FROM tOrder ord " +
                        "LEFT JOIN tPrice pr ON pr.Id = ord.Price " +
                        "LEFT JOIN tBrand br ON br.Id = ord.BrandId " +
                        "LEFT JOIN tModel mod ON mod.Id = ord.ModelId ";

                if (selectYear != "Все")
                {
                    selectSQL += string.Format("WHERE YEAR(ord.InsDate) = {0}) ", selectYear);
                }
                else
                {
                    selectSQL += ") ";
                }

                selectSQL += "SELECT cte.ModelId, cte.Model, [Date]=MONTH(cte.Date), sum(Total) Total " +
                    "into #t1 FROM cte GROUP BY cte.ModelId, cte.Model, MONTH(cte.Date) " +
                    "select* from(select ModelId, Model, Total, [Date] as mon from #t1) as t " +
                    "pivot(sum(Total) for mon in ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])) as result ";

                if (selectModelId >= 0)
                {
                    selectSQL += string.Format("WHERE ModelId = {0}", selectModelId);
                }

                using (SqlCommand sqlCommand = new SqlCommand(selectSQL, Connection.DBConnection()))
                {
                    DataTable dataTable = new DataTable("tOrders");
                    sqlCommand.CommandText = selectSQL;
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    sqlDataAdapter.Fill(dataTable);
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                error = ex.HResult;
                return new DataTable();
            }
        }
    }
}
