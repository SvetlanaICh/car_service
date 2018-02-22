namespace CarService.Model.Entities
{
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;

	public /*partial*/ class CarServiceContext : DbContext, ICarServiceContext
	{
		public CarServiceContext(string aConnection)
			: base(aConnection)
		{
		}

		public virtual IDbSet<Car> CarSet { get; set; }
		public virtual IDbSet<Operation> OperationSet { get; set; }
		public virtual IDbSet<Order> OrderSet { get; set; }
		public virtual IDbSet<Person> PersonSet { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Car>()
				.Property(e => e.CarBrand)
				.IsUnicode(true);

			modelBuilder.Entity<Car>()
				.Property(e => e.CarModel)
				.IsUnicode(true);

			modelBuilder.Entity<Car>()
				.Property(e => e.TransmissionType)
				.IsFixedLength()
				.IsUnicode(true);

			modelBuilder.Entity<Car>()
				.HasMany(e => e.Orders)
				.WithRequired(e => e.Car)
				.HasForeignKey(e => e.CarId)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Operation>()
				.Property(e => e.NameOperation)
				.IsUnicode(true);

			modelBuilder.Entity<Operation>()
				.Property(e => e.Price)
				.HasPrecision(10, 4);

			modelBuilder.Entity<Operation>()
				.HasMany(e => e.Orders)
				.WithRequired(e => e.Operation)
				.HasForeignKey(e => e.OperationId)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Person>()
				.Property(e => e.LastName)
				.IsUnicode(true);

			modelBuilder.Entity<Person>()
				.Property(e => e.FirstName)
				.IsUnicode(true);

			modelBuilder.Entity<Person>()
				.Property(e => e.MiddleName)
				.IsUnicode(true);

			modelBuilder.Entity<Person>()
				.Property(e => e.Phone)
				.IsFixedLength()
				.IsUnicode(false);

			modelBuilder.Entity<Person>()
				.HasMany(e => e.Cars)
				.WithRequired(e => e.Person)
				.HasForeignKey(e => e.PersonId)
				.WillCascadeOnDelete(false);
		}
	}
}
