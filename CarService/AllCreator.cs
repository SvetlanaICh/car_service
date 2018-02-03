using CarService.ViewModel;
using CarService.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarService.Model;
using System.Windows;

namespace CarService
{
	enum WindowMode : byte
	{
		Default = 1,
		Alternative
	}

	public class AllCreator: IStatisticsShower, Icar_serviceEntitiesCreator, IPaginalDataCreator
    {
		private IDataHandler mDataHandler;
		private StatisticsViewModel mStatisticsViewModel;       
        
		public Window TheMainWindow { get; private set; }

        public AllCreator()
        {
			IServiceDB serviceDB = new ServiceDB(this);
            mDataHandler = new DataHandler(serviceDB, new OrderExtendedComparisons());
			mStatisticsViewModel = new StatisticsViewModel(serviceDB);

			WindowMode winMode = WindowMode.Alternative;

			if (winMode == WindowMode.Default)
			{
				MainViewModel mainViewModel = new MainViewModel(this, this, serviceDB);
				TheMainWindow = new MainWindow(mainViewModel);
			}
			if (winMode == WindowMode.Alternative)
			{
				MainAlternativeViewModel mainAltVM = 
					new MainAlternativeViewModel(this, this, serviceDB);
				TheMainWindow = new MainAlternativeWindow(mainAltVM);
			}			            
        }

        public void StatisticsShow()
        {
			StatisticsWindow statisticsWindow = 
				new StatisticsWindow(mStatisticsViewModel);

            if (statisticsWindow == null)
                return;

            statisticsWindow.Show();
        }

        public car_serviceEntities GetCar_serviceEntities()
        {
            return new car_serviceEntities();
        }

		public IPaginalData GetPaginalData(bool aIsPaginal)
		{
			if (mDataHandler == null)
				return null;

			if (aIsPaginal)
				return new PaginalData(mDataHandler);
			else
				return new PaginalDataFake(mDataHandler);
		}
	}
}
