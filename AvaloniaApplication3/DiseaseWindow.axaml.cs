using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaloniaApplication3;

public partial class DiseaseWindow : Window
{
    public DiseaseWindow(Disease disease)
    {
        InitializeComponent();
        NameText.Text = disease.Name;
        DescriptionText.Text = $"Опис: {disease.Description}";
        SymptomsText.Text = disease.SymptomsString;
        MedicationsText.Text = disease.MedicationsString;
    }
}