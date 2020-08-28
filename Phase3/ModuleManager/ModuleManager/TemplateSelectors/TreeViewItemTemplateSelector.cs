namespace ModuleManager.TemplateSelectors
{
    using System.Windows;
    using System.Windows.Controls;
    using ModuleObjects;
    using Unity.Injection;

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
                ModuleConstructor mc = (ModuleConstructor)item;
                ModuleProperty mp = (ModuleProperty)item;
                ModuleMethod mm = (ModuleMethod)item;

                if (mc.Name.Contains("Constructor"))
                {
                    return element.FindResource("moduleMemberTreeItemConstructor") as DataTemplate;
                }

                if (mp.DataType != null)
                {
                    return element.FindResource("moduleMemberTreeItemProperty") as DataTemplate;
                }

                if (!string.IsNullOrEmpty(mm.ReturnType))
                {
                    return element.FindResource("moduleMemberTreeItemMethod") as DataTemplate;
                }
            }

            return null;
        }
    }
}