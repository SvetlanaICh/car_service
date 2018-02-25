using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarServiceCore.Entities;
using CarServiceCore.Interfaces;

namespace CarServiceCore
{
	public class CarServiceContextCreator : ICarServiceContextCreator
	{
		private string mConnection;
		public CarServiceContextCreator(string aName = "name=CarServiceContext")
		{
			mConnection = aName;
		}

		public ICarServiceContext GetCarServiceContext()
		{
			return new CarServiceContext(mConnection);
		}
	}
}
