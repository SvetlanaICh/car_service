using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarService.Model.Experiments;
using CarService.Model;

namespace CarService.Tests
{
	[TestClass]
	public class OrderExtendedComparisons_2Tests
	{
		[TestMethod]
		public void GetComparison_when_null_and_true_then_returned_null()
		{
			OrderExtendedComparisons_2 orderExtendedComparisons_2 = 
				new OrderExtendedComparisons_2();
			Comparison<OrderExtended> comparison =
				orderExtendedComparisons_2.GetComparison(null, true);

			Assert.IsNull(comparison);
		}

		[TestMethod]
		public void GetComparison_when_BadName_and_true_then_returned_null()
		{
			OrderExtendedComparisons_2 orderExtendedComparisons_2 =
				new OrderExtendedComparisons_2();
			Comparison<OrderExtended> comparison =
				orderExtendedComparisons_2.GetComparison("BadName", true);

			Assert.IsNull(comparison);
		}

		[TestMethod]
		public void GetComparison_when_IdOrder_and_true_then_returned_not_null()
		{
			OrderExtendedComparisons_2 orderExtendedComparisons_2 =
				new OrderExtendedComparisons_2();
			Comparison<OrderExtended> comparison =
				orderExtendedComparisons_2.GetComparison("IdOrder", true);

			Assert.IsNotNull(comparison);
		}

		// it's not so good name
		[TestMethod]
		public void GetComparison_when_IdOrder_and_true_then_result_works_1()	//It could be too long!!
		{
			OrderExtendedComparisons_2 orderExtendedComparisons_2 =
				new OrderExtendedComparisons_2();
			OrderExtended oe_1 = new OrderExtended { IdOrder = 3 };
			OrderExtended oe_2 = new OrderExtended { IdOrder = 5 };
			int exp = -1;

			Comparison<OrderExtended> comparison =
				orderExtendedComparisons_2.GetComparison("IdOrder", true);
			int act = comparison.Invoke(oe_1, oe_2);

			Assert.AreEqual(exp, act);
		}

		// it's not so good name
		[TestMethod]
		public void GetComparison_when_IdOrder_and_true_then_result_works_2()
		{
			OrderExtendedComparisons_2 orderExtendedComparisons_2 =
				new OrderExtendedComparisons_2();
			OrderExtended oe_1 = new OrderExtended { IdOrder = 5 };
			OrderExtended oe_2 = new OrderExtended { IdOrder = 3 };
			int exp = 1;

			Comparison<OrderExtended> comparison =
				orderExtendedComparisons_2.GetComparison("IdOrder", true);
			int act = comparison.Invoke(oe_1, oe_2);

			Assert.AreEqual(exp, act);
		}

		// it's not so good name
		[TestMethod]
		public void GetComparison_when_IdOrder_and_false_then_result_works_3()
		{
			OrderExtendedComparisons_2 orderExtendedComparisons_2 =
				new OrderExtendedComparisons_2();
			OrderExtended oe_1 = new OrderExtended { IdOrder = 5 };
			OrderExtended oe_2 = new OrderExtended { IdOrder = 3 };
			int exp = -1;

			Comparison<OrderExtended> comparison =
				orderExtendedComparisons_2.GetComparison("IdOrder", false);
			int act = comparison.Invoke(oe_1, oe_2);

			Assert.AreEqual(exp, act);
		}

		// it's not so good name
		[TestMethod]
		public void GetComparison_when_IdOrder_and_true_then_result_works_4()
		{
			OrderExtendedComparisons_2 orderExtendedComparisons_2 =
				new OrderExtendedComparisons_2();
			OrderExtended oe_1 = new OrderExtended { IdOrder = 5 };
			OrderExtended oe_2 = new OrderExtended { IdOrder = 5 };
			int exp = 0;

			Comparison<OrderExtended> comparison =
				orderExtendedComparisons_2.GetComparison("IdOrder", true);
			int act = comparison.Invoke(oe_1, oe_2);

			Assert.AreEqual(exp, act);
		}
	}
}
