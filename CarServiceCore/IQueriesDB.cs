using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServiceCore
{
    public interface IQueriesDB
    {
        List<OrderExtended> GetResultAll();
    }
}
