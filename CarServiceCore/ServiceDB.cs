using CarServiceCore.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using CarServiceCore.Entities;
using CarServiceCore.Interfaces;
using System.Linq.Expressions;
using System.Collections;

namespace CarServiceCore
{
	public class ServiceDB : IQueriesDB, IServiceDB
	{
		private ICarServiceContextCreator mCarServiceContextCreator;

		private Dictionary<
				string,
				Func<List<string>>
				> mGetFilterValues;

		public ServiceDB(
			ICarServiceContextCreator aCarServiceContextCreator)
		{
			mCarServiceContextCreator = aCarServiceContextCreator;

			InitForGetFilterValues();
		}

		public List<OrderExtended> GetResultAll()
		{
			List<OrderExtended> result = null;

			try
			{
				using (ICarServiceContext db =
					mCarServiceContextCreator.GetCarServiceContext())
				{
					var res = GetResult(db);
					result = res.ToList();
				}
			}
			catch (Exception ex)
			{
				if (ex != null)
					Console.WriteLine(ex.Message);
				result = null;
				return null;
			}

			return result;
		}

		public IQueryable<OrderExtended> GetResult(ICarServiceContext aDB)
		{
			return GetResult(aDB, null, null, null, null);
		}

		public IQueryable<OrderExtended> GetResult(
			ICarServiceContext aDB,
			Expression<Func<Order, bool>> aOrderPredicate)
		{
			return GetResult(aDB, aOrderPredicate, null, null, null);
		}

		public IQueryable<OrderExtended> GetResult(
			ICarServiceContext aDB,
			Expression<Func<Car, bool>> aCarPredicate)
		{
			return GetResult(aDB, null, aCarPredicate, null, null);
		}

		public IQueryable<OrderExtended> GetResult(
			ICarServiceContext aDB,
			Expression<Func<Operation, bool>> aOperationPredicate)
		{
			return GetResult(aDB, null, null, aOperationPredicate, null);
		}

		public IQueryable<OrderExtended> GetResult(
			ICarServiceContext aDB,
			Expression<Func<Person, bool>> aPersonPredicate)
		{
			return GetResult(aDB, null, null, null, aPersonPredicate);
		}

		public IQueryable<OrderExtended> GetResult(
			ICarServiceContext aDB,
			Expression<Func<Order, bool>> aOrderPredicate,
			Expression<Func<Car, bool>> aCarPredicate,
			Expression<Func<Operation, bool>> aOperationPredicate,
			Expression<Func<Person, bool>> aPersonPredicate)
		{
			IQueryable<Order> orders;
			if (aOrderPredicate == null)
				orders = aDB.OrderSet;
			else
				orders = aDB.OrderSet.Where(aOrderPredicate);

			IQueryable<Car> cars;
			if (aCarPredicate == null)
				cars = aDB.CarSet;
			else
				cars = aDB.CarSet.Where(aCarPredicate);

			IQueryable<Operation> operations;
			if (aOperationPredicate == null)
				operations = aDB.OperationSet;
			else
				operations = aDB.OperationSet.Where(aOperationPredicate);

			IQueryable<Person> people;
			if (aPersonPredicate == null)
				people = aDB.PersonSet;
			else
				people = aDB.PersonSet.Where(aPersonPredicate);

			return GetResult(orders, cars, operations, people);
		}

		public IQueryable<OrderExtended> GetResult(
			IQueryable<Order> aOrders,
			IQueryable<Car> aCars,
			IQueryable<Operation> aOperations,
			IQueryable<Person> aPeople
			)
		{
			IQueryable<OrderExtended> result = null;

			if (aOrders == null || aCars == null || aOperations == null || aPeople == null)
				return null;

			result = from or in aOrders
					 join c in aCars on or.CarId equals c.IdCar
					 join op in aOperations on or.OperationId equals op.IdOperation
					 join p in aPeople on c.PersonId equals p.IdPerson
					 select new OrderExtended
					 {
						 IdOrder = or.IdOrder,
						 CarBrand = c.CarBrand,
						 CarModel = c.CarModel,
						 ReleaseYear = c.ReleaseYear,
						 TransmissionType = c.TransmissionType,
						 EnginePower = c.EnginePower,
						 BeginTime = or.BeginTime,
						 EndTime = or.EndTime,
						 NameOperation = op.NameOperation,
						 Price = op.Price,
						 PersonLastName = p.LastName,
						 PersonFirstName = p.FirstName,
						 PersonMiddleName = p.MiddleName,
						 PersonBirthYear = p.BirthYear,
						 PersonPhone = p.Phone
					 };

			return result;
		}

