namespace ModuleManager.Behavoirs
{
    using System.Windows;
    using System.Windows.Controls;
    using ModuleManager.Classes;

    /// <summary>
    /// Used to selected a DataTemplate in the view.
    /// </summary>
    public class TreeViewItemSelector : DataTemplateSelector
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
                if (item is ModuleConstructor)
                {
                    return element.FindResource("constructorTreeViewItem") as DataTemplate;
                }

                if (item is ModuleProperty)
                {
                    return element.FindResource("propertyTreeViewItem") as DataTemplate;
                }

                if (item is ModuleMethod)
                {
                    return element.FindResource("methodTreeViewItem") as DataTemplate;
                }
            }

            return null;
        }
    }
}
