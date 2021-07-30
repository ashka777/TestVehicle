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
        SqlConnection sqlConnection;

        public MainWindow()
        {
            InitializeComponent();
            string stateConnLabel;
            try
            {
                sqlConnection = Connection.DBConnection(out stateConnLabel);
            }
            catch
            {
                labelConnect.Content = "Подключение с БД отсутствует!";
            }  
            GetData();
        }

        private void GetData()
        {

            // GetAllReport();
            GetModel();
            GetTableGrid();
        }
        private void GetTableGrid()
        {
            string selectSQL = "SELECT ord.Number, br.Name Brand, mod.Name Model, ve.Equipment Equip, vc.Color, ord.TotalSum Total, cus.Name " +
                "FROM tOrder ord " +
                "LEFT JOIN tPrice pr ON pr.Id = ord.Price " +
                "LEFT JOIN tBrand br ON br.Id = ord.BrandId " +
                "LEFT JOIN tModel mod ON mod.Id = ord.ModelId " +
                "LEFT JOIN tVehicleEquipment ve ON ve.Id = pr.EquipmentId " +
                "LEFT JOIN tVehicleColor vc ON vc.Id = pr.ColorId " +
                "LEFT JOIN tCustomer cus ON cus.Id = ord.CustomerId";
            var dgWidth = (int)dataGrid.Width;
            //var dgHeight = dataGrid.Height;
            using (SqlCommand sqlCommand = new SqlCommand(selectSQL, sqlConnection))
            {
                DataTable dataTable = new DataTable("tOrders");                // создаём таблицу в приложении
                sqlCommand.CommandText = selectSQL;                             // присваиваем команде текст
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand); // создаём обработчик
                sqlDataAdapter.Fill(dataTable);
                string[] columnNameProp = new string[] { "Номер", "Марка", "Модель", "Комплектация", "Цвет", "Стоимость", "Заказчик" };
                int[] columnWidthProp = new int[] { 100, 100, 100, 120, 120, 120, 120 };
                int i = 0;
                foreach (DataColumn col in dataTable.Columns)
                {
                    dgWidth -= columnWidthProp[i];
                    if (dgWidth < 100)
                        columnWidthProp[i] += dgWidth;
                    dataGrid.Columns.Add(
                      new DataGridTextColumn
                      {
                          Header = columnNameProp[i],
                          Binding = new Binding(string.Format("{0}", col.ColumnName)),
                          
                    Width = columnWidthProp[i]
                      });
                    //dgWidth -= columnWidthProp[i];
                    i++;
                }

                dataGrid.DataContext = dataTable;
                //DataGridTextColumn textColumn = new DataGridTextColumn();
                //textColumn.Header = "Numbersss";
                //textColumn.Binding = new Binding("Number");
                dataGrid.ItemsSource = dataTable.DefaultView;
                //dataGrid.Columns[0].Width = 100;
                //dataGrid.Columns[0].Header = "Номер";
                //dataGrid.Columns[1].Width = 100;
                //dataGrid.Columns[1].Header = "Марка";
                //dataGrid.Columns[2].Width = 100;
                //dataGrid.Columns[2].Header = "Модель";
                //dataGrid.Columns[3].Width = 120;
                //dataGrid.Columns[3].Header = "Комплектация";
                //dataGrid.Columns[4].Width = 120;
                //dataGrid.Columns[4].Header = "Цвет";
                //dataGrid.Columns[5].Width = 120;
                //dataGrid.Columns[5].Header = "Цена";
                //dataGrid.Columns[6].Width = 120;
                //dataGrid.Columns[6].Header = "Заказчик";
                labelConnect.Content = dataTable.Rows.Count;
            }
               
        }


        public void GetModel() // функция подключения к базе данных и обработка запросов
        {
            string selectSQL = "Select * from dbo.tModel";
            DataTable dataTable = new DataTable("tModel");                // создаём таблицу в приложении
            using (SqlCommand sqlCommand = new SqlCommand(selectSQL, sqlConnection))
            {
                sqlCommand.CommandText = selectSQL;                             // присваиваем команде текст
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand); // создаём обработчик
                sqlDataAdapter.Fill(dataTable);
                modelBox.ItemsSource = dataTable.DefaultView;
                
                //modelBox.DisplayMemberPath = "Name";
                //modelBox.SelectedValuePath = "id";
                //modelBox.Text = "Name";
                //foreach (DataRow dataRow in dataTable.Rows)
                //{
                //    modelBox.Items.Add(dataRow);
                //    //modelBox.DataContext = dataRow;
                //}
            }
        }

        private void ExcelBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void modelBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var select = (ComboBoxItem)modelBox;
        }
    }
}
