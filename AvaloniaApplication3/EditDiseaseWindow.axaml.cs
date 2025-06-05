using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AvaloniaApplication3;

public partial class EditDiseaseWindow : Window
{
    private Disease Original { get; set; }

    public EditDiseaseWindow(Disease disease)
    {
        InitializeComponent();
        Original = disease;
        
        NameBox.Text = disease.Name;
        DescriptionBox.Text = disease.Description;
        SymptomsBox.Text = string.Join(", ", disease.Symptoms);
        MedicationsBox.Text = string.Join(", ", disease.Medications);
    }

    private void OnSaveClick(object? sender, RoutedEventArgs e)
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

        if (DiseaseView.db.StrongSearchByName(name).Any() && name!=Original.Name)
        {
            ErrorText.Text = "Хвороба з такою назвою вже є.";
            return;
        }

        List<string> symptoms = new(symptomsRaw.Split(','));
        List<string> medications = new(medsRaw.Split(','));

        Original = new Disease(name, description, symptoms, medications);
        Close(Original);
    }
}