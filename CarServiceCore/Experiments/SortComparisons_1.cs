using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServiceCore.Experiments
{
	public class SortComparisons_1 : ISortComparisons_1
	{
		public int CompareIdOrder(OrderExtended aOE1, OrderExtended aOE2)
		{
			return aOE1.IdOrder.CompareTo(aOE2.IdOrder);
		}

		public int CompareBeginTime(OrderExtended aOE1, OrderExtended aOE2)
		{
			if (aOE1.BeginTime == null || aOE2.BeginTime == null)
				return (-1) * Nullable.Compare(aOE1.BeginTime, aOE2.BeginTime);

			return DateTime.Compare((DateTime)aOE1.BeginTime, (DateTime)aOE2.BeginTime);
		}

		public int CompareCarBrand(OrderExtended aOE1, OrderExtended aOE2)
		{
			return string.Compare(aOE1.CarBrand, aOE2.CarBrand);
		}

		public int CompareCarModel(OrderExtended aOE1, OrderExtended aOE2)
		{
			return string.Compare(aOE1.CarModel, aOE2.CarModel);
		}

		public int CompareEndTime(OrderExtended aOE1, OrderExtended aOE2)
		{
			if (aOE1.EndTime == null || aOE2.EndTime == null)
				return (-1) * Nullable.Compare(aOE1.EndTime, aOE2.EndTime);

			return DateTime.Compare((DateTime)aOE1.EndTime, (DateTime)aOE2.EndTime);
		}

		public int CompareEnginePower(OrderExtended aOE1, OrderExtended aOE2)
		{
			if (aOE1.EnginePower == null || aOE2.EnginePower == null)
				return (-1) * Nullable.Compare(aOE1.EnginePower, aOE2.EnginePower);

			int i1 = (int)aOE1.EnginePower;
			int i2 = (int)aOE2.EnginePower;
			return i1.CompareTo(i2);
		}

		public int CompareNameOperation(OrderExtended aOE1, OrderExtended aOE2)
		{
			return string.Compare(aOE1.NameOperation, aOE2.NameOperation);
		}

		public int ComparePrice(OrderExtended aOE1, OrderExtended aOE2)
		{
			if (aOE1.Price == null || aOE2.Price == null)
				return (-1) * Nullable.Compare(aOE1.Price, aOE2.Price);

			return decimal.Compare((decimal)aOE1.Price, (decimal)aOE2.Price);
		}

		public int CompareReleaseYear(OrderExtended aOE1, OrderExtended aOE2)
		{
			if (aOE1.ReleaseYear == null || aOE2.ReleaseYear == null)
				return (-1) * Nullable.Compare(aOE1.ReleaseYear, aOE2.ReleaseYear);

			int i1 = (int)aOE1.ReleaseYear;
			int i2 = (int)aOE2.ReleaseYear;
			return i1.CompareTo(i2);
		}

		public int CompareTransmissionType(OrderExtended aOE1, OrderExtended aOE2)
		{
			return string.Compare(aOE1.TransmissionType, aOE2.TransmissionType);
		}
	}
}
