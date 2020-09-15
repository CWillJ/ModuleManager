namespace ModuleManager.TemplateSelectors
{
    using System.Windows;
    using System.Windows.Controls;
    using ModuleObjects.Classes;

    /// <summary>
    /// Used to selected a DataTemplate in the view.
    /// </summary>
    public class TreeViewItemTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// Used to select the DataTemplate for tree view items.
        /// </summary>
        /// <param name="item">Object.</param>
        /// <param name="container">Container.</param>
        /// <returns>A DataTemplate depending on the object passed in.</returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (container is FrameworkElement element && container != null && item != null)
            {
                if (item.ToString().Equals("ModuleConstructor"))
                {
                    return element.FindResource("moduleMemberTreeItemConstructor") as DataTemplate;
                }
                else if (item.ToString().Equals("ModuleProperty"))
                {
                    return element.FindResource("moduleMemberTreeItemProperty") as DataTemplate;
                }
                else if (item.ToString().Equals("ModuleMethod"))
                {
                    return element.FindResource("moduleMemberTreeItemMethod") as DataTemplate;
                }
            }

            return null;
        }
    }
}