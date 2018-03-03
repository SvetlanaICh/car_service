# �������� ����������� ���������� CarServiceCore

## ������ � ����������

### ICarServiceContextCreator
������� � ���������� ��������� ������, ������������ ��������� ICarServiceContext.
���������� - CarServiceContextCreator (����������� ��������� ������ �����������).
* ICarServiceContext GetCarServiceContext()

### ICarServiceContext
������ � ���� ������.
��������: CarServiceContext, CarServiceContextTrial.

### IQueriesDB
���������� - ServiceDB.
* List<OrderExtended> GetResultAll() - ���������� ������ �� ���� ������� (List<OrderExtended>).
* IQueryable<OrderExtended> GetResult(��������� ������������� ������) - ���������� IQueryable<OrderExtended>. 
������������� ������ ��������� �������� ������� �������.

### IServiceDB
���������� - ServiceDB.
* List<string> GetFilterValues(string aFilterColumn) - ���������� ��� �������� �� ���� �� ���������� ����.

### IDiagramData
������ ��� ���������� ��������.
���������� - DiagramData (�� ���� - IQueriesDB).
* int Year { get; set; }
* List<int> Values { get; set; }
* List<KeyValuePair<string, int>> DataForDiagramCarBrand { get; }
* List<KeyValuePair<string, int>> DataForDiagramMonth { get; }
* List<KeyValuePair<string, int>> DataForDiagramPrice { get; }

### IDataHandlerDB
���������� - DataHandlerDB.
��������� ������ (����������, ����������, ����� ��� ��� ��������).
* IQueryable<OrderExtended> Create(ICarServiceContext aDB);
* IQueryable<OrderExtended> MakeSort(ICarServiceContext aDB, string aCondition, bool aIsAscending);
* IQueryable<OrderExtended> MakeSearch(ICarServiceContext aDB, string aCondition, string aValue);

### IDataHandler
��������� ������ (����������, ����������, ����� ��� ��� ��������).
* List<OrderExtended> Result { get; } - ��������� ���������.
* void Create() - ����� ��������
* void MakeSort(string aCondition, bool aIsAscending)
* void MakeSearch(string aCondition, string aValue)  
��������: DataHandler_1, DataHandler_2, DataHandler_3.
DataHandler_1 � DataHandler_2 - ���������� � ����� ������ ������������ � ���������� ������ List<OrderExtended>. 
������������ ��������� ������, ������������ ��������� IQueriesDB (�������� � �����������).
DataHandler_3 �������� �� ���� ��������� ������, ������������ ICarServiceContextCreator ��� ������� � ������. 
����������/����������/����� ������������ � ������� LINQ to SQL.
* public DataHandler_1(
			IQueriesDB aQueriesDB, 
			ISortComparisons_1 aSortComparisons)
* public DataHandler_2(
			IQueriesDB aQueriesDB
			, ISortComparisons_2 aSortComparisons
			, ISearchPredicats aSearchPredicats)
* public DataHandler_3(ICarServiceContextCreator aCarServiceContextCreator)

### ISortComparisons_1
������������ ��� ���������� ������ � ������� � ������ DataHandler_1.
���������� - SortComparisons_1.

### ISortComparisons_2
������������ ��� ���������� ������ � ������� � ������ DataHandler_2.
���������� - SortComparisons_2.

### ISearchPredicats
���������� - SearchPredicats.
������������ ��� ������ ������ � ������� � ������ DataHandler_2.

### IPaginalData
�������� �� ������������ �������� ������� � �������.
��������: PaginalDataDB, PaginalDataRAM, PaginalDataFake.  
PaginalDataDB - ���������� ������������ �������� ������ �� ����. 
�� ���� - ICarServiceContextCreator � IDataHandlerDB.
PaginalDataRAM - ���������� ������������� ��������� ������, ������� ��������� �� ���� ���������.
�� ���� - IDataHandler.
PaginalDataFake - ���������� ��� ������ ��������� ������ �� ����� ��������.

### IPaginalDataCreator
���������� - AllCreator.
���������� PaginalDataDB, PaginalDataRAM ��� PaginalDataFake.

### IStatisticsShower
���������� - AllCreator.
�������� �� ����������� ���� ����������.

### Person, Car, Operation, Order
������ ��������� Entity Framework.

### OrderExtended
�����, ���������� � ���� ���������� � ������ (����������� Person, Car, Operation, Order).

### Usefully
* public static bool IsNullOrEmpty<T>(IEnumerable<T> aCollection)