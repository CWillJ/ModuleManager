namespace ModuleManager.Behavoirs
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// BoolToVisibilityConverter converts a Visibility object to a bool value.
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts bool to Visibility.
        /// </summary>
        /// <param name="value">Object.</param>
        /// <param name="targetType">Target type.</param>
        /// <param name="parameter">Object parameter.</param>
        /// <param name="culture">Culture Info.</param>
        /// <returns>True if visibility is visible, false if collapsed.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
            {
                return Visibility.Collapsed;
            }

            bool objValue = (bool)value;

            if (objValue)
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        /// <summary>
        /// Converts Visibility to bool.
        /// </summary>
        /// <param name="value">Object to convert.</param>
        /// <param name="targetType">Target type.</param>
        /// <param name="parameter">Object parameter.</param>
        /// <param name="culture">Culture Info.</param>
        /// <returns>Visibility.Visible if true and Visibility.Collapsed if false.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((Visibility)value == Visibility.Visible)
            {
                return true;
            }

            return false;
        }
    }
}