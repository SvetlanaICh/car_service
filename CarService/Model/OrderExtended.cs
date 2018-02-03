using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Model
{
    public class OrderExtended
    {
        public int IdOrder { get; set; }
        public string CarBrand { get; set; }
        public string CarModel { get; set; }
        public short? ReleaseYear { get; set; }
        public string TransmissionType { get; set; }
        public short? EnginePower { get; set; }
        public string NameOperation { get; set; }
        public DateTime? BeginTime { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal? Price { get; set; }
        public string PersonLastName { get; set; }
        public string PersonFirstName { get; set; }
        public string PersonMiddleName { get; set; }
        public short? PersonBirthYear { get; set; }
        public string PersonPhone { get; set; }
    }
}
