namespace ModuleManager
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Overrides the OnStartup method.
        /// </summary>
        /// <param name="e">StartupEvenArgs.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            new MainWindow().Show();
            base.OnStartup(e);
        }
    }
}