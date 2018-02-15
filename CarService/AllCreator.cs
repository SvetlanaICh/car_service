using CarService.ViewModel;
using CarService.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarService.Model;
using System.Windows;
using CarService.Model.Entities;
using CarService.Model.Experiments;

namespace CarService
{
	enum WindowMode : byte
	{
		Default = 1,
		Alternative
	}

	public class AllCreator: IStatisticsShower, ICarServiceContextCreator, IPaginalDataCreator
    {
		private IDataHandler mDataHandler;
		private StatisticsViewModel mStatisticsViewModel;       
        
		public Window TheMainWindow { get; private set; }

        public AllCreator()
        {
			ServiceDB serviceDB = new ServiceDB(this);
            mDataHandler = GetDataHandler(serviceDB);
			IDiagramData diagramData = new DiagramData(serviceDB);
			mStatisticsViewModel = new StatisticsViewModel(diagramData);

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

		public ICarServiceContext GetCarServiceContext()
		{
			return new CarServiceContext();
			//return new CarServiceContextTrial();
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

		// Experiments
		private IDataHandler GetDataHandler(IQueriesDB aQueriesDB, int aMode = 3)
		{
			IDataHandler dataHandler = null;

			switch (aMode)
			{
				case 1:
					dataHandler = new DataHandler_1(aQueriesDB, 
						new OrderExtendedComparisons_1());
					break;
				case 2:
					dataHandler = new DataHandler_2(aQueriesDB,
						new OrderExtendedComparisons_2(),
						new OrderExtendedPredicats() );
					break;
				case 3:
					dataHandler = new DataHandler_3(this);
					break;
				default:
					dataHandler = null;
					break;
			}
			return dataHandler;
		}
	}
}
