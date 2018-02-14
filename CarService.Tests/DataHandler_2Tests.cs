using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarService.Model;
using Moq;
using CarService.Model.Experiments;
using System.Collections.Generic;

namespace CarService.Tests
{
	[TestClass]
	public class DataHandler_2Tests
	{
		[TestMethod]
		public void MakeSort_when_IdOrder_and_true_then_it_calls_GetComparison_with_the_same_args()
		{
			var mockIQueriesDB = new Mock<IQueriesDB>();
			mockIQueriesDB.Setup(a => a.GetResultAll()).Returns(new List<OrderExtended>());

			var mockIOrderExtendedComparisons_2 = new Mock<IOrderExtendedComparisons_2>();

			var mockIOrderExtendedPredicats = new Mock<IOrderExtendedPredicats>();

			DataHandler_2 dataHandler_2 = new DataHandler_2(
				mockIQueriesDB.Object,
				mockIOrderExtendedComparisons_2.Object,
				mockIOrderExtendedPredicats.Object
				);

			dataHandler_2.MakeSort("IdOrder", true);

			mockIOrderExtendedComparisons_2.Verify(
				a => a.GetComparison("IdOrder", true), Times.Once);
		}

		[TestMethod]
		public void MakeSort_when_null_and_true_then_it_does_not_call_GetComparison()
		{
			var mockIQueriesDB = new Mock<IQueriesDB>();
			mockIQueriesDB.Setup(a => a.GetResultAll()).Returns(new List<OrderExtended>());

			var mockIOrderExtendedComparisons_2 = new Mock<IOrderExtendedComparisons_2>();

			var mockIOrderExtendedPredicats = new Mock<IOrderExtendedPredicats>();

			DataHandler_2 dataHandler_2 = new DataHandler_2(
				mockIQueriesDB.Object,
				mockIOrderExtendedComparisons_2.Object,
				mockIOrderExtendedPredicats.Object
				);

			dataHandler_2.MakeSort(null, true);

			mockIOrderExtendedComparisons_2.Verify(
				a => a.GetComparison(null, true), Times.Never);
		}

		[TestMethod]
		public void MakeSearch_when_IdOrder_and_1_then_it_calls_GetPredicate_with_the_same_args()
		{
			var mockIQueriesDB = new Mock<IQueriesDB>();
			mockIQueriesDB.Setup(a => a.GetResultAll()).Returns(new List<OrderExtended>());

			var mockIOrderExtendedComparisons_2 = new Mock<IOrderExtendedComparisons_2>();

			var mockIOrderExtendedPredicats = new Mock<IOrderExtendedPredicats>();

			DataHandler_2 dataHandler_2 = new DataHandler_2(
				mockIQueriesDB.Object,
				mockIOrderExtendedComparisons_2.Object,
				mockIOrderExtendedPredicats.Object
				);

			dataHandler_2.MakeSearch("IdOrder", "1");

			mockIOrderExtendedPredicats.Verify(
				a => a.GetPredicate("IdOrder", "1"), Times.Once);
		}

		[TestMethod]
		public void MakeSearch_when_null_and_null_then_it_does_not_call_GetPredicate()
		{
			var mockIQueriesDB = new Mock<IQueriesDB>();
			mockIQueriesDB.Setup(a => a.GetResultAll()).Returns(new List<OrderExtended>());

			var mockIOrderExtendedComparisons_2 = new Mock<IOrderExtendedComparisons_2>();

			var mockIOrderExtendedPredicats = new Mock<IOrderExtendedPredicats>();

			DataHandler_2 dataHandler_2 = new DataHandler_2(
				mockIQueriesDB.Object,
				mockIOrderExtendedComparisons_2.Object,
				mockIOrderExtendedPredicats.Object
				);

			dataHandler_2.MakeSearch(null, null);

			mockIOrderExtendedPredicats.Verify(
				a => a.GetPredicate(null, null), Times.Never);
		}
	}
}
