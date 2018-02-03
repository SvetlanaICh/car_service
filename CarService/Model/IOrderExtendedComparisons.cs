using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Model
{
	public interface IOrderExtendedComparisons
	{
		int OrderExtendedCompareIdOrder(OrderExtended aOE1, OrderExtended aOE2);
		int OrderExtendedCompareCarBrand(OrderExtended aOE1, OrderExtended aOE2);
		int OrderExtendedCompareCarModel(OrderExtended aOE1, OrderExtended aOE2);
		int OrderExtendedCompareReleaseYear(OrderExtended aOE1, OrderExtended aOE2);
		int OrderExtendedCompareTransmissionType(OrderExtended aOE1, OrderExtended aOE2);
		int OrderExtendedCompareEnginePower(OrderExtended aOE1, OrderExtended aOE2);
		int OrderExtendedCompareNameOperation(OrderExtended aOE1, OrderExtended aOE22);
		int OrderExtendedCompareBeginTime(OrderExtended aOE1, OrderExtended aOE2);
		int OrderExtendedCompareEndTime(OrderExtended aOE1, OrderExtended aOE2);
		int OrderExtendedComparePrice(OrderExtended aOE1, OrderExtended aOE2);				
	}
}
