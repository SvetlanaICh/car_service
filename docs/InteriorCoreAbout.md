# Описание внутреннего устройства CarServiceCore

## Классы и интерфейсы

### ICarServiceContextCreator
Создает и возвращает экземпляр класса, реализующего интерфейс ICarServiceContext.
Реализация - CarServiceContextCreator (конструктор принимает строку подключения).
* ICarServiceContext GetCarServiceContext()

### ICarServiceContext
Доступ к базе данных.
Реализии: CarServiceContext, CarServiceContextTrial.

### IQueriesDB
Реализация - ServiceDB.
* List<OrderExtended> GetResultAll() - Возвращает данные по всем заказам (List<OrderExtended>).
* IQueryable<OrderExtended> GetResult(несколько перегруженных версий) - Возвращает IQueryable<OrderExtended>. 
Перегруженные версии позволяют задавать условия выборки.

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

### IDataHandlerDB
Реализация - DataHandlerDB.
Обработка данных (сортировка, фильтрация, поиск или без фильтров).
* IQueryable<OrderExtended> Create(ICarServiceContext aDB);
* IQueryable<OrderExtended> MakeSort(ICarServiceContext aDB, string aCondition, bool aIsAscending);
* IQueryable<OrderExtended> MakeSearch(ICarServiceContext aDB, string aCondition, string aValue);

### IDataHandler
Обработка данных (сортировка, фильтрация, поиск или без фильтров).
* List<OrderExtended> Result { get; } - результат обработок.
* void Create() - сброс фильтров
* void MakeSort(string aCondition, bool aIsAscending)
* void MakeSearch(string aCondition, string aValue)  
Реализии: DataHandler_1, DataHandler_2, DataHandler_3.
DataHandler_1 и DataHandler_2 - сортировка и поиск данных производятся в экземпляре класса List<OrderExtended>. 
Используется экземпляр класса, реализующего интерфейс IQueriesDB (подается в конструктор).
DataHandler_3 получает на вход экземпляр класса, реализующего ICarServiceContextCreator для доступа к данным. 
Сортировка/фильтрация/поиск производятся в запросе LINQ to SQL.
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
Реализии: PaginalDataDB, PaginalDataRAM, PaginalDataFake.  
PaginalDataDB - реализация постраничной загрузки данных из базы. 
На вход - ICarServiceContextCreator и IDataHandlerDB.
PaginalDataRAM - реализация постраничного просмотра данных, которые загружены из базы полностью.
На вход - IDataHandler.
PaginalDataFake - реализация для режима просмотра данных на одной странице.

### IPaginalDataCreator
Реализация - AllCreator.
Возвращает PaginalDataDB, PaginalDataRAM или PaginalDataFake.

### IStatisticsShower
Реализация - AllCreator.
Отвечает за отображения окна статистики.

### Person, Car, Operation, Order
Классы сущностей Entity Framework.

### OrderExtended
Класс, содержащий в себе информацию о заказе (объединение Person, Car, Operation, Order).

### Usefully
* public static bool IsNullOrEmpty<T>(IEnumerable<T> aCollection)