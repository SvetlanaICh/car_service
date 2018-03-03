using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CarServiceCore.Helpers;
using CarServiceCore;
using System.Windows;
using CarServiceWPF;
using CarServiceCore.Interfaces;

namespace CarServiceWPF.ViewModel
{
    public class MainAlternativeViewModel : INotifyPropertyChanged
    {
        private IStatisticsShower mStatisticsShower;
        private IPaginalData mPaginalData;
        private IServiceDB mQueriesDB;
		private IPaginalDataCreator mPaginalDataCreator;

		private KeyValuePair<string, string> mCurrentSortColumn;
        private KeyValuePair<string, string> mCurrentFilterColumn;
        private KeyValuePair<string, string> mCurrentSearchColumn;
		private bool mDefaultMode;
		private bool mSortMode;
		private bool mFilterMode;
		private bool mSearchMode;
		private bool mHasNavigation;
		private string mCurrentFilterValue;
		private string mSearchValue;
		private KeyValuePair<bool, string> mCurrentSortMode;
		//private int mRowCount;
		private ICommand mPrevious;
		private ICommand mNext;

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
			get { return mDefaultMode; }
			set
			{
				mDefaultMode = value;

				if (mDefaultMode)
				{
					SortMode = false;
					FilterMode = false;
					SearchMode = false;

					if (mPaginalData != null)
						mPaginalData.Create();
				}
				
				OnPropertyChanged("DefaultMode");
			}
		}
		
		public bool SortMode
		{
			get { return mSortMode; }
			set
			{
				mSortMode = value;

				if (mSortMode)
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
			get { return mFilterMode; }
			set
			{
				mFilterMode = value;

				if (mFilterMode)
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
			get { return mSearchMode; }
			set
			{
				mSearchMode = value;

				if (mSearchMode)
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

		public string CurrentFilterValue
		{
			get { return mCurrentFilterValue; }
			set
			{
				mCurrentFilterValue = value;

				if (FilterMode)
					DoFilter();

				OnPropertyChanged("CurrentFilterValue");
			}
		}
		
		public string SearchValue
		{
			get { return mSearchValue; }
			set
			{
				mSearchValue = value;

				if (SearchMode)
					DoSearch();
			}
		}
        		
		public KeyValuePair<bool, string> CurrentSortMode
		{
			get { return mCurrentSortMode; }
			set
			{
				mCurrentSortMode = value;

				if (SortMode)
					DoSort();
			}				
		}

		public KeyValuePair<string, string> CurrentSortColumn
		{
			get { return mCurrentSortColumn; }
			set
			{
				mCurrentSortColumn = value;
				OnPropertyChanged("CurrentSortColumn");

				if (SortMode)
					DoSort();
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
				
				if (!Usefully.IsNullOrEmpty(FilterValues))
					CurrentFilterValue = FilterValues[0];

				OnPropertyChanged("FilterValues");
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

				if (SearchMode)
					DoSearch();
			}
		}

        public ICommand ShowStatistics { get; private set; }
		
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

		public MainAlternativeViewModel(   IStatisticsShower aStatisticsShower
                                , IPaginalDataCreator aPaginalDataCreator
								, IServiceDB aQueriesDB
							)
        {
            mStatisticsShower = aStatisticsShower;
            mQueriesDB = aQueriesDB;
			mPaginalDataCreator = aPaginalDataCreator;

			DefaultMode = true;
			HasNavigation = true;   //It calls CreatePaginalData()

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
//			if (CurrentFilterValue == null)
//				return;

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
