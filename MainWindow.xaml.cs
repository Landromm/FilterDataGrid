using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace FilterDataGrid
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GetDataTablePLC();
        }

        // public string connection = @"Provider = Microsoft.Jet.OLEDB.4.0; Data Source = \\orion\users\OASUTP\Инфа для деж. АСУТП\DataBaseTest НЕ УДАЛЯТЬ\DB_Warehouse.mdb"; // путь к базе данных
        public string connection = @"Provider = Microsoft.Jet.OLEDB.4.0; Data Source = D:\DB_Warehouse.mdb";
        public OleDbConnection OpenConnectDataBase()
        {
            OleDbConnection connect = new OleDbConnection(connection); // подключаемся к базе данных
            connect.Open(); // открываем базу данных

            return connect;
        }

        public OleDbDataReader Select(string selectSQL) // функция подключения к базе данных и обработка запросов
        {

            OleDbCommand cmd = new OleDbCommand(selectSQL, OpenConnectDataBase()); // создаём запрос
            OleDbDataReader reader = cmd.ExecuteReader(); // получаем данные

            return reader; // возвращаем
        }

        public OleDbDataAdapter SelectTable(string selectSQL) // функция подключения к базе данных и обработка запросов
        {
            OleDbDataAdapter adapter = new OleDbDataAdapter(selectSQL, OpenConnectDataBase());

            return adapter; // возвращаем
        }

        public void ConnectClose()
        {
            OleDbConnection conn = new OleDbConnection();
            conn.Close();
        }

        public string SelectPLCTable()
        {
            return "SELECT * " +
                    "FROM ПЛК;";
        }
        private void GetDataTablePLC()
        {
            OleDbDataAdapter da = SelectTable(SelectPLCTable());

            var ObColl = new ObservableCollection<PLCTable>();

            OleDbCommand cmd = new OleDbCommand(SelectPLCTable(), OpenConnectDataBase());
            OleDbDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                string fabric = reader.GetString(2);
                string model = reader.GetString(3);
                int countModul = reader.GetInt32(4);
                string ip = reader.GetString(5);
                string district = reader.GetString(6);
                string typeOfObject = reader.GetString(7);
                string numberOfObject = reader.GetString(8); 
                string place = reader.GetString(9);
                string typeBoard = reader.GetString(10);
                int number = reader.GetInt32(11);
                string comment = reader.GetString(12);

                PLCTable pLCTable = new PLCTable(id, name, fabric, model, countModul, ip, district, typeOfObject, numberOfObject, place, typeBoard, number, comment);
                ObColl.Add(pLCTable);
            }

            var _itemSourceList = new CollectionViewSource() { Source = ObColl };

            ICollectionView Itemlist = _itemSourceList.View;

            var stringFilter = filter.Text;

            var yourCostumFilter = new Predicate<object>(item => ((PLCTable)item).Name.Contains(stringFilter));

            Itemlist.Filter = yourCostumFilter;

            dtGrid.ItemsSource = Itemlist;
        }

        private void Button_Click_Search(object sender, RoutedEventArgs e)
        {
            GetDataTablePLC();
        }
    }
}
