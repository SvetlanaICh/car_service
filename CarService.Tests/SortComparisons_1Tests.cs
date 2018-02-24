using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarServiceCore.Experiments;
using CarServiceCore;

namespace CarServiceCore.Tests
{
	[TestClass]
	public class SortComparisons_1Tests
	{
		[TestMethod]
		public void CompareIdOrder_when_argSmall_and_argBig_then_returned_minus_1()
		{
			OrderExtended oe_1 = new OrderExtended { IdOrder = 3 };
			OrderExtended oe_2 = new OrderExtended { IdOrder = 5 };

			SortComparisons_1 sortComparisons_1 = 
				new SortComparisons_1();
			int exp = -1;

			int act = sortComparisons_1.CompareIdOrder(oe_1, oe_2);

			Assert.AreEqual(exp, act);
		}

		[TestMethod]
		public void CompareIdOrder_when_argBig_and_argSmall_then_returned_1()
		{
			OrderExtended oe_1 = new OrderExtended { IdOrder = 5 };
			OrderExtended oe_2 = new OrderExtended { IdOrder = 3 };

			SortComparisons_1 sortComparisons_1 =
				new SortComparisons_1();
			int exp = 1;

			int act = sortComparisons_1.CompareIdOrder(oe_1, oe_2);

			Assert.AreEqual(exp, act);
		}

		[TestMethod]
		public void CompareIdOrder_when_arg_and_argTheSame_then_returned_0()
		{
			OrderExtended oe_1 = new OrderExtended { IdOrder = 5 };
			OrderExtended oe_2 = new OrderExtended { IdOrder = 5 };

			SortComparisons_1 sortComparisons_1 =
				new SortComparisons_1();
			int exp = 0;

			int act = sortComparisons_1.CompareIdOrder(oe_1, oe_2);

			Assert.AreEqual(exp, act);
		}
	}
}
