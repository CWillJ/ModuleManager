namespace ModuleManager.Core.UI
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Xml.Serialization;
    using ModuleManager.Common.Classes;
    using ModuleManager.Common.Interfaces;
    using ModuleManager.Common.Services;
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
        /// <summary>
        /// Perform required initialization methods for this Module.
        /// </summary>
        /// <param name="containerProvider">A <see cref="IContainerProvider"/> used for progam-wide type resolving.</param>
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();

            regionManager.Regions[@"ContentRegion"].Add(containerProvider.Resolve<ModuleManagerView>(), @"ModuleManagerView");

            regionManager.RegisterViewWithRegion(@"ButtonsRegion", typeof(ButtonsView));
            regionManager.RegisterViewWithRegion(@"AssemblyDataRegion", typeof(AssemblyDataView));
            regionManager.RegisterViewWithRegion(@"AssemblyDataTreeRegion", typeof(AssemblyDataTreeView));

            LoadSavedModules(containerProvider.Resolve<IAssemblyCollectionService>(), containerProvider.Resolve<IAssemblyLoaderService>());
        }

        /// <summary>
        /// Register types with the container that will be used by the application.
        /// </summary>
        /// <param name="containerRegistry"><see cref="IContainerRegistry"/> used for program-wide type registration.</param>
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IProgressBarService, ProgressBarService>();
            containerRegistry.RegisterSingleton<IAssemblyCollectionService, AssemblyCollectionService>();

            containerRegistry.RegisterForNavigation<ButtonsView>();
            containerRegistry.RegisterForNavigation<AssemblyDataView>();
            containerRegistry.RegisterForNavigation<AssemblyDataTreeView>();

            containerRegistry.RegisterSingleton<IAssemblyLoaderService, AssemblyLoaderService>();
            containerRegistry.RegisterSingleton<ILoadedViewsService, LoadedViewsService>();

            containerRegistry.Register<IAssemblyData, AssemblyData>();
            containerRegistry.Register<ITypeData, TypeData>();
            containerRegistry.Register<ITypeMemberData, TypeMemberData>();

            containerRegistry.RegisterForNavigation<ModuleManagerView>();
            containerRegistry.RegisterForNavigation<ProgressBarView>();
        }

        /// <summary>
        /// Loads an <see cref="ObservableCollection{AssemblyData}"/> from an xml file.
        /// </summary>
        /// <param name="assemblyCollectionService">The <see cref="IAssemblyCollectionService"/>.</param>
        /// <param name="assemblyLoaderService">The <see cref="IAssemblyLoaderService"/>.</param>
        private void LoadSavedModules(IAssemblyCollectionService assemblyCollectionService, IAssemblyLoaderService assemblyLoaderService)
        {
            // Load previously saved module configuration only if the ModuleSaveFile exists
            if (!File.Exists(Directory.GetCurrentDirectory() + @"\ModuleSaveFile.xml"))
            {
                return;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<AssemblyData>));
            ObservableCollection<AssemblyData> assemblies = new ObservableCollection<AssemblyData>();
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
