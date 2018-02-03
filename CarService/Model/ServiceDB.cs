using CarService.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace CarService.Model
{
    class ServiceDB : IServiceDB
    {
        private Icar_serviceEntitiesCreator mCarServiceEntitiesCreator;

        public ServiceDB(Icar_serviceEntitiesCreator aCarServiceEntitiesCreator)
        {
            mCarServiceEntitiesCreator = aCarServiceEntitiesCreator;
        }

        public List<OrderExtended> GetResultAll()
        {
            List<OrderExtended> result = null;

            try
            {
                using (car_serviceEntities db = mCarServiceEntitiesCreator.GetCar_serviceEntities())
                {
                    result = (from or in db.OrderSet
                              join c in db.CarSet on or.CarId equals c.IdCar
                              join op in db.OperationSet on or.OperationId equals op.IdOperation
                              join p in db.PersonSet on c.PersonId equals p.IdPerson
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
                              }).ToList();
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


        public List<string> GetFilterValues(string aFilterColumn)
        {
            List<OrderExtended> result = GetResultAll();
            if ( Usefully.IsNullOrEmpty(result) )
                return null;
            List<string> res_str = new List<string>();
            try
            {
                var res_var = from r in result
                              select r.GetType().GetProperty(aFilterColumn).GetValue(r);
                foreach (var rv in res_var)
                {
                    if (rv == null)
                        res_str.Add(null);
                    else
                        res_str.Add(rv.ToString());
                }
                return res_str.Distinct().ToList();
            }
            catch (Exception ex)
            {
                if (ex != null)
                    Console.WriteLine(ex.Message);
                return null;
            }
        }

        public List<KeyValuePair<string, int>> GetDataForDiagramCarBrand()
        {
            List<OrderExtended> result = GetResultAll();
            if (Usefully.IsNullOrEmpty(result))
                return null;            

            var query = from r in result
                        group r by r.CarBrand into g
                        select new
                        {
                            CarBrandCount = g.Count(),
                            CarBrand = g.Key
                        };
             
            if(Usefully.IsNullOrEmpty(query))
                return null;

            List<KeyValuePair<string, int>> data = new List<KeyValuePair<string, int>>();

            foreach (var x in query)
                data.Add(new KeyValuePair<string, int>(x.CarBrand, x.CarBrandCount));

            return data;
        }

        public List<KeyValuePair<string, int>> GetDataForDiagramMonth()
        {
            List<OrderExtended> result = GetResultAll();
            if ( Usefully.IsNullOrEmpty(result) )
                return null;            

            var orders_without_null = from r in result
                                      where r.BeginTime != null
                                      select r;

            int year_current = DateTime.Now.Year;

            var orders_new = from o in orders_without_null
                             where o.BeginTime.Value.Year == year_current
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

        public List<KeyValuePair<string, int>> GetDataForDiagramPrice(ref List<int> aValues)
        {
            List<OrderExtended> result = GetResultAll();
            if (Usefully.IsNullOrEmpty(result))
                return null;
            if (Usefully.IsNullOrEmpty(aValues))
                return null;
            List<KeyValuePair<string, int>> data = new List<KeyValuePair<string, int>>();

            var orders_without_null = from r in result
                                      where r.Price != null
                                      select r;

            if (Usefully.IsNullOrEmpty(orders_without_null))
                return null;

            for (int i = 0; i <= (aValues.Count - 2); i++)
            {
                int i1 = aValues[i];
                int i2 = aValues[i + 1];
                string s = string.Format("Цена от {0} до {1}", i1, i2);
                int count = (from o in orders_without_null
                             where o.Price >= i1 && o.Price < i2
                             select o).Count();
                data.Add(new KeyValuePair<string, int>(s, count));
            }

            int i3 = aValues[aValues.Count - 1];
            string s2 = string.Format("Цена от {0}", i3);
            int count2 = (from o in orders_without_null
                          where o.Price >= i3
                          select o).Count();
            data.Add(new KeyValuePair<string, int>(s2, count2));

            return data;
        }

    }
}
