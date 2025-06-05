using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AvaloniaApplication3;

public partial class AddHistoryWindow : Window
{
    public History? NewHistory { get; set; }
    public AddHistoryWindow()
    {
        InitializeComponent();
    }
    
    private void OnAddClick(object? sender, RoutedEventArgs e)
    {
        string name = NameBox.Text?.Trim() ?? "";
        string ageText = AgeBox.Text?.Trim() ?? "";
        string heightText = HeightBox.Text?.Trim() ?? "";
        string weightText = WeightBox.Text?.Trim() ?? "";
        
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(ageText) || string.IsNullOrWhiteSpace(heightText) || 
            string.IsNullOrWhiteSpace(weightText))
        {
            ErrorText.Text = "Будь ласка, заповніть усі поля.";
            return;
        }
        
        var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length < 2 || parts.Length > 3)
        {
            ErrorText.Text = "Введіть повне ПІБ.";
            return;
        }
        
        bool onlyLetters = parts.All(part => part.All(char.IsLetter));

        if (!onlyLetters)
        {
            ErrorText.Text = "Ім’я має містити тільки літери (без цифр і символів).";
            return;
        }

        if (HistoryView.db.StrongSearchByName(name).Any())
        {
            ErrorText.Text = "Історія з таким пацієнтом вже є.";
            return;
        }

        if (double.TryParse(ageText, out double age) &&
            double.TryParse(heightText, out double height) &&
            double.TryParse(weightText, out double weight))
        {
            NewHistory = new History(name, age, height, weight);
            Close(NewHistory);
        }
        else
        {
            ErrorText.Text = "Введіть коректні дані.";
            return;
        }
    }
}