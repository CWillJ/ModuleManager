namespace ModuleManager.Behavoirs
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interactivity;

    /// <summary>
    /// SelectedTreeViewItemBehavior class used to allow selected
    /// items for TreeView controls. TODO make this v Work.
    /// </summary>
    public class SelectedTreeViewItemBehavior : Behavior<TreeView>
    {
        /// <summary>
        /// SelectedItemProperty.
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(
                "SelectedItem",
                typeof(object),
                typeof(SelectedTreeViewItemBehavior),
                new UIPropertyMetadata(null, OnSelectedItemChanged));

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        /// <summary>
        /// OnAttached.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.SelectedItemChanged += OnTreeViewSelectedItemChanged;
        }

        /// <summary>
        /// OnDetaching.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject != null)
            {
                AssociatedObject.SelectedItemChanged -= OnTreeViewSelectedItemChanged;
            }
        }

        private static void OnSelectedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is TreeViewItem item)
            {
                item.SetValue(TreeViewItem.IsSelectedProperty, true);
            }
        }

        private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedItem = e.NewValue;
        }
    }
}
