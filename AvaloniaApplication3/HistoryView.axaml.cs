using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AvaloniaApplication3;

public partial class HistoryView : UserControl //ну тут все те саме що в DiseaseView
{

    public static HistoryDataBase db = new HistoryDataBase();
    public ObservableCollection<History> HisList { get; set; }

    public HistoryView()
    {
        InitializeComponent();
        try
        {
            db.Load("saveH.json");
        }
        catch {}
        HisList = new ObservableCollection<History>(db.Histories);
        DataContext = this;
    }
    
    private async void OnAddHistoryClick(object? sender, RoutedEventArgs e)
    {
        var addWindow = new AddHistoryWindow();
        var window = this.VisualRoot as Window;
        var result = await addWindow.ShowDialog<History?>(window);

        if (result != null)
        {
            HisList.Add(result);
            db.AddHistory(result);
        }
    }
    
    private void OnDeleteClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (sender is Button clickButton)
        {
            string? name = clickButton.Name;
            HisList.Remove(HisList.FirstOrDefault(d => d.FullName.Equals(name)));
            db.RemoveHistory(name);
        }
    }
    
    private async void OnEditHistoryClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (sender is Button clickButton)
        {
            string? name = clickButton.Name;
            var history = db.StrongSearchByName(name).FirstOrDefault();

            if (history is not null)
            {
                var editWindow = new EditHistoryWindow(history);
                var window = this.VisualRoot as Window;
                var result = await editWindow.ShowDialog<History?>(window);

                if (result != null)
                {
                    db.RemoveHistory(name); 
                    db.AddHistory(result);

                    HisList.Remove(HisList.FirstOrDefault(d => d.FullName.Equals(name)));
                    HisList.Add(result);
                }
            }
        }
    }
    
    private void OnSearch(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var input = SearchBox.Text?.Trim() ?? "";
        HisList.Clear();

        if (input == "")
        {
            foreach (var d in db.Histories)
                HisList.Add(d);
            return;
        }
        List<History> results;
        
        results = db.Search(input);

        foreach (var d in results)
            HisList.Add(d);
    }
    
    private async void OnViewClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button clickButton)
        {
            string? name = clickButton.Name;
            var history = db.StrongSearchByName(name).FirstOrDefault();

            if (history != null)
            {
                var viewWindow = new ViewHistoryWindow(history);
                var window = this.VisualRoot as Window;
                await viewWindow.ShowDialog(window);
            }
        }
    }

}
