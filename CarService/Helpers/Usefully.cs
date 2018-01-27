﻿using CarService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Helpers
{
    class Usefully
    {
        public static bool IsNullOrEmpty<T>(IEnumerable<T> CollectionIn)
        {
            if (CollectionIn == null)
                return true;

            if (!CollectionIn.Any())
                return true;

            return false;
        }
    }
}