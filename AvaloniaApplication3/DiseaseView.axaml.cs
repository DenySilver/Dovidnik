using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AvaloniaApplication3;

public partial class DiseaseView : UserControl
{
    public static DataBase db = new DataBase(); //створення екземпляру бази даних
    public ObservableCollection<Disease> DisList { get; set; } //колекція для відображення
    public DiseaseView()
    {
        InitializeComponent();
        try //перевірка наявності файлу
        {
            db.Load("saveD.json"); //завантаження даних
        }
        catch{}
        DisList = new ObservableCollection<Disease>(db.Diseases); //вивантаження даних з бази даних в колекцію
        DataContext = this;
    }
    private async void OnAddDiseaseClick(object? sender, RoutedEventArgs e) //обробка натискання додати хворобу
    {
        var addWindow = new AddDiseaseWindow(); 
        var window = this.VisualRoot as Window;
        var result = await addWindow.ShowDialog<Disease?>(window); //відкриває діалогове вікно

        if (result != null) //якщо є результат взаємодії, додає хворобу до колекції та до бд
        {
            DisList.Add(result);
            db.AddDisease(result);
        }
    }
    
    private async void OnEditDiseaseClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (sender is Button clickButton)
        {
            string? name = clickButton.Name; //назва хвороби (ім'я кнопки)
            var disease = db.StrongSearchByName(name).FirstOrDefault();

            if (disease is not null) //якщо є хвороба з такою назвою, з'являється вікно
            {
                var editWindow = new EditDiseaseWindow(disease);
                var window = this.VisualRoot as Window;
                var result = await editWindow.ShowDialog<Disease?>(window);

                if (result != null) //якщо є результат, в колекції і в бд замінюється хвороба
                {
                    db.RemoveDisease(name); 
                    db.AddDisease(result); 
                    
                    DisList.Remove(DisList.FirstOrDefault(d => d.Name.Equals(name)));
                    DisList.Add(result);
                }
            }
        }
    }
    private void OnDeleteClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e) //видалення
    {
        if (sender is Button clickButton)
        {
            string? name = clickButton.Name;
            DisList.Remove(DisList.FirstOrDefault(d => d.Name.Equals(name)));
            db.RemoveDisease(name);
        }
    }

    private void OnSearch(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var input = SearchBox.Text?.Trim() ?? ""; //якщо є текст в полі вводу, видалення зайвих пробілів, інакше пустий string
        DisList.Clear();

        if (input == "")
        {
            foreach (var d in db.Diseases) //просто додаються хвороби з бд
                DisList.Add(d);
            return;
        }
        List<Disease> results;
        
        results = db.Search(input);

        foreach (var d in results) //додаються хвороби з пошуку
            DisList.Add(d);
    }

    private void OnDisease(object? sender, Avalonia.Interactivity.RoutedEventArgs e) //показ інформації хвороби
    {
        if (sender is Button clickButton)
        {
            string? name = clickButton.Name;
            var disease = db.StrongSearchByName(name).FirstOrDefault();
            if (disease != null)
            {
                var disWindow = new DiseaseWindow(disease);
                disWindow.Show();
            }
        }
    }
    
    private async void OnAssign(object? sender, RoutedEventArgs e)
    {
        if (sender is Button clickButton)
        {
            string? name = clickButton.Name;
            var disease = db.StrongSearchByName(name).FirstOrDefault();

            if (disease != null)
            {
                var selectWindow = new SelectHistoryWindow(HistoryView.db.Histories);
                var window = this.VisualRoot as Window;
                var selected = await selectWindow.ShowDialog<History?>(window); //обрана історія хвороби

                if (selected != null)
                {
                    selected.Records.Add(new HistoryRecord(disease, DateTime.Now)); //додається новий запис
                    HistoryView.db.Save("saveH.json"); //зберігання
                }
            }
        }
    }

}
