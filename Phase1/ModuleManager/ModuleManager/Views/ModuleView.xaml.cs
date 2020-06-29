namespace ModuleManager.Views
{
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for MethodView.xaml.
    /// </summary>
    public partial class ModuleView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleView"/> class.
        /// </summary>
        public ModuleView()
        {
            InitializeComponent();
            DataContext = new ViewModels.ModuleManagerViewModel();
        }
    }
}