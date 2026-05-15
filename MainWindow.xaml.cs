using Microsoft.Web.WebView2.Core;
using System;
using System.Windows;
using System.Windows.Input;

namespace TradingBrowser;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Loaded += MainWindow_Loaded;
    }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        await Browser.EnsureCoreWebView2Async();

        Browser.Source = new Uri("https://www.google.com");
        AddressBar.Text = Browser.Source.ToString();

        Browser.NavigationCompleted += (_, _) =>
        {
            if (Browser.Source != null)
                AddressBar.Text = Browser.Source.ToString();
        };
    }

    private void Navigate()
    {
        var text = AddressBar.Text.Trim();

        if (string.IsNullOrWhiteSpace(text))
            return;

        if (!text.StartsWith("http://") && !text.StartsWith("https://"))
        {
            if (text.Contains('.'))
                text = "https://" + text;
            else
                text = "https://www.google.com/search?q=" +
                       Uri.EscapeDataString(text);
        }

        Browser.CoreWebView2.Navigate(text);
    }

    private void Go_Click(object sender, RoutedEventArgs e) => Navigate();

    private void AddressBar_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
            Navigate();
    }

    private void Back_Click(object sender, RoutedEventArgs e)
    {
        if (Browser.CanGoBack)
            Browser.GoBack();
    }

    private void Forward_Click(object sender, RoutedEventArgs e)
    {
        if (Browser.CanGoForward)
            Browser.GoForward();
    }

    private void Refresh_Click(object sender, RoutedEventArgs e)
    {
        Browser.Reload();
    }
}
