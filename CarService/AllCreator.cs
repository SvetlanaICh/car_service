using CarService.ViewModel;
using CarService.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarService.Model;

namespace CarService
{
    public class AllCreator: IStatisticsShower, Icar_serviceEntitiesFactory, IPaginalDataCreator
    {
        private MainViewModel mainViewModel;
        private StatisticsViewModel statisticsViewModel;
        private StatisticsWindow statisticsWindow;
        private IServiceDB serviceDB;
        private IDataHandler dataHandler;

		public MainWindow TheMainWindow { get; private set; }

        public AllCreator()
        {
            serviceDB = new ServiceDB(this);
            dataHandler = new DataHandler(serviceDB);

			mainViewModel = new MainViewModel(this, this, serviceDB);
			TheMainWindow = new MainWindow(mainViewModel);

            statisticsViewModel = new StatisticsViewModel(serviceDB);            
        }

        public void StatisticsShow()
        {
            statisticsWindow = new StatisticsWindow(statisticsViewModel);
            if (statisticsWindow == null)
                return;

            statisticsWindow.Show();
        }

        public car_serviceEntities Build()
        {
            return new car_serviceEntities();
        }

		public IPaginalData GetPaginalData(bool IsPaginal)
		{
			if (dataHandler == null)
				return null;

			if (IsPaginal)
				return new PaginalData(dataHandler);
			else
				return new PaginalDataFake(dataHandler);
		}
	}
}
