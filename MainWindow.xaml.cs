using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TradingBrowser;

public partial class MainWindow : Window
{
    private const string HomeUrl = "https://www.tradingview.com/";

    // Stores all open tabs and their WebView2 controls
    private readonly List<WebView2> _tabs = new();

    // Index of the currently selected tab
    private int _currentTabIndex = -1;

    public MainWindow()
    {
        InitializeComponent();
        Loaded += MainWindow_Loaded;
    }

    private async void MainWindow_Loaded(object? sender, RoutedEventArgs e)
    {
        if (_tabs.Count == 0)
            await CreateNewTabAsync(HomeUrl);
    }

    // Returns the currently active browser
    private WebView2? Browser =>
        (_currentTabIndex >= 0 && _currentTabIndex < _tabs.Count)
            ? _tabs[_currentTabIndex]
            : null;

    // Creates a real new tab
    private async Task CreateNewTabAsync(string url)
    {
        var browser = new WebView2();

        await browser.EnsureCoreWebView2Async();

        // Chrome-like user agent
        browser.CoreWebView2.Settings.UserAgent =
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
            "AppleWebKit/537.36 (KHTML, like Gecko) " +
            "Chrome/136.0.0.0 Safari/537.36";

        browser.NavigationCompleted += (_, _) =>
        {
            if (Browser == browser && browser.Source != null)
                AddressBar.Text = browser.Source.ToString();
        };

        _tabs.Add(browser);
        _currentTabIndex = _tabs.Count - 1;

        // Show only the active tab
        BrowserHost.Children.Clear();
        BrowserHost.Children.Add(browser);

        browser.Source = new Uri(url);
    }

    // Switches to an existing tab
    private void ShowTab(int index)
    {
        if (index < 0 || index >= _tabs.Count)
            return;

        _currentTabIndex = index;

        BrowserHost.Children.Clear();
        BrowserHost.Children.Add(_tabs[index]);

        if (_tabs[index].Source != null)
            AddressBar.Text = _tabs[index].Source.ToString();
    }

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

    // Clicking + creates a real new tab in the same window
    private async void NewTabButton_Click(object sender, RoutedEventArgs e)
    {
        await CreateNewTabAsync(HomeUrl);
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
