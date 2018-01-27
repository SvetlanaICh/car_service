using CarService.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Model
{
    class DataHandler : INotifyPropertyChanged
    {
        private List<OrderExtended> result_all;

        public List<OrderExtended> Result { get; set; }

        public DataHandler()
        {
            result_all = ServiceDB.GetAllOrderExtended();
            Result = result_all;
        }

        public void MakeSort(string condition, bool is_ascending)
        {
            if (condition == null)
                return;
            if (result_all == null)
                return;

            switch (condition)
            {
                case "IdOrder":
                    result_all.Sort(OrderExtended.OrderExtendedCompareIdOrder);
                    break;
                case "CarBrand":
                    result_all.Sort(OrderExtended.OrderExtendedCompareCarBrand);
                    break;
                case "CarModel":
                    result_all.Sort(OrderExtended.OrderExtendedCompareCarModel);
                    break;
                case "ReleaseYear":
                    result_all.Sort(OrderExtended.OrderExtendedCompareReleaseYear);
                    break;
                case "TransmissionType":
                    result_all.Sort(OrderExtended.OrderExtendedCompareTransmissionType);
                    break;
                case "EnginePower":
                    result_all.Sort(OrderExtended.OrderExtendedCompareEnginePower);
                    break;
                case "NameOperation":
                    result_all.Sort(OrderExtended.OrderExtendedCompareNameOperation);
                    break;
                case "BeginTime":
                    result_all.Sort(OrderExtended.OrderExtendedCompareBeginTime);
                    break;
                case "EndTime":
                    result_all.Sort(OrderExtended.OrderExtendedCompareEndTime);
                    break;
                case "Price":
                    result_all.Sort(OrderExtended.OrderExtendedComparePrice);
                    break;
                default:
                    Console.WriteLine("Не сработало...");
                    break;
            }

            if (!is_ascending)
                result_all.Reverse();

            Result = result_all;
        }

        public void MakeSearch(string condition, string value)
        {
            if (condition == null || value == null)
                return;
            if (result_all == null)
                return;
            Result = null;

            switch (condition)
            {
                case "IdOrder":
                    int value_int;
                    if (int.TryParse(value, out value_int))
                        Result = result_all.FindAll(OrderExtended => OrderExtended.IdOrder == value_int);
                    break;
                case "CarBrand":
                    Result = result_all.FindAll(OrderExtended => OrderExtended.CarBrand == value);
                    break;
                case "CarModel":
                    Result = result_all.FindAll(OrderExtended => OrderExtended.CarModel == value);
                    break;
                case "ReleaseYear":
                    short value_short;
                    if (short.TryParse(value, out value_short))
                        Result = result_all.FindAll(OrderExtended => OrderExtended.ReleaseYear == value_short);
                    break;
                case "TransmissionType":
                    Result = result_all.FindAll(OrderExtended => OrderExtended.TransmissionType == value);
                    break;
                case "EnginePower":
                    short value_s;
                    if (short.TryParse(value, out value_s))
                        Result = result_all.FindAll(OrderExtended => OrderExtended.EnginePower == value_s);
                    break;
                case "NameOperation":
                    Result = result_all.FindAll(OrderExtended => OrderExtended.NameOperation == value);
                    break;
                case "BeginTime":
                    DateTime value_dt;
                    if (DateTime.TryParse(value, out value_dt))
                        Result = result_all.FindAll(OrderExtended => OrderExtended.BeginTime == value_dt);
                    break;
                case "EndTime":
                    DateTime value_dte;
                    if (DateTime.TryParse(value, out value_dte))
                        Result = result_all.FindAll(OrderExtended => OrderExtended.EndTime == value_dte);
                    break;
                case "Price":
                    decimal value_d;
                    if (decimal.TryParse(value, out value_d))
                        Result = result_all.FindAll(OrderExtended => OrderExtended.Price == value_d);
                    break;
                default:
                    Result = null;
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
