using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TradingBrowser.Models;

namespace TradingBrowser.Services;

public class HistoryManager
{
    private readonly string _filePath;

    public List<HistoryItem> Items { get; private set; } = new();

    public HistoryManager(string userDataPath)
    {
        _filePath = Path.Combine(userDataPath, "history.json");
        Load();
    }

    public void Add(HistoryItem item)
    {
        Items.Insert(0, item);
        Save();
    }

    public void Save()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
        File.WriteAllText(_filePath,
            JsonSerializer.Serialize(Items, new JsonSerializerOptions { WriteIndented = true }));
    }

    public void Load()
    {
        if (!File.Exists(_filePath)) return;
        Items = JsonSerializer.Deserialize<List<HistoryItem>>(
            File.ReadAllText(_filePath)) ?? new();
    }
}
