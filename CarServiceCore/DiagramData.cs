using CarServiceCore.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarServiceCore.Interfaces;

namespace CarServiceCore
{
	public class DiagramData : IDiagramData
	{
		private ICarServiceContextCreator mCarServiceContextCreator;
		private IQueriesDB mQueriesDB;
		private Dictionary<int, string> mMonthStr;

		public DiagramData(
			ICarServiceContextCreator aCarServiceContextCreator,
			IQueriesDB aQueriesDB)
		{
			Builder(
				aCarServiceContextCreator,
				aQueriesDB,
				DateTime.Now.Year,
				new List<int> { 0, 1000, 5000, 10000 }
				);
		}

		public DiagramData(
			ICarServiceContextCreator aCarServiceContextCreator,
			IQueriesDB aQueriesDB, 
			int aYear, 
			List<int> aValues)
		{
			Builder(aCarServiceContextCreator, 
				aQueriesDB, aYear, aValues);
		}

		private void Builder(
			ICarServiceContextCreator aCarServiceContextCreator,
			IQueriesDB aQueriesDB, 
			int aYear, 
			List<int> aValues)
		{
			mCarServiceContextCreator = aCarServiceContextCreator;
			mQueriesDB = aQueriesDB;
			Values = aValues;
			Year = aYear;

			InitMonthStr();
		}

		private void InitMonthStr()
		{
			mMonthStr = new Dictionary<int, string>
			{
				{ 1, "Январь" },
				{ 2, "Февраль" },
				{ 3, "Март" },
				{ 4, "Апрель" },
				{ 5, "Май" },
				{ 6, "Июнь" },
				{ 7, "Июль" },
				{ 8, "Август" },
				{ 9, "Сентябрь" },
				{ 10, "Октябрь" },
				{ 11, "Ноябрь" },
				{ 12, "Декабрь" },
			};
		}

		public int Year { get; set; }

		public List<int> Values { get; set; }
		
		public List<KeyValuePair<string, int>> DataForDiagramCarBrand
		{
			get
			{
				return GetDataForDiagram(
					TryGetDataCarBrand);
			}
		}

		public List<KeyValuePair<string, int>> DataForDiagramMonth
		{
			get
			{
				return GetDataForDiagram(					
					TryGetDataMonth);
			}
		}

		public List<KeyValuePair<string, int>> DataForDiagramPrice
		{
			get
			{
				return GetDataForDiagram(
					TryGetDataPrice);
			}
		}

		private List<KeyValuePair<string, int>> GetDataForDiagram(
			Func<List<KeyValuePair<string, int>>> aFunc)
		{
			try
			{
				return aFunc.Invoke();				
			}
			catch (Exception ex)
			{
				if (ex != null)
					Console.WriteLine(ex.Message);
				return null;
			}
		}

		private List<KeyValuePair<string, int>> TryGetDataCarBrand()
		{
			List<KeyValuePair<string, int>> result =
				new List<KeyValuePair<string, int>>();

			using (ICarServiceContext db =
			mCarServiceContextCreator.GetCarServiceContext())
			{
				var res =
					(from or in db.OrderSet
					 join c in db.CarSet
							on or.CarId equals c.IdCar
					 group or by c.CarBrand into g
					 select new { CarBrand = g.Key, Count = g.Count() })
					.ToList();

				db.Dispose();

				foreach (var r in res)
					result.Add(new KeyValuePair<string, int>(
						r.CarBrand, r.Count));
			}
			return result;
		}

		private List<KeyValuePair<string, int>> TryGetDataMonth()
		{
			List<KeyValuePair<string, int>> result = 
				new List<KeyValuePair<string, int>>();

			using (ICarServiceContext db =
					mCarServiceContextCreator.GetCarServiceContext())
			{
				var orders_valid =
					from o in db.OrderSet
					where o.BeginTime != null
					select o;

				var res =
					(from o in orders_valid
					 where o.BeginTime.Value.Year == Year
					 group o.BeginTime by o.BeginTime.Value.Month into g
					 select new { Month = g.Key, Count = g.Count() })
					.ToList();

				db.Dispose();

				foreach (var r in res)
					result.Add(new KeyValuePair<string, int>(
						mMonthStr[r.Month], r.Count));
			}

			return result;
		}

		private List<KeyValuePair<string, int>> TryGetDataPrice()
		{
			List<KeyValuePair<string, int>> result = 
				new List<KeyValuePair<string, int>>();

			using (ICarServiceContext db =
					mCarServiceContextCreator.GetCarServiceContext())
			{
				var operations_valid =
					from op in db.OperationSet
					where op.Price != null
					select op;

				var res =
					(from or in db.OrderSet
					 join op in operations_valid
							 on or.OperationId equals op.IdOperation
					 group or by op.Price into g
					 select new { Price = g.Key, Count = g.Count() })
					.ToList();

				db.Dispose();

				for (int i = 0; i <= (Values.Count - 2); i++)
				{
					int p1 = Values[i];
					int p2 = Values[i + 1];
					string s = string.Format("Цена от {0} до {1}", p1, p2);
					int count = (from r in res
								 where r.Price >= p1 && r.Price < p2
								 select r.Count).Sum();
					result.Add(new KeyValuePair<string, int>(s, count));
				}
				int p3 = Values[Values.Count - 1];
				string s2 = string.Format("Цена от {0}", p3);
				int count2 = (from r in res
							  where r.Price >= p3
							  select r.Count).Sum();
				result.Add(new KeyValuePair<string, int>(s2, count2));
			}

			return result;
		}
	}
}
