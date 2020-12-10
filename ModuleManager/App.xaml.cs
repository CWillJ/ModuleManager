namespace ModuleManager
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Windows;
    using System.Xml.Serialization;
    using ModuleManager.ModuleLoader.Interfaces;
    using ModuleManager.ModuleLoader.Services;
    using ModuleManager.ModuleObjects.Classes;
    using ModuleManager.ModuleObjects.Interfaces;
    using ModuleManager.UI;
    using ModuleManager.UI.Interfaces;
    using ModuleManager.UI.Views;
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

            LoadSavedModules();

            shell.Visibility = Visibility.Visible;
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
            containerRegistry.RegisterSingleton<IAssemblyLoaderService, AssemblyLoaderService>();
            containerRegistry.RegisterSingleton<ILoadedViewsService, LoadedViewsService>();

            containerRegistry.Register<IAssemblyData, AssemblyData>();
            containerRegistry.Register<ITypeData, TypeData>();
            containerRegistry.Register<ITypeMemberData, TypeMemberData>();

            containerRegistry.RegisterForNavigation<ModuleManagerView>();
            containerRegistry.RegisterForNavigation<ProgressBarView>();
        }

        /// <summary>
        /// Creates the module catalog.
        /// </summary>
        /// <returns>New <see cref="IModuleCatalog"/>.</returns>
        protected override IModuleCatalog CreateModuleCatalog()
        {
            ModuleCatalog moduleCatalog = new ModuleCatalog();

            moduleCatalog.AddModule<UIModule>();

            return moduleCatalog;
        }

        /// <summary>
        /// Loads an <see cref="ObservableCollection{AssemblyData}"/> from an xml file.
        /// </summary>
        private void LoadSavedModules()
        {
            // Load previously saved module configuration only if the ModuleSaveFile exists
            if (!File.Exists(Directory.GetCurrentDirectory() + @"\ModuleSaveFile.xml"))
            {
                return;
            }

            var assemblyLoaderService = Container.Resolve<IAssemblyLoaderService>();
            var assemblyCollectionService = Container.Resolve<IAssemblyCollectionService>();

            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<AssemblyData>));
            ObservableCollection<AssemblyData>? assemblies = new ObservableCollection<AssemblyData>();
            string loadFile = Directory.GetCurrentDirectory() + @"\ModuleSaveFile.xml";

            using (StreamReader rd = new StreamReader(loadFile))
            {
                try
                {
                    assemblies = serializer.Deserialize(rd) as ObservableCollection<AssemblyData>;
                }
                catch (InvalidOperationException)
                {
                    // There is something wrong with the xml file.
                    // Return an empty collection of assemblies.
                    return;
                }
            }

            if (assemblies == null)
            {
                return;
            }

            // Load and get data.
            assemblyLoaderService.LoadAll(ref assemblies);

            // Unload all disabled assemblies.
            assemblyLoaderService.LoadUnload(ref assemblies);

            assemblyCollectionService.Assemblies = assemblies;
        }
    }
}