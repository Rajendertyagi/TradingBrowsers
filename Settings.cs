using System.Collections.Generic;

namespace TradingBrowser;

public class Settings
{
    public string HomeUrl { get; set; } = "https://www.tradingview.com/";
    public bool RestoreSession { get; set; } = true;
    public bool AdBlockEnabled { get; set; } = true;
    public int MaxSessionTabs { get; set; } = 4;
    public List<BrokerLink> BrokerLinks { get; set; } = new();

    public Settings()
    {
        BrokerLinks.Add(new BrokerLink("Dhan", "https://web.dhan.co/"));
        BrokerLinks.Add(new BrokerLink("Zerodha", "https://kite.zerodha.com/"));
        BrokerLinks.Add(new BrokerLink("TradingView", "https://www.tradingview.com/"));
    }
}

public record BrokerLink(string Name, string Url);
