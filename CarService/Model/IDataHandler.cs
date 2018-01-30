using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Model
{
    public interface IDataHandler : INotifyPropertyChanged
    {
        List<OrderExtended> Result { get; set; }

        void Create();
        void MakeSort(string condition, bool is_ascending);
        void MakeSearch(string condition, string value);
    }
}
