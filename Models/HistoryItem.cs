namespace TradingBrowser.Models;

public class HistoryItem
{
    public string Title { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public System.DateTime VisitedAt { get; set; } = System.DateTime.Now;
}
