using CarServiceCore.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServiceCore
{
	public class DiagramData : IDiagramData
	{
		private IQueriesDB mQueriesDB;

		public DiagramData (IQueriesDB aQueriesDB)
		{
			mQueriesDB = aQueriesDB;
			Values = new List<int> { 0, 1000, 5000, 10000 };
			Year = DateTime.Now.Year;
		}

		public DiagramData(IQueriesDB aQueriesDB, int aYear, List<int> aValues)
		{
			mQueriesDB = aQueriesDB;
			Values = aValues;
			Year = aYear;
		}

		public int Year { get; set; }

		public List<int> Values { get; set; }
		
		public List<KeyValuePair<string, int>> DataForDiagramCarBrand
		{
			get
			{
				if (mQueriesDB == null)
					return null;
				List<OrderExtended> result = mQueriesDB.GetResultAll();
				if (Usefully.IsNullOrEmpty(result))
					return null;

				var query = from r in result
							group r by r.CarBrand into g
							select new
							{
								CarBrandCount = g.Count(),
								CarBrand = g.Key
							};

				if (Usefully.IsNullOrEmpty(query))
					return null;

				List<KeyValuePair<string, int>> data = new List<KeyValuePair<string, int>>();

				foreach (var x in query)
					data.Add(new KeyValuePair<string, int>(x.CarBrand, x.CarBrandCount));

				return data;
			}
		}

		public List<KeyValuePair<string, int>> DataForDiagramMonth
		{
			get
			{
				if (mQueriesDB == null)
					return null;
				List<OrderExtended> result = mQueriesDB.GetResultAll();
				if (Usefully.IsNullOrEmpty(result))
					return null;

				var orders_without_null = from r in result
										  where r.BeginTime != null
										  select r;

				var orders_new = from o in orders_without_null
								 where o.BeginTime.Value.Year == Year
								 select new
								 {
									 MonthInt = o.BeginTime.Value.Month,
									 ID = o.IdOrder,
									 MonthStr = ((DateTime)o.BeginTime).ToString("MMMM")
								 };

				if (Usefully.IsNullOrEmpty(orders_new))
					return null;

				var query = from o in orders_new
							group o by o.MonthStr into g
							select new { Month = g.Key, MonthCount = g.Count() };

				if (Usefully.IsNullOrEmpty(query))
					return null;

				List<KeyValuePair<string, int>> data = new List<KeyValuePair<string, int>>();

				foreach (var x in query)
					data.Add(new KeyValuePair<string, int>(x.Month, x.MonthCount));

				return data;
			}
		}

		public List<KeyValuePair<string, int>> DataForDiagramPrice
		{
			get
			{
				if (mQueriesDB == null)
					return null;
				List<OrderExtended> result = mQueriesDB.GetResultAll();
				if (Usefully.IsNullOrEmpty(result))
					return null;
				if (Usefully.IsNullOrEmpty(Values))
					return null;
				List<KeyValuePair<string, int>> data = new List<KeyValuePair<string, int>>();

				var orders_without_null = from r in result
										  where r.Price != null
										  select r;

				if (Usefully.IsNullOrEmpty(orders_without_null))
					return null;

				for (int i = 0; i <= (Values.Count - 2); i++)
				{
					int i1 = Values[i];
					int i2 = Values[i + 1];
					string s = string.Format("Цена от {0} до {1}", i1, i2);
					int count = (from o in orders_without_null
								 where o.Price >= i1 && o.Price < i2
								 select o).Count();
					data.Add(new KeyValuePair<string, int>(s, count));
				}

				int i3 = Values[Values.Count - 1];
				string s2 = string.Format("Цена от {0}", i3);
				int count2 = (from o in orders_without_null
							  where o.Price >= i3
							  select o).Count();
				data.Add(new KeyValuePair<string, int>(s2, count2));

				return data;
			}
		}
	}
}
