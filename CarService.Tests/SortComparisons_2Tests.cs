using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarServiceCore.Experiments;
using CarServiceCore;
using Moq;

namespace CarServiceCore.Tests
{
	[TestClass]
	public class SortComparisons_2Tests
	{
		[TestMethod]
		public void GetComparison_when_null_and_true_then_returned_null()
		{
			var mockISortComparisons_1 = new Mock<ISortComparisons_1>();
			SortComparisons_2 sortComparisons_2 = 
				new SortComparisons_2(mockISortComparisons_1.Object);
			Comparison<OrderExtended> comparison =
				sortComparisons_2.GetComparison(null, true);

			Assert.IsNull(comparison);
		}

		[TestMethod]
		public void GetComparison_when_BadName_and_true_then_returned_null()
		{
			var mockISortComparisons_1 = new Mock<ISortComparisons_1>();
			SortComparisons_2 sortComparisons_2 =
				new SortComparisons_2(mockISortComparisons_1.Object);
			Comparison<OrderExtended> comparison =
				sortComparisons_2.GetComparison("BadName", true);

			Assert.IsNull(comparison);
		}

		[TestMethod]
		public void GetComparison_when_IdOrder_and_true_then_returned_not_null()
		{
			var mockISortComparisons_1 = new Mock<ISortComparisons_1>();
			SortComparisons_2 sortComparisons_2 =
				new SortComparisons_2(mockISortComparisons_1.Object);
			Comparison<OrderExtended> comparison =
				sortComparisons_2.GetComparison("IdOrder", true);

			Assert.IsNotNull(comparison);
		}

	}
}
