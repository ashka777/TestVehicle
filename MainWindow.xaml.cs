using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestVehicle;

namespace TestVehicle
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GetData();
        }

        private void GetData()
        {
            // GetAllReport();
            GetModel();
        }
        //private DataTable GetAllReport()
        //{
        //    string selectSQL = "Select * from dbo.tOrder" +
        //        "INNER JOIN  where InsDate<='20170101'";


        //    return;
        //}

        
        public bool GetModel() // функция подключения к базе данных и обработка запросов
        {
            string selectSQL = "Select * from dbo.tModel";
            DataTable dataTable = new DataTable("tModel");                // создаём таблицу в приложении
            SqlConnection sqlConnection = Connection.DBConnection();                             // подключаемся к базе данных
            using (SqlCommand sqlCommand = new SqlCommand(selectSQL, sqlConnection))
            {
                sqlCommand.CommandText = selectSQL;                             // присваиваем команде текст
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand); // создаём обработчик
                sqlDataAdapter.Fill(dataTable);

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    modelBox.Items.Add(dataRow);
                    modelBox.SelectionBoxItem.
                    modelBox.DataContext = dataRow;
                }


                if (sqlConnection.State == ConnectionState.Open)
                    return true;
                //};
                return false;
            }
        }

        private void ExcelBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
