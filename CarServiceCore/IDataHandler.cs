using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServiceCore
{
    public interface IDataHandler : INotifyPropertyChanged
    {
        List<OrderExtended> Result { get; }

        void Create();
        void MakeSort(string aCondition, bool aIsAscending);
        void MakeSearch(string aCondition, string aValue);
    }
}
