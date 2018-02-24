namespace CarServiceCore.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdOrder { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? BeginTime { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? EndTime { get; set; }

        public int CarId { get; set; }

        public int OperationId { get; set; }

        public virtual Car Car { get; set; }

        public virtual Operation Operation { get; set; }
    }
}
