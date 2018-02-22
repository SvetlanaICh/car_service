using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Model.Entities
{
	public interface ICarServiceContext : IDisposable, IObjectContextAdapter
	{
		IDbSet<Car> CarSet { get; set; }
		IDbSet<Operation> OperationSet { get; set; }
		IDbSet<Order> OrderSet { get; set; }
		IDbSet<Person> PersonSet { get; set; }
	}
}
