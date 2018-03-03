using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarServiceCore.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CarServiceCore
{
	public class PaginalDataDB : IPaginalData
	{
		ICarServiceContextCreator mCarServiceContextCreator;
		IDataHandlerDB mDataHandlerDB;
		Func<ICarServiceContext, 
			IQueryable<OrderExtended>> mDataHandling;

		private List<OrderExtended> mResult;
		int mCurrentPageReal = 0;
		private int mRowCount = 15;
		private int mPageCount;

		public PaginalDataDB(
			ICarServiceContextCreator aCarServiceContextCreator, 
			IDataHandlerDB aDataHandlerDB)
		{
			mCarServiceContextCreator = aCarServiceContextCreator;
			mDataHandlerDB = aDataHandlerDB;

			Create();
		}
	
		public List<OrderExtended> Result {
			get { return mResult; }
			private set
			{
				mResult = value;
				OnPropertyChanged("Result");
			}
		}
		
		private int CurrentPageReal
		{
			get { return mCurrentPageReal; }
			set
			{
				mCurrentPageReal = value;
				CreateResult();
				
				OnPropertyChanged("CurrentPageDisplayed");
				OnPropertyChanged("PageStatus");				
				OnPropertyChanged("HasPrevious");
				OnPropertyChanged("HasNext");

//				OnPropertyChanged("CurrentPageReal");
//				OnPropertyChanged("Result");
			}
		}
	
		public int RowCount
		{
			get { return mRowCount; }
			set
			{
				if (value <= 0)
					return;

				mRowCount = value;

				GetPageCount();
				CurrentPageReal = 0;    // it calls CreateResult()

				OnPropertyChanged("RowCount");
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
				return string.Format("Стр.{0} из {1}", 
					CurrentPageDisplayed, PageCount);
			}
		}
	
		public int PageCount
		{
			get { return mPageCount; }
			private set
			{
				mPageCount = value;
				OnPropertyChanged("PageCount");
			}
		}

		public bool HasPrevious
		{
			get
			{
				if (CurrentPageReal <= 0)
					return false;

				return true;
			}
		}

		public bool HasNext
		{
			get
			{
				if (CurrentPageReal >= (PageCount - 1))
					return false;

				return true;
			}
		}

		public void DoPrevious()
		{
			if (HasPrevious)
				CurrentPageReal--;	// it calls CreateResult()
		}

		public void DoNext()
		{
			if (HasNext)
				CurrentPageReal++;  // it calls CreateResult()
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

		public void Create()
		{
			mDataHandling = mDataHandlerDB.Create;
			GetPageCount();
			CurrentPageReal = 0;
		}

		public void MakeSort(string aCondition, bool aIsAscending)
		{
			mDataHandling = (c) =>
				mDataHandlerDB.MakeSort(c, aCondition, aIsAscending);
			GetPageCount();
			CurrentPageReal = 0;
		}

		public void MakeSearch(string aCondition, string aValue)
		{
			mDataHandling = (c) =>
				mDataHandlerDB.MakeSearch(c, aCondition, aValue);
			GetPageCount();
			CurrentPageReal = 0;
		}

		private void CreateResult()
		{
			Result = null;
			try
			{
				using (ICarServiceContext db =
					mCarServiceContextCreator.GetCarServiceContext())
				{
					int skiped = CurrentPageReal * RowCount;

					var res = mDataHandling(db)
						.Skip(skiped)
						.Take(RowCount);

					Result = res.ToList();
				}
			}
			catch (Exception ex)
			{
				if (ex != null)
					Console.WriteLine(ex.Message);
				Result = null;
			}
		}

		private void GetPageCount()
		{
			PageCount = 0;
			int res_count;
			try
			{
				using (ICarServiceContext db =
					mCarServiceContextCreator.GetCarServiceContext())
				{
					res_count = mDataHandling(db)
						.Count();					
				}

				if (RowCount != 0)
				{
					decimal d = (decimal)res_count / (decimal)RowCount;
					decimal p_d = Math.Ceiling(d);
					PageCount = Convert.ToInt32(p_d);
				}
			}
			catch (Exception ex)
			{
				if (ex != null)
					Console.WriteLine(ex.Message);
				
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName]string aProp = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(aProp));
		}
	}
}
