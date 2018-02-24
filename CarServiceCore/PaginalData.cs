using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CarServiceCore.Helpers;

namespace CarServiceCore
{
    public class PaginalData : IPaginalData
    {
		private List<List<OrderExtended>> mResultAll;
        private IDataHandler mDataHandler;
        private int mRowCount = 10;
        private int mCurrentPageReal;

		public List<OrderExtended> Result
		{
			get { return GetRusultCurrent(); }
		} 
        
        private List<OrderExtended> GetRusultCurrent()
        {
            if (Usefully.IsNullOrEmpty(mResultAll))
                return null;

            if (CurrentPageReal < 0 || CurrentPageReal >= mResultAll.Count)
            {
                CurrentPageReal = 0;
                return GetRusultCurrent();    //Is it ok?
            }

            return mResultAll[CurrentPageReal];
        }

        public int RowCount
        {
            get { return mRowCount; }
            set
            {
                if (value <= 0) 
                    return;

				mRowCount = value;
                CreateResultAll();
                OnPropertyChanged("RowCount");
            }
        }
        
        private int CurrentPageReal
        {
            get { return mCurrentPageReal; }
            set
            {
                if (Usefully.IsNullOrEmpty(mResultAll))
                {
					mCurrentPageReal = 0;
                    //OnPropertyChanged("CurrentPageReal");
                    OnPropertyChanged("CurrentPageDisplayed");
                    OnPropertyChanged("PageStatus");
                    OnPropertyChanged("Result");
                    OnPropertyChanged("HasPrevious");
                    OnPropertyChanged("HasNext");
                    return;
                }
                
                if (value < 0 || value >= mResultAll.Count)
                {
                    if (mCurrentPageReal >= 0 && mCurrentPageReal < mResultAll.Count) //На всякий пожарный
                        return;
                    else
                        value = 0;
                }

				mCurrentPageReal = value;
                //OnPropertyChanged("CurrentPageReal");
                OnPropertyChanged("CurrentPageDisplayed");
                OnPropertyChanged("PageStatus");
                OnPropertyChanged("Result");
                OnPropertyChanged("HasPrevious");
                OnPropertyChanged("HasNext");
            }
        }

        public int CurrentPageDisplayed
        {
            get
            {
                if (PageCount == 0)
                    return 0;

                return CurrentPageReal + 1;
            }
        }

        public string PageStatus
        {
            get
            {
                return string.Format("Стр.{0} из {1}", CurrentPageDisplayed, PageCount);
            }
        }

        public int PageCount
        {
            get
            {
                if (mDataHandler == null)
                    return 0;
                if (mRowCount == 0 || mDataHandler.Result == null)
                    return 0;           //
                decimal d = (decimal)mDataHandler.Result.Count / (decimal)mRowCount;
                decimal p_d = Math.Ceiling(d);
                return Convert.ToInt32(p_d);
            }
        }

        public PaginalData(IDataHandler aDataHandler)
        {
            mDataHandler = aDataHandler;
			mRowCount = 15;
            CreateResultAll();
        }

        public void RefreshProperties()
        {
            OnPropertyChanged("RowCount");            
            OnPropertyChanged("CurrentPageDisplayed");
            OnPropertyChanged("PageStatus");
            OnPropertyChanged("Result");
            OnPropertyChanged("HasPrevious");
            OnPropertyChanged("HasNext");
			//OnPropertyChanged("CurrentPageReal");
		}

		private void CreateResultAll(int aPageCurrent = 0) //Заполнение листа листов + задание текущей страницы 
        {
            if (mDataHandler == null)
                return;

            if (RowCount == 0)
                return;
            
            if ( Usefully.IsNullOrEmpty( mDataHandler.Result) )
            {
				mResultAll = null;
                CurrentPageReal = 0;
                return;
            }

			mResultAll = new List<List<OrderExtended>>();
            int k = 0;
            for (int i = 0; i < PageCount; i++)
            {
				mResultAll.Add(new List<OrderExtended>());
                for (int j = 0; j < mRowCount; j++) 
                {
                    if (k >= mDataHandler.Result.Count)
                        break;
					mResultAll[i].Add(mDataHandler.Result[k]);
                    k++;
                }
            }

            CurrentPageReal = aPageCurrent;
        }

        public void DoPrevious()
        {
            CurrentPageReal--; 
        }
        public void DoNext()
        {
            CurrentPageReal++; 
        }

        public void MakeSort(string aCondition, bool aIsAscending)
        {
            if (mDataHandler != null)
                mDataHandler.MakeSort(aCondition, aIsAscending);

            CreateResultAll();
        }

        public void MakeSearch(string aCondition, string aValue)
        {
            if (mDataHandler != null)
                mDataHandler.MakeSearch(aCondition, aValue);

            CreateResultAll();
        }

        public bool HasPrevious
        {
            get
            {
                if (Usefully.IsNullOrEmpty(mResultAll))
                    return false;

                if (CurrentPageReal <= 0)
                    return false;

                return true;
            }
        }
        public bool HasNext
        {
            get
            {
                if (Usefully.IsNullOrEmpty(mResultAll))
                    return false;

                if (CurrentPageReal >= (PageCount - 1))
                    return false;

                return true;
            }
        }

        

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string aProp = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(aProp));
        }

        public void Create()
        {
            if (mDataHandler != null)
                mDataHandler.Create();

            CreateResultAll();
        }
    }
}
