# Описание внутреннего устройства

## Классы и интерфейсы

### Startup
Точка входа.

### AllCreator
Класс, создающий все.
Реализует: IStatisticsShower, ICarServiceContextCreator, IPaginalDataCreator.

### ICarServiceContextCreator
Создает и возвращает экземпляр класса, реализующего интерфейс ICarServiceContext.
Реализация - AllCreator.
* ICarServiceContext GetCarServiceContext()

### ICarServiceContext
Доступ к базе данных.
Реализии: CarServiceContext, CarServiceContextTrial.

### IQueriesDB
Реализация - ServiceDB.
* List<OrderExtended> GetResultAll() - Возвращает данные по всем заказам.

### IServiceDB
Реализация - ServiceDB.
* List<string> GetFilterValues(string aFilterColumn) - Возвращает все значения из базы по выбранному полю.

### IDiagramData
Данные для построения диаграмм.
Реализация - DiagramData (на вход - IQueriesDB).
* int Year { get; set; }
* List<int> Values { get; set; }
* List<KeyValuePair<string, int>> DataForDiagramCarBrand { get; }
* List<KeyValuePair<string, int>> DataForDiagramMonth { get; }
* List<KeyValuePair<string, int>> DataForDiagramPrice { get; }

### IDataHandler
Обработка данных (сортировка, фильтрация, поиск или без фильтров)
* List<OrderExtended> Result { get; } - результат обработок.
* void Create() - сброс фильтров
* void MakeSort(string aCondition, bool aIsAscending)
* void MakeSearch(string aCondition, string aValue)  
Реализии: DataHandler_1, DataHandler_2, DataHandler_3.
DataHandler_1 и DataHandler_2 - сортировка и поиск данных производятся в экземпляре класса List<OrderExtended>. Используется экземпляр класса, реализующего интерфейс IQueriesDB (подается в конструктор).
DataHandler_3 получает на вход экземпляр класса, реализующего ICarServiceContextCreator для доступа к данным. Сортировка/фильтрация/поиск производятся в запросе LINQ to SQL.
* public DataHandler_1(
			IQueriesDB aQueriesDB, 
			ISortComparisons_1 aSortComparisons)
* public DataHandler_2(
			IQueriesDB aQueriesDB
			, ISortComparisons_2 aSortComparisons
			, ISearchPredicats aSearchPredicats)
* public DataHandler_3(ICarServiceContextCreator aCarServiceContextCreator)

### ISortComparisons_1
Используется для сортировки данных о заказах в классе DataHandler_1.
Реализация - SortComparisons_1.

### ISortComparisons_2
Используется для сортировки данных о заказах в классе DataHandler_2.
Реализация - SortComparisons_2.

### ISearchPredicats
Реализация - SearchPredicats.
Используется для поиска данных о заказах в классе DataHandler_2.

### IPaginalData
Отвечает за постраничный просмотр заказов в таблице.
Реализии: PaginalData, PaginalDataFake.
PaginalDataFake - реализация для режима просмотра данных на одной странице.

### IPaginalDataCreator
Реализация - AllCreator.
Возвращает PaginalData или PaginalDataFake.

### IStatisticsShower
Реализация - AllCreator.
Отвечает за отображения окна статистики.

### Person, Car, Operation, Order
Классы сущностей Entity Framework.

### OrderExtended
Класс, содержащий в себе информацию о заказе (объединение Person, Car, Operation, Order).

### SimpleCommand
Реализует ICommand

### Usefully
* public static bool IsNullOrEmpty<T>(IEnumerable<T> aCollection)

### MainWindow - MainViewModel
Главное окно. Сортировка, фильтрация, поиск заказов расположены на разных вкладках и производятся после нажатия кнопки "Применить".

### MainAlternativeWindow - MainAlternativeViewModel
Вариант главного окна. Отличается тем, что сортировка, фильтрация, поиск заказов - на одной панели, производятся при выборе соответствующего пункта и вызываются каждый раз при изменении условий, если соответствующий пункт все еще выбран.

### StatisticsWindow - StatisticsViewModel
Окно, отображающее статистику по заказам в виде диаграмм.