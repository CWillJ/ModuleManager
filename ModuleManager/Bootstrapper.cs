namespace ModuleManager
{
    using System.Windows;
    using ModuleManager.Views;
    using Prism.Unity;

    /// <summary>
    /// A bootstrapper for ModuleManager application.
    /// </summary>
    public class Bootstrapper : UnityBootstrapper
    {
        /// <summary>
        /// Overrides the CreateShell method.
        /// </summary>
        /// <returns>Returns the resolved shell.</returns>
        protected override DependencyObject CreateShell()
        {
            return Container.TryResolve<ShellView>();
        }

        /// <summary>
        /// Overrides the InitializeShell method.
        /// </summary>
        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }
    }
}
