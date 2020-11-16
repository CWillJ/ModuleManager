namespace ModuleManager.ModuleObjects.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ModuleManager.ModuleObjects.Classes;
    using ModuleManager.ModuleObjects.Interfaces;
    using Prism.Modularity;

    /// <summary>
    /// Service providing concrete <see cref="IModuleCatalogService"/> implementations.
    /// </summary>
    public class ModuleCatalogService : IModuleCatalogService
    {
        private IModuleManagerCatalog _moduleManagerCatalog;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleCatalogService"/> class.
        /// </summary>
        /// <param name="moduleManagerCatalog">The implementation of <see cref="IModuleManagerCatalog"/>.</param>
        public ModuleCatalogService(IModuleManagerCatalog moduleManagerCatalog)
        {
            _moduleManagerCatalog = moduleManagerCatalog;
        }

        /// <summary>
        /// Gets or sets the <see cref="IModuleManagerCatalog"/>.
        /// </summary>
        public IModuleManagerCatalog TheModuleManagerCatalog
        {
            get { return _moduleManagerCatalog; }
            set { _moduleManagerCatalog = value; }
        }

        /// <summary>
        /// Adds a module to the catalog from a <see cref="Type"/>.
        /// </summary>
        /// <param name="type">Module Type.</param>
        public void AddModule(Type type)
        {
            TheModuleManagerCatalog.AddModule(type);
        }

        /// <summary>
        /// Adds a module to the catalog from a <see cref="ModuleInfo"/>.
        /// </summary>
        /// <param name="module">Module ModuleInfo.</param>
        public void AddModule(ModuleInfo module)
        {
            TheModuleManagerCatalog.AddModule(module);
        }

        /// <summary>
        /// Removes a module from the catalog from a <see cref="Type"/>.
        /// </summary>
        /// <param name="type">Module Type.</param>
        public void RemoveModule(Type type)
        {
            ModuleInfo module = new ModuleInfo
            {
                ModuleName = type.Name,
                ModuleType = type.AssemblyQualifiedName,
            };

            if (TheModuleManagerCatalog.Modules.Contains(module))
            {
                IEnumerable<ModuleInfo> modules;
                modules = (IEnumerable<ModuleInfo>)TheModuleManagerCatalog.Modules.Where(mod => mod != module);
                TheModuleManagerCatalog = new ModuleManagerCatalog(modules);
            }
        }

        /// <summary>
        /// Removes a module from the catalog from a <see cref="ModuleInfo"/>.
        /// </summary>
        /// <param name="module">Module ModuleInfo.</param>
        public void RemoveModule(ModuleInfo module)
        {
            if (TheModuleManagerCatalog.Modules.Contains(module))
            {
                IEnumerable<ModuleInfo> modules;
                modules = (IEnumerable<ModuleInfo>)TheModuleManagerCatalog.Modules.Where(mod => mod != module);
                TheModuleManagerCatalog = new ModuleManagerCatalog(modules);
            }
        }

        /// <summary>
        /// Removes all modules from the catalog from a <see cref="ModuleInfo"/>.
        /// </summary>
        public void RemoveAllModules()
        {
            TheModuleManagerCatalog = new ModuleManagerCatalog();
        }
    }
}
