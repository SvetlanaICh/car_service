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
using System.Windows.Data;
using System.Windows;
using System.Globalization;

namespace CarService.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        private QueriesDB DB;
        private StatisticsWindow statisticsWindow;
        private List<OrderExtended> ordersCurrentPage;
        private KeyValuePair<string, string> currentSortColumn;
        private KeyValuePair<string, string> currentFilterColumn;
        private KeyValuePair<string, string> currentSearchColumn;
        private int rowCount;
        private int currentPage;        

        public ObservableCollection< KeyValuePair<string, string> > ConditionColumns { get; private set; }
        public List<string> FilterValues { get; private set; }
        public string CurrentFilterValue { get; set; }
        public string SearchValue { get; set; }
        public List< KeyValuePair<bool, string> > SortModes { get; set; }
        public KeyValuePair<bool, string> CurrentSortMode { get; set; }
        public bool PreviousIsEnabled { get; set; }
        public bool NextIsEnabled { get; set; }
        public string PageStatus { set; get; }

        public ICommand Sort { get; private set; }
        public ICommand Filter { get; private set; }
        public ICommand Search { get; private set; }
        public ICommand ShowStatistics { get; private set; }
        public ICommand Previous { get; private set; }
        public ICommand Next { get; private set; }

        public MainViewModel()
        {
            statisticsWindow = new StatisticsWindow();
            //currentPage = 1;
            DB = new QueriesDB();
            if (DB != null)
            {
                //CurrentPage = 1;
                DB.CreateAll();
                RowCount = 10; //
                CheckButtonsEnabled();
            }

            Sort = new Command(DoSort);
            Filter = new Command(DoFilter);
            Search = new Command(DoSearch);
            ShowStatistics = new Command(DoShowStatistics);
            Previous = new Command(DoPrevious);
            Next = new Command(DoNext);

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


        public int RowCount
        {
            get { return rowCount; }
            set
            {
                rowCount = 10;
                if (value != 0)
                    rowCount = value;

                CreateOrdersAll();

                CheckButtonsEnabled();
                OnPropertyChanged("RowCount");
            }
        }

        private void CreateOrdersAll()
        {
            OrdersCurrentPage = null;
            if (RowCount != 0 && DB != null)
            {
                DB.CreateResultAll(RowCount);
                CurrentPage = 1;

                if (DB.ResultAll == null)
                    return;
                if (DB.ResultAll.Count == 0)
                    return;
                if (CurrentPage < 1 || CurrentPage > (DB.ResultAll.Count)) //
                    return;

                OrdersCurrentPage = DB.ResultAll[CurrentPage - 1];
                
            }
            OnPropertyChanged("OrdersCurrentPage");
        }
        
        public int CurrentPage
        {
            get { return currentPage; }
            set
            {
                //
                currentPage = value;
                OnPropertyChanged("CurrentPage");
                PageStatus = string.Format("Стр.{0} из {1}", CurrentPage, DB.GetPageCount(rowCount));
                OnPropertyChanged("PageStatus");
            }
        }       

        public List<OrderExtended> OrdersCurrentPage
        {
            get { return ordersCurrentPage; }
            set
            {
                ordersCurrentPage = null;
                ordersCurrentPage = value;
                OnPropertyChanged("OrdersCurrentPage");
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

                FilterValues = DB.GetFilterValues(CurrentFilterColumn.Key);
                OnPropertyChanged("FilterValues");
                if (FilterValues != null && FilterValues.Count > 0)
                {
                    CurrentFilterValue = FilterValues[0];
                    OnPropertyChanged("CurrentFilterValue");
                }
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
            OrdersCurrentPage = null;
            DB.MakeSort(CurrentSortColumn.Key, CurrentSortMode.Key);
            CreateOrdersAll();
        }

        private void DoFilter()
        {
            if (CurrentFilterValue == null)
                return;
            DB.MakeSearch(CurrentFilterColumn.Key, CurrentFilterValue);
            CreateOrdersAll();
        }

        private void DoSearch()
        {
            if (SearchValue == null)
                return;

            DB.MakeSearch(CurrentSearchColumn.Key, SearchValue);
            CreateOrdersAll();
        }

        private void DoPrevious()
        {
            if (DB == null)
                return;
            if (DB.ResultAll == null)
                return;
            if (DB.ResultAll.Count == 0)
                return;

            if (CurrentPage > 1)
                CurrentPage--;

            OrdersCurrentPage = DB.ResultAll[CurrentPage-1];
            OnPropertyChanged("CurrentPage");

            CheckButtonsEnabled();
        }
        private void DoNext()
        {
            if (DB == null)
                return;
            if (DB.ResultAll == null)
                return;
            if (DB.ResultAll.Count == 0)
                return;

            if (CurrentPage < DB.GetPageCount(rowCount))
                CurrentPage++;

            OrdersCurrentPage = DB.ResultAll[CurrentPage-1];
            OnPropertyChanged("CurrentPage");

            CheckButtonsEnabled();
        }

        private void CheckButtonsEnabled()
        {
            PreviousIsEnabled = false;
            NextIsEnabled = false;

            if (DB == null)
                return;
            if (DB.ResultAll == null)
                return;
            if (DB.ResultAll.Count == 0)
                return;

            if (CurrentPage > 1)
                PreviousIsEnabled = true;
            if (CurrentPage < DB.GetPageCount(rowCount))
                NextIsEnabled = true;

            OnPropertyChanged("PreviousIsEnabled");
            OnPropertyChanged("NextIsEnabled");
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
