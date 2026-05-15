using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TradingBrowser.Services;

public class TabManager
{
    private readonly Grid _browserHost;
    private readonly Action<string> _updateAddressBar;

    private readonly List<WebView2> _tabs = new();

    public int CurrentTabIndex { get; private set; } = -1;

    public TabManager(Grid browserHost, Action<string> updateAddressBar)
    {
        _browserHost = browserHost;
        _updateAddressBar = updateAddressBar;
    }

    public WebView2? CurrentBrowser =>
        CurrentTabIndex >= 0 && CurrentTabIndex < _tabs.Count
            ? _tabs[CurrentTabIndex]
            : null;

    public int TabCount => _tabs.Count;

    public async Task CreateTabAsync(string url)
    {
        var browser = new WebView2();

        _browserHost.Children.Add(browser);

        await browser.EnsureCoreWebView2Async();

        browser.CoreWebView2.Settings.UserAgent =
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
            "AppleWebKit/537.36 (KHTML, like Gecko) " +
            "Chrome/136.0.0.0 Safari/537.36";

        browser.NavigationCompleted += (_, _) =>
        {
            if (CurrentBrowser == browser && browser.Source != null)
            {
                _updateAddressBar(browser.Source.ToString());
            }
        };

        browser.Visibility = System.Windows.Visibility.Collapsed;

        _tabs.Add(browser);

        ShowTab(_tabs.Count - 1);

        browser.Source = new Uri(url);
    }

    public void ShowTab(int index)
    {
        if (index < 0 || index >= _tabs.Count)
            return;

        for (int i = 0; i < _tabs.Count; i++)
        {
            _tabs[i].Visibility =
                i == index
                    ? System.Windows.Visibility.Visible
                    : System.Windows.Visibility.Collapsed;
        }

        CurrentTabIndex = index;

        if (CurrentBrowser?.Source != null)
        {
            _updateAddressBar(CurrentBrowser.Source.ToString());
        }
    }

    public void Navigate(string url)
    {
        CurrentBrowser?.CoreWebView2?.Navigate(url);
    }

    public void Back()
    {
        if (CurrentBrowser?.CanGoBack == true)
            CurrentBrowser.GoBack();
    }

    public void Forward()
    {
        if (CurrentBrowser?.CanGoForward == true)
            CurrentBrowser.GoForward();
    }

    public void Refresh()
    {
        CurrentBrowser?.Reload();
    }
}
