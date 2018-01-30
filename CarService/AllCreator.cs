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
    public class AllCreator: IStatisticsShower, Icar_serviceEntitiesFactory
    {
        private MainViewModel mainViewModel;
        private StatisticsViewModel statisticsViewModel;
        private StatisticsWindow statisticsWindow;
        private IServiceDB serviceDB;
        private IDataHandler dataHandler;
        private IPaginalData paginalData;

        public MainWindow TheMainWindow { get; private set; }
        public AllCreator()
        {
            serviceDB = new ServiceDB(this);
            dataHandler = new DataHandler(serviceDB);
            paginalData = new PaginalData(dataHandler);

            mainViewModel = new MainViewModel(this, paginalData, serviceDB);
			TheMainWindow = new MainWindow(mainViewModel);

            statisticsViewModel = new StatisticsViewModel(serviceDB);            
        }

        public void StatisticsShow()
        {
            statisticsWindow = new StatisticsWindow(statisticsViewModel);
            if (statisticsWindow == null)
                return;

            statisticsWindow.Show();

            /*statisticsWindow.Closed += (o, e) =>
            {
                statisticsWindow = null;
            };*/
        }

        public car_serviceEntities Build()
        {
            return new car_serviceEntities();
        }
    }
}
