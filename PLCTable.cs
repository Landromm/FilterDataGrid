using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterDataGrid
{
    class PLCTable
    {
        public PLCTable(int id, string name, string fabric, string model, int countModul, string ip, string district, string typeOfObject, string numberOfObject, string place, string typeBoard, int number, string comment)
        {
            Id = id;
            Name = name;
            Fabric = fabric;
            Model = model;
            CountModul = countModul;
            Ip = ip;
            District = district;
            TypeOfObject = typeOfObject;
            NumberOfObject = numberOfObject;
            Place = place;
            TypeBoard = typeBoard;
            Number = number;
            Comment = comment;
        }

        public PLCTable()
        {
            Console.WriteLine("Использование пустого конструктора.");
        }

        public string[] nameFieldPLCTable = new string[] { "Наименование",
                                                            "Производитель",
                                                            "Модель",
                                                            "Количество модулей"};

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Fabric { get; private set; }
        public string Model { get; private set; }
        public int CountModul { get; private set; }
        public string Ip { get; private set; }
        public string District { get; private set; }
        public string TypeOfObject { get; private set; }
        public string NumberOfObject { get; private set; }
        public string Place { get; private set; }
        public string TypeBoard { get; private set; }
        public int Number { get; private set; }
        public string Comment { get; private set; }
    }
}
