using CarService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Helpers
{
    public class Usefully
    {
        public static bool IsNullOrEmpty<T>(IEnumerable<T> aCollection)
        {
            if (aCollection == null)
                return true;

            if (!aCollection.Any())
                return true;

            return false;
        }
    }
}
