using System.Drawing;

namespace FuturisticAntivirus
{
    #region UI Theming
    /// <summary>
    /// Enumeration for UI themes.
    /// </summary>
    public enum Theme { Light, Dark }

    /// <summary>
    /// Provides color palettes for different UI themes.
    /// </summary>
    public static class ThemeColors
    {
        // Dark Theme Colors
        public static Color DarkBackground = Color.FromArgb(21, 23, 30);
        public static Color DarkSurface = Color.FromArgb(31, 34, 44);
        public static Color DarkPrimaryText = Color.FromArgb(230, 230, 230);
        public static Color DarkSecondaryText = Color.FromArgb(160, 160, 160);
        public static Color DarkAccent = Color.FromArgb(0, 122, 204);
        public static Color DarkControlBorder = Color.FromArgb(80, 80, 80);
        public static Color DarkSuccess = Color.FromArgb(40, 167, 69);
        public static Color DarkDanger = Color.FromArgb(220, 53, 69);

        // Light Theme Colors
        public static Color LightBackground = Color.FromArgb(245, 245, 245);
        public static Color LightSurface = Color.FromArgb(255, 255, 255);
        public static Color LightPrimaryText = Color.FromArgb(33, 33, 33);
        public static Color LightSecondaryText = Color.FromArgb(117, 117, 117);
        public static Color LightAccent = Color.FromArgb(0, 122, 204);
        public static Color LightControlBorder = Color.FromArgb(200, 200, 200);
        public static Color LightSuccess = Color.FromArgb(223, 240, 216);
        public static Color LightDanger = Color.FromArgb(242, 222, 222);
    }
    #endregion
}
