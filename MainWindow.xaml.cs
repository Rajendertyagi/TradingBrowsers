using Microsoft.Web.WebView2.Wpf;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TradingBrowser;

public partial class MainWindow : Window
{
    private const string HomeUrl = "https://www.tradingview.com/";

    public MainWindow()
    {
        InitializeComponent();
        Loaded += async (_, _) => await CreateNewTab(HomeUrl);
    }

    private async Task CreateNewTab(string url)
    {
        BrowserHost.Children.Clear();
        var browser = new WebView2();
        BrowserHost.Children.Add(browser);
        await browser.EnsureCoreWebView2Async();
        UserAgentManager.ApplyTo(browser);
        browser.Source = new Uri(url);
        browser.NavigationCompleted += (_, _) =>
        {
            if (browser.Source != null)
                AddressBar.Text = browser.Source.ToString();
        };
    }

    private WebView2? CurrentBrowser =>
        BrowserHost.Children.Count > 0 ? BrowserHost.Children[0] as WebView2 : null;

    private void Navigate()
    {
        if (CurrentBrowser?.CoreWebView2 == null) return;
        var text = AddressBar.Text.Trim();
        if (string.IsNullOrWhiteSpace(text)) return;

        if (!text.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
            !text.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            text = text.Contains(".")
                ? "https://" + text
                : "https://www.google.com/search?q=" + Uri.EscapeDataString(text);
        }

        CurrentBrowser.CoreWebView2.Navigate(text);
    }

    private void AddressBar_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter) Navigate();
    }

    private void Back_Click(object sender, RoutedEventArgs e)
    {
        if (CurrentBrowser?.CanGoBack == true) CurrentBrowser.GoBack();
    }

    private void Forward_Click(object sender, RoutedEventArgs e)
    {
        if (CurrentBrowser?.CanGoForward == true) CurrentBrowser.GoForward();
    }

    private void Refresh_Click(object sender, RoutedEventArgs e) => CurrentBrowser?.Reload();
    private void Stop_Click(object sender, RoutedEventArgs e) => CurrentBrowser?.CoreWebView2?.Stop();
    private async void NewTabButton_Click(object sender, RoutedEventArgs e) => await CreateNewTab(HomeUrl);
    private void Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
}
