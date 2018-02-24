using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServiceCore.Experiments
{
	public class SearchPredicats : ISearchPredicats
	{
		private Dictionary<string, Func<string, Predicate<OrderExtended>>> mPredicats;

		public SearchPredicats()
		{
			mPredicats = new Dictionary<string, Func<string, Predicate<OrderExtended>>>
			{
				{ "IdOrder", GetPredicateIdOrder },
				{ "CarBrand", GetPredicateCarBrand },
				{ "CarModel", GetPredicateCarModel },
				{ "ReleaseYear", GetPredicateReleaseYear },
				{ "TransmissionType", GetPredicateTransmissionType },
				{ "EnginePower", GetPredicateEnginePower },
				{ "NameOperation", GetPredicateNameOperation },
				{ "BeginTime", GetPredicateBeginTime },
				{ "EndTime", GetPredicateEndTime },
				{ "Price", GetPredicatePrice }
			};
		}

		public Predicate<OrderExtended> GetPredicate(string aCondition, string aValue)
		{
			try
			{
				return mPredicats[aCondition](aValue);
			}
			catch (Exception ex)
			{
				if (ex != null)
					Console.WriteLine(ex.Message);
				return null;
			}
		}

		private Predicate<OrderExtended> GetPredicateIdOrder(string aValue)
		{
			int value_int;
			if (int.TryParse(aValue, out value_int))
				return OrderExtended => OrderExtended.IdOrder == value_int;
			else
				return null;
		}

		private Predicate<OrderExtended> GetPredicateCarBrand(string aValue)
		{
			return OrderExtended => OrderExtended.CarBrand == aValue;
		}						

		private Predicate<OrderExtended> GetPredicateCarModel(string aValue)
		{
			return OrderExtended => OrderExtended.CarModel == aValue;
		}

		private Predicate<OrderExtended> GetPredicateReleaseYear(string aValue)
		{
			short value_short;
			if (short.TryParse(aValue, out value_short))
				return OrderExtended => OrderExtended.ReleaseYear == value_short;
			else
				return null;
		}

		private Predicate<OrderExtended> GetPredicateTransmissionType(string aValue)
		{
			return OrderExtended => OrderExtended.TransmissionType == aValue;
		}

		private Predicate<OrderExtended> GetPredicateEnginePower(string aValue)
		{
			short value_s;
			if (short.TryParse(aValue, out value_s))
				return OrderExtended => OrderExtended.EnginePower == value_s;
			else
				return null;
		}

		private Predicate<OrderExtended> GetPredicateNameOperation(string aValue)
		{
			return OrderExtended => OrderExtended.NameOperation == aValue;
		}

		private Predicate<OrderExtended> GetPredicateBeginTime(string aValue)
		{
			DateTime value_dt;
			if (DateTime.TryParse(aValue, out value_dt))
				return OrderExtended => OrderExtended.BeginTime == value_dt;
			else
				return null;
		}

		private Predicate<OrderExtended> GetPredicateEndTime(string aValue)
		{
			DateTime value_dte;
			if (DateTime.TryParse(aValue, out value_dte))
				return OrderExtended => OrderExtended.EndTime == value_dte;
			else
				return null;
		}

		private Predicate<OrderExtended> GetPredicatePrice(string aValue)
		{
			decimal value_d;
			if (decimal.TryParse(aValue, out value_d))
				return OrderExtended => OrderExtended.Price == value_d;
			else
				return null;
		}
	}
}
