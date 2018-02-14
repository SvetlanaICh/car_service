using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarService.Model.Experiments;
using CarService.Model;

namespace CarService.Tests
{
	[TestClass]
	public class OrderExtendedComparisons_1Tests
	{
		[TestMethod]
		public void OrderExtendedCompareIdOrder_when_argSmall_and_argBig_then_returned_minus_1()
		{
			OrderExtended oe_1 = new OrderExtended { IdOrder = 3 };
			OrderExtended oe_2 = new OrderExtended { IdOrder = 5 };

			OrderExtendedComparisons_1 orderExtendedComparisons_1 = 
				new OrderExtendedComparisons_1();
			int exp = -1;

			int act = orderExtendedComparisons_1.OrderExtendedCompareIdOrder(oe_1, oe_2);

			Assert.AreEqual(exp, act);
		}

		[TestMethod]
		public void OrderExtendedCompareIdOrder_when_argBig_and_argSmall_then_returned_1()
		{
			OrderExtended oe_1 = new OrderExtended { IdOrder = 5 };
			OrderExtended oe_2 = new OrderExtended { IdOrder = 3 };

			OrderExtendedComparisons_1 orderExtendedComparisons_1 =
				new OrderExtendedComparisons_1();
			int exp = 1;

			int act = orderExtendedComparisons_1.OrderExtendedCompareIdOrder(oe_1, oe_2);

			Assert.AreEqual(exp, act);
		}

		[TestMethod]
		public void OrderExtendedCompareIdOrder_when_arg_and_argTheSame_then_returned_0()
		{
			OrderExtended oe_1 = new OrderExtended { IdOrder = 5 };
			OrderExtended oe_2 = new OrderExtended { IdOrder = 5 };

			OrderExtendedComparisons_1 orderExtendedComparisons_1 =
				new OrderExtendedComparisons_1();
			int exp = 0;

			int act = orderExtendedComparisons_1.OrderExtendedCompareIdOrder(oe_1, oe_2);

			Assert.AreEqual(exp, act);
		}
	}
}
