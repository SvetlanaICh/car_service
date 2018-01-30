using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Model
{
    public interface Icar_serviceEntitiesFactory
    {
        car_serviceEntities Build();
    }
}
