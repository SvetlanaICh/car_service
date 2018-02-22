using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Model.Entities
{
    class CarServiceTrialInitializer :
		DropCreateDatabaseAlways<CarServiceContext>
	{
		protected override void Seed(CarServiceContext db)
        {
			Person person1 = new Person
			{
				IdPerson = 1,
				LastName = "Иванов",
				FirstName = "Иван",				
				MiddleName = "Иванович",
				BirthYear = 1988,
				Phone = "89871231234"
			};

			Person person2 = new Person
			{
				IdPerson = 2,
				LastName = "Петров",
				FirstName = "Петр",				
				MiddleName = "Петрович",
				BirthYear = 1970,
				Phone = "89871234321"
			};

			Car car1 = new Car
			{
				IdCar = 1,
				PersonId = 1,
				CarBrand = "Renault",
				CarModel = "Logan",
				ReleaseYear = 2013,
				EnginePower = 90,
				TransmissionType = "Механика"
			};

			Car car2 = new Car
			{
				IdCar = 2,
				PersonId = 2,
				CarBrand = "Kia",
				CarModel = "Rio",
				ReleaseYear = 2017,
				EnginePower = 100,
				TransmissionType = "Автомат"
			};

			Operation operaiton1 = new Operation
			{
				IdOperation = 1,
				NameOperation = "Замена свечей зажигания",
				Price = 200
			};

			Operation operaiton2 = new Operation
			{
				IdOperation = 2,
				NameOperation = "Капитальный ремонт",
				Price = 10000
			};

			Order order1 = new Order
			{
				IdOrder = 1,
				OperationId = 1,
				CarId = 1,
				BeginTime = new DateTime(2018, 01, 01),
				EndTime = new DateTime(2018, 02, 07)
			};

			Order order2 = new Order
			{
				IdOrder = 2,
				OperationId = 2,
				CarId = 2,
				BeginTime = new DateTime(2018, 01, 15),
				EndTime = new DateTime(2018, 01, 16)
			};

			db.PersonSet.Add(person1);
			db.PersonSet.Add(person2);
			db.CarSet.Add(car1);
			db.CarSet.Add(car2);
			db.OperationSet.Add(operaiton1);
			db.OperationSet.Add(operaiton2);
			db.OrderSet.Add(order1);
			db.OrderSet.Add(order2);

			base.Seed(db);
        }
    }
}
