namespace ModuleManager.Behavoirs
{
    using System.Windows;
    using System.Windows.Controls;
    using ModuleManager.DataObjects;

    /// <summary>
    /// Used to selected a DataTemplate in the view.
    /// </summary>
    public class TemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// Used to select the DataTemplate used in the view.
        /// </summary>
        /// <param name="item">Object.</param>
        /// <param name="container">Container.</param>
        /// <returns>A DataTemplate depending on the object passed in.</returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (container is FrameworkElement element && container != null && item != null)
            {
                if (item is Module)
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