using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CarServiceCore
{
	public class PaginalDataFake : IPaginalData
	{
		private IDataHandler mDataHandler;
		public PaginalDataFake(IDataHandler aDataHandler)
		{
			mDataHandler = aDataHandler;
		}

		public int CurrentPageDisplayed { get { return 1; } }
		public bool HasNext { get { return false; } }
		public bool HasPrevious { get { return false; } }
		public int PageCount { get { return 1; } }
		public string PageStatus { get { return string.Format("Стр.1 из 1"); } }

		public int RowCount
		{
			get { return 0; }
			set { }
		}

		public void DoNext() { }

		public void DoPrevious() { }


		public List<OrderExtended> Result
		{
			get
			{
				if (mDataHandler != null)
					return mDataHandler.Result;
				else
					return null;
			}
		}		

		public void Create()
		{
			if (mDataHandler != null)
				mDataHandler.Create();
			OnPropertyChanged("Result");
		}

		public void MakeSearch(string aCondition, string aValue)
		{
			if (mDataHandler != null)
				mDataHandler.MakeSearch(aCondition, aValue);
			OnPropertyChanged("Result");
		}

		public void MakeSort(string aCondition, bool aIsAscending)
		{
			if (mDataHandler != null)
				mDataHandler.MakeSort(aCondition, aIsAscending);
			OnPropertyChanged("Result");
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

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName]string aProp = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(aProp));
		}
	}
}
