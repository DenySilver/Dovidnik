using Avalonia;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AvaloniaApplication3;

public class Disease
{
    public string Name { get; set; }
    public string Description { get; }
    public List<string> Symptoms { get; }
    public List<string> Medications { get; }

    public string SymptomsString => $"Симптоми: {string.Join(", ", Symptoms)}";

    public string MedicationsString => $"Ліки: {string.Join(", ", Medications)}";

    public Disease(string name, string description, List<string> symptoms, List<string> medications)
    {
        Name = name;
        Description = description;
        Symptoms = symptoms;
        Medications = medications;
    }
}

public class DataBase
{
    public List<Disease> Diseases { get; set; } = new();

    private void Save(string path)
    {
        var jsonD = JsonSerializer.Serialize(Diseases);
        File.WriteAllText(path, jsonD);
    }

    public void Load(string path)
    {
        var lines = File.ReadAllText(path);
        Diseases = JsonSerializer.Deserialize<List<Disease>>(lines, new JsonSerializerOptions
        { PropertyNameCaseInsensitive = true, IgnoreReadOnlyProperties = true});
    }

    public List<Disease> StrongSearchByName(string name)
    {
        return Diseases.Where(d => d.Name.Equals(name)).ToList();
    }

    public List<Disease> Search(string input)
    {
        var keywords = input.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        return Diseases.Where(d =>
            keywords.Any(kw =>
                d.Name.Contains(kw, StringComparison.OrdinalIgnoreCase) ||
                d.Symptoms.Any(sym => sym.Contains(kw, StringComparison.OrdinalIgnoreCase)) ||
                d.Medications.Any(med => med.Contains(kw, StringComparison.OrdinalIgnoreCase))
            )).ToList();
    }


    public void AddDisease(Disease disease)
    {
        Diseases.Add(disease);
        this.Save("saveD.json");
    }

    public void RemoveDisease(string name)
    {
        var diseaseToRemove = Diseases.FirstOrDefault(d => d.Name.Equals(name));
        if (diseaseToRemove != null)
        {
            Diseases.Remove(diseaseToRemove);
            this.Save("saveD.json");
        }
    }
}
//===================================================
public class HistoryRecord
{
    public Disease Disease { get;}
    public DateTime Date { get;}
    
    public HistoryRecord() {}
    public HistoryRecord(Disease dis, DateTime date)
    {
        Disease = dis;
        Date = date;
    }
}

public class History
{
    public string FullName { get; set; }
    public double Age { get;}
    public double Height { get;}
    public double Weight { get;}
    public List<HistoryRecord> Records { get; } = new();

    public History(string fullName, double age, double height, double weight)
    {
        FullName = fullName;
        Age = age;
        Height = height;
        Weight = weight;
    }
    public string Info => $"{FullName} ({Age} років, {Height} см, {Weight} кг)";
}

public class HistoryDataBase
{
    public List<History> Histories { get; set; } = new();
    
    public List<History> StrongSearchByName(string name)
    {
        return Histories.Where(d => d.FullName.Equals(name)).ToList();
    }
    
    public void AddHistory(History history)
    {
        Histories.Add(history);
        this.Save("saveH.json");
    }

    public void RemoveHistory(string name)
    {
        var diseaseToRemove = Histories.FirstOrDefault(d => d.FullName.Equals(name));
        if (diseaseToRemove != null)
        {
            Histories.Remove(diseaseToRemove);
            this.Save("saveH.json");
        }
    }
    
    public List<History> Search(string input)
    {
        var keywords = input.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        return Histories.Where(h =>
            keywords.Any(kw =>
                h.FullName.Contains(kw, StringComparison.OrdinalIgnoreCase) ||
                h.Age.ToString().Contains(kw) ||
                h.Height.ToString().Contains(kw) ||
                h.Weight.ToString().Contains(kw)
            )).ToList();
    }
    public void Save(string path)
    {
        var jsonH = JsonSerializer.Serialize(Histories);
        File.WriteAllText(path, jsonH);
    }

    public void Load(string path)
    {
        var lines = File.ReadAllText(path);
        Histories = JsonSerializer.Deserialize<List<History>>(lines, new JsonSerializerOptions
        { PropertyNameCaseInsensitive = true, IgnoreReadOnlyProperties = true});
    }
}



class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}