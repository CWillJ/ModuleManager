namespace ModuleManager
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModels.ModuleManagerViewModel();
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
                MemberText.Text = e.NewValue.ToString();
            }
        }
    }
}