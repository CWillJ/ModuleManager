namespace ModuleManager.UI.TemplateSelectors
{
    using System.Windows;
    using System.Windows.Controls;
    using ModuleManager.ModuleObjects.Classes;

    /// <summary>
    /// Used to selected a DataTemplate in the description.
    /// </summary>
    public class DescriptionTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// Used to select the DataTemplate for the description.
        /// </summary>
        /// <param name="item">A <see cref="object"/> sent in.</param>
        /// <param name="container">The <see cref="DependencyObject"/>.</param>
        /// <returns>A <see cref="DataTemplate"/> depending on the object passed in.</returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (container is FrameworkElement element && container != null && item != null)
            {
                if (item is AssemblyData)
                {
                    return element.FindResource("assemblyTemplate") as DataTemplate;
                }

                if (item is ModuleData)
                {
                    return element.FindResource("moduleTemplate") as DataTemplate;
                }

                if (item is ModuleConstructor)
                {
                    return element.FindResource("moduleConstructorTemplate") as DataTemplate;
                }

                if (item is ModuleProperty)
                {
                    return element.FindResource("modulePropertyTemplate") as DataTemplate;
                }

                if (item is ModuleMethod)
                {
                    return element.FindResource("moduleMethodTemplate") as DataTemplate;
                }

                if (item is MemberParameter)
                {
                    return element.FindResource("memberParameterTemplate") as DataTemplate;
                }
            }

            return null;
        }
    }
}