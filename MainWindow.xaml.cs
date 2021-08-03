using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace TestVehicle
{
    public partial class MainWindow : Window
    {
        private const int markerLine = 25000000;
        public static float dgWidth;
        private SqlConnection sqlConnection;
        private int selectModelId = -1;
        private string selectYear = DateTime.Now.Year.ToString();
        GetData getData = new GetData();
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                sqlConnection = Connection.DBConnection();
                GetData();
            }
            catch (Exception ex)
            {
                labelConnect.Content = "Ошибка: " + ex.Message;
            }
        }

        private void GetData()
        {
            GetYearComboBox();
            GetModelComboBox();
            GetTableGrid(getData.GetAllModels(selectYear, selectModelId));
        }

        private void GetTableGrid(DataTable dataTable)
        {
            int i = 0;
            dgWidth = (int)dataGrid.Width;
            List<string> columnNameProp = ColumnsProperties.GetColumnName();
            float[] columnWidthProp = ColumnsProperties.GetColumnWidth();
            
            foreach (DataColumn col in dataTable.Columns)
            {
                dgWidth -= columnWidthProp[i];
                dataGrid.Columns.Add(
                      new DataGridTextColumn
                      {
                          Header = columnNameProp[i],
                          Width = columnWidthProp[i],
                          Binding = new Binding(string.Format("{0}", col.ColumnName))
                      }); ;
                i++;
            }

            for (int rows = 0; rows < dataTable.Rows.Count; rows++)
            {
                for (int columns = 1; columns < dataTable.Columns.Count; columns++)
                {
                    if (columns > 1)
                    {
                        if (string.IsNullOrEmpty(dataTable.Rows[rows][columns].ToString()))
                        {
                            dataTable.Rows[rows][columns] = "0";
                        }
                    }
                }
            }

            dataGrid.Columns[0].Visibility = Visibility.Collapsed;
            dataGrid.DataContext = dataTable;
            dataGrid.ItemsSource = dataTable.DefaultView;
            labelConnect.Content = "Всего записей: " + dataTable.Rows.Count;
        }

        public void GetModelComboBox()
        {
            string selectSQL = "Select m.Id ModelId, b.Name +' '+ m.Name as Name from dbo.tBrand as b, dbo.tModel as m where b.Id = m.BrandId";
            DataTable dataTableModel = new DataTable("tModel");                // создаём таблицу в приложении
            using (SqlCommand sqlCommand = new SqlCommand(selectSQL, sqlConnection))
            {
                sqlCommand.CommandText = selectSQL;                             // присваиваем команде текст
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand); // создаём обработчик
                sqlDataAdapter.Fill(dataTableModel);
                dataTableModel.Rows.Add(new string[] { "-1", "Все" });
                modelBox.ItemsSource = dataTableModel.DefaultView;
                modelBox.SelectedIndex = modelBox.Items.Count - 1;
            }
        }

        public void GetYearComboBox()
        {
            string selectSQL = "SELECT DISTINCT Convert(nvarchar,YEAR(InsDate)) [Date] FROM tOrder ORDER BY [Date]";
            DataTable dataTableYear = new DataTable("tYear");                // создаём таблицу в приложении
            using (SqlCommand sqlCommand = new SqlCommand(selectSQL, sqlConnection))
            {
                sqlCommand.CommandText = selectSQL;                             // присваиваем команде текст
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand); // создаём обработчик
                sqlDataAdapter.Fill(dataTableYear);
                dataTableYear.Rows.Add("Все");
                yearBox.ItemsSource = dataTableYear.DefaultView;
                yearBox.SelectedIndex = yearBox.Items.Count-1;
            }
        }

        private void ExcelBtn_Click(object sender, RoutedEventArgs e)
        {
            ExportExcel excel = new ExportExcel(getData.GetAllModels(selectYear, selectModelId), markerLine);
        }

        private void modelBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectModelId = (int)modelBox.SelectedValue;
            GetTableGrid(getData.GetAllModels(selectYear, selectModelId));
        }

        private void yearBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectYear = (string)yearBox.SelectedValue;
            GetTableGrid(getData.GetAllModels(selectYear, selectModelId));
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Андрей.\n+79316055903.");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Закрыть программу?", "Выход", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
                e.Cancel = true;
        }
    }
}
