using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarService.Model;
using Moq;
using System.Collections.Generic;
using CarService.Model.Entities;
using System.Data.Entity;
using System.Linq;

namespace CarService.Tests
{
    [TestClass]
    public class ServiceDBTests
    {
		[TestMethod]
		public void Constructor_result_is_not_null()
		{
			var mockICarServiceContextCreator =
				GetMockICarServiceContextCreator_2_OE();

			ServiceDB serviceDB = new ServiceDB(mockICarServiceContextCreator.Object);

			Assert.IsNotNull(serviceDB);
		}

		[TestMethod]
		public void GetResultAll_given_DbSets_are_null_then_null()
		{
			IDbSet<Person> personSet = null;
			IDbSet<Car> carSet = null;
			IDbSet<Operation> operationSet = null;
			IDbSet<Order> orderSet = null;
			
			var mockICarServiceContext = new Mock<ICarServiceContext>();
			mockICarServiceContext.Setup(a => a.PersonSet).Returns(personSet);
			mockICarServiceContext.Setup(a => a.CarSet).Returns(carSet);
			mockICarServiceContext.Setup(a => a.OperationSet).Returns(operationSet);
			mockICarServiceContext.Setup(a => a.OrderSet).Returns(orderSet);

			var mockICarServiceContextCreator = new Mock<ICarServiceContextCreator>();
			mockICarServiceContextCreator.Setup(
				a => a.GetCarServiceContext()).Returns(mockICarServiceContext.Object);

			ServiceDB serviceDB = new ServiceDB(mockICarServiceContextCreator.Object);

			List<OrderExtended> res = serviceDB.GetResultAll();

			Assert.IsNull(res);
		}

