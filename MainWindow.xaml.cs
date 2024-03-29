﻿using System;
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
        PLCTable PLCTable = new PLCTable();

        static ObservableCollection<PLCTable> ObColl = new ObservableCollection<PLCTable>();

        public MainWindow()
        {
            InitializeComponent();


            foreach (string newField in PLCTable.nameFieldPLCTable)
            {
                cbSearchIteam.Items.Add(newField);
            }
            cbSearchIteam.SelectedIndex = 0;

            GetDataTablePLC();
        }

        public string connection = @"Provider = Microsoft.Jet.OLEDB.4.0; Data Source = \\orion\users\OASUTP\Инфа для деж. АСУТП\DataBaseTest НЕ УДАЛЯТЬ\DB_Warehouse.mdb"; // путь к базе данных
        //public string connection = @"Provider = Microsoft.Jet.OLEDB.4.0; Data Source = D:\DB_Warehouse.mdb";
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
            if (ObColl != null)
            {
                ObColl.Clear();
            }
            

            OleDbDataAdapter da = SelectTable(SelectPLCTable());

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

            //CollectionViewSource _itemSourceList = new CollectionViewSource() { Source = ObColl };
            //ICollectionView Itemlist = _itemSourceList.View;

            dtGrid.ItemsSource = ObColl;

        }

        private void Button_Click_Search(object sender, RoutedEventArgs e)
        {
            ReturnFilterTable();
        }


        // Возвращает BOOL в соответсвие с выбранным значением в CombobBox (по какой колонке будем искать)
        public bool ReturnFilterField(FilterEventArgs e, string field)
        {

            var comBoxField = cbSearchIteam.SelectedValue.ToString();
            var obj = e.Item as PLCTable;
            switch (comBoxField)
            {
                case "Наименование": return obj.Name.Contains(field);
                case "Производитель": return obj.Fabric.Contains(field);
                default: return obj.Name.Contains(field);
            }
        }

        private void yourFilter(object sender, FilterEventArgs e)
        {
            var stringFilter = filter.Text;
            var obj = e.Item as PLCTable;
            if (obj != null)
            {
                if (ReturnFilterField(e, stringFilter))
                    e.Accepted = true;
                else
                    e.Accepted = false;
            }
        }

        private void ReturnFilterTable()
        {
            CollectionViewSource _itemSourceList = new CollectionViewSource() { Source = ObColl };
            //now we add our Filter
            _itemSourceList.Filter += new FilterEventHandler(yourFilter);

            // ICollectionView the View/UI part 
            ICollectionView Itemlist = _itemSourceList.View;

            dtGrid.ItemsSource = Itemlist;
        }

        private void filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            ReturnFilterTable();
        }

        private void Button_Click_Refresh(object sender, RoutedEventArgs e)
        {
            GetDataTablePLC();
        }
    }
}
