﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Model
{
    public interface IQueriesDB
    {
        List<OrderExtended> GetResultAll();
        List<string> GetFilterValues(string aFilterColumn);
    }
}
