using Microsoft.Web.WebView2.Wpf;
using System;
using System.Threading.Tasks;
using System.Windows;
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

    private async void MainWindow_Loaded(object? sender, RoutedEventArgs e)
    {
        await InitializeBrowserAsync();
    }

    private async Task InitializeBrowserAsync()
    {
        BrowserHost.Children.Clear();

        var browser = new WebView2();
        BrowserHost.Children.Add(browser);

        await browser.EnsureCoreWebView2Async();

        // Chrome-like user agent
        browser.CoreWebView2.Settings.UserAgent =
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
            "AppleWebKit/537.36 (KHTML, like Gecko) " +
            "Chrome/136.0.0.0 Safari/537.36";

        browser.Source = new Uri(HomeUrl);

        browser.NavigationCompleted += (_, _) =>
        {
            if (browser.Source != null)
                AddressBar.Text = browser.Source.ToString();
        };
    }

    private WebView2? Browser =>
        BrowserHost.Children.Count > 0
            ? BrowserHost.Children[0] as WebView2
            : null;

    private void Navigate()
    {
        if (Browser?.CoreWebView2 == null)
            return;

        string text = AddressBar.Text.Trim();

        if (string.IsNullOrWhiteSpace(text))
            return;

        if (!text.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
            !text.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            text = text.Contains(".")
                ? "https://" + text
                : "https://www.google.com/search?q=" +
                  Uri.EscapeDataString(text);
        }

        Browser.CoreWebView2.Navigate(text);
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
        if (Browser?.CanGoBack == true)
            Browser.GoBack();
    }

    private void Forward_Click(object sender, RoutedEventArgs e)
    {
        if (Browser?.CanGoForward == true)
            Browser.GoForward();
    }

    private void Refresh_Click(object sender, RoutedEventArgs e)
    {
        Browser?.Reload();
    }

    private void Home_Click(object sender, RoutedEventArgs e)
    {
        Browser?.CoreWebView2?.Navigate(HomeUrl);
    }

    // Clicking + opens a new independent browser window
    private void NewTabButton_Click(object sender, RoutedEventArgs e)
    {
        var newWindow = new MainWindow();
        newWindow.Show();
    }

    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount == 2)
        {
            WindowState = WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;
        }
        else
        {
            DragMove();
        }
    }
}
