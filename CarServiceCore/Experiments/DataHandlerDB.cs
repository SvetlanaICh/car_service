using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarServiceCore.Interfaces;
using System.Linq.Expressions;
using CarServiceCore.Entities;

namespace CarServiceCore.Experiments
{
	public class DataHandlerDB : IDataHandlerDB
	{
		IQueriesDB mQueriesDB;

		Dictionary<
				string,
				Func<
					ICarServiceContext,
					string,
					IQueryable<OrderExtended>
					>
				> mSearchFuncs;

		Dictionary<
				string,
				Func<
					IQueryable<OrderExtended>,
					IQueryable<OrderExtended>
					>
				> mAscSortFuncs;

		Dictionary<
				string,
				Func<
					IQueryable<OrderExtended>,
					IQueryable<OrderExtended>
					>
				> mDescSortFuncs;

		public DataHandlerDB(IQueriesDB aQueriesDB)
		{
			mQueriesDB = aQueriesDB;

			InitSearchFuncs();
			InitSortFuncs();

			//Create();
		}

		public IQueryable<OrderExtended> Create(ICarServiceContext aDB)
		{
			return mQueriesDB.GetResult(aDB)
				.OrderBy(o => o.IdOrder);
		}

		public IQueryable<OrderExtended> MakeSearch(ICarServiceContext aDB, 
			string aCondition, string aValue)
		{
			IQueryable<OrderExtended> res;

			try
			{
				var make_search = mSearchFuncs[aCondition];
				res = make_search.Invoke(aDB, aValue)
					.OrderBy(o => o.IdOrder);
			}
			catch
			{
				res = null;
				return null;
			}
			return res;
		}

		public IQueryable<OrderExtended> MakeSort(ICarServiceContext aDB, 
			string aCondition, bool aIsAscending)
		{
			try
			{
				var res = mQueriesDB.GetResult(aDB);
				Func <IQueryable<OrderExtended>, 
					IQueryable<OrderExtended>> make_sort;

				if (aIsAscending)
					make_sort = mAscSortFuncs[aCondition];
				else
					make_sort = mDescSortFuncs[aCondition];
				return make_sort.Invoke(res);
			}
			catch
			{
				return null;
			}
		}

		private void InitSortFuncs()
		{
			mAscSortFuncs = new Dictionary<
				string,
				Func<
					IQueryable<OrderExtended>,
					IQueryable<OrderExtended>
					>>
			{
				{ "IdOrder",
					(iq) => iq.OrderBy(o => o.IdOrder) },
				{ "CarBrand",
					(iq) => iq.OrderBy(o => o.CarBrand) },
				{ "CarModel",
					(iq) => iq.OrderBy(o => o.CarModel) },
				{ "ReleaseYear",
					(iq) => iq.OrderBy(o => o.ReleaseYear) },
				{ "TransmissionType",
					(iq) => iq.OrderBy(o => o.TransmissionType) },
				{ "EnginePower",
					(iq) => iq.OrderBy(o => o.EnginePower) },
				{ "NameOperation",
					(iq) => iq.OrderBy(o => o.NameOperation) },
				{ "BeginTime",
					(iq) => iq.OrderBy(o => o.BeginTime) },
				{ "EndTime",
					(iq) => iq.OrderBy(o => o.EndTime) },
				{ "Price",
					(iq) => iq.OrderBy(o => o.Price) }
			};


			mDescSortFuncs = new Dictionary<
				string,
				Func<
					IQueryable<OrderExtended>,
					IQueryable<OrderExtended>
					>>
			{
				{ "IdOrder",
					(iq) => iq.OrderByDescending(o => o.IdOrder) },
				{ "CarBrand",
					(iq) => iq.OrderByDescending(o => o.CarBrand) },
				{ "CarModel",
					(iq) => iq.OrderByDescending(o => o.CarModel) },
				{ "ReleaseYear",
					(iq) => iq.OrderByDescending(o => o.ReleaseYear) },
				{ "TransmissionType",
					(iq) => iq.OrderByDescending(o => o.TransmissionType) },
				{ "EnginePower",
					(iq) => iq.OrderByDescending(o => o.EnginePower) },
				{ "NameOperation",
					(iq) => iq.OrderByDescending(o => o.NameOperation) },
				{ "BeginTime",
					(iq) => iq.OrderByDescending(o => o.BeginTime) },
				{ "EndTime",
					(iq) => iq.OrderByDescending(o => o.EndTime) },
				{ "Price",
					(iq) => iq.OrderByDescending(o => o.Price) }
			};
		}

