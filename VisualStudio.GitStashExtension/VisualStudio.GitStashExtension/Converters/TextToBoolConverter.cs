using System;
using System.Globalization;
using System.Windows.Data;

namespace VisualStudio.GitStashExtension.Converters
{
    /// <summary>
    /// Converts text (string) value to bool, ensures that string value is not empty.
    /// </summary>
    public class TextToBoolConverter : IValueConverter
    {
        /// <summary>
        /// Ensures that string value is not empty and returns true if text is empty otherwise - false.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = value.ToString();

            return string.IsNullOrEmpty(text);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
