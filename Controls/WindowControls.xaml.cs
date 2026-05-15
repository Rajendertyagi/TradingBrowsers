using System.Windows;
using System.Windows.Controls;
namespace TradingBrowser.Controls;
public partial class WindowControls : UserControl
{
    public WindowControls()
    {
        InitializeComponent();
        MinimizeButton.Click += (_, _) => Window.GetWindow(this)!.WindowState = WindowState.Minimized;
        MaximizeButton.Click += (_, _) => {
            var w = Window.GetWindow(this)!;
            w.WindowState = w.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        };
        CloseButton.Click += (_, _) => Window.GetWindow(this)!.Close();
    }
}