using CarService.Helpers;
using CarService.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace CarService.ViewModel
{
    class StatisticsViewModel : INotifyPropertyChanged
    {
        private KeyValuePair<int, string> currentDiagramMode;
        private KeyValuePair<int, string> currentDiagramType;
        private string titleOfDiagramSerie;

        public Visibility IsLineVisible { get; set; }
        public Visibility IsColumnVisible { get; set; }
        public Visibility IsPieVisible { get; set; }

        public List< KeyValuePair<string, int> > DiagramData { get; set; }
        public List< KeyValuePair<int, string> > DiagramModes { get; set; }
        public List< KeyValuePair<int, string> > DiagramTypes { get; set; }

        public StatisticsViewModel()
        {
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

            if (!Usefully.IsNullOrEmpty(DiagramModes))
                CurrentDiagramMode = DiagramModes[0];

            if (!Usefully.IsNullOrEmpty(DiagramTypes))
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

        public string TitleOfDiagramSerie
        {
            get { return titleOfDiagramSerie; }
            set
            {
                titleOfDiagramSerie = value;
                OnPropertyChanged("TitleOfDiagramSerie");
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
                    TitleOfDiagramSerie = "Марки авто";
                    DiagramData = ServiceDB.GetDataForDiagramCarBrand();
                    break;
                case 1:
                    int year_current = DateTime.Now.Year;
                    TitleOfDiagramSerie = string.Format("Год {0}", year_current);
                    DiagramData = ServiceDB.GetDataForDiagramMonth();
                    break;
                case 2:
                    TitleOfDiagramSerie = "Цены";
                    List<int> values = new List<int> { 0, 1000, 5000, 10000 };
                    DiagramData = ServiceDB.GetDataForDiagramPrice( ref values);
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
