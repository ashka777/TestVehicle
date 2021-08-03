using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace TestVehicle
{
    internal class Connection:MainWindow
    {
        internal static SqlConnection DBConnection()
        {
            string server = "DESKTOP-JFTOVAG"; // Изменить имя сервера
            string DbName = "DBVehicle";        //Подключить БД
            SqlConnection sqlConnection = new SqlConnection(string.Format("server={0};Trusted_Connection=Yes;DataBase={1};", server, DbName));
            
            try
            {
                    if(sqlConnection.State == ConnectionState.Closed )
                    sqlConnection.Open();
                    return sqlConnection;
            }
            catch (Exception ex)
            {
                error = ex.HResult;
                throw new InvalidOperationException("Ошибка: " + ex.Message);
            }
        }
    }
}
