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
        float dgWidth;
        SqlConnection sqlConnection;
        private int selectModelId = -1;
        private string selectYear = DateTime.Now.Year.ToString();

        public MainWindow()
        {
            InitializeComponent();
            string stateConnLabel;
            try
            {
                sqlConnection = Connection.DBConnection(out stateConnLabel);
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
            GetTableGrid(GetAllModels());
        }

        private DataTable GetAllModels()
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
                selectSQL += ") ";
            selectSQL += "SELECT cte.ModelId, cte.Model, [Date]=MONTH(cte.Date), sum(Total) Total " +
                "into #t1 FROM cte GROUP BY cte.ModelId, cte.Model, MONTH(cte.Date) " +
                "select* from(select ModelId, Model, Total, [Date] as mon from #t1) as t " +
                "pivot(sum(Total) for mon in ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])) as result ";

            if (selectModelId >= 0)
            {
                selectSQL += string.Format("WHERE ModelId = {0}", selectModelId);
            }

            using (SqlCommand sqlCommand = new SqlCommand(selectSQL, sqlConnection))
            {
                DataTable dataTable = new DataTable("tOrders");
                sqlCommand.CommandText = selectSQL;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
        }

        private void GetTableGrid(DataTable dataTable)
        {
            int i = 0;
            dgWidth = (int)dataGrid.Width;
            List<string> columnNameProp = ColumnProperty.GetColumnName();
            float[] columnWidthProp = ColumnProperty.GetColumnWidth(dgWidth);
            foreach (DataColumn col in dataTable.Columns)
            {
                dgWidth -= columnWidthProp[i];
                dataGrid.Columns.Add(
                      new DataGridTextColumn
                      {
                          Header = columnNameProp[i],
                          Width = columnWidthProp[i],
                          Binding = new Binding(string.Format("{0}", col.ColumnName)),
                      });
                i++;
            }
            dataGrid.Columns[0].Visibility = Visibility.Collapsed;
            dataGrid.DataContext = dataTable;
            dataGrid.ItemsSource = dataTable.DefaultView;

            //for (int rows = 0; rows < dataGrid.Items.Count; rows++)
            //{
                foreach (var item in dataGrid.Columns)
                {
                    var s = item;
                
                for (int columns = 2; columns < dataGrid.Items.Count; columns++)
                {
                    if (dataGrid.Items[columns] is not null)
                        if (Convert.ToInt32(dataGrid.Items[columns]) > 25000000)
                            dataGrid.Items[columns] = System.Windows.Media.Brushes.LightYellow;
                }
            }
            //}
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
            ExportExcel excel = new ExportExcel(GetAllModels());
        }

        private void modelBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectModelId = (int)modelBox.SelectedValue;
            GetTableGrid(GetAllModels());
        }

        private void yearBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectYear = (string)yearBox.SelectedValue;
            GetTableGrid(GetAllModels());
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Андрей.\n+79316055903.");
        }
    }
}
