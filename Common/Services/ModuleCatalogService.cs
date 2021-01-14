namespace ModuleManager.Common.Services
{
    using System;
    using ModuleManager.Common.Classes;
    using ModuleManager.Common.Interfaces;
    using Prism.Ioc;
    using Prism.Modularity;

    /// <inheritdoc cref="IModuleCatalogService"/>
    public class ModuleCatalogService : IModuleCatalogService
    {
        private readonly IContainerExtension _containerExtension;
        private readonly AggregateModuleCatalog _moduleCatalog;
        private readonly IModuleLoadingService _moduleLoadingService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleCatalogService"/> class.
        /// </summary>
        /// <param name="containerExtension">The application's <see cref="IContainerExtension"/>.</param>
        /// <param name="moduleCatalog">The application's <see cref="IModuleCatalog"/>.</param>
        /// <param name="moduleLoadingService">The application's <see cref="IModuleLoadingService"/>.</param>
        public ModuleCatalogService(IContainerExtension containerExtension, IModuleCatalog moduleCatalog, IModuleLoadingService moduleLoadingService)
        {
            _containerExtension = containerExtension;
            _moduleCatalog = (AggregateModuleCatalog)moduleCatalog;
            _moduleLoadingService = moduleLoadingService;
        }

        /// <inheritdoc/>
        public AggregateModuleCatalog ModuleCatalog
        {
            get { return _moduleCatalog; }
        }

        /// <inheritdoc/>
        public void UnloadExpansionModule(string moduleInfoName)
        {
            // Removes the module's views from the view collection (call module unload action)
            try
            {
                _moduleLoadingService.UnloadActions[moduleInfoName]();
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                // It's not there, don't try to remove
            }

            foreach (var catalog in ModuleCatalog.Catalogs)
            {
                if (catalog is DirectoryLoaderModuleCatalog directoryLoaderModuleCatalog)
                {
                    directoryLoaderModuleCatalog.RemoveModule(moduleInfoName);
                }
            }
        }

        /// <inheritdoc/>
        public void LoadExpansionModule(string dllFilePath)
        {
            string moduleInfoName = string.Empty;

            for (int i = 0; i < ModuleCatalog.Catalogs.Count; i++)
            {
                if (ModuleCatalog.Catalogs[i] is DirectoryLoaderModuleCatalog directoryLoaderModuleCatalog)
                {
                    IModuleInfo moduleInfo = directoryLoaderModuleCatalog.AddModule(dllFilePath);
                    moduleInfoName = moduleInfo.ModuleName;

                    moduleInfo.State = ModuleState.ReadyForInitialization;
                    InitializeModule(moduleInfo);
                }
            }

            // Store the module's views back in the view collection (call module reload action)
            if (!string.IsNullOrEmpty(moduleInfoName))
            {
                _moduleLoadingService.LoadActions[moduleInfoName]();
            }
        }

        /// <summary>
        /// Initializes an <see cref="IModuleInfo"/> using steps from the Prism ModuleManager and ModuleInitializer.
        /// </summary>
        /// <param name="moduleInfo">The <see cref="IModuleInfo"/> to initialize.</param>
        private void InitializeModule(IModuleInfo moduleInfo)
        {
            if (moduleInfo == null)
            {
                throw new ArgumentNullException(nameof(moduleInfo));
            }

            moduleInfo.State = ModuleState.Initializing;

            Type moduleType = Type.GetType(moduleInfo.ModuleType);
            IModule moduleInstance = (IModule)_containerExtension.Resolve(moduleType);

            if (moduleInstance != null)
            {
                moduleInstance.RegisterTypes(_containerExtension);
                moduleInstance.OnInitialized(_containerExtension);
            }

            moduleInfo.State = ModuleState.Initialized;
        }
    }
}