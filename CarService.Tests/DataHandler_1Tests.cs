using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarService.Model;
using System.Collections.Generic;
using Moq;
using CarService.Model.Experiments;

namespace CarService.Tests
{
	[TestClass]
	public class DataHandler_1Tests
	{
		[TestMethod]
		public void MakeSort_when_IdOrder_and_true_then_objSmall_is_the_first()
		{
			var mockIOrderExtendedComparisons_1 = new Mock<IOrderExtendedComparisons_1>();

			OrderExtended oe_1 = new OrderExtended { IdOrder = 5 };
			OrderExtended oe_2 = new OrderExtended { IdOrder = 3 };

			Comparison<OrderExtended> comparison = (aOE1, aOE2) => 
			{ return aOE1.IdOrder.CompareTo(aOE2.IdOrder); } ;

			mockIOrderExtendedComparisons_1.Setup(
				a => a.OrderExtendedCompareIdOrder(oe_1, oe_2)).Returns(comparison);

			var mockIQueriesDB = new Mock<IQueriesDB>();
			mockIQueriesDB.Setup(a => a.GetResultAll()).Returns(new List<OrderExtended> { oe_1, oe_2 });

			DataHandler_1 dataHandler_1 = new DataHandler_1(mockIQueriesDB.Object, 
				mockIOrderExtendedComparisons_1.Object);
			int expected = 3;

			dataHandler_1.MakeSort("IdOrder", true);
			var res = dataHandler_1.Result;
			int actual = res[0].IdOrder;

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void MakeSort_when_IdOrder_and_false_then_objBig_is_the_first()
		{
			OrderExtended oe_1 = new OrderExtended { IdOrder = 3 };
			OrderExtended oe_2 = new OrderExtended { IdOrder = 5 };

			var mockIQueriesDB = new Mock<IQueriesDB>();
			mockIQueriesDB.Setup(a => a.GetResultAll()).Returns(
				new List<OrderExtended> { oe_1, oe_2 });

			var mockIOrderExtendedComparisons_1 = new Mock<IOrderExtendedComparisons_1>();			
			Comparison<OrderExtended> comparison = (aOE1, aOE2) =>
			{ return aOE1.IdOrder.CompareTo(aOE2.IdOrder); };
			mockIOrderExtendedComparisons_1.Setup(
				a => a.OrderExtendedCompareIdOrder(oe_1, oe_2)).Returns(comparison);			

			DataHandler_1 dataHandler_1 = new DataHandler_1(mockIQueriesDB.Object,
				mockIOrderExtendedComparisons_1.Object);
			int expected = 5;

			dataHandler_1.MakeSort("IdOrder", false);
			var res = dataHandler_1.Result;
			int actual = res[0].IdOrder;

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void MakeSort_when_IdOrder_and_true_then_it_calls_OrderExtendedCompareIdOrder()
		{
			OrderExtended oe_1 = new OrderExtended();
			OrderExtended oe_2 = new OrderExtended();

			var mockIQueriesDB = new Mock<IQueriesDB>();
			mockIQueriesDB.Setup(a => a.GetResultAll()).Returns(new List<OrderExtended> { oe_1, oe_2 });

			var mockIOrderExtendedComparisons_1 = new Mock<IOrderExtendedComparisons_1>();

			DataHandler_1 dataHandler_1 = new DataHandler_1(mockIQueriesDB.Object,
				mockIOrderExtendedComparisons_1.Object);

			dataHandler_1.MakeSort("IdOrder", true);

			mockIOrderExtendedComparisons_1.Verify(
				cc => cc.OrderExtendedCompareIdOrder(oe_1, oe_2), Times.Once());
		}

		[TestMethod]
		public void MakeSort_when_CarBrand_and_true_then_it_calls_OrderExtendedCompareCarBrand()
		{
			OrderExtended oe_1 = new OrderExtended();
			OrderExtended oe_2 = new OrderExtended();

			var mockIQueriesDB = new Mock<IQueriesDB>();
			mockIQueriesDB.Setup(a => a.GetResultAll()).Returns(new List<OrderExtended> { oe_1, oe_2 });

			var mockIOrderExtendedComparisons_1 = new Mock<IOrderExtendedComparisons_1>();

			DataHandler_1 dataHandler_1 = new DataHandler_1(mockIQueriesDB.Object,
				mockIOrderExtendedComparisons_1.Object);

			dataHandler_1.MakeSort("CarBrand", true);
			mockIOrderExtendedComparisons_1.Verify(
				cc => cc.OrderExtendedCompareCarBrand(oe_1, oe_2), Times.Once());
		}
	}
}
