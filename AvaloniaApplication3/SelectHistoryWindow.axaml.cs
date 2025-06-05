using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;

namespace AvaloniaApplication3;

public partial class SelectHistoryWindow : Window
{
    public History? SelectedHistory { get; private set; }

    public SelectHistoryWindow(List<History> histories)
    {
        InitializeComponent();
        HistoryBox.ItemsSource = histories;
        HistoryBox.SelectedIndex = 0;
        DataContext = this;
    }

    private void OnConfirmClick(object? sender, RoutedEventArgs e)
    {
        SelectedHistory = HistoryBox.SelectedItem as History; //обране зі списку зберегти як History і потім повернути це значення
        Close(SelectedHistory);
    }
}
