namespace ModuleManager.Core.UI
{
    using System.Collections.ObjectModel;
    using System.IO;
    using ModuleManager.Common.Classes;
    using ModuleManager.Common.Interfaces;
    using ModuleManager.Core.UI.Interfaces;
    using ModuleManager.Core.UI.Services;
    using ModuleManager.Core.UI.Views;
    using Prism.Ioc;
    using Prism.Regions;

    /// <summary>
    /// The UI Module Class.
    /// </summary>
    public class UIModule : IModuleManagerCoreModule
    {
        private readonly IAssemblyDataLoaderService _assemblyDataLoaderService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UIModule"/> class.
        /// </summary>
        /// <param name="assemblyDataLoaderService">The <see cref="IAssemblyDataLoaderService"/>.</param>
        public UIModule(IAssemblyDataLoaderService assemblyDataLoaderService)
        {
            _assemblyDataLoaderService = assemblyDataLoaderService;
        }

        /// <summary>
        /// Perform required initialization methods for this Module.
        /// </summary>
        /// <param name="containerProvider">A <see cref="IContainerProvider"/> used for progam-wide type resolving.</param>
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();

            regionManager.RegisterViewWithRegion(@"AssemblyDataRegion", typeof(AssemblyDataView));
            regionManager.RegisterViewWithRegion(@"AssemblyDataTreeRegion", typeof(AssemblyDataTreeView));
            regionManager.RegisterViewWithRegion(@"ButtonViewsRegion", typeof(ViewDisplayView));

            // Register module initialization actions with CoreStartupService.
            var startupService = containerProvider.Resolve<IModuleStartUpService>();
            startupService.AddViewInjectionAction(() => InjectViewsIntoRegions(containerProvider));

            LoadSavedModules(containerProvider.Resolve<IAssemblyCollectionService>());
            LoadSavedViewNames(containerProvider.Resolve<ILoadedViewNamesService>());
        }

        /// <summary>
        /// Register types with the container that will be used by the application.
        /// </summary>
        /// <param name="containerRegistry"><see cref="IContainerRegistry"/> used for program-wide type registration.</param>
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IProgressBarService, ProgressBarService>();

            containerRegistry.RegisterForNavigation<ModuleManagerView>();
            containerRegistry.RegisterForNavigation<ProgressBarView>();
        }

        /// <summary>
        /// Adds the main view of this module to the ContentRegion of the shell.
        /// </summary>
        /// <param name="containerProvider">The <see cref="IContainerProvider"/>.</param>
        private void InjectViewsIntoRegions(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.Regions[@"ContentRegion"].Add(containerProvider.Resolve<ModuleManagerView>());
        }

        /// <summary>
        /// Loads an <see cref="ObservableCollection{AssemblyData}"/> from an xml file.
        /// </summary>
        /// <param name="assemblyCollectionService">The <see cref="IAssemblyCollectionService"/>.</param>
        private void LoadSavedModules(IAssemblyCollectionService assemblyCollectionService)
        {
            ObservableCollection<AssemblyData> assemblies = new ObservableCollection<AssemblyData>();

            // Load previously saved module configuration only if the ModuleSaveFile exists
            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), @"ModuleSaveFile.json")))
            {
                assemblies = JsonExtensions.Load<ObservableCollection<AssemblyData>>(Path.Combine(Directory.GetCurrentDirectory(), @"ModuleSaveFile.json"));

                if (assemblies == null)
                {
                    return;
                }

                _assemblyDataLoaderService.LoadAll(ref assemblies);
                _assemblyDataLoaderService.LoadUnload(ref assemblies);
                assemblyCollectionService.Assemblies = assemblies;
            }

            assemblies = assemblyCollectionService.Assemblies;
            _assemblyDataLoaderService.LoadUnload(ref assemblies);
            assemblyCollectionService.Assemblies = assemblies;
        }

        /// <summary>
        /// Loads an <see cref="ObservableCollection{String}"/> from an xml file.
        /// </summary>
        /// <param name="loadedViewNamesService">The <see cref="ILoadedViewNamesService"/>.</param>
        private void LoadSavedViewNames(ILoadedViewNamesService loadedViewNamesService)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), @"LoadedViewsSaveFile.json");
            if (File.Exists(filePath))
            {
                ObservableCollection<string> viewNames =
                    JsonExtensions.Load<ObservableCollection<string>>(filePath);

                if (viewNames == null)
                {
                    return;
                }

                loadedViewNamesService.LoadedViewNames = viewNames;
            }
        }
    }
}