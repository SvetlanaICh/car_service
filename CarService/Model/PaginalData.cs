using CarService.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CarService.Helpers;

namespace CarService.Model
{
    class PaginalData : INotifyPropertyChanged
    {
        private List<List<OrderExtended>> resultAll { get; set; }
        private DataHandler myDataHandler;
        private int rowCount = 10;
        private int currentPageReal;

        public List<OrderExtended> ResultCurrent
        {
            get
            {
                return GetRusultCurrent();
            }
        } 
        
        private List<OrderExtended> GetRusultCurrent()
        {
            if (Usefully.IsNullOrEmpty(resultAll))
                return null;

            if (CurrentPageReal < 0 || CurrentPageReal >= resultAll.Count)
            {
                CurrentPageReal = 0;
                return GetRusultCurrent();    //Is it ok?
            }

            return resultAll[CurrentPageReal];
        }

        public int RowCount
        {
            get { return rowCount; }
            set
            {
                if (value <= 0) 
                    return;

                rowCount = value;
                CreateResultAll();
                OnPropertyChanged("RowCount");
            }
        }
        
        private int CurrentPageReal
        {
            get { return currentPageReal; }
            set
            {
                if (Usefully.IsNullOrEmpty(resultAll))
                {
                    currentPageReal = 0;
                    //OnPropertyChanged("CurrentPageReal");
                    OnPropertyChanged("CurrentPageDisplayed");
                    OnPropertyChanged("PageStatus");
                    OnPropertyChanged("ResultCurrent");
                    OnPropertyChanged("HasPrevious");
                    OnPropertyChanged("HasNext");
                    return;
                }
                
                if (value < 0 || value >= resultAll.Count)
                {
                    if (currentPageReal >= 0 && currentPageReal < resultAll.Count) //На всякий пожарный
                        return;
                    else
                        value = 0;
                }

                currentPageReal = value;
                //OnPropertyChanged("CurrentPageReal");
                OnPropertyChanged("CurrentPageDisplayed");
                OnPropertyChanged("PageStatus");
                OnPropertyChanged("ResultCurrent");
                OnPropertyChanged("HasPrevious");
                OnPropertyChanged("HasNext");
            }
        }

        public int CurrentPageDisplayed
        {
            get
            {
                if (PageCount == 0)
                    return 0;

                return CurrentPageReal + 1;
            }
        }

        public string PageStatus
        {
            get
            {
                return string.Format("Стр.{0} из {1}", CurrentPageDisplayed, PageCount);
            }
        }

        public int PageCount
        {
            get
            {
                if (myDataHandler == null)
                    return 0;
                if (rowCount == 0 || myDataHandler.Result == null)
                    return 0;           //
                decimal d = (decimal)myDataHandler.Result.Count / (decimal)rowCount;
                decimal p_d = Math.Ceiling(d);
                return Convert.ToInt32(p_d);
            }
        }

        public PaginalData()
        {
            myDataHandler = new DataHandler();
            rowCount = 15;
            CreateResultAll();
        }

        public void RefreshProperties()
        {
            //OnPropertyChanged("RowCount");

            //OnPropertyChanged("CurrentPageReal");
            OnPropertyChanged("CurrentPageDisplayed");
            OnPropertyChanged("PageStatus");
            OnPropertyChanged("ResultCurrent");
            OnPropertyChanged("HasPrevious");
            OnPropertyChanged("HasNext");
        }

        private void CreateResultAll(int page_current = 0) //Заполнение листа листов + задание текущей страницы 
        {
            if (myDataHandler == null)
                return;

            if (RowCount == 0)
                return;
            
            if ( Usefully.IsNullOrEmpty( myDataHandler.Result) )
            {
                resultAll = null;
                CurrentPageReal = 0;
                return;
            }

            resultAll = new List<List<OrderExtended>>();
            int k = 0;
            for (int i = 0; i < PageCount; i++)
            {
                resultAll.Add(new List<OrderExtended>());
                for (int j = 0; j < rowCount; j++) 
                {
                    if (k >= myDataHandler.Result.Count)
                        break;
                    resultAll[i].Add(myDataHandler.Result[k]);
                    k++;
                }
            }

            CurrentPageReal = page_current;
        }

        public void DoPrevious()
        {
            CurrentPageReal--; 
        }
        public void DoNext()
        {
            CurrentPageReal++; 
        }

        public void MakeSort(string condition, bool is_ascending)
        {
            if (myDataHandler != null)
                myDataHandler.MakeSort(condition, is_ascending);

            CreateResultAll();
        }

        public void MakeSearch(string condition, string value)
        {
            if (myDataHandler != null)
                myDataHandler.MakeSearch(condition, value);

            CreateResultAll();
        }

        public bool HasPrevious
        {
            get
            {
                if (Usefully.IsNullOrEmpty(resultAll))
                    return false;

                if (CurrentPageReal <= 0)
                    return false;

                return true;
            }
        }
        public bool HasNext
        {
            get
            {
                if (Usefully.IsNullOrEmpty(resultAll))
                    return false;

                if (CurrentPageReal >= (PageCount - 1))
                    return false;

                return true;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
