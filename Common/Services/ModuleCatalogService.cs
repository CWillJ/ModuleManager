namespace ModuleManager.Common.Services
{
    using System;
    using ModuleManager.Common.Classes;
    using ModuleManager.Common.Interfaces;
    using Prism.Modularity;

    /// <inheritdoc cref="IModuleCatalogService"/>
    public class ModuleCatalogService : IModuleCatalogService
    {
        private readonly AggregateModuleCatalog _moduleCatalog;
        private readonly IAssemblyDataLoaderService _assemblyDataLoaderService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleCatalogService"/> class.
        /// </summary>
        /// <param name="moduleCatalog">The application's <see cref="IModuleCatalog"/>.</param>
        /// <param name="assemblyDataLoaderService">The application's <see cref="IAssemblyDataLoaderService"/>.</param>
        public ModuleCatalogService(IModuleCatalog moduleCatalog, IAssemblyDataLoaderService assemblyDataLoaderService)
        {
            _moduleCatalog = (AggregateModuleCatalog)moduleCatalog;
            _assemblyDataLoaderService = assemblyDataLoaderService ?? throw new ArgumentNullException("AssemblyDataLoaderService");
        }

        /// <inheritdoc/>
        public AggregateModuleCatalog ModuleCatalog
        {
            get { return _moduleCatalog; }
        }

        /// <summary>
        /// Unloads an <see cref="IModuleInfo"/> from the <see cref="IModuleCatalog"/>.
        /// </summary>
        /// <param name="moduleInfo">The <see cref="IModuleInfo"/> to unload from the <see cref="IModuleCatalog"/>.</param>
        public void UnloadModule(IModuleInfo moduleInfo)
        {
            DirectoryLoaderModuleCatalog directoryCatalog = new DirectoryLoaderModuleCatalog(_assemblyDataLoaderService);

            foreach (var module in ModuleCatalog.Catalogs[^1].Modules)
            {
                if (module.ModuleName != moduleInfo.ModuleName)
                {
                    directoryCatalog.AddModule(module);
                }
            }

            ModuleCatalog.Catalogs[^1] = directoryCatalog;
        }

        /// <summary>
        /// Reloads an <see cref="IModuleInfo"/> to the <see cref="IModuleCatalog"/>.
        /// </summary>
        /// <param name="dllFilePath">The <see cref="string"/> of the dll file.</param>
        public void ReloadModule(string dllFilePath)
        {
            // Get the DirectoryLoaderModuleCatalog
            DirectoryLoaderModuleCatalog directoryCatalog = new DirectoryLoaderModuleCatalog(_assemblyDataLoaderService);
            ModuleCatalog.AddModule(directoryCatalog.GetModuleInfoFromFile(dllFilePath));
        }
    }
}