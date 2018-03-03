using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServiceCore.Interfaces
{
	public interface IDataHandlerDB
	{
		IQueryable<OrderExtended> Create(
			ICarServiceContext aDB);

		IQueryable<OrderExtended> MakeSort(
			ICarServiceContext aDB, 
			string aCondition, 
			bool aIsAscending);

		IQueryable<OrderExtended> MakeSearch(
			ICarServiceContext aDB, 
			string aCondition, 
			string aValue);
	}
}
