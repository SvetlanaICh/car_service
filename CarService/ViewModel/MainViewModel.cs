using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CarService.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    { 
        public List<OrderExtended> OrdersAll { get; private set; }

        public MainViewModel()
        {
            OrdersAll = QueriesDB.GetOrdersExtended();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
