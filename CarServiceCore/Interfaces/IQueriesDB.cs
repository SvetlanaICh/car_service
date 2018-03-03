using CarServiceCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CarServiceCore.Interfaces
{
    public interface IQueriesDB
    {
        List<OrderExtended> GetResultAll();

		IQueryable<OrderExtended> GetResult(ICarServiceContext aDB);

		IQueryable<OrderExtended> GetResult(
			ICarServiceContext aDB,
			Expression<Func<Order, bool>> aOrderPredicate,
			Expression<Func<Car, bool>> aCarPredicate,
			Expression<Func<Operation, bool>> aOperationPredicate,
			Expression<Func<Person, bool>> aPersonPredicate);

		IQueryable<OrderExtended> GetResult(
			ICarServiceContext aDB,
			Expression<Func<Order, bool>> aOrderPredicate);

		IQueryable<OrderExtended> GetResult(
			ICarServiceContext aDB,
			Expression<Func<Car, bool>> aCarPredicate);

		IQueryable<OrderExtended> GetResult(
			ICarServiceContext aDB,
			Expression<Func<Operation, bool>> aOperationPredicate);

		IQueryable<OrderExtended> GetResult(
			ICarServiceContext aDB,
			Expression<Func<Person, bool>> aPersonPredicate);

		IQueryable<OrderExtended> GetResult(
			IQueryable<Order> aOrders,
			IQueryable<Car> aCars,
			IQueryable<Operation> aOperations,
			IQueryable<Person> aPeople
			);
	}
}
