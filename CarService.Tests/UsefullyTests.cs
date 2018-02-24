using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using CarServiceCore.Helpers;

namespace CarServiceCore.Tests
{
    [TestClass]
    public class UsefullyTests
    {
        [TestMethod]
        public void IsNullOrEmpty_when_null_then_reterned_true()
        {
            // arrange
            List<object> null_list = null;

            // act
            bool actual = Usefully.IsNullOrEmpty(null_list);

            // assert
            Assert.IsTrue(actual);
        }

		[TestMethod]
		public void IsNullOrEmpty_when_empty_list_then_returned_true()
		{
			// arrange
			List<object> null_list = new List<object>();

			// act
			bool actual = Usefully.IsNullOrEmpty(null_list);

			// assert
			Assert.IsTrue(actual);
		}

		[TestMethod]
		public void IsNullOrEmpty_when_not_empty_list_then_returned_false()
		{
			// arrange
			List<object> null_list = new List<object> { new object() };

			// act
			bool actual = Usefully.IsNullOrEmpty(null_list);

			// assert
			Assert.IsFalse(actual);
		}
	}
}
