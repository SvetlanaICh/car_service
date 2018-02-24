# Описание внутреннего устройства CarServiceWPF

## Классы и интерфейсы

### Startup
Точка входа.

### AllCreator
Класс, создающий все.
Реализует: IStatisticsShower, ICarServiceContextCreator, IPaginalDataCreator библиотеки CarServiceCore.

### SimpleCommand
Реализует ICommand

### MainWindow - MainViewModel
Главное окно. Сортировка, фильтрация, поиск заказов расположены на разных вкладках и производятся после нажатия кнопки "Применить".

### MainAlternativeWindow - MainAlternativeViewModel
Вариант главного окна. Отличается тем, что сортировка, фильтрация, поиск заказов - на одной панели, производятся при выборе соответствующего пункта и вызываются каждый раз при изменении условий, если соответствующий пункт все еще выбран.

### StatisticsWindow - StatisticsViewModel
Окно, отображающее статистику по заказам в виде диаграмм.