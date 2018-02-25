using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServiceCore.Interfaces
{
    public interface IPaginalData : IDataHandler
    {
        int RowCount { get; set; }
        int CurrentPageDisplayed { get; }
        string PageStatus { get; }
        int PageCount { get; }
        bool HasPrevious { get; }
        bool HasNext { get; }

        void DoPrevious();
        void DoNext();
        void RefreshProperties();
    }
}
