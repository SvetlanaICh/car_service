using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Model.Experiments
{
	public class DataHandler_2 : IDataHandler
	{
		IQueriesDB mQueriesDB;
		ISortComparisons_2 mSortComparisons;
		private List<OrderExtended> mResultAll;
		private ISearchPredicats mSearchPredicats;

		public List<OrderExtended> Result { get; private set; }

		public DataHandler_2(
			IQueriesDB aQueriesDB
			, ISortComparisons_2 aSortComparisons
			, ISearchPredicats aSearchPredicats)
		{
			mQueriesDB = aQueriesDB;
			mSortComparisons = aSortComparisons;
			mSearchPredicats = aSearchPredicats;
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
			if (mSortComparisons == null)
				return;

			Comparison<OrderExtended> comparison =
				mSortComparisons.GetComparison(aCondition, aIsAscending);

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

			if (mSearchPredicats == null)
				return;

			Predicate<OrderExtended> predicate =
				mSearchPredicats.GetPredicate(aCondition, aValue);

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
