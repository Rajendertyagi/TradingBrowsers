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

    private void MainWindow_Loaded(object? sender, RoutedEventArgs e)
    {
        AddressBar.Text = HomeUrl;
    }

    private void Navigate()
    {
        // Placeholder only.
        // Real browser engine can be added later.
        if (string.IsNullOrWhiteSpace(AddressBar.Text))
            AddressBar.Text = HomeUrl;
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
        // Placeholder
    }

    private void Forward_Click(object sender, RoutedEventArgs e)
    {
        // Placeholder
    }

    private void Refresh_Click(object sender, RoutedEventArgs e)
    {
        // Placeholder
    }

    private void Home_Click(object sender, RoutedEventArgs e)
    {
        AddressBar.Text = HomeUrl;
    }

    private void NewTabButton_Click(object sender, RoutedEventArgs e)
    {
        AddressBar.Text = HomeUrl;
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
