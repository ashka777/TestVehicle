using System;
using System.Data.SqlClient;

namespace TestVehicle
{
    internal class Connection
    {
        internal static SqlConnection DBConnection()
        {
            string server = "DESKTOP-JFTOVAG"; // Изменить имя сервера
            string DbName = "DBVehicle";        //Подключить БД
            try
            {
                SqlConnection sqlConnection = new SqlConnection(string.Format("server={0};Trusted_Connection=Yes;DataBase={1};", server, DbName));
                return sqlConnection;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ошибка: " + ex.Message);
            }
        }
    }
}
