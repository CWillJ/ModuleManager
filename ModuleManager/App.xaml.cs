namespace ModuleManager
{
    using System;
    using System.IO;
    using System.Windows;
    using ModuleManager.Classes;
    using ModuleManager.Common.Interfaces;
    using ModuleManager.Common.Services;
    using ModuleManager.Views;
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

            shell.Visibility = Visibility.Visible;

            LoadCore();
            LoadTestModules();
        }

        /// <summary>
        /// Creates the shell or main window of the application.
        /// </summary>
        /// <returns>The shell of the application.</returns>
        #nullable enable
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
        }

        /// <summary>
        /// Used to register types with the container that will be used by the application.
        /// </summary>
        /// <param name="containerRegistry"><see cref="IContainerRegistry"/> used for container type registration.</param>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            ////containerRegistry.RegisterSingleton<ILoadedViewsService, LoadedViewsService>();
            containerRegistry.RegisterSingleton<IAssemblyCollectionService, AssemblyCollectionService>();
            containerRegistry.RegisterSingleton<IModuleStartUpService, ModuleStartUpService>();
            containerRegistry.RegisterSingleton<IViewCollectionService, ViewCollectionService>();
        }

        /// <summary>
        /// Creates the module catalog.
        /// </summary>
        /// <returns>New <see cref="IModuleCatalog"/>.</returns>
        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new AggregateModuleCatalog();
        }

        /// <summary>
        /// Configures the <see cref="IModuleCatalog"/> used by Prism.
        /// </summary>
        /// <param name="moduleCatalog">The aggregate <see cref="IModuleCatalog"/> used for storing all modules.</param>
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            // Load core modules for this application.
            ConfigurationModuleCatalog configurationCatalog = new ConfigurationModuleCatalog();
            ((AggregateModuleCatalog)moduleCatalog).AddCatalog(configurationCatalog);

            // Load any extra modules that this application will use to display views/data.
            DirectoryLoaderModuleCatalog directoryCatalog = new DirectoryLoaderModuleCatalog(Container)
            { ModulePath = Path.Combine(Directory.GetCurrentDirectory(), @"TestModules") };
            ((AggregateModuleCatalog)moduleCatalog).AddCatalog(directoryCatalog);
        }

        /// <summary>
        /// Loads the core modules for this project.
        /// </summary>
        private void LoadCore()
        {
            var moduleStartupService = Container.Resolve<IModuleStartUpService>();

            foreach (Action registerModuleView in moduleStartupService.ViewInjectionActions)
            {
                registerModuleView();
            }
        }

        /// <summary>
        /// Stores the test modules that this project will display.
        /// </summary>
        private void LoadTestModules()
        {
            var moduleStartupService = Container.Resolve<IModuleStartUpService>();

            foreach (Action registerModuleView in moduleStartupService.StoreViewActions)
            {
                registerModuleView();
            }
        }
    }
}