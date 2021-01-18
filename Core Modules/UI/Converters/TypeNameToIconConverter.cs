namespace ModuleManager.Core.UI.Converters
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
        /// <param name="value"><see cref="object"/> passed in.</param>
        /// <param name="targetType">Target <see cref="Type"/>.</param>
        /// <param name="parameter">Parameter <see cref="object"/>.</param>
        /// <param name="culture"><see cref="CultureInfo"/>.</param>
        /// <returns><see cref="string"/> path to an image icon.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string iconDirectory = Path.Combine(
                Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(
                    Directory.GetCurrentDirectory()).FullName).FullName).FullName).FullName).FullName, @"Core Modules\UI\Icons");

            return value.ToString() switch
            {
                @"TypeConstructor" => Path.Combine(iconDirectory, @"Constructor Icon.png"),
                @"TypeProperty" => Path.Combine(iconDirectory, @"Property Icon.png"),
                @"TypeMethod" => Path.Combine(iconDirectory, @"Method Icon.png"),
                _ => null
            };
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="value"><see cref="object"/> passed in.</param>
        /// <param name="targetType">Target <see cref="Type"/>.</param>
        /// <param name="parameter">Parameter <see cref="object"/>.</param>
        /// <param name="culture"><see cref="CultureInfo"/>.</param>
        /// <returns>Nothing, this is unused.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
