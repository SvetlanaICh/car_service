using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarService.Model.Experiments;
using CarService.Model;

namespace CarService.Tests
{
	[TestClass]
	public class SearchPredicatsTests
	{
		[TestMethod]
		public void GetPredicate_when_null_and_null_then_null()
		{
			SearchPredicats searchPredicats = new SearchPredicats();

			Predicate<OrderExtended> predicate =
				searchPredicats.GetPredicate(null, null);

			Assert.IsNull(predicate);
		}

		// it's not so good name
		[TestMethod]
		public void GetPredicate_when_IdOrder_and_1_then_result_works_1()	
		{
			SearchPredicats searchPredicats = new SearchPredicats();
			OrderExtended oe_1 = new OrderExtended { IdOrder = 1 };

			Predicate<OrderExtended> predicate =
				searchPredicats.GetPredicate("IdOrder", "1");
			bool act = predicate.Invoke(oe_1);

			Assert.IsTrue(act);
		}

		// it's not so good name
		[TestMethod]
		public void GetPredicate_when_IdOrder_and_1_then_result_works_2()
		{
			SearchPredicats searchPredicats = new SearchPredicats();
			OrderExtended oe_1 = new OrderExtended { IdOrder = 2 };

			Predicate<OrderExtended> predicate =
				searchPredicats.GetPredicate("IdOrder", "1");
			bool act = predicate.Invoke(oe_1);

			Assert.IsFalse(act);
		}
	}
}
