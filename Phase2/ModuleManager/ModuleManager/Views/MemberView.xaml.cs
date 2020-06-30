namespace ModuleManager.Views
{
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for ModuleView.xaml.
    /// </summary>
    public partial class MemberView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberView"/> class.
        /// </summary>
        public MemberView()
        {
            InitializeComponent();
            DataContext = new ViewModels.ModuleManagerViewModel();
        }
    }
}