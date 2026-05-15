using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TradingBrowser.Services;

namespace TradingBrowser;

public partial class MainWindow : Window
{
    private const string HomeUrl = "https://www.tradingview.com/";

    private TabManager? _tabManager;

    public MainWindow()
    {
        InitializeComponent();
        Loaded += MainWindow_Loaded;
    }

    private async void MainWindow_Loaded(object? sender, RoutedEventArgs e)
    {
        // Create the tab manager after the XAML controls are initialized.
        _tabManager = new TabManager(
            BrowserHost,
            url => AddressBar.Text = url
        );

        // Open the first tab.
        await _tabManager.CreateTabAsync(HomeUrl);
    }

    private void Navigate()
    {
        if (_tabManager == null)
            return;

        string text = AddressBar.Text.Trim();

        if (string.IsNullOrWhiteSpace(text))
            return;

        // If the text is not already a URL, treat it as either a domain or a search query.
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

        _tabManager.Navigate(text);
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
        _tabManager?.Back();
    }

    private void Forward_Click(object sender, RoutedEventArgs e)
    {
        _tabManager?.Forward();
    }

    private void Refresh_Click(object sender, RoutedEventArgs e)
    {
        _tabManager?.Refresh();
    }

    private void Home_Click(object sender, RoutedEventArgs e)
    {
        _tabManager?.Navigate(HomeUrl);
    }

    // Opens a true new tab in the same window.
    private async void NewTabButton_Click(object sender, RoutedEventArgs e)
    {
        if (_tabManager == null)
            return;

        await _tabManager.CreateTabAsync(HomeUrl);
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
