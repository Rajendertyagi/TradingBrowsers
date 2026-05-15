using System.Windows.Media;

namespace TradingBrowser;

public static class ThemeManager
{
    public static SolidColorBrush WindowBackground => Brush("#202124");
    public static SolidColorBrush ToolbarBackground => Brush("#292A2D");
    public static SolidColorBrush OmniboxBackground => Brush("#303134");
    public static SolidColorBrush BorderBrush => Brush("#3C4043");
    public static SolidColorBrush ActiveTabBackground => Brush("#1F1F1F");
    public static SolidColorBrush InactiveTabBackground => Brush("#2B2B2B");
    public static SolidColorBrush TextBrush => Brush("#E8EAED");

    private static SolidColorBrush Brush(string hex)
    {
        return new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex)!);
    }
}
