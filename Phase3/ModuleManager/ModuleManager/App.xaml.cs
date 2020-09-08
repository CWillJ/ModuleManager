namespace ModuleManager
{
    using System.Windows;
    using ModuleObjects.Classes;
    using ModuleObjects.Interfaces;
    using ModuleRetriever;
    using ModuleRetriever.Interfaces;
    using Prism.Ioc;
    using Prism.Modularity;
    using Prism.Regions;
    using Prism.Unity;

    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : PrismApplication
    {
        /// <summary>
        /// Creates the shell or main window of the application.
        /// </summary>
        /// <returns>The shell of the application.</returns>
        protected override Window CreateShell()
        {
            // because RadWindow is not Window, cannot create here.
            // https://github.com/PrismLibrary/Prism/issues/1413
            return null;
        }

        /// <summary>
        /// Overrides the OnStartup method.
        /// </summary>
        /// <param name="e">StartupEvenArgs.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            ////new MainWindow().Show();
            base.OnStartup(e);
        }

        /// <summary>
        /// Used to register types with the container that will be used by the application.
        /// </summary>
        /// <param name="containerRegistry"><see cref="IContainerRegistry"/> used for container type registration.</param>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IModuleInfoRetriever, ModuleInfoRetriever>();

            containerRegistry.Register<IModuleMember, ModuleConstructor>();
            containerRegistry.Register<IModuleMember, ModuleProperty>();
            containerRegistry.Register<IModuleMember, ModuleMethod>();
        }
    }
}