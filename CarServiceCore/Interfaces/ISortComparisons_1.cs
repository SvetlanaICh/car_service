using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServiceCore.Interfaces
{
	public interface ISortComparisons_1
	{
		int CompareIdOrder(OrderExtended aOE1, OrderExtended aOE2);
		int CompareCarBrand(OrderExtended aOE1, OrderExtended aOE2);
		int CompareCarModel(OrderExtended aOE1, OrderExtended aOE2);
		int CompareReleaseYear(OrderExtended aOE1, OrderExtended aOE2);
		int CompareTransmissionType(OrderExtended aOE1, OrderExtended aOE2);
		int CompareEnginePower(OrderExtended aOE1, OrderExtended aOE2);
		int CompareNameOperation(OrderExtended aOE1, OrderExtended aOE22);
		int CompareBeginTime(OrderExtended aOE1, OrderExtended aOE2);
		int CompareEndTime(OrderExtended aOE1, OrderExtended aOE2);
		int ComparePrice(OrderExtended aOE1, OrderExtended aOE2);				
	}
}