		[TestMethod]
		public void GetResultAll_given_DbSets_are_empty_then_result_count_0()
		{
			var mockICarServiceContextCreator =
				GetMockICarServiceContextCreator_fromLists(null, null, null, null);
			ServiceDB serviceDB = new ServiceDB(mockICarServiceContextCreator.Object);
			int expected = 0;

			List<OrderExtended> res = serviceDB.GetResultAll();
			int actual = res.Count;

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetResultAll_given_context_then_result_count_1()// ?
		{
			var mockICarServiceContextCreator =
				GetMockICarServiceContextCreator_1_OE();
			ServiceDB serviceDB = new ServiceDB(mockICarServiceContextCreator.Object);
			int expected = 1;

			List<OrderExtended> res = serviceDB.GetResultAll();
			int actual = res.Count;

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetResultAll_given_context_then_result_count_0()//
		{
			var mockICarServiceContextCreator =
				GetMockICarServiceContextCreator_0_OE();
			ServiceDB serviceDB = new ServiceDB(mockICarServiceContextCreator.Object);
			int expected = 0;

			List<OrderExtended> res = serviceDB.GetResultAll();
			int actual = res.Count;

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetResultAll_given_context_then_result_count_2()//
		{
			var mockICarServiceContextCreator =
				GetMockICarServiceContextCreator_2_OE();
			ServiceDB serviceDB = new ServiceDB(mockICarServiceContextCreator.Object);
			int expected = 2;

			List<OrderExtended> res = serviceDB.GetResultAll();
			int actual = res.Count;

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetResultAll_then_it_calls_GetCarServiceContext()
		{
			var mockICarServiceContextCreator = new Mock<ICarServiceContextCreator>();
			var mockICarServiceContext = new Mock<ICarServiceContext>();
			mockICarServiceContextCreator.Setup(
				a => a.GetCarServiceContext()).Returns(mockICarServiceContext.Object);
			ServiceDB serviceDB = new ServiceDB(mockICarServiceContextCreator.Object);
			
			List<OrderExtended> res = serviceDB.GetResultAll();

			mockICarServiceContextCreator.Verify(cc => cc.GetCarServiceContext(), Times.Once());
		}

		[TestMethod]
		public void GetFilterValues_given_DbSets_are_null_when_IdOrder_then_null()
		{
			IDbSet<Person> personSet = null;
			IDbSet<Car> carSet = null;
			IDbSet<Operation> operationSet = null;
			IDbSet<Order> orderSet = null;

			var mockICarServiceContext = new Mock<ICarServiceContext>();
			mockICarServiceContext.Setup(a => a.PersonSet).Returns(personSet);
			mockICarServiceContext.Setup(a => a.CarSet).Returns(carSet);
			mockICarServiceContext.Setup(a => a.OperationSet).Returns(operationSet);
			mockICarServiceContext.Setup(a => a.OrderSet).Returns(orderSet);

			var mockICarServiceContextCreator = new Mock<ICarServiceContextCreator>();			
			mockICarServiceContextCreator.Setup(
				a => a.GetCarServiceContext()).Returns(mockICarServiceContext.Object);

			ServiceDB serviceDB = new ServiceDB(mockICarServiceContextCreator.Object);

			List<string> res = serviceDB.GetFilterValues("IdOrder");

			Assert.IsNull(res);
		}

		[TestMethod]
		public void GetFilterValues_given_context_when_null_then_null()//
		{
			var mockICarServiceContextCreator =
				GetMockICarServiceContextCreator_2_OE();
			ServiceDB serviceDB = new ServiceDB(mockICarServiceContextCreator.Object);

			List<string> res = serviceDB.GetFilterValues(null);

			Assert.IsNull(res);
		}

		[TestMethod]
		public void GetFilterValues_given_context_when_BadName_then_null()//
		{
			var mockICarServiceContextCreator =
				GetMockICarServiceContextCreator_2_OE();
			ServiceDB serviceDB = new ServiceDB(mockICarServiceContextCreator.Object);

			List<string> res = serviceDB.GetFilterValues("BadName");

			Assert.IsNull(res);
		}

		[TestMethod]
		public void GetFilterValues_given_context_when_empty_then_null()//
		{
			var mockICarServiceContextCreator =
				GetMockICarServiceContextCreator_2_OE();
			ServiceDB serviceDB = new ServiceDB(mockICarServiceContextCreator.Object);
			List<string> res = serviceDB.GetFilterValues("");

			Assert.IsNull(res);
		}

		[TestMethod]
		public void GetFilterValues_given_context_when_IdOrder_then_result_count_is_2()//
		{
			var mockICarServiceContextCreator =
				GetMockICarServiceContextCreator_2_OE();
			ServiceDB serviceDB = new ServiceDB(mockICarServiceContextCreator.Object);
			int expected = 2;

			List<string> res = serviceDB.GetFilterValues("IdOrder");
			int actual = res.Count;

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetFilterValues_given_context_when_IdOrder_then_result0_is_2()//
		{
			var mockICarServiceContextCreator =
				GetMockICarServiceContextCreator_2_OE();
			ServiceDB serviceDB = new ServiceDB(mockICarServiceContextCreator.Object);
			string expected = "2";

			List<string> res = serviceDB.GetFilterValues("IdOrder");
			string actual = res[0];

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetFilterValues_given_context_when_IdOrder_then_result1_is_1()//
		{
			var mockICarServiceContextCreator =
				GetMockICarServiceContextCreator_2_OE();
			ServiceDB serviceDB = new ServiceDB(mockICarServiceContextCreator.Object);
			string expected = "1";

			List<string> res = serviceDB.GetFilterValues("IdOrder");
			string actual = res[1];

			Assert.AreEqual(expected, actual);
		}

		private Mock<ICarServiceContextCreator> GetMockICarServiceContextCreator_fromLists(
			List<Person> aListPerson = null, List<Car> aListCar = null,
			List<Operation> aListOperation = null, List<Order> aListOrder = null)
		{
			var mockPersonDbSet = MockDbSet<Person>(aListPerson);
			var mockCarDbSet = MockDbSet<Car>(aListCar);
			var mockOperationDbSet = MockDbSet<Operation>(aListOperation);
			var mockOrderDbSet = MockDbSet<Order>(aListOrder);

			var mockICarServiceContext = new Mock<ICarServiceContext>();
			mockICarServiceContext.Setup(a => a.PersonSet).Returns(mockPersonDbSet.Object);
			mockICarServiceContext.Setup(a => a.CarSet).Returns(mockCarDbSet.Object);
			mockICarServiceContext.Setup(a => a.OperationSet).Returns(mockOperationDbSet.Object);
			mockICarServiceContext.Setup(a => a.OrderSet).Returns(mockOrderDbSet.Object);

			Mock<ICarServiceContextCreator> mockICarServiceContextCreator =
				new Mock<ICarServiceContextCreator>();
			mockICarServiceContextCreator.Setup(
				a => a.GetCarServiceContext()).Returns(mockICarServiceContext.Object);

			return mockICarServiceContextCreator;
		}

		// Из этих данных в ServiceDB получится пустой List<OrderExtended>,
		// они передаются в GetMockICarServiceContextCreator_fromLists
		private Mock<ICarServiceContextCreator> GetMockICarServiceContextCreator_0_OE()
		{
			Person person = new Person
			{
				IdPerson = 1,
				FirstName = "FirstName",
				LastName = "LastName",
				MiddleName = "MiddleName",
				Phone = "Phone"
			};

			Car car = new Car
			{
				IdCar = 1,
				PersonId = 1,
				CarBrand = "CarBrand",
				CarModel = "CarModel",
				ReleaseYear = 2014,
				EnginePower = 1,
				TransmissionType = "Auto"
			};

			Operation operaiton = new Operation
			{
				IdOperation = 1,
				NameOperation = "Operation"
			};

			Order order = new Order
			{
				IdOrder = 2,
				OperationId = 2,
				CarId = 2
			};

			List<Person> listPerson = new List<Person> { person };
			List<Car> listCar = new List<Car> { car };
			List<Operation> listOperation = new List<Operation> { operaiton };
			List<Order> listOrder = new List<Order> { order };

			return GetMockICarServiceContextCreator_fromLists(
				listPerson, listCar, listOperation, listOrder);
		}

		// Из этих данных в ServiceDB получится List<OrderExtended> с 1 элементом,
		// они передаются в GetMockICarServiceContextCreator_fromLists
		private Mock<ICarServiceContextCreator> GetMockICarServiceContextCreator_1_OE()
		{
			Person person = new Person
			{
				IdPerson = 1,
				FirstName = "FirstName",
				LastName = "LastName",
				MiddleName = "MiddleName",
				Phone = "Phone"
			};

			Car car = new Car
			{
				IdCar = 1,
				PersonId = 1,
				CarBrand = "CarBrand",
				CarModel = "CarModel",
				ReleaseYear = 2014,
				EnginePower = 1,
				TransmissionType = "Auto"
			};

			Operation operaiton = new Operation
			{
				IdOperation = 1,
				NameOperation = "Operation"
			};

			Order order = new Order
			{
				IdOrder = 1,
				OperationId = 1,
				CarId = 1
			};

			List<Person> listPerson = new List<Person> { person };
			List<Car> listCar = new List<Car> { car };
			List<Operation> listOperation = new List<Operation> { operaiton };
			List<Order> listOrder = new List<Order> { order };

			return GetMockICarServiceContextCreator_fromLists(
				listPerson, listCar, listOperation, listOrder);
		}

		// Из этих данных в ServiceDB получится List<OrderExtended> с 2мя элеми,
		// они передаются в GetMockICarServiceContextCreator_fromLists
		private Mock<ICarServiceContextCreator> GetMockICarServiceContextCreator_2_OE()
		{
			Person person = new Person
			{
				IdPerson = 1,
				FirstName = "FirstName",
				LastName = "LastName",
				MiddleName = "MiddleName",
				Phone = "Phone"
			};

			Car car = new Car
			{
				IdCar = 1,
				PersonId = 1,
				CarBrand = "CarBrand",
				CarModel = "CarModel",
				ReleaseYear = 2014,
				EnginePower = 1,
				TransmissionType = "Auto"
			};

			Operation operaiton = new Operation
			{
				IdOperation = 1,
				NameOperation = "Operation"
			};

			Order order1 = new Order
			{
				IdOrder = 1,
				OperationId = 1,
				CarId = 1
			};

			Order order2 = new Order
			{
				IdOrder = 2,
				OperationId = 1,
				CarId = 1
			};

			List<Person> listPerson = new List<Person> { person };
			List<Car> listCar = new List<Car> { car };
			List <Operation> listOperation = new List<Operation> { operaiton };
			List<Order>  listOrder = new List<Order> { order2, order1 };

			return GetMockICarServiceContextCreator_fromLists(
				listPerson, listCar, listOperation, listOrder);
		}

		private Mock<DbSet<T>> MockDbSet<T>(List<T> aData = null) where T : class
		{
			if (aData == null) aData = new List<T>();

			var queryable = aData.AsQueryable();

			var mock = new Mock<DbSet<T>>();

			mock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
			mock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
			mock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
			mock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

			return mock;
		}
	}
}
