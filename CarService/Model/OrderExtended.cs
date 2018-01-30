using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Model
{
    public class OrderExtended : Order
    {
        //public int IdOrder { get; set; }
        public string CarBrand { get; set; }
        public string CarModel { get; set; }
        public short? ReleaseYear { get; set; }
        public string TransmissionType { get; set; }
        public short? EnginePower { get; set; }
        public string NameOperation { get; set; }
        //public DateTime? BeginTime { get; set; }
        //public DateTime? EndTime { get; set; }
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
            if (oe1.BeginTime == null || oe2.BeginTime == null)
                return (-1) * Nullable.Compare(oe1.BeginTime, oe2.BeginTime);

            return DateTime.Compare((DateTime)oe1.BeginTime, (DateTime)oe2.BeginTime);
        }

        static public int OrderExtendedComparePrice(OrderExtended oe1, OrderExtended oe2)
        {
            if (oe1.Price == null || oe2.Price == null)
                return (-1) * Nullable.Compare(oe1.Price, oe2.Price);

            return decimal.Compare((decimal)oe1.Price, (decimal)oe2.Price);
        }

        static public int OrderExtendedCompareEndTime(OrderExtended oe1, OrderExtended oe2)
        {
            if (oe1.EndTime == null || oe2.EndTime == null)
                return (-1) * Nullable.Compare(oe1.EndTime, oe2.EndTime);

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

        static public int OrderExtendedCompareIdOrder(OrderExtended oe1, OrderExtended oe2)
        {
            return oe1.IdOrder.CompareTo(oe2.IdOrder);
        }

        static public int OrderExtendedCompareReleaseYear(OrderExtended oe1, OrderExtended oe2)
        {
            if (oe1.ReleaseYear == null || oe2.ReleaseYear == null)
                return (-1) * Nullable.Compare(oe1.ReleaseYear, oe2.ReleaseYear);

            int i1 = (int)oe1.ReleaseYear;
            int i2 = (int)oe2.ReleaseYear;
            return i1.CompareTo(i2);
        }

        static public int OrderExtendedCompareEnginePower(OrderExtended oe1, OrderExtended oe2)
        {
            if (oe1.EnginePower == null || oe2.EnginePower == null)
                return (-1) * Nullable.Compare(oe1.EnginePower, oe2.EnginePower);

            int i1 = (int)oe1.EnginePower;
            int i2 = (int)oe2.EnginePower;
            return i1.CompareTo(i2);
        }
    }
}
