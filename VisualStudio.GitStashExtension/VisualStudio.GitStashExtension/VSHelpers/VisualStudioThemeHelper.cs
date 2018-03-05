using System.Drawing;
using Microsoft.VisualStudio.PlatformUI;
using VisualStudio.GitStashExtension.Models;

namespace VisualStudio.GitStashExtension.VSHelpers
{
    /// <summary>
    /// Helper class for getting visual studio theme color.
    /// </summary>
    public class VisualStudioThemeHelper
    {
        /// <summary>
        /// Gets VS theme.
        /// </summary>
        public static VsColorTheme GetCurrentTheme()
        {
            var color = VSColorTheme.GetThemedColor(EnvironmentColors.AccentMediumColorKey);
            var col = Color.FromArgb(color.A, color.R, color.G, color.B);

            if (col == Constants.BlueThemeColor)
                return VsColorTheme.Blue;
            if (col == Constants.LightThemeColor)
                return VsColorTheme.Light;
            if (col == Constants.DarkThemeColor)
                return VsColorTheme.Dark;

            var colorBrightness = color.GetBrightness();
            var isDark = colorBrightness < 0.5;
            return isDark ? VsColorTheme.Dark : VsColorTheme.Light;
        }
    }
}
