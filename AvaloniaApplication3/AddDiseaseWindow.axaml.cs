using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AvaloniaApplication3;

public partial class AddDiseaseWindow : Window
{
    public Disease? NewDisease { get; set; }

    public AddDiseaseWindow()
    {
        InitializeComponent();
    }

    private void OnAddClick(object? sender, RoutedEventArgs e)
    {
        string name = NameBox.Text?.Trim() ?? "";
        string description = DescriptionBox.Text?.Trim() ?? "";
        string symptomsRaw = SymptomsBox.Text?.Trim() ?? "";
        string medsRaw = MedicationsBox.Text?.Trim() ?? "";
        
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(description) || string.IsNullOrWhiteSpace(symptomsRaw) || 
            string.IsNullOrWhiteSpace(medsRaw))
        {
            ErrorText.Text = "Будь ласка, заповніть усі поля.";
            return;
        }

        if (DiseaseView.db.StrongSearchByName(name).Any())
        {
            ErrorText.Text = "Хвороба з такою назвою вже є.";
            return;
        }

        List<string> symptoms = new(symptomsRaw.Split(',', StringSplitOptions.RemoveEmptyEntries));
        List<string> medications = new(medsRaw.Split(',', StringSplitOptions.RemoveEmptyEntries));

        NewDisease = new Disease(name, description, symptoms, medications);
        Close(NewDisease);
    }

}