namespace ModuleManager.Views
{
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for ModuleView.xaml.
    /// </summary>
    public partial class MethodView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodView"/> class.
        /// </summary>
        public MethodView()
        {
            InitializeComponent();
            DataContext = new ViewModels.ModuleManagerViewModel();
        }
    }
}