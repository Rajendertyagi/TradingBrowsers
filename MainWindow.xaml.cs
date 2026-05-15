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
        Loaded += MainWindow_Loaded;
    }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        await CreateNewTab(HomeUrl);
    }

    private async Task CreateNewTab(string url)
    {
        BrowserHost.Children.Clear();

        var browser = new WebView2();

        BrowserHost.Children.Add(browser);

        await browser.EnsureCoreWebView2Async();

        // Apply selected user agent
        UserAgentManager.ApplyTo(browser);

        browser.Source = new Uri(url);

        browser.NavigationCompleted += (s, e) =>
        {
            if (browser.Source != null)
                AddressBar.Text = browser.Source.ToString();
        };
    }

    private WebView2? GetCurrentBrowser()
    {
        if (BrowserHost.Children.Count == 0)
            return null;

        return BrowserHost.Children[0] as WebView2;
    }

    private void Navigate()
    {
        var browser = GetCurrentBrowser();

        if (browser?.CoreWebView2 == null)
            return;

        string text = AddressBar.Text.Trim();

        if (string.IsNullOrWhiteSpace(text))
            return;

        if (!text.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
            !text.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            if (text.Contains("."))
            {
                text = "https://" + text;
            }
            else
            {
                text = "https://www.google.com/search?q=" +
                       Uri.EscapeDataString(text);
            }
        }

        browser.CoreWebView2.Navigate(text);
    }

    private void Go_Click(object sender, RoutedEventArgs e)
    {
        Navigate();
    }

    private void AddressBar_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            Navigate();
            e.Handled = true;
        }
    }

    private void Back_Click(object sender, RoutedEventArgs e)
    {
        var browser = GetCurrentBrowser();

        if (browser?.CanGoBack == true)
            browser.GoBack();
    }

    private void Forward_Click(object sender, RoutedEventArgs e)
    {
        var browser = GetCurrentBrowser();

        if (browser?.CanGoForward == true)
            browser.GoForward();
    }

    private void Refresh_Click(object sender, RoutedEventArgs e)
    {
        GetCurrentBrowser()?.Reload();
    }

    private void Stop_Click(object sender, RoutedEventArgs e)
    {
        GetCurrentBrowser()?.CoreWebView2?.Stop();
    }

    private void Home_Click(object sender, RoutedEventArgs e)
    {
        GetCurrentBrowser()?.CoreWebView2?.Navigate(HomeUrl);
    }

    private async void NewTabButton_Click(object sender, RoutedEventArgs e)
    {
        await CreateNewTab(HomeUrl);
    }

    private void Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Placeholder handler required by MainWindow.xaml.
        // Multi-tab switching logic can be added here later.
    }
}
