using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarService.Model;
using System.Data.Entity.Infrastructure;
using System.Windows.Data;

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

        static public int OrderExtendedCompareCarBrand(OrderExtended oe1, OrderExtended oe2)
        {
            return string.Compare(oe1.CarBrand, oe2.CarBrand);
        }

        static public int OrderExtendedCompareBeginTime(OrderExtended oe1, OrderExtended oe2)
        {
            return DateTime.Compare( (DateTime)oe1.BeginTime, (DateTime)oe2.BeginTime );
        }

        static public int OrderExtendedComparePrice(OrderExtended oe1, OrderExtended oe2)
        {
            if (oe1.Price == null && oe2.Price == null)
                return 0;
            if (oe1.Price == null && oe2.Price != null)
                return 1;
            if (oe1.Price != null && oe2.Price == null)
                return -1;

            return decimal.Compare( (decimal)oe1.Price, (decimal)oe2.Price );
        }

        static public int OrderExtendedCompareEndTime(OrderExtended oe1, OrderExtended oe2)
        {
            if (oe1.EndTime == null && oe2.EndTime == null)
                return 0;
            if (oe1.EndTime == null && oe2.EndTime != null)
                return 1;
            if (oe1.EndTime != null && oe2.EndTime == null)
                return -1;

            return DateTime.Compare((DateTime)oe1.BeginTime, (DateTime)oe2.BeginTime);
        }

        static public int OrderExtendedCompareCarModel(OrderExtended oe1, OrderExtended oe2)
        {
            return string.Compare(oe1.CarModel, oe2.CarModel);
        }

        static public int OrderExtendedCompareTransmissionType(OrderExtended oe1, OrderExtended oe2)
        {
            return string.Compare(oe1.TransmissionType, oe2.TransmissionType);
        }

        static public int OrderExtendedCompareNameOperation(OrderExtended oe1, OrderExtended oe2)
        {
            return string.Compare(oe1.NameOperation, oe2.NameOperation);
        }

        /*static public int OrderExtendedCompareIdOrder(OrderExtended oe1, OrderExtended oe2)
        {

        }

        static public int OrderExtendedCompareReleaseYear(OrderExtended oe1, OrderExtended oe2)
        {

        }

        static public int OrderExtendedCompareEnginePower(OrderExtended oe1, OrderExtended oe2)
        {

        }*/
    }

    class QueriesDB
    {
        private List<OrderExtended> result;
        private List<OrderExtended> result_search;

        public QueriesDB()
        {
            result = new List<OrderExtended> { };
            GetAll();
        }

        private void GetAll()
        {
            using (dbEntities db = new dbEntities())
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

        public List<OrderExtended> GetOrdersExtended()
        {
            return result;
        }

        public List<OrderExtended> GetSort(string condition, bool is_ascending = true)
        {
            if (result == null || condition == null)
                return null;
                
            switch (condition)
            {
                /*case "IdOrder":
                    result.Sort(OrderExtended.OrderExtendedCompareIdOrder);
                    break;*/
                case "CarBrand":
                    result.Sort(OrderExtended.OrderExtendedCompareCarBrand);
                    break;
                case "CarModel":
                    result.Sort(OrderExtended.OrderExtendedCompareCarModel);
                    break;
                /*case "ReleaseYear":
                    result.Sort(OrderExtended.OrderExtendedCompareReleaseYear);
                    break;*/
                case "TransmissionType":
                    result.Sort(OrderExtended.OrderExtendedCompareTransmissionType);
                    break;
                /*case "EnginePower":
                    result.Sort(OrderExtended.OrderExtendedCompareEnginePower);
                    break;*/
                case "NameOperation":
                    result.Sort(OrderExtended.OrderExtendedCompareNameOperation);
                    break;
                case "BeginTime":
                    result.Sort(OrderExtended.OrderExtendedCompareBeginTime);
                    break;
                case "EndTime":
                    result.Sort(OrderExtended.OrderExtendedCompareEndTime);
                    break;
                case "Price":
                    result.Sort(OrderExtended.OrderExtendedComparePrice);
                    break;
                default:
                    Console.WriteLine("Не сработало...");
                    break;
            }

            if (!is_ascending)
                result.Reverse();

            return result;
        }

        public List<OrderExtended> GetSearch(string condition, string value)
        {
            if (result == null || condition == null || value == null)
                return null;

            switch (condition)
            {
                case "IdOrder":
                    int value_int;
                    if ( int.TryParse(value, out value_int) )
                        result_search = result.FindAll(OrderExtended => OrderExtended.IdOrder == value_int);
                    break;
                case "CarBrand":
                    result_search = result.FindAll(OrderExtended => OrderExtended.CarBrand == value);
                    break;
                case "CarModel":
                    result_search = result.FindAll(OrderExtended => OrderExtended.CarModel == value);
                    break;
                case "ReleaseYear":
                    short value_short;
                    if ( short.TryParse(value, out value_short) )
                        result_search = result.FindAll(OrderExtended => OrderExtended.ReleaseYear == value_short);
                    break;
                case "TransmissionType":
                    result_search = result.FindAll(OrderExtended => OrderExtended.TransmissionType == value);
                    break;
                case "EnginePower":
                    short value_s;
                    if (short.TryParse(value, out value_s))
                        result_search = result.FindAll(OrderExtended => OrderExtended.ReleaseYear == value_s);
                    break;
                case "NameOperation":
                    result_search = result.FindAll(OrderExtended => OrderExtended.NameOperation == value);
                    break;
                case "BeginTime":
                    DateTime value_dt;
                    if ( DateTime.TryParse(value, out value_dt) )
                        result_search = result.FindAll(OrderExtended => OrderExtended.BeginTime == value_dt);
                    break;
                case "EndTime":
                    DateTime value_dte;
                    if (DateTime.TryParse(value, out value_dte))
                        result_search = result.FindAll(OrderExtended => OrderExtended.EndTime == value_dte);
                    break;
                case "Price":
                    decimal value_d;
                    if ( decimal.TryParse(value, out value_d) )
                        result_search = result.FindAll(OrderExtended => OrderExtended.Price == value_d);
                    break;
                default:
                    Console.WriteLine("Не сработало...");
                    break;
            }

            return result_search;
        }

        public List<KeyValuePair<string, int>> GetDataForDiagram1()
        {
            List<KeyValuePair<string, int>> I1 = new List<KeyValuePair<string, int>> { };

            var query1 =
                from res in result.Distinct()
                orderby res.CarBrand
                group res by new { CarBrand = res.CarBrand } into g

                select new
                {
                    CarBrandCount = g.Count(),
                    CarBrand = g.Key.CarBrand
                };

            foreach (var x in query1)
            {
                I1.Add(new KeyValuePair<string, int>(x.CarBrand, x.CarBrandCount));
            }
            
            return I1;
        }

        public List<KeyValuePair<string, int>> GetDataForDiagram2() //Изменить, не соответствует заданию
        {
            List<KeyValuePair<string, int>> I1 = new List<KeyValuePair<string, int>> { };

            var query1 =
                from res in result.Distinct()
                orderby res.BeginTime
                group res by new { BeginTime = res.BeginTime } into g

                select new
                {
                    BeginTimeCount = g.Count(),
                    BeginTime = g.Key.BeginTime
                };

            foreach (var x in query1)
            {
                DateTime dt = (DateTime)x.BeginTime;
                string dt_str = dt.ToString();
                I1.Add(new KeyValuePair<string, int>(dt_str, x.BeginTimeCount));
            }

            return I1;
        }

        public List<KeyValuePair<string, int>> GetDataForDiagram3()             //Изменить, не соответствует заданию
        {
            List<KeyValuePair<string, int>> I1 = new List<KeyValuePair<string, int>> { };

            var query1 =
                from res in result.Distinct()
                orderby res.Price
                group res by new { Price = res.Price } into g

                select new
                {
                    PriceCount = g.Count(),
                    Price = g.Key.Price
                };

            foreach (var x in query1)
            {
                decimal? price_dc = x.Price;
                I1.Add(new KeyValuePair<string, int>(price_dc.ToString(), x.PriceCount));
            }

            return I1;
        }        
    }
}