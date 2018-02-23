using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Model.Experiments
{
	public interface ISortComparisons_2
	{
		Comparison<OrderExtended> GetComparison(string aName, bool aIsAscending);
	}
}
