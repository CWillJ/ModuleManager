namespace LoadDLLs.Views
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
            this.InitializeComponent();
            this.DataContext = new LoadDLLs.ViewModels.LoadDLLsViewModel();
        }
    }
}
