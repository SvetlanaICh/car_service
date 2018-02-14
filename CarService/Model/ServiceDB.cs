using CarService.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using CarService.Model.Entities;

namespace CarService.Model
{
    class ServiceDB : IQueriesDB, IServiceDB
    {
        private ICarServiceContextCreator mCarServiceContextCreator;

        public ServiceDB(ICarServiceContextCreator aCarServiceContextCreator)
        {
			mCarServiceContextCreator = aCarServiceContextCreator;
        }

        public List<OrderExtended> GetResultAll()
        {
            List<OrderExtended> result = null;

            try
            {
                using (ICarServiceContext db = mCarServiceContextCreator.GetCarServiceContext())
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
    }
}
