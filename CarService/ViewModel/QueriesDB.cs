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
        public Int32 IdOrder { get; set; }
        public string CarBrand { get; set; }
        public string CarModel { get; set; }
        public Int16 ReleaseYear { get; set; }
        public string TransmissionType { get; set; }
        public Int16 EnginePower { get; set; }
        public string NameOperation { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public Decimal Price { get; set; }
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
                             select new OrderExtended
                             {
                                 IdOrder = or.IdOrder,
                                 CarBrand = c.CarBrand,
                                 CarModel = c.CarModel,
                                 ReleaseYear = (Int16)c.ReleaseYear,
                                 TransmissionType = (string)c.TransmissionType,
                                 EnginePower = (Int16)c.EnginePower,
                                 BeginTime = (DateTime)or.BeginTime,
                                 //EndTime = (DateTime)or.EndTime,
                                 NameOperation = op.NameOperation,
                                 Price = (Decimal)op.Price
                             };
                res = result.ToList();
            }

            return res;
        }
    }
}