using CarService.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Model.Experiments
{
	delegate void DelegateSearch(ref IQueryable<OrderExtended> aResult, string aValue);
	delegate void DelegateSort(ref IQueryable<OrderExtended> aResult, bool aIsAscending);

	class DataHandler_3 : IDataHandler
	{
		private ICarServiceContextCreator mCarServiceContextCreator;
		private Dictionary<string, DelegateSearch> mSearchActions;
		private Dictionary<string, DelegateSort> mSortActions;

		public List<OrderExtended> Result { get; private set; }

		public DataHandler_3(ICarServiceContextCreator aCarServiceContextCreator)
		{
			mCarServiceContextCreator = aCarServiceContextCreator;

			mSearchActions = new Dictionary<string, DelegateSearch>
			{
				{ "IdOrder", SearchByIdOrder },
				{ "CarBrand", SearchByCarBrand },
				{ "CarModel", SearchByCarModel },
				{ "ReleaseYear", SearchByReleaseYear },
				{ "TransmissionType", SearchByTransmissionType },
				{ "EnginePower", SearchByEnginePower },
				{ "NameOperation", SearchByNameOperation },
				{ "BeginTime", SearchByBeginTime },
				{ "EndTime", SearchByEndTime },
				{ "Price", SearchByPrice }
			};

			mSortActions = new Dictionary<string, DelegateSort>
			{
				{ "IdOrder", SortByIdOrder },
				{ "CarBrand", SortByCarBrand },
				{ "CarModel", SortByCarModel },
				{ "ReleaseYear", SortByReleaseYear },
				{ "TransmissionType", SortByTransmissionType },
				{ "EnginePower", SortByEnginePower },
				{ "NameOperation", SortByNameOperation },
				{ "BeginTime", SortByBeginTime },
				{ "EndTime", SortByEndTime },
				{ "Price", SortByPrice }
			};

			Create();
		}

		private void GetResultAll(DelegateSearch aSearch, string aValue)
		{
			GetResultAll(aSearch, aValue, null, false);
		}

		private void GetResultAll(DelegateSort aSort, bool aIsAscending)
		{
			GetResultAll(null, null, aSort, aIsAscending);
		}

		public void GetResultAll(DelegateSearch aSearch = null, string aValue = null, 
			DelegateSort aSort = null, bool aIsAscending = false)
		{
			IQueryable<OrderExtended> result;

			try
			{
				using (ICarServiceContext db = mCarServiceContextCreator.GetCarServiceContext())
				{
					result = from or in db.OrderSet
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

					aSearch?.Invoke(ref result, aValue);
					aSort?.Invoke(ref result, aIsAscending);

					Result = result.ToList();
				}
			}
			catch (Exception ex)
			{
				if (ex != null)
					Console.WriteLine(ex.Message);
				Result = null;
			}
		}

		public void Create()
		{
			GetResultAll();
		}

		public void MakeSearch(string aCondition, string aValue)
		{
			Result = null;

			if (string.IsNullOrEmpty(aCondition))
				return;
			if (string.IsNullOrEmpty(aValue))
				return;
			
			try
			{
				GetResultAll(mSearchActions[aCondition], aValue);
			}
			catch (Exception ex)
			{
				if (ex != null)
					Console.WriteLine(ex.Message);
				Result = null;
			}
		}

		private void SearchByIdOrder(ref IQueryable<OrderExtended> aResult, string aValue)
		{
			int value;
			if (!int.TryParse(aValue, out value))
				return;

			aResult = from r in aResult
					  where r.IdOrder == value
					  select r;
		}

		private void SearchByCarBrand(ref IQueryable<OrderExtended> aResult, string aValue)
		{
			aResult = from r in aResult
					  where r.CarBrand == aValue
					  select r;
		}

		private void SearchByCarModel(ref IQueryable<OrderExtended> aResult, string aValue)
		{
			aResult = from r in aResult
					  where r.CarModel == aValue
					  select r;
		}

		private void SearchByReleaseYear(ref IQueryable<OrderExtended> aResult, string aValue)
		{
			short value;
			if (!short.TryParse(aValue, out value))
				return;

			aResult = from r in aResult
					  where r.ReleaseYear == value
					  select r;
		}

		private void SearchByTransmissionType(ref IQueryable<OrderExtended> aResult, string aValue)
		{
			aResult = from r in aResult
					  where r.TransmissionType == aValue
					  select r;
		}

		private void SearchByEnginePower(ref IQueryable<OrderExtended> aResult, string aValue)
		{
			short value;
			if (!short.TryParse(aValue, out value))
				return;

			aResult = from r in aResult
					  where r.EnginePower == value
					  select r;
		}

		private void SearchByNameOperation(ref IQueryable<OrderExtended> aResult, string aValue)
		{
			aResult = from r in aResult
					  where r.NameOperation == aValue
					  select r;
		}

		private void SearchByBeginTime(ref IQueryable<OrderExtended> aResult, string aValue)
		{
			DateTime value;
			if (!DateTime.TryParse(aValue, out value))
				return;

			aResult = from r in aResult
					  where r.BeginTime == value
					  select r;
		}

		private void SearchByEndTime(ref IQueryable<OrderExtended> aResult, string aValue)
		{
			DateTime value;
			if (!DateTime.TryParse(aValue, out value))
				return;

			aResult = from r in aResult
					  where r.EndTime == value
					  select r;
		}

		private void SearchByPrice(ref IQueryable<OrderExtended> aResult, string aValue)
		{
			decimal value;
			if (!decimal.TryParse(aValue, out value))
				return;

			aResult = from r in aResult
					  where r.Price == value
					  select r;
		}

		public void MakeSort(string aCondition, bool aIsAscending)
		{
			Result = null;
			if (string.IsNullOrEmpty(aCondition))
				return;

			try
			{
				GetResultAll(mSortActions[aCondition], aIsAscending);
			}
			catch (Exception ex)
			{
				if (ex != null)
					Console.WriteLine(ex.Message);
				Result = null;
			}
		}

		private void SortByIdOrder(ref IQueryable<OrderExtended> aResult, bool aIsAscending)
		{
			if (aIsAscending)
				aResult = from r in aResult
						  orderby r.IdOrder ascending
						  select r;
			else
				aResult = from r in aResult
						  orderby r.IdOrder descending
						  select r;
		}

		private void SortByCarBrand(ref IQueryable<OrderExtended> aResult, bool aIsAscending)
		{
			if (aIsAscending)
				aResult = from r in aResult
					  orderby r.CarBrand ascending
						  select r;
			else
				aResult = from r in aResult
					  orderby r.CarBrand descending
					  select r;
		}

		private void SortByCarModel(ref IQueryable<OrderExtended> aResult, bool aIsAscending)
		{
			if (aIsAscending)
				aResult = from r in aResult
						  orderby r.CarModel ascending
						  select r;
			else
				aResult = from r in aResult
						  orderby r.CarModel descending
						  select r;
		}

		private void SortByReleaseYear(ref IQueryable<OrderExtended> aResult, bool aIsAscending)
		{
			if (aIsAscending)
				aResult = from r in aResult
						  orderby r.ReleaseYear ascending
						  select r;
			else
				aResult = from r in aResult
						  orderby r.ReleaseYear descending
						  select r;
		}

		private void SortByTransmissionType(ref IQueryable<OrderExtended> aResult, bool aIsAscending)
		{
			if (aIsAscending)
				aResult = from r in aResult
						  orderby r.TransmissionType ascending
						  select r;
			else
				aResult = from r in aResult
						  orderby r.TransmissionType descending
						  select r;
		}

		private void SortByEnginePower(ref IQueryable<OrderExtended> aResult, bool aIsAscending)
		{
			if (aIsAscending)
				aResult = from r in aResult
						  orderby r.EnginePower ascending
						  select r;
			else
				aResult = from r in aResult
						  orderby r.EnginePower descending
						  select r;
		}

		private void SortByNameOperation(ref IQueryable<OrderExtended> aResult, bool aIsAscending)
		{
			if (aIsAscending)
				aResult = from r in aResult
						  orderby r.NameOperation ascending
						  select r;
			else
				aResult = from r in aResult
						  orderby r.NameOperation descending
						  select r;
		}
		private void SortByBeginTime(ref IQueryable<OrderExtended> aResult, bool aIsAscending)
		{
			if (aIsAscending)
				aResult = from r in aResult
						  orderby r.BeginTime ascending
						  select r;
			else
				aResult = from r in aResult
						  orderby r.BeginTime descending
						  select r;
		}

		private void SortByEndTime(ref IQueryable<OrderExtended> aResult, bool aIsAscending)
		{
			if (aIsAscending)
				aResult = from r in aResult
						  orderby r.EndTime ascending
						  select r;
			else
				aResult = from r in aResult
						  orderby r.EndTime descending
						  select r;
		}

		private void SortByPrice(ref IQueryable<OrderExtended> aResult, bool aIsAscending)
		{
			if (aIsAscending)
				aResult = from r in aResult
						  orderby r.Price ascending
						  select r;
			else
				aResult = from r in aResult
						  orderby r.Price descending
						  select r;
		}


		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName]string aProp = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(aProp));
		}
	}
}
