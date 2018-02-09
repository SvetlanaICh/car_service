using CarService.Helpers;
using CarService.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace CarService.ViewModel
{
    public class StatisticsViewModel : INotifyPropertyChanged
    {
        IDiagramData mDiagramDataObj;
        private KeyValuePair<int, string> mCurrentDiagramMode;
        private KeyValuePair<int, string> mCurrentDiagramType;
        private string mTitleOfDiagramSerie;
		private int mYear;
		private Visibility mYearVisibility;

		public Visibility IsLineVisible { get; set; }
        public Visibility IsColumnVisible { get; set; }
        public Visibility IsPieVisible { get; set; }
		public List< KeyValuePair<string, int> > DiagramData { get; set; }
        public List< KeyValuePair<int, string> > DiagramModes { get; set; }
        public List< KeyValuePair<int, string> > DiagramTypes { get; set; }

        public StatisticsViewModel(IDiagramData aDiagramData)
        {
			mDiagramDataObj = aDiagramData;

			SetDefaultState();

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

			Year = 2017;    //It calls SetDiagramData();
		}

		private void SetDefaultState()
		{
			TitleOfDiagramSerie = "";
			DiagramData = null;
			IsLineVisible = Visibility.Hidden;
			IsColumnVisible = Visibility.Hidden;
			IsPieVisible = Visibility.Hidden;
			OnPropertyChanged("IsLineVisible");
			OnPropertyChanged("IsColumnVisible");
			OnPropertyChanged("IsPieVisible");
			YearVisibility = Visibility.Hidden;
		}

        public KeyValuePair<int, string> CurrentDiagramMode
        {
            get { return mCurrentDiagramMode; }
            set
            {
                mCurrentDiagramMode = value;
                SetDiagramVisibility();                
            }
        }

        public KeyValuePair<int, string> CurrentDiagramType
        {
            get { return mCurrentDiagramType; }
            set
            {
                mCurrentDiagramType = value;

				if (CurrentDiagramType.Key == 1)
					YearVisibility = Visibility.Visible;
				else
					YearVisibility = Visibility.Hidden;

				SetDiagramData();
                OnPropertyChanged("CurrentDiagramType");
            }
        }

        public string TitleOfDiagramSerie
        {
            get { return mTitleOfDiagramSerie; }
            set
            {
                mTitleOfDiagramSerie = value;
                OnPropertyChanged("TitleOfDiagramSerie");
            }
        }

		public int Year
		{
			get
			{
				if (mDiagramDataObj != null)
					mYear = mDiagramDataObj.Year;

				return mYear;
			}
			set
			{
				mYear = value;

				if (mDiagramDataObj != null)
					mDiagramDataObj.Year = mYear;

				SetDiagramData();
				OnPropertyChanged("Year");
			}
		}

		public Visibility YearVisibility
		{
			get { return mYearVisibility; }
			set
			{
				mYearVisibility = value;
				OnPropertyChanged("YearVisibility");
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
					IsLineVisible = Visibility.Hidden;
					IsColumnVisible = Visibility.Hidden;
					IsPieVisible = Visibility.Hidden;
					break;
            }
			OnPropertyChanged("CurrentDiagramMode");
			OnPropertyChanged("IsLineVisible");
			OnPropertyChanged("IsColumnVisible");
			OnPropertyChanged("IsPieVisible");
		}

        private void SetDiagramData()
        {
            if (mDiagramDataObj == null)
                return;

			switch (CurrentDiagramType.Key)
            {
                case 0:
                    TitleOfDiagramSerie = "Марки авто";
                    DiagramData = mDiagramDataObj.DataForDiagramCarBrand;
                    break;
                case 1:
                    TitleOfDiagramSerie = string.Format("Год {0}", mDiagramDataObj.Year);
					DiagramData = mDiagramDataObj.DataForDiagramMonth;
					break;
                case 2:
                    TitleOfDiagramSerie = "Цены";                    
                    DiagramData = mDiagramDataObj.DataForDiagramPrice;
                    break;
                default:
					TitleOfDiagramSerie = "";
					DiagramData = null;
                    break;
            }
			OnPropertyChanged("DiagramData");
		}


		public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string aProp = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(aProp));
        }
    }
}
