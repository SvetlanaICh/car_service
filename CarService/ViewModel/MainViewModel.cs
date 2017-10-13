using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CarService1.ViewModel;
using CarService.View;


namespace CarService.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        QueriesDB DB;
        private StatisticsWindow statisticsWindow;
        private List<OrderExtended> ordersAll;
        private KeyValuePair<string, string> currentSortColumn;
        private KeyValuePair<string, string> currentFilterColumn;
        private KeyValuePair<string, string> currentSearchColumn;

        public ObservableCollection< KeyValuePair<string, string> > ConditionColumns { get; private set; }
        public ObservableCollection< KeyValuePair<string, string> > FilterValues { get; private set; }
        public string SearchValue { get; set; }
        public List< KeyValuePair<bool, string> > SortModes { get; set; }
        public KeyValuePair<bool, string> CurrentSortMode { get; set; }

        public ICommand Sort { get; private set; }
        public ICommand Filter { get; private set; }
        public ICommand Search { get; private set; }
        public ICommand ShowStatistics { get; private set; }

        public MainViewModel()
        {
            statisticsWindow = new StatisticsWindow();

            DB = new QueriesDB();
            if (DB != null)
                OrdersAll = DB.GetOrdersExtended();

            Sort = new Command(DoSort);
            Filter = new Command(DoFilter);
            Search = new Command(DoSearch);
            ShowStatistics = new Command(DoShowStatistics);

            ConditionColumns = new ObservableCollection<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string> ("IdOrder", "ID заказа"),
                new KeyValuePair<string, string> ("CarBrand", "Марка авто"),
                new KeyValuePair<string, string> ("CarModel", "Модель авто"),
                new KeyValuePair<string, string> ("ReleaseYear", "Год выпуска авто"),
                new KeyValuePair<string, string> ("TransmissionType", "Тип трансмиссии"),
                new KeyValuePair<string, string> ("EnginePower", "Мощность двигателя"),
                new KeyValuePair<string, string> ("NameOperation", "Наименование работ"),
                new KeyValuePair<string, string> ("BeginTime", "Время начала работ"),
                new KeyValuePair<string, string> ("EndTime", "Время окончания работ"),
                new KeyValuePair<string, string> ("Price", "Стоимость работ")
            };

            SortModes = new List<KeyValuePair<bool, string>>
            {
                new KeyValuePair<bool, string>(true, "По возрастанию"),
                new KeyValuePair<bool, string>(false, "По убыванию")
            };

            if (ConditionColumns != null)
            { 
                CurrentSortColumn = ConditionColumns[0];
                CurrentFilterColumn = ConditionColumns[0];
                CurrentSearchColumn = ConditionColumns[0];
            }

            if (SortModes != null)
                CurrentSortMode = SortModes[0];
        }

        public List<OrderExtended> OrdersAll
        {
            get { return ordersAll; }
            set
            {
                ordersAll = value;
                OnPropertyChanged("OrdersAll");
            }
        }
  
        public KeyValuePair<string, string> CurrentSortColumn
        {
            get { return currentSortColumn; }
            set
            {
                currentSortColumn = value;
                OnPropertyChanged("CurrentSortColumn");
            }
        }
        
        public KeyValuePair<string, string> CurrentFilterColumn
        {
            get { return currentFilterColumn; }
            set
            {
                currentFilterColumn = value;
                OnPropertyChanged("CurrentFilterColumn");
            }
        }
                  
        public KeyValuePair<string, string> CurrentSearchColumn
        {
            get { return currentSearchColumn; }
            set
            {
                currentSearchColumn = value;
                
                OnPropertyChanged("CurrentSearchColumn");
                SearchValue = null;
                OnPropertyChanged("SearchValue");
            }
        }

        private void DoSort()
        {
            OrdersAll = null;
            OrdersAll = DB.GetSort(CurrentSortColumn.Key, CurrentSortMode.Key);
        }

        private void DoFilter()
        {
            
        }

        private void DoSearch()
        {
            if (SearchValue != null)
            {                
                OrdersAll = DB.GetSearch(CurrentSearchColumn.Key, SearchValue);
            }
        }

        private void DoShowStatistics()
        {
            statisticsWindow = new StatisticsWindow();
            statisticsWindow.Show();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
