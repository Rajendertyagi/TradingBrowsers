using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TradingBrowser.Models;

namespace TradingBrowser.Services;

public class DownloadManager
{
    private readonly string _filePath;

    public List<DownloadItem> Items { get; private set; } = new();

    public DownloadManager(string userDataPath)
    {
        _filePath = Path.Combine(userDataPath, "downloads.json");
        Load();
    }

    public void Add(DownloadItem item)
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
        Items = JsonSerializer.Deserialize<List<DownloadItem>>(
            File.ReadAllText(_filePath)) ?? new();
    }
}