		private void InitSearchFuncs()
		{
			Func<ICarServiceContext, string,
				IQueryable<OrderExtended>> func_IdOrder = (cc, s) =>
				{
					Expression<Func<Order, bool>> predicate = null;
					int value_int;
					if (!int.TryParse(s, out value_int))
						return null;
					predicate = (or) => or.IdOrder == value_int;
					return mQueriesDB.GetResult(cc, predicate);
				};

			Func<ICarServiceContext, string,
				IQueryable<OrderExtended>> func_CarBrand = (cc, s) =>
				{
					Expression<Func<Car, bool>> predicate = null;
					predicate = (or) => or.CarBrand == s;
					return mQueriesDB.GetResult(cc, predicate);
				};

			Func<ICarServiceContext, string,
				IQueryable<OrderExtended>> func_CarModel = (cc, s) =>
				{
					Expression<Func<Car, bool>> predicate = null;
					predicate = (or) => or.CarModel == s;
					return mQueriesDB.GetResult(cc, predicate);
				};

			Func<ICarServiceContext, string,
				IQueryable<OrderExtended>> func_ReleaseYear = (cc, s) =>
				{
					Expression<Func<Car, bool>> predicate = null;
					short value_short;
					if (!short.TryParse(s, out value_short))
						return null;
					predicate = (c) => c.ReleaseYear == value_short;
					return mQueriesDB.GetResult(cc, predicate);
				};

			Func<ICarServiceContext, string,
				IQueryable<OrderExtended>> func_TransmissionType = (cc, s) =>
				{
					Expression<Func<Car, bool>> predicate = null;
					predicate = (or) => or.TransmissionType == s;
					return mQueriesDB.GetResult(cc, predicate);
				};

			Func<ICarServiceContext, string,
				IQueryable<OrderExtended>> func_EnginePower = (cc, s) =>
				{
					Expression<Func<Car, bool>> predicate = null;
					short value_short;
					if (!short.TryParse(s, out value_short))
						return null;
					predicate = (c) => c.EnginePower == value_short;
					return mQueriesDB.GetResult(cc, predicate);
				};

			Func<ICarServiceContext, string,
				IQueryable<OrderExtended>> func_NameOperation = (cc, s) =>
				{
					Expression<Func<Operation, bool>> predicate = null;
					predicate = (or) => or.NameOperation == s;
					return mQueriesDB.GetResult(cc, predicate);
				};

			Func<ICarServiceContext, string,
				IQueryable<OrderExtended>> func_BeginTime = (cc, s) =>
				{
					Expression<Func<Order, bool>> predicate = null;
					DateTime value_dt;
					if (!DateTime.TryParse(s, out value_dt))
						return null;
					predicate = (o) => o.BeginTime == value_dt;
					return mQueriesDB.GetResult(cc, predicate);
				};

			Func<ICarServiceContext, string,
				IQueryable<OrderExtended>> func_EndTime = (cc, s) =>
				{
					Expression<Func<Order, bool>> predicate = null;
					DateTime value_dt;
					if (!DateTime.TryParse(s, out value_dt))
						return null;
					predicate = (o) => o.EndTime == value_dt;
					return mQueriesDB.GetResult(cc, predicate);
				};

			Func<ICarServiceContext, string,
				IQueryable<OrderExtended>> func_Price = (cc, s) =>
				{
					Expression<Func<Operation, bool>> predicate = null;
					decimal value_d;
					if (!decimal.TryParse(s, out value_d))
						return null;
					predicate = (o) => o.Price == value_d;
					return mQueriesDB.GetResult(cc, predicate);
				};



			mSearchFuncs = new Dictionary<
				string,
				Func<
					ICarServiceContext,
					string,
					IQueryable<OrderExtended>
					>>
			{
				{"IdOrder", func_IdOrder },
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
