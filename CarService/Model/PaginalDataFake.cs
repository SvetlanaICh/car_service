using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Model
{
	class PaginalDataFake : IPaginalData
	{
		private IDataHandler myDataHandler;
		public PaginalDataFake(IDataHandler DataHandler)
		{
			myDataHandler = DataHandler;
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
				if (myDataHandler != null)
					return myDataHandler.Result;
				else
					return null;
			}
		}		

		public void Create()
		{
			if (myDataHandler != null)
				myDataHandler.Create();
			OnPropertyChanged("Result");
		}

		public void MakeSearch(string condition, string value)
		{
			if (myDataHandler != null)
				myDataHandler.MakeSearch(condition, value);
			OnPropertyChanged("Result");
		}

		public void MakeSort(string condition, bool is_ascending)
		{
			if (myDataHandler != null)
				myDataHandler.MakeSort(condition, is_ascending);
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
		public void OnPropertyChanged([CallerMemberName]string prop = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		}
	}
}
