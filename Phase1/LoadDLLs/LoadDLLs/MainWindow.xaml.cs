using System.Windows;

namespace LoadDLLs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new LoadDLLs.ViewModels.LoadDLLsViewModel();
        }
    }
}
