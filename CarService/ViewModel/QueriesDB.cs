using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarService.Model;

namespace CarService.ViewModel
{
     class OrderExtended
    {
        public int IdOrder { get; set; }
        public string CarBrand { get; set; }
        public string CarModel { get; set; }
        public short? ReleaseYear { get; set; }
        public string TransmissionType { get; set; }
        public short? EnginePower { get; set; }
        public string NameOperation { get; set; }
        public DateTime? BeginTime { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal? Price { get; set; }
        public string PersonLastName { get; set; }
        public string PersonFirstName { get; set; }
        public string PersonMiddleName { get; set; }
        public short? PersonBirthYear { get; set; }
        public string PersonPhone { get; set; }
    }
    class QueriesDB
    {           
        public static List<OrderExtended> GetOrdersExtended()
        {
            List<OrderExtended> res;

            using (dbEntities db = new dbEntities())
            {
                var result = from or in db.OrderSet
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
                             }; 
                res = result.ToList();
            }

            return res;
        }
    }
}