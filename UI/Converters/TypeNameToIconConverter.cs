namespace ModuleManager.UI.Converters
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Windows.Data;

    /// <summary>
    /// Converter to convert the string name of a member type to an icon.
    /// </summary>
    public class TypeNameToIconConverter : IValueConverter
    {
        /// <summary>
        /// Convery string name of a member type to an icon.
        /// </summary>
        /// <param name="value">Object passed in.</param>
        /// <param name="targetType">Target type.</param>
        /// <param name="parameter">Parameter object.</param>
        /// <param name="culture">Culture.</param>
        /// <returns>String path to an image icon.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string iconDirectory = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName).FullName + @"\Images\";

            return value.ToString() switch
            {
                @"ModuleConstructor" => iconDirectory + @"Constructor Icon.png",
                @"ModuleProperty" => iconDirectory + @"Property Icon.png",
                @"ModuleMethod" => iconDirectory + @"Method Icon.png",
                _ => null
            };
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
