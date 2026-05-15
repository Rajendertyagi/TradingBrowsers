using Microsoft.Web.WebView2.Wpf;
            CurrentBrowser.GoBack();
    }

    private void Forward_Click(object sender, RoutedEventArgs e)
    {
        if (CurrentBrowser?.CanGoForward == true)
            CurrentBrowser.GoForward();
    }

    private void Refresh_Click(object sender, RoutedEventArgs e)
    {
        CurrentBrowser?.Reload();
    }

    private void Stop_Click(object sender, RoutedEventArgs e)
    {
        CurrentBrowser?.CoreWebView2?.Stop();
    }

    private void Home_Click(object sender, RoutedEventArgs e)
    {
        CurrentBrowser?.CoreWebView2?.Navigate(HomeUrl);
    }

    private async void NewTabButton_Click(object sender, RoutedEventArgs e)
    {
        await CreateNewTab(HomeUrl);
    }

    private void CloseTabButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is WebView2 browser)
        {
            var tab = GetTabForBrowser(browser);
            if (tab != null)
                Tabs.Items.Remove(tab);

            _browsers.Remove(browser);
            browser.Dispose();

            if (Tabs.Items.Count == 0)
                Close();
        }
    }

    private async void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        // Ctrl+T = New Tab
        if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.T)
        {
            await CreateNewTab(HomeUrl);
            e.Handled = true;
        }

        // Ctrl+W / Ctrl+F4 = Close Tab
        if (Keyboard.Modifiers == ModifierKeys.Control &&
            (e.Key == Key.W || e.Key == Key.F4))
        {
            if (Tabs.SelectedItem is TabItem item)
                Tabs.Items.Remove(item);

            if (Tabs.Items.Count == 0)
                Close();

            e.Handled = true;
        }

        // Ctrl+L = Focus Address Bar
        if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.L)
        {
            AddressBar.Focus();
            AddressBar.SelectAll();
            e.Handled = true;
        }

        // Refresh
        if ((Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.R) ||
            e.Key == Key.F5)
        {
            CurrentBrowser?.Reload();
            e.Handled = true;
        }

        // Stop loading
        if (e.Key == Key.Escape)
        {
            CurrentBrowser?.CoreWebView2?.Stop();
            e.Handled = true;
        }

        // Back / Forward
        if (Keyboard.Modifiers == ModifierKeys.Alt && e.Key == Key.Left)
        {
            if (CurrentBrowser?.CanGoBack == true)
                CurrentBrowser.GoBack();
            e.Handled = true;
        }

        if (Keyboard.Modifiers == ModifierKeys.Alt && e.Key == Key.Right)
    
