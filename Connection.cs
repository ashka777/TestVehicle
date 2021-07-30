using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestVehicle
{
    internal class Connection
    {
        internal static SqlConnection DBConnection()
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection("server=DESKTOP-JFTOVAG;Trusted_Connection=Yes;DataBase=DBVehicle;");

                if (sqlConnection.State == ConnectionState.Open)
                    return sqlConnection;
                else
                    sqlConnection.Open();
                return sqlConnection;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ошибка: " + ex.Message);
            }
            //return new SqlConnection();


            //    string dbPath = @"E:\Программирование\_С#\Test_Vehicle\DBVehicle.mdf";
            //string connStr = @"server=.\DESKTOP-JFTOVAG;Trusted_Connection=Yes;DataBase=DBVehicle;";
            //string connStr2 = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + dbPath;
            //string connectionString = @"Data Source=E:\Программирование\_С#\Test_Vehicle\DBVehicle.mdf;Initial Catalog=DBVehicle;Integrated Security=True" +
            //                            "providerName =System.Data.SqlClient";
            //OleDbConnection connection = new OleDbConnection(connStr2);
            //connection.Open();
        }
    }
}
