using CarServiceWPF.ViewModel;
using CarServiceWPF.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarServiceCore;
using System.Windows;
using CarServiceCore.Entities;
using CarServiceCore.Experiments;
using CarServiceCore.Interfaces;

namespace CarServiceWPF
{
	enum WindowMode : byte
	{
		Default = 1,
		Alternative
	}

	public class AllCreator: IStatisticsShower, ICarServiceContextCreator, IPaginalDataCreator
    {
		private StatisticsViewModel mStatisticsViewModel;
		private ServiceDB mServiceDB;
		
		// Если истинно - постраничная загрузка из БД,
		// если ложно - все данные из БД - в память
		bool mIsPaginalFromBD = true;	


		public Window TheMainWindow { get; private set; }

        public AllCreator()
        {
			mServiceDB =  new ServiceDB(this);

			IDiagramData diagramData = new DiagramData(this, mServiceDB);
			mStatisticsViewModel = new StatisticsViewModel(diagramData);

			WindowMode winMode = WindowMode.Alternative;
			TheMainWindow = GetTheMainWindow(winMode);			            
        }

		private Window GetTheMainWindow(WindowMode aWinMode)
		{
			if (aWinMode == WindowMode.Alternative)
			{
				MainAlternativeViewModel mainAltVM =
					new MainAlternativeViewModel(this, this, mServiceDB);
				return new MainAlternativeWindow(mainAltVM);
			}

			MainViewModel mainViewModel =
					new MainViewModel(this, this, mServiceDB);
			return new MainWindow(mainViewModel);
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
			return new CarServiceContext("name=CarServiceContext");
			//return new CarServiceContext("name=CarServiceContextTrial");
		}

		// Experiments
		public IPaginalData GetPaginalData(
			bool aIsPaginal)
		{
			if (!aIsPaginal) 
				return new PaginalDataFake(GetDataHandler());

			if (aIsPaginal && mIsPaginalFromBD)
			{
				IDataHandlerDB dataHandlerDB = 
					new DataHandlerDB(mServiceDB);
				return new PaginalDataDB(this, dataHandlerDB);
			}

			if (aIsPaginal && !mIsPaginalFromBD)
				return new PaginalDataRAM(GetDataHandler());

			return null;
		}

		// Experiments
		private IDataHandler GetDataHandler(int aMode = 2)
		{
			IDataHandler dataHandler = null;

			switch (aMode)
			{
				case 1:
					dataHandler = new DataHandler_1(mServiceDB, 
						new SortComparisons_1());
					break;
				case 2:
					dataHandler = new DataHandler_2(mServiceDB,
						new SortComparisons_2(
							new SortComparisons_1()),
						new SearchPredicats() );
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
