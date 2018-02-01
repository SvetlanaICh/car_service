using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CarService.Helpers;
using CarService.View;
using CarService.Model;
using System.Windows;

namespace CarService.ViewModel
{
    public class MainAlternativeViewModel : INotifyPropertyChanged
    {
        private IStatisticsShower statisticsShower;
        private IPaginalData paginalData;
        private IQueriesDB queriesDB;
		private IPaginalDataCreator paginalDataCreator;

		private KeyValuePair<string, string> currentSortColumn;
        private KeyValuePair<string, string> currentFilterColumn;
        private KeyValuePair<string, string> currentSearchColumn;
		private bool defaultMode;
		private bool sortMode;
		private bool filterMode;
		private bool searchMode;
		private bool hasNavigation;
		private string currentFilterValue;
		private string searchValue;
		private KeyValuePair<bool, string> currentSortMode;
		//private int rowCount;
		private ICommand previous;
		private ICommand next;

		public Visibility NavigationVisibility { get; private set; }
		public List<OrderExtended> ResultCurrent { get; set; }
		public string PageStatus { get; set; }
		public bool HasPrevious { get; set; }
		public bool HasNext { get; set; }
		public ObservableCollection<KeyValuePair<string, string>> ConditionColumns { get; private set; }
		public List<string> FilterValues { get; private set; }
		public List<KeyValuePair<bool, string>> SortModes { get; set; }

		public bool DefaultMode
		{
			get { return defaultMode; }
			set
			{
				defaultMode = value;

				if (defaultMode)
				{
					SortMode = false;
					FilterMode = false;
					SearchMode = false;

					if (paginalData != null)
						paginalData.Create();
				}
				
				OnPropertyChanged("DefaultMode");
			}
		}
		
		public bool SortMode
		{
			get { return sortMode; }
			set
			{
				sortMode = value;

				if (sortMode)
				{
					DefaultMode = false;
					FilterMode = false;
					SearchMode = false;

					DoSort();
				}

				OnPropertyChanged("SortMode");
			}
		}
		
		public bool FilterMode
		{
			get { return filterMode; }
			set
			{
				filterMode = value;

				if (filterMode)
				{
					DefaultMode = false;
					SortMode = false;
					SearchMode = false;

					DoFilter();
				}

				OnPropertyChanged("FilterMode");
			}
		}
		
		public bool SearchMode
		{
			get { return searchMode; }
			set
			{
				searchMode = value;

				if (searchMode)
				{
					DefaultMode = false;
					SortMode = false;
					FilterMode = false;

					DoSearch();
				}

				OnPropertyChanged("SearchMode");
			}
		}
		
		public bool HasNavigation
		{
			get { return hasNavigation; }
			set
			{
				hasNavigation = value;

				if (hasNavigation)
					NavigationVisibility = Visibility.Visible;
				else
					NavigationVisibility = Visibility.Hidden;
				
				OnPropertyChanged("NavigationVisibility");
				OnPropertyChanged("HasNavigation");
				CreatePaginalData();
			}
		}
				
		public int RowCount
        {
            get
            {
                if (paginalData != null)
                    return paginalData.RowCount;
                return 0;   // ?
            }
            set
            {
                if (paginalData != null)
                    paginalData.RowCount = value;
            }
        }

		public string CurrentFilterValue
		{
			get { return currentFilterValue; }
			set
			{
				currentFilterValue = value;

				if (FilterMode)
					DoFilter();
			}
		}
		
		public string SearchValue
		{
			get { return searchValue; }
			set
			{
				searchValue = value;

				if (SearchMode)
					DoSearch();
			}
		}
        		
		public KeyValuePair<bool, string> CurrentSortMode
		{
			get { return currentSortMode; }
			set
			{
				currentSortMode = value;

				if (SortMode)
					DoSort();
			}				
		}

		public KeyValuePair<string, string> CurrentSortColumn
		{
			get { return currentSortColumn; }
			set
			{
				currentSortColumn = value;
				OnPropertyChanged("CurrentSortColumn");

				if (SortMode)
					DoSort();
			}
		}

		public KeyValuePair<string, string> CurrentFilterColumn
		{
			get { return currentFilterColumn; }
			set
			{
				currentFilterColumn = value;

				if (queriesDB != null)
					FilterValues = queriesDB.GetFilterValues(CurrentFilterColumn.Key);

				OnPropertyChanged("FilterValues");
				if (!Usefully.IsNullOrEmpty(FilterValues))
				{
					CurrentFilterValue = FilterValues[0];
					OnPropertyChanged("CurrentFilterValue");
				}
				OnPropertyChanged("CurrentFilterColumn");

				if (FilterMode)
					DoFilter();
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

				if (SearchMode)
					DoSearch();
			}
		}

        public ICommand ShowStatistics { get; private set; }
		
		public ICommand Previous
		{
			get
			{
				return previous ??
				  (previous = new Command( () =>
				  {
					  if (paginalData != null)
						  paginalData.DoPrevious();
				  } )
				  );
			}
		}
		
		public ICommand Next
		{
			get
			{
				return next ??
				  (next = new Command(() =>
				 {
					 if (paginalData != null)
						 paginalData.DoNext();
				 }));
			}
		}

		public MainAlternativeViewModel(   IStatisticsShower statisticsShowerIn
                                , IPaginalDataCreator paginalDataCreatorIn
								, IQueriesDB queriesDBIn
							)
        {
            statisticsShower = statisticsShowerIn;
            queriesDB = queriesDBIn;
			paginalDataCreator = paginalDataCreatorIn;

			DefaultMode = true;
			HasNavigation = true;   //It calls CreatePaginalData()

			ShowStatistics = new Command(DoShowStatistics);

            CreateSortFilterSearchPoperties();
        }

        private void CreateSortFilterSearchPoperties()
        {
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

            if ( !Usefully.IsNullOrEmpty(ConditionColumns) )
            {
                CurrentSortColumn = ConditionColumns[0];
                CurrentFilterColumn = ConditionColumns[0];
                CurrentSearchColumn = ConditionColumns[0];
            }

            if ( !Usefully.IsNullOrEmpty(SortModes) )
                CurrentSortMode = SortModes[0];
        }

        private void CreatePaginalData()
        {
			if (paginalDataCreator == null)
				return;

			paginalData = paginalDataCreator.GetPaginalData(HasNavigation);

            if (paginalData == null)
                return;

            paginalData.PropertyChanged += (o, e) =>
            {
                if (o == null || e == null)
                    return;

                switch (e.PropertyName)
                {
                    case "Result":
                        ResultCurrent = paginalData.Result;
                        OnPropertyChanged("ResultCurrent");
                        break;
                    case "PageStatus":
                        PageStatus = paginalData.PageStatus;
                        OnPropertyChanged("PageStatus");
                        break;
                    case "HasPrevious":
                        HasPrevious = paginalData.HasPrevious;
                        OnPropertyChanged("HasPrevious");
                        break;
                    case "HasNext":
                        HasNext = paginalData.HasNext;
                        OnPropertyChanged("HasNext");
                        break;
                    case "RowCount":
                        //RowCount = paginalData.RowCount;
                        OnPropertyChanged("RowCount");
                        break;
                    default:
                        break;
                }
            };

            paginalData.RefreshProperties();
        }

        private void DoSort()
        {
            if (paginalData != null)
                paginalData.MakeSort(CurrentSortColumn.Key, CurrentSortMode.Key);
        }

        private void DoFilter()
        {
            if (CurrentFilterValue == null)
                return;

            if (paginalData != null)
                paginalData.MakeSearch(CurrentFilterColumn.Key, CurrentFilterValue);
        }

        private void DoSearch()
        {
//			if (SearchValue == null)
//				return;

            if (paginalData != null)
                paginalData.MakeSearch(CurrentSearchColumn.Key, SearchValue);
        }

        private void DoShowStatistics()
        {
            if (statisticsShower != null)
                statisticsShower.StatisticsShow();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
