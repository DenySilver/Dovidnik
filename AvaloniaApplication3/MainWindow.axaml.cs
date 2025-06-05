using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AvaloniaApplication3;

public partial class MainWindow : Window
{
    private DiseaseView _diseaseView = new DiseaseView(); //інтерфейс зі списком хвороб
    private HistoryView _historyView = new HistoryView(); //інтерфейс зі списком історій
    public MainWindow()
    {
        InitializeComponent();
        MainContentControl.Content = _diseaseView; //за замовчуванням інтерфейс з хворобами
    }
    
    private void OnDis(object? sender, RoutedEventArgs e) //при натисканні на кнопку зміна до інтерфейсу з хворобами
    {
        MainContentControl.Content = _diseaseView;
    }

    private void OnHis(object? sender, RoutedEventArgs e) //при натисканні на кнопку зміна до інтерфейсу з хворобами
    {
        MainContentControl.Content = _historyView;
    }
}
