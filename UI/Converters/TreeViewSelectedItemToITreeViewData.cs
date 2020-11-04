namespace ModuleManager.UI.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using ModuleManager.ModuleObjects.Interfaces;

    /// <summary>
    /// Used to convert TreeViewSelectedItems to ITreeViewData.
    /// </summary>
    public class TreeViewSelectedItemToITreeViewData : IValueConverter
    {
        /// <summary>
        /// Convert a TreeViewSelectedItem to ITreeViewData.
        /// </summary>
        /// <param name="value">Object passed in.</param>
        /// <param name="targetType">Target type.</param>
        /// <param name="parameter">Parameter object.</param>
        /// <param name="culture">Culture.</param>
        /// <returns>An <see cref="ITreeViewData"/> object.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ////return (ITreeViewData)value;
            return value;
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="value">Object passed in.</param>
        /// <param name="targetType">Target type.</param>
        /// <param name="parameter">Parameter object.</param>
        /// <param name="culture">Culture.</param>
        /// <returns>Nothing.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}