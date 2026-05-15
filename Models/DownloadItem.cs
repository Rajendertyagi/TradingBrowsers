namespace TradingBrowser.Models;

public class DownloadItem
{
    public string FileName { get; set; } = string.Empty;
    public string SourceUrl { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public long BytesReceived { get; set; }
    public long TotalBytes { get; set; }
    public double ProgressPercent =>
        TotalBytes > 0 ? (double)BytesReceived / TotalBytes * 100.0 : 0.0;
    public string Status { get; set; } = "Pending";
    public System.DateTime StartedAt { get; set; } = System.DateTime.Now;
}
