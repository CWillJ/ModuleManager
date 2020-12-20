namespace ModuleManager.Common.Services
{
    using System;
    using System.IO;
    using ModuleManager.Common.Classes;
    using ModuleManager.Common.Interfaces;
    using Prism.Modularity;

    /// <inheritdoc cref="IModuleCatalogService"/>
    public class ModuleCatalogService : IModuleCatalogService
    {
        private readonly AggregateModuleCatalog _moduleCatalog;
        private readonly IAssemblyDataLoaderService _assemblyDataLoaderService;
        private readonly IModuleLoadingService _moduleLoadingService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleCatalogService"/> class.
        /// </summary>
        /// <param name="moduleCatalog">The application's <see cref="IModuleCatalog"/>.</param>
        /// <param name="assemblyDataLoaderService">The application's <see cref="IAssemblyDataLoaderService"/>.</param>
        /// <param name="moduleLoadingService">The application's <see cref="IModuleLoadingService"/>.</param>
        public ModuleCatalogService(IModuleCatalog moduleCatalog, IAssemblyDataLoaderService assemblyDataLoaderService, IModuleLoadingService moduleLoadingService)
        {
            _moduleCatalog = (AggregateModuleCatalog)moduleCatalog;
            _assemblyDataLoaderService = assemblyDataLoaderService ?? throw new ArgumentNullException("AssemblyDataLoaderService");
            _moduleLoadingService = moduleLoadingService ?? throw new ArgumentNullException("ModuleLoadingService");
        }

        /// <inheritdoc/>
        public AggregateModuleCatalog ModuleCatalog
        {
            get { return _moduleCatalog; }
        }

        /// <summary>
        /// Unloads an <see cref="IModuleInfo"/> from the <see cref="IModuleCatalog"/> and removes the module's view from
        /// the <see cref="IViewCollectionService"/>.
        /// </summary>
        /// <param name="moduleInfo">The <see cref="IModuleInfo"/> to unload from the <see cref="IModuleCatalog"/>.</param>
        public void UnloadModule(IModuleInfo moduleInfo)
        {
            // Removes the module's views from the view collection (call module unload action)
            _moduleLoadingService.UnloadActions[moduleInfo.ModuleName]();

            // Removes the module from the catalog
            DirectoryLoaderModuleCatalog directoryCatalog = new DirectoryLoaderModuleCatalog(_assemblyDataLoaderService);

            foreach (var module in ModuleCatalog.Catalogs[^1].Modules)
            {
                if (module.ModuleName == moduleInfo.ModuleName)
                {
                    // need to call the module's Unload method here
                    var what1 = module.InitializationMode;
                }
                else
                {
                    directoryCatalog.AddModule(module);
                }
            }

            ModuleCatalog.Catalogs[^1] = directoryCatalog;
        }

        /// <summary>
        /// Reloads an <see cref="IModuleInfo"/> to the <see cref="IModuleCatalog"/> and adds any associated views
        /// to the <see cref="IViewCollectionService"/>.
        /// </summary>
        /// <param name="dllFilePath">The <see cref="string"/> of the dll file.</param>
        public void ReloadModule(string dllFilePath)
        {
            // Remove the old DirectoryLoaderModuleCatalog
            ModuleCatalog.Catalogs.RemoveAt(ModuleCatalog.Catalogs.Count - 1);

            DirectoryLoaderModuleCatalog directoryCatalog = new DirectoryLoaderModuleCatalog(_assemblyDataLoaderService);
            IModuleInfo moduleInfo = directoryCatalog.GetModuleInfoFromFile(dllFilePath);
            directoryCatalog.AddModule(moduleInfo);
            ModuleCatalog.AddCatalog(directoryCatalog);

            // Adds the module to the module catalog
            ////DirectoryLoaderModuleCatalog directoryCatalog = new DirectoryLoaderModuleCatalog(_assemblyDataLoaderService);
            ////IModuleInfo moduleInfo = directoryCatalog.GetModuleInfoFromFile(dllFilePath);

            ////ModuleCatalog.Catalogs[^1].AddModule(moduleInfo);

            // Store the module's views back in the view collection (call module reload action)
            _moduleLoadingService.ReloadActions[moduleInfo.ModuleName]();
        }
    }
}