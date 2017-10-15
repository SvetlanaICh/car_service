﻿using CarService1.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CarService.ViewModel
{
    class StatisticsViewModel : INotifyPropertyChanged
    {
        QueriesDB DB;

        private KeyValuePair<int, string> currentDiagramMode;
        private KeyValuePair<int, string> currentDiagramType;

        public Visibility IsLineVisible { get; set; }
        public Visibility IsColumnVisible { get; set; }
        public Visibility IsPieVisible { get; set; }

        public List< KeyValuePair<string, int> > DiagramData { get; set; }
        public List< KeyValuePair<int, string> > DiagramModes { get; set; }
        public List< KeyValuePair<int, string> > DiagramTypes { get; set; }

        public StatisticsViewModel()
        {
            DB = new QueriesDB();

            DiagramData = new List<KeyValuePair<string, int>> { };

            DiagramModes = new List<KeyValuePair<int, string>>
            {
                new KeyValuePair<int, string>(0, "Линейчатая"),
                new KeyValuePair<int, string>(1, "Гистограмма"),
                new KeyValuePair<int, string>(2, "Круговая")
            };

            DiagramTypes = new List<KeyValuePair<int, string>>
            {
                new KeyValuePair<int, string>(0, "Марки авто"),
                new KeyValuePair<int, string>(1, "Заказы - месяцы"),
                new KeyValuePair<int, string>(2, "Заказы - цены")
            };

            if ( DiagramModes != null )
                CurrentDiagramMode = DiagramModes[0];

            if (DiagramTypes != null)
                CurrentDiagramType = DiagramTypes[0];

            SetDiagramVisibility();
            SetDiagramData();
        }

        public KeyValuePair<int, string> CurrentDiagramMode
        {
            get { return currentDiagramMode; }
            set
            {
                currentDiagramMode = value;
                SetDiagramVisibility();
                OnPropertyChanged("CurrentDiagramMode");
                OnPropertyChanged("IsLineVisible");
                OnPropertyChanged("IsColumnVisible");
                OnPropertyChanged("IsPieVisible");
            }
        }

        public KeyValuePair<int, string> CurrentDiagramType
        {
            get { return currentDiagramType; }
            set
            {
                currentDiagramType = value;
                SetDiagramVisibility();
                SetDiagramData();
                OnPropertyChanged("CurrentDiagramType");
                OnPropertyChanged("DiagramData");
            }
        }

        private void SetDiagramVisibility()
        {
            switch (CurrentDiagramMode.Key)
            {
                case 0:
                    IsLineVisible = Visibility.Visible;
                    IsColumnVisible = Visibility.Hidden;
                    IsPieVisible = Visibility.Hidden;
                    break;
                case 1:
                    IsLineVisible = Visibility.Hidden;
                    IsColumnVisible = Visibility.Visible;
                    IsPieVisible = Visibility.Hidden;
                    break;
                case 2:
                    IsLineVisible = Visibility.Hidden;
                    IsColumnVisible = Visibility.Hidden;
                    IsPieVisible = Visibility.Visible;
                    break;
                default:
                    Console.WriteLine("Не сработало...");
                    break;
            }
        }

        private void SetDiagramData()
        {
            switch (CurrentDiagramType.Key)
            {
                case 0:
                    DiagramData = DB.GetDataForDiagram1();
                    break;
                case 1:
                    DiagramData = DB.GetDataForDiagram2();
                    break;
                case 2:
                    DiagramData = DB.GetDataForDiagram3();
                    break;
                default:
                    Console.WriteLine("Не сработало...");
                    break;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}