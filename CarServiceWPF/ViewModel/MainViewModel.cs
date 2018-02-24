using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CarServiceCore.Helpers;
using CarServiceCore;
using System.Windows;
//using CarServiceWPF;

namespace CarServiceWPF.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private IStatisticsShower mStatisticsShower;
        private IPaginalData mPaginalData;
        private IServiceDB mQueriesDB;
		private IPaginalDataCreator mPaginalDataCreator;

		private KeyValuePair<string, string> mCurrentSortColumn;
        private KeyValuePair<string, string> mCurrentFilterColumn;
        private KeyValuePair<string, string> mCurrentSearchColumn;
		private bool mHasNavigation;
		private ICommand mRefresh;
		private ICommand mPrevious;
		private ICommand mNext;
		//private int mRowCount;

		public Visibility NavigationVisibility { get; private set; }
		public List<OrderExtended> ResultCurrent { get; set; }
        public string PageStatus { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }        
        public ObservableCollection< KeyValuePair<string, string> > ConditionColumns { get; private set; }
        public List<string> FilterValues { get; private set; }
        public string CurrentFilterValue { get; set; }
        public string SearchValue { get; set; }
        public List< KeyValuePair<bool, string> > SortModes { get; set; }
        public KeyValuePair<bool, string> CurrentSortMode { get; set; }                

        public ICommand Sort { get; private set; }
        public ICommand Filter { get; private set; }
        public ICommand Search { get; private set; }
        public ICommand ShowStatistics { get; private set; }

		public int RowCount
		{
			get
			{
				if (mPaginalData != null)
					return mPaginalData.RowCount;
				return 0;   // ?
			}
			set
			{
				if (mPaginalData != null)
					mPaginalData.RowCount = value;
			}
		}

		public bool HasNavigation
		{
			get { return mHasNavigation; }
			set
			{
				mHasNavigation = value;

				if (mHasNavigation)
					NavigationVisibility = Visibility.Visible;
				else
					NavigationVisibility = Visibility.Collapsed;

				OnPropertyChanged("NavigationVisibility");
				OnPropertyChanged("HasNavigation");
				CreatePaginalData();
			}
		}

		public KeyValuePair<string, string> CurrentSortColumn
		{
			get { return mCurrentSortColumn; }
			set
			{
				mCurrentSortColumn = value;
				OnPropertyChanged("CurrentSortColumn");
			}
		}

		public KeyValuePair<string, string> CurrentFilterColumn
		{
			get { return mCurrentFilterColumn; }
			set
			{
				mCurrentFilterColumn = value;

				if (mQueriesDB != null)
					FilterValues = mQueriesDB.GetFilterValues(CurrentFilterColumn.Key);

				OnPropertyChanged("FilterValues");
				if (!Usefully.IsNullOrEmpty(FilterValues))
				{
					CurrentFilterValue = FilterValues[0];
					OnPropertyChanged("CurrentFilterValue");
				}
				OnPropertyChanged("CurrentFilterColumn");
			}
		}

		public KeyValuePair<string, string> CurrentSearchColumn
		{
			get { return mCurrentSearchColumn; }
			set
			{
				mCurrentSearchColumn = value;

				OnPropertyChanged("CurrentSearchColumn");
				SearchValue = null;
				OnPropertyChanged("SearchValue");
			}
		}

		public ICommand Refresh
		{
			get
			{
				return mRefresh ??
				  (mRefresh = new SimpleCommand(() =>
				  {
					  if (mPaginalData != null)
						  mPaginalData.Create();
				  }));
			}
		}
		
		public ICommand Previous
		{
			get
			{
				return mPrevious ??
				  (mPrevious = new SimpleCommand( () =>
				  {
					  if (mPaginalData != null)
						  mPaginalData.DoPrevious();
				  } )
				  );
			}
		}
		
		public ICommand Next
		{
			get
			{
				return mNext ??
				  (mNext = new SimpleCommand(() =>
				 {
					 if (mPaginalData != null)
						 mPaginalData.DoNext();
				 }));
			}
		}

		public MainViewModel(   IStatisticsShower aStatisticsShower
                                , IPaginalDataCreator aPaginalDataCreator
								, IServiceDB aQueriesDB
                            )
        {
            mStatisticsShower = aStatisticsShower;
            mQueriesDB = aQueriesDB;
			mPaginalDataCreator = aPaginalDataCreator;

			HasNavigation = false;   //It calls CreatePaginalData()

			Sort = new SimpleCommand(DoSort);
            Filter = new SimpleCommand(DoFilter);
            Search = new SimpleCommand(DoSearch);
            ShowStatistics = new SimpleCommand(DoShowStatistics);

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
			if (mPaginalDataCreator == null)
				return;

			mPaginalData = mPaginalDataCreator.GetPaginalData(HasNavigation);

            if (mPaginalData == null)
                return;

            mPaginalData.PropertyChanged += (o, e) =>
            {
                if (o == null || e == null)
                    return;

                switch (e.PropertyName)
                {
                    case "Result":
                        ResultCurrent = mPaginalData.Result;
                        OnPropertyChanged("ResultCurrent");
                        break;
                    case "PageStatus":
                        PageStatus = mPaginalData.PageStatus;
                        OnPropertyChanged("PageStatus");
                        break;
                    case "HasPrevious":
                        HasPrevious = mPaginalData.HasPrevious;
                        OnPropertyChanged("HasPrevious");
                        break;
                    case "HasNext":
                        HasNext = mPaginalData.HasNext;
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

            mPaginalData.RefreshProperties();
        }
        
        private void DoSort()
        {
            if (mPaginalData != null)
                mPaginalData.MakeSort(CurrentSortColumn.Key, CurrentSortMode.Key);
        }

        private void DoFilter()
        {
            if (CurrentFilterValue == null)
                return;

            if (mPaginalData != null)
                mPaginalData.MakeSearch(CurrentFilterColumn.Key, CurrentFilterValue);
        }

        private void DoSearch()
        {
//			if (SearchValue == null)
//				return;

            if (mPaginalData != null)
                mPaginalData.MakeSearch(CurrentSearchColumn.Key, SearchValue);
        }

        private void DoShowStatistics()
        {
            if (mStatisticsShower != null)
                mStatisticsShower.StatisticsShow();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string aProp = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(aProp));
        }
    }
}