		public List<string> GetFilterValues(string aFilterColumn)
		{
			List<string> result = null;

			try
			{
				result = mGetFilterValues[aFilterColumn].Invoke();
			}
			catch (Exception ex)
			{
				if (ex != null)
					Console.WriteLine(ex.Message);
				result = null;
				return null;
			}

			return result;
		}

		private List<string> GetFilterValues<T>(
			Func<ICarServiceContext, IQueryable<T>> aFunc)
		{
			List<string> result = new List<string>();			

			try
			{
				List<T> res;
				using (ICarServiceContext db =
					mCarServiceContextCreator.GetCarServiceContext())
				{
					IQueryable<T> query = aFunc(db);					
					res = query.ToList();
				}

				result = ToListString(res);
			}
			catch (Exception ex)
			{
				if (ex != null)
					Console.WriteLine(ex.Message);
				result = null;
				return null;
			}

			return result;
		}

		private List<string> ToListString<T>(List<T> aList)
		{
			List<string> result = new List<string>();

			foreach (T r in aList)
				result.Add(r.ToString());

			return result;
		}

		private void InitForGetFilterValues()
		{
			Func<List<string>> func_IdOrder = () =>
			{
				Func<ICarServiceContext, IQueryable<int>> func =
				(db) => db.OrderSet.Select(o => o.IdOrder)
				.Distinct().OrderBy(i => i);
				return GetFilterValues(func);
			};

			Func<List<string>> func_CarBrand = () =>
			{
				Func<ICarServiceContext, IQueryable<string>> func =
				(db) => db.CarSet.Select(c => c.CarBrand)
				.Distinct().OrderBy(i => i);
				return GetFilterValues(func);
			};

			Func<List<string>> func_CarModel = () =>
			{
				Func<ICarServiceContext, IQueryable<string>> func =
				(db) => db.CarSet.Select(c => c.CarModel)
				.Distinct().OrderBy(i => i);
				return GetFilterValues(func);
			};

			Func<List<string>> func_ReleaseYear = () =>
			{
				Func<ICarServiceContext, IQueryable<short?>> func =
				(db) => db.CarSet.Select(c => c.ReleaseYear)
				.Distinct().OrderBy(i => i);
				return GetFilterValues(func);
			};

			Func<List<string>> func_TransmissionType = () =>
			{
				Func<ICarServiceContext, IQueryable<string>> func =
				(db) => db.CarSet.Select(c => c.TransmissionType)
				.Distinct().OrderBy(i => i);
				return GetFilterValues(func);
			};

			Func<List<string>> func_EnginePower = () =>
			{
				Func<ICarServiceContext, IQueryable<short?>> func =
				(db) => db.CarSet.Select(c => c.EnginePower)
				.Distinct().OrderBy(i => i);
				return GetFilterValues(func);
			};

			Func<List<string>> func_NameOperation = () =>
			{
				Func<ICarServiceContext, IQueryable<string>> func =
				(db) => db.OperationSet.Select(o => o.NameOperation)
				.Distinct().OrderBy(i => i);
				return GetFilterValues(func);
			};

			Func<List<string>> func_BeginTime = () =>
			{
				Func<ICarServiceContext, IQueryable<DateTime?>> func =
				(db) => db.OrderSet.Select(o => o.BeginTime)
				.Distinct().OrderBy(i => i);
				return GetFilterValues(func);
			};

			Func<List<string>> func_EndTime = () =>
			{
				Func<ICarServiceContext, IQueryable<DateTime?>> func =
				(db) => db.OrderSet.Select(o => o.EndTime)
				.Distinct().OrderBy(i => i);
				return GetFilterValues(func);
			};

			Func<List<string>> func_Price = () =>
			{
				Func<ICarServiceContext, IQueryable<decimal?>> func =
				(db) => db.OperationSet.Select(o => o.Price)
				.Distinct().OrderBy(i => i);
				return GetFilterValues(func);
			};

			mGetFilterValues = new Dictionary<
				string,
				Func<List<string>>
				>
			{
				{ "IdOrder", func_IdOrder },
				{ "CarBrand", func_CarBrand },
				{ "CarModel", func_CarModel },
				{ "ReleaseYear", func_ReleaseYear },
				{ "TransmissionType", func_TransmissionType },
				{ "EnginePower", func_EnginePower },
				{ "NameOperation", func_NameOperation },
				{ "BeginTime", func_BeginTime },
				{ "EndTime", func_EndTime },
				{ "Price", func_Price }
			};
		}


	}
}
