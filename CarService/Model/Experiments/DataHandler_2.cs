using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Model.Experiments
{
	class DataHandler_2 : IDataHandler
	{
		IQueriesDB mQueriesDB;
		IOrderExtendedComparisons_2 mOrderExtendedComparisons;
		private List<OrderExtended> mResultAll;
		private IOrderExtendedPredicats mOrderExtendedPredicats;

		public List<OrderExtended> Result { get; private set; }

		public DataHandler_2(
			IQueriesDB aQueriesDB
			, IOrderExtendedComparisons_2 aOrderExtendedComparisons
			, IOrderExtendedPredicats aOrderExtendedPredicats)
		{
			mQueriesDB = aQueriesDB;
			mOrderExtendedComparisons = aOrderExtendedComparisons;
			mOrderExtendedPredicats = aOrderExtendedPredicats;
			Create();
		}

		public void Create()
		{
			mResultAll = mQueriesDB.GetResultAll();
			Result = mResultAll;
		}

		public void MakeSort(string aCondition, bool aIsAscending)
		{
			Result = null;
			mResultAll = mQueriesDB.GetResultAll();
			
			if (string.IsNullOrEmpty(aCondition))
				return;
			if (mResultAll == null)
				return;
			if (mOrderExtendedComparisons == null)
				return;

			Comparison<OrderExtended> comparison = 
				mOrderExtendedComparisons.GetComparison(aCondition, aIsAscending);

			if (comparison == null)
				return;

			mResultAll.Sort(comparison);

			Result = mResultAll;
		}

		public void MakeSearch(string aCondition, string aValue)
		{
			mResultAll = mQueriesDB.GetResultAll();
			Result = null;

			if (aCondition == null || aValue == null)
				return;
			if (mResultAll == null)
				return;

			if (mOrderExtendedPredicats == null)
				return;

			Predicate<OrderExtended> predicate =
				mOrderExtendedPredicats.GetPredicate(aCondition, aValue);

			if (predicate != null)
				Result = mResultAll.FindAll(predicate);
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName]string aProp = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(aProp));
		}
	}
}
