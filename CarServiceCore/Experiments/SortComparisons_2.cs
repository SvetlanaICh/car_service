using CarServiceCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServiceCore.Experiments
{
	public class SortComparisons_2 : ISortComparisons_2
	{
		private Dictionary<string, Comparison<OrderExtended>> mComparisons;
		ISortComparisons_1 mComparisons_1;

		public SortComparisons_2(ISortComparisons_1 aComparisons_1)
		{
			mComparisons_1 = aComparisons_1;
			if (mComparisons_1 == null)
				return;

			mComparisons = new Dictionary<string, Comparison<OrderExtended>>
			{
				{ "IdOrder", mComparisons_1.CompareIdOrder },
				{ "CarBrand",  mComparisons_1.CompareCarBrand },
				{ "CarModel", mComparisons_1.CompareCarModel },
				{ "ReleaseYear", mComparisons_1.CompareReleaseYear },
				{ "TransmissionType", mComparisons_1.CompareTransmissionType },
				{ "EnginePower", mComparisons_1.CompareEnginePower },
				{ "NameOperation", mComparisons_1.CompareNameOperation },
				{ "BeginTime", mComparisons_1.CompareBeginTime },
				{ "EndTime", mComparisons_1.CompareEndTime },
				{ "Price", mComparisons_1.ComparePrice }
			};
		}

		public Comparison<OrderExtended> GetComparison(string aName, bool aIsAscending)
		{
			try
			{
				if (aIsAscending)
					return mComparisons[aName];
				else
					return ReverseSomething(mComparisons[aName]);
			}
			catch (Exception ex)
			{
				if (ex != null)
					Console.WriteLine(ex.Message);
				return null;
			}
		}

		private Comparison<OrderExtended> ReverseSomething(Comparison<OrderExtended> aComparison)
		{
			return (OrderExtended aOE1, OrderExtended aOE2) => aComparison(aOE1, aOE2) * (-1); 
		}	
	}
}
