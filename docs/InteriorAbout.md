# �������� ����������� ����������

## ������ � ����������

### Startup
����� �����.

### AllCreator
�����, ��������� ���.
���������: IStatisticsShower, ICarServiceContextCreator, IPaginalDataCreator.

### ICarServiceContextCreator
������� � ���������� ��������� ������, ������������ ��������� ICarServiceContext.
���������� - AllCreator.
* ICarServiceContext GetCarServiceContext()

### ICarServiceContext
������ � ���� ������.
��������: CarServiceContext, CarServiceContextTrial.

### IQueriesDB
���������� - ServiceDB.
* List<OrderExtended> GetResultAll() - ���������� ������ �� ���� �������.

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

### IDataHandler
��������� ������ (����������, ����������, ����� ��� ��� ��������)
* List<OrderExtended> Result { get; } - ��������� ���������.
* void Create() - ����� ��������
* void MakeSort(string aCondition, bool aIsAscending)
* void MakeSearch(string aCondition, string aValue)  
��������: DataHandler_1, DataHandler_2, DataHandler_3.
DataHandler_1 � DataHandler_2 - ���������� � ����� ������ ������������ � ���������� ������ List<OrderExtended>. ������������ ��������� ������, ������������ ��������� IQueriesDB (�������� � �����������).
DataHandler_3 �������� �� ���� ��������� ������, ������������ ICarServiceContextCreator ��� ������� � ������. ����������/����������/����� ������������ � ������� LINQ to SQL.
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
��������: PaginalData, PaginalDataFake.
PaginalDataFake - ���������� ��� ������ ��������� ������ �� ����� ��������.

### IPaginalDataCreator
���������� - AllCreator.
���������� PaginalData ��� PaginalDataFake.

### IStatisticsShower
���������� - AllCreator.
�������� �� ����������� ���� ����������.

### Person, Car, Operation, Order
������ ��������� Entity Framework.

### OrderExtended
�����, ���������� � ���� ���������� � ������ (����������� Person, Car, Operation, Order).

### SimpleCommand
��������� ICommand

### Usefully
* public static bool IsNullOrEmpty<T>(IEnumerable<T> aCollection)

### MainWindow - MainViewModel
������� ����. ����������, ����������, ����� ������� ����������� �� ������ �������� � ������������ ����� ������� ������ "���������".

### MainAlternativeWindow - MainAlternativeViewModel
������� �������� ����. ���������� ���, ��� ����������, ����������, ����� ������� - �� ����� ������, ������������ ��� ������ ���������������� ������ � ���������� ������ ��� ��� ��������� �������, ���� ��������������� ����� ��� ��� ������.

### StatisticsWindow - StatisticsViewModel
����, ������������ ���������� �� ������� � ���� ��������.