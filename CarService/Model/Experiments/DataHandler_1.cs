using CarService.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Model.Experiments
{
    public class DataHandler_1 : IDataHandler
    {
        private IQueriesDB mQueriesDB;
        private List<OrderExtended> mResultAll;
		private IOrderExtendedComparisons_1 mOrderExtendedComparisons;

		public List<OrderExtended> Result { get; private set; }

		public DataHandler_1(IQueriesDB aQueriesDB, IOrderExtendedComparisons_1 aOrderExtendedComparisons)
        {
            mQueriesDB = aQueriesDB;
            Create();
			mOrderExtendedComparisons = aOrderExtendedComparisons;
		}

        public void Create()
        {
			mResultAll = mQueriesDB.GetResultAll();
            Result = mResultAll;
        }

        public void MakeSort(string aCondition, bool aIsAscending)
        {
			Result = null;
			mResultAll = mQueriesDB.GetResultAll();
			
			if (string.IsNullOrEmpty(aCondition))
                return;
            if (mResultAll == null)
                return;

            switch (aCondition)
            {
                case "IdOrder":
					mResultAll.Sort(mOrderExtendedComparisons.OrderExtendedCompareIdOrder);
                    break;
                case "CarBrand":
					mResultAll.Sort(mOrderExtendedComparisons.OrderExtendedCompareCarBrand);
                    break;
                case "CarModel":
					mResultAll.Sort(mOrderExtendedComparisons.OrderExtendedCompareCarModel);
                    break;
                case "ReleaseYear":
					mResultAll.Sort(mOrderExtendedComparisons.OrderExtendedCompareReleaseYear);
                    break;
                case "TransmissionType":
					mResultAll.Sort(mOrderExtendedComparisons.OrderExtendedCompareTransmissionType);
                    break;
                case "EnginePower":
					mResultAll.Sort(mOrderExtendedComparisons.OrderExtendedCompareEnginePower);
                    break;
                case "NameOperation":
					mResultAll.Sort(mOrderExtendedComparisons.OrderExtendedCompareNameOperation);
                    break;
                case "BeginTime":
					mResultAll.Sort(mOrderExtendedComparisons.OrderExtendedCompareBeginTime);
                    break;
                case "EndTime":
					mResultAll.Sort(mOrderExtendedComparisons.OrderExtendedCompareEndTime);
                    break;
                case "Price":
					mResultAll.Sort(mOrderExtendedComparisons.OrderExtendedComparePrice);
                    break;
                default:
                    Console.WriteLine("Не сработало...");
                    break;
            }

            if (!aIsAscending)
				mResultAll.Reverse();

            Result = mResultAll;
        }

        public void MakeSearch(string aCondition, string aValue)
        {
			mResultAll = mQueriesDB.GetResultAll();
			Result = null;

			if (aCondition == null || aValue == null)
                return;
            if (mResultAll == null)
                return;           

            switch (aCondition)
            {
                case "IdOrder":
                    int value_int;
                    if (int.TryParse(aValue, out value_int))
                        Result = mResultAll.FindAll(OrderExtended => OrderExtended.IdOrder == value_int);
                    break;
                case "CarBrand":
                    Result = mResultAll.FindAll(OrderExtended => OrderExtended.CarBrand == aValue);
                    break;
                case "CarModel":
                    Result = mResultAll.FindAll(OrderExtended => OrderExtended.CarModel == aValue);
                    break;
                case "ReleaseYear":
                    short value_short;
                    if (short.TryParse(aValue, out value_short))
                        Result = mResultAll.FindAll(OrderExtended => OrderExtended.ReleaseYear == value_short);
                    break;
                case "TransmissionType":
                    Result = mResultAll.FindAll(OrderExtended => OrderExtended.TransmissionType == aValue);
                    break;
                case "EnginePower":
                    short value_s;
                    if (short.TryParse(aValue, out value_s))
                        Result = mResultAll.FindAll(OrderExtended => OrderExtended.EnginePower == value_s);
                    break;
                case "NameOperation":
                    Result = mResultAll.FindAll(OrderExtended => OrderExtended.NameOperation == aValue);
                    break;
                case "BeginTime":
                    DateTime value_dt;
                    if (DateTime.TryParse(aValue, out value_dt))
                        Result = mResultAll.FindAll(OrderExtended => OrderExtended.BeginTime == value_dt);
                    break;
                case "EndTime":
                    DateTime value_dte;
                    if (DateTime.TryParse(aValue, out value_dte))
                        Result = mResultAll.FindAll(OrderExtended => OrderExtended.EndTime == value_dte);
                    break;
                case "Price":
                    decimal value_d;
                    if (decimal.TryParse(aValue, out value_d))
                        Result = mResultAll.FindAll(OrderExtended => OrderExtended.Price == value_d);
                    break;
                default:
                    Result = null;
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string aProp = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(aProp));
        }
    }
}
