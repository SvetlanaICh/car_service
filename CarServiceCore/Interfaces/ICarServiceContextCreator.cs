using CarServiceCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServiceCore.Interfaces
{
    public interface ICarServiceContextCreator
	{
        ICarServiceContext GetCarServiceContext();
	}
}
