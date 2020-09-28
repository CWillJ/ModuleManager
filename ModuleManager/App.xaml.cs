namespace ModuleManager
{
    using System;
    using System.Windows;
    using ModuleManager.Views;
    using ModuleRetriever;
    using ModuleRetriever.Interfaces;
    using Prism.Ioc;
    using Prism.Modularity;
    using Prism.Regions;
    using Prism.Unity;
    using Telerik.Windows.Controls;

    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : PrismApplication
    {
        /// <summary>
        /// Startup logic of the application. Raises the Startup event.
        /// </summary>
        /// <param name="args">A <see cref="StartupEventArgs"/> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs args)
        {
            try
            {
                base.OnStartup(args);
            }
            catch (ApplicationException)
            {
            }
        }

        /// <summary>
        /// Contains actions that should occur last.
        /// </summary>
        protected override void OnInitialized()
        {
            RadWindow shell;

            shell = Container.Resolve<ShellView>();

            shell.Width = SystemParameters.PrimaryScreenWidth / 2;
            shell.Height = SystemParameters.PrimaryScreenHeight / 2;
            shell.Top = SystemParameters.PrimaryScreenHeight / 4;
            shell.Left = SystemParameters.PrimaryScreenWidth / 4;
            shell.Show();
            MainWindow = shell.ParentOfType<Window>();

            RegionManager.SetRegionManager(MainWindow, Container.Resolve<IRegionManager>());
            RegionManager.UpdateRegions();
            ////shell.Visibility = Visibility.Hidden;

            ////Container.Resolve<CommonParameterFactory>();

            ////LoadCore();
            ////LoadModules();

            shell.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Creates the shell or main window of the application.
        /// </summary>
        /// <returns>The shell of the application.</returns>
        protected override Window? CreateShell()
        {
            // because RadWindow is not Window, cannot create here.
            // https://github.com/PrismLibrary/Prism/issues/1413
            return null;
        }

        /// <summary>
        /// Initializes the modules in the module catalog.
        /// </summary>
        protected override void InitializeModules()
        {
            base.InitializeModules();
            ////AttachExceptionHandler();
        }

        /// <summary>
        /// Used to register types with the container that will be used by the application.
        /// </summary>
        /// <param name="containerRegistry"><see cref="IContainerRegistry"/> used for container type registration.</param>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IModuleInfoRetriever, ModuleInfoRetriever>();

            containerRegistry.Register<ModuleObjects.Interfaces.IModule, ModuleObjects.Classes.Module>();
        }

        /// <summary>
        /// Creates the module catalog.
        /// </summary>
        /// <returns>New module catalog.</returns>
        protected override IModuleCatalog CreateModuleCatalog()
        {
            return ModuleCatalog.CreateFromXaml(new Uri("/ModuleManager;component/ModuleCatalog.xaml", UriKind.Relative));
        }
    }
}