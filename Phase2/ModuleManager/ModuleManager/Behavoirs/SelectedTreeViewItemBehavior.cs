namespace ModuleManager.Behavoirs
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// SelectedTreeViewItemBehavior class used to allow selected
    /// items for TreeView controls.
    /// </summary>
    public class SelectedTreeViewItemBehavior
    {
        /// <summary>
        /// Declare our attached property, it needs to be a DependencyProperty so
        /// we can bind to it from our ViewMode.
        /// </summary>
        public static readonly DependencyProperty TreeViewSelectedItemProperty =
            DependencyProperty.RegisterAttached(
            "TreeViewSelectedItem",
            typeof(object),
            typeof(SelectedTreeViewItemBehavior),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(TreeViewSelectedItemChanged)));

        /// <summary>
        /// We need a Get method for our new property.
        /// </summary>
        /// <param name="dependencyObject">DependencyObject.</param>
        /// <returns>Object.</returns>
        public static object GetTreeViewSelectedItem(DependencyObject dependencyObject)
        {
            return (object)dependencyObject.GetValue(TreeViewSelectedItemProperty);
        }

        /// <summary>
        /// As well as a Set method for our new property.
        /// </summary>
        /// <param name="dependencyObject">DependencyObject.</param>
        /// <param name="value">Object.</param>
        public static void SetTreeViewSelectedItem(
          DependencyObject dependencyObject, object value)
        {
            dependencyObject.SetValue(TreeViewSelectedItemProperty, value);
        }

        /// <summary>
        /// This is the handler for when our new property's value changes
        /// When our property is set to a non null value we need to add an event handler
        /// for the TreeView's SelectedItemChanged event.
        /// </summary>
        /// <param name="dependencyObject">DependencyObject.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs.</param>
        private static void TreeViewSelectedItemChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            TreeView tv = dependencyObject as TreeView;

            if (e.NewValue == null && e.OldValue != null)
            {
                tv.SelectedItemChanged -=
                    new RoutedPropertyChangedEventHandler<object>(TV_SelectedItemChanged);
            }
            else if (e.NewValue != null && e.OldValue == null)
            {
                tv.SelectedItemChanged +=
                    new RoutedPropertyChangedEventHandler<object>(TV_SelectedItemChanged);
            }
        }

        // When TreeView.SelectedItemChanged fires, set our new property to the value
        private static void TV_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SetTreeViewSelectedItem((DependencyObject)sender, e.NewValue);
        }
    }
}
