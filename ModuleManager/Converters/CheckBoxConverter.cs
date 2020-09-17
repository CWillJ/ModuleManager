namespace ModuleManager.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// CheckBoxConverter is used to tell if a module is enabled or disabled.
    /// </summary>
    public class CheckBoxConverter : IMultiValueConverter
    {
        /// <summary>
        /// CheckBox converter.
        /// </summary>
        /// <param name="values">CheckBox values.</param>
        /// <param name="targetType">Target type of values.</param>
        /// <param name="parameter">Parameters.</param>
        /// <param name="culture">CultureInfo.</param>
        /// <returns>A clone of the object array values.</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Clone();
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="value">CheckBox values.</param>
        /// <param name="targetTypes">Target type of values.</param>
        /// <param name="parameter">Parameters.</param>
        /// <param name="culture">CultureInfo.</param>
        /// <returns>Not implenemted.</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}