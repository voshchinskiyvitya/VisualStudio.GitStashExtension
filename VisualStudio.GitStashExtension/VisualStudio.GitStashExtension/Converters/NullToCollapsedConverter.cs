using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace VisualStudio.GitStashExtension.Converters
{
    /// <summary>
    /// Converts object value to Visibility, ensures that value is not null.
    /// </summary>
    public class NullToCollapsedConverter: IValueConverter
    {
        /// <summary>
        /// Ensures that object value is not null and returns Visible if not null otherwise - Collapsed.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
