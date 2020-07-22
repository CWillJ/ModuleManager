namespace ModuleManager.Views
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for MemberDescription.xaml
    /// </summary>
    public partial class MemberDescription : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberDescription"/> class.
        /// </summary>
        public MemberDescription()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Bad bad bad.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">e.</param>
        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue != null)
            {
                MemberText.ItemsSource = new ObservableCollection<object> { e.NewValue };
            }
        }
    }
}