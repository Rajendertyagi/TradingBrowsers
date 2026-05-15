using System;
using System.Windows.Controls;

namespace TradingBrowser.Controls;

public partial class TabButton : UserControl
{
    public event EventHandler? CloseRequested;

    public TabButton()
    {
        InitializeComponent();
        CloseButton.Click += (_, _) => CloseRequested?.Invoke(this, EventArgs.Empty);
    }

    public string Title
    {
        get => TitleText.Text;
        set => TitleText.Text = value;
    }
}
