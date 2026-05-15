namespace TradingBrowser;

public static class UserAgentManager
{
    public static void ApplyTo(Microsoft.Web.WebView2.Wpf.WebView2 browser)
    {
        if (browser?.CoreWebView2 != null)
            browser.CoreWebView2.Settings.UserAgent =
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/136.0.0.0 Safari/537.36";
    }
}
