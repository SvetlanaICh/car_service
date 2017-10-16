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
    class QueriesDB
    {
        public List<List<OrderExtended>> ResultAll { get; set; }
        private List<OrderExtended> resultCurrent { get; set; }
        private List<OrderExtended> result;

        public QueriesDB()
        {
            result = new List<OrderExtended> { };
            CreateAll();
        }
  
        public void CreateResultAll( int row_count)
        {
            if (row_count == 0)
                return;
            int count_of_pages = GetPageCount(row_count);
            if (resultCurrent == null)
                return;
            ResultAll = new List<List<OrderExtended>> { };
            int k = 0;            
            for (int i = 0; i < count_of_pages; i++)
            {
                ResultAll.Add(new List<OrderExtended> { } );
                for (int j = 0; j < row_count; j++)
                {
                    if (k >= resultCurrent.Count )
                        break;
                    ResultAll[i].Add(resultCurrent[k]);
                    k++;
                }
            }                                   
        }

        public int GetPageCount(int row_count)
        {
            if (row_count == 0 || resultCurrent == null)
                return 0;           //
            decimal d = (decimal)resultCurrent.Count / (decimal)row_count;
            decimal p_d = Math.Ceiling(d);
            return Convert.ToInt32(p_d);
        }

        public void CreateAll()
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
            resultCurrent = result;
        }

        public void MakeSort(string condition, bool is_ascending)
        {
            if (condition == null)
                return;
            CreateAll();
            if (result == null)
                return;
                
            switch (condition)
            {
                case "IdOrder":
                    result.Sort(OrderExtended.OrderExtendedCompareIdOrder);
                    break;
                case "CarBrand":
                    result.Sort(OrderExtended.OrderExtendedCompareCarBrand);
                    break;
                case "CarModel":
                    result.Sort(OrderExtended.OrderExtendedCompareCarModel);
                    break;
                case "ReleaseYear":
                    result.Sort(OrderExtended.OrderExtendedCompareReleaseYear);
                    break;
                case "TransmissionType":
                    result.Sort(OrderExtended.OrderExtendedCompareTransmissionType);
                    break;
                case "EnginePower":
                    result.Sort(OrderExtended.OrderExtendedCompareEnginePower);
                    break;
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

            resultCurrent = result;
        }

        public void MakeSearch(string condition, string value)
        {
            if (condition == null || value == null)
                return;
            CreateAll();
            if (result == null)
                return;
            resultCurrent = null;

            switch (condition)
            {
                case "IdOrder":
                    int value_int;
                    if ( int.TryParse(value, out value_int) )
                        resultCurrent = result.FindAll(OrderExtended => OrderExtended.IdOrder == value_int);
                    break;
                case "CarBrand":
                    resultCurrent = result.FindAll(OrderExtended => OrderExtended.CarBrand == value);
                    break;
                case "CarModel":
                    resultCurrent = result.FindAll(OrderExtended => OrderExtended.CarModel == value);
                    break;
                case "ReleaseYear":
                    short value_short;
                    if ( short.TryParse(value, out value_short) )
                       resultCurrent = result.FindAll(OrderExtended => OrderExtended.ReleaseYear == value_short);
                    break;
                case "TransmissionType":
                    resultCurrent = result.FindAll(OrderExtended => OrderExtended.TransmissionType == value);
                    break;
                case "EnginePower":
                    short value_s;
                    if (short.TryParse(value, out value_s))
                        resultCurrent = result.FindAll(OrderExtended => OrderExtended.ReleaseYear == value_s);
                    break;
                case "NameOperation":
                    resultCurrent = result.FindAll(OrderExtended => OrderExtended.NameOperation == value);
                    break;
                case "BeginTime":
                    DateTime value_dt;
                    if ( DateTime.TryParse(value, out value_dt) )
                        resultCurrent = result.FindAll(OrderExtended => OrderExtended.BeginTime == value_dt);
                    break;
                case "EndTime":
                    DateTime value_dte;
                    if (DateTime.TryParse(value, out value_dte))
                        resultCurrent = result.FindAll(OrderExtended => OrderExtended.EndTime == value_dte);
                    break;
                case "Price":
                    decimal value_d;
                    if ( decimal.TryParse(value, out value_d) )
                        resultCurrent = result.FindAll(OrderExtended => OrderExtended.Price == value_d);
                    break;
                default:
                    resultCurrent = null;
                    break;
            }
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

        public List<KeyValuePair<string, int>> GetDataForDiagram3() //Изменить, не соответствует заданию
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