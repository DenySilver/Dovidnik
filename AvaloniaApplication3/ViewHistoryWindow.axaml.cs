using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia;

namespace AvaloniaApplication3;

public partial class ViewHistoryWindow : Window
{
    private readonly List<HistoryRecord> originalRecords;
    private readonly History _history;
    public ViewHistoryWindow(History history)
    {
        InitializeComponent();
        originalRecords = history.Records;
        _history = history;
        DisplayRecords(originalRecords);
    }

    private void OnSearchClick(object? sender, RoutedEventArgs e)
    {
        string input = SearchBox.Text?.Trim() ?? "";

        var filtered = originalRecords.Where(r =>
            r.Disease.Name.Contains(input, StringComparison.OrdinalIgnoreCase) ||
            r.Date.ToString("dd.MM.yyyy").Contains(input)).ToList();

        DisplayRecords(filtered);
    }

    private void DisplayRecords(List<HistoryRecord> records)
    {
        ResultsPanel.Children.Clear();

        var grouped = records
            .GroupBy(r => r.Date.Date)
            .OrderByDescending(g => g.Key);

        foreach (var group in grouped)
        {
            var dateHeader = new TextBlock
            {
                Text = group.Key.ToString("dd.MM.yyyy"),
                FontWeight = Avalonia.Media.FontWeight.Bold,
                Margin = new Thickness(0, 10, 0, 5)
            };
            ResultsPanel.Children.Add(dateHeader);

            foreach (var record in group)
            {
                ResultsPanel.Children.Add(new TextBlock
                {
                    Text = $"- {record.Disease?.Name ?? "[невідомо]"}",
                    Margin = new Thickness(10, 2, 0, 2)
                });
            }
        }
    }
    
    private async void OnExportClick(object? sender, RoutedEventArgs e)
    {
        string fileName = $"{_history.FullName}.txt".Replace(" ", "_");
        var lines = new List<string>
        {
            $"Інформація про пацієнта:",
            $"ПІБ: {_history.FullName}",
            $"Вік: {_history.Age} років",
            $"Зріст: {_history.Height} см",
            $"Вага: {_history.Weight} кг",
            "",
            "Історія хвороб:"
        };

        var grouped = _history.Records
            .GroupBy(r => r.Date.Date)
            .OrderByDescending(g => g.Key);

        foreach (var group in grouped)
        {
            lines.Add($"Дата: {group.Key:dd.MM.yyyy}");
            foreach (var record in group)
            {
                lines.Add($" - {record.Disease?.Name ?? "[Невідоме захворювання]"}");
            }
            lines.Add(""); // порожній рядок між групами
        }

        // зберігаємо у файл
        await File.WriteAllLinesAsync(fileName, lines);

        // сповіщення
        var window = new Window
        {
            Title = "Експорт завершено",
            Width = 300,
            Height = 100,
            Content = new TextBlock
            {
                Text = $"Файл збережено як:\n{fileName}",
                Margin = new Thickness(10)
            }
        };
        await window.ShowDialog(this);
    }

}