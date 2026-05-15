using System.Collections.Generic;

namespace TradingBrowser;

public static class UserAgentManager
{
    private static readonly Dictionary<string, string> _agents = new()
    {
        { "Default", string.Empty },

        { "Chrome Windows",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
            "AppleWebKit/537.36 (KHTML, like Gecko) " +
            "Chrome/136.0.0.0 Safari/537.36" },

        { "Firefox",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:139.0) " +
            "Gecko/20100101 Firefox/139.0" },

        { "Edge",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
            "AppleWebKit/537.36 (KHTML, like Gecko) " +
            "Chrome/136.0.0.0 Safari/537.36 Edg/136.0.0.0" },

        { "Chrome Android",
            "Mozilla/5.0 (Linux; Android 14; Pixel 8 Pro) " +
            "AppleWebKit/537.36 (KHTML, like Gecko) " +
            "Chrome/136.0.0.0 Mobile Safari/537.36" },

        { "Safari macOS",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) " +
            "AppleWebKit/605.1.15 (KHTML, like Gecko) " +
            "Version/18.0 Safari/605.1.15" }
    };

    private static string _currentAgent = "Chrome Windows";

    public static List<string> GetAgents()
    {
        return new List<string>(_agents.Keys);
    }

    public static string GetCurrentAgentName()
    {
        return _currentAgent;
    }

    public static string GetCurrentAgentString()
    {
        return _agents.TryGetValue(_currentAgent, out var agent)
            ? agent
            : string.Empty;
    }

    public static void SetUserAgent(string agentName)
    {
        if (_agents.ContainsKey(agentName))
        {
            _currentAgent = agentName;
        }
    }

    public static void AddCustomAgent(string name, string userAgent)
    {
        if (string.IsNullOrWhiteSpace(name))
            return;

        _agents[name] = userAgent ?? string.Empty;
    }

    public static bool RemoveAgent(string name)
    {
        if (name == "Default" || name == "Chrome Windows")
            return false;

        if (_currentAgent == name)
            _currentAgent = "Chrome Windows";

        return _agents.Remove(name);
    }

    public static void ApplyTo(Microsoft.Web.WebView2.Wpf.WebView2 browser)
    {
        if (browser?.CoreWebView2 == null)
            return;

        browser.CoreWebView2.Settings.UserAgent =
            GetCurrentAgentString();
    }
}
