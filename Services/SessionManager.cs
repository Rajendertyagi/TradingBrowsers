using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace TradingBrowser.Services;

public class SessionManager
{
    private readonly string _filePath;

    public List<string> OpenTabs { get; set; } = new();

    public SessionManager(string userDataPath)
    {
        _filePath = Path.Combine(userDataPath, "session.json");
        Load();
    }

    public void Save()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
        File.WriteAllText(_filePath,
            JsonSerializer.Serialize(OpenTabs, new JsonSerializerOptions { WriteIndented = true }));
    }

    public void Load()
    {
        if (!File.Exists(_filePath)) return;
        OpenTabs = JsonSerializer.Deserialize<List<string>>(
            File.ReadAllText(_filePath)) ?? new();
    }
}
