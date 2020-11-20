namespace ModuleManager.ModuleObjects.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ModuleManager.ModuleObjects.Classes;
    using ModuleManager.ModuleObjects.Interfaces;
    using Prism.Modularity;

    /// <inheritdoc cref="IModuleCatalogService"/>
    public class ModuleCatalogService : IModuleCatalogService
    {
        private IModuleManagerCatalog _moduleManagerCatalog;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleCatalogService"/> class.
        /// </summary>
        /// <param name="moduleManagerCatalog">The implementation of <see cref="IModuleManagerCatalog"/>.</param>
        public ModuleCatalogService(IModuleManagerCatalog moduleManagerCatalog)
        {
            _moduleManagerCatalog = new ModuleManagerCatalog();
        }

        /// <inheritdoc cref="IModuleCatalogService"/>
        public IModuleManagerCatalog TheModuleManagerCatalog
        {
            get { return _moduleManagerCatalog; }
            set { _moduleManagerCatalog = value; }
        }

        /// <inheritdoc cref="IModuleCatalogService"/>
        public void AddModule(Type type)
        {
            TheModuleManagerCatalog.AddModule(type);
            return;
        }

        /// <inheritdoc cref="IModuleCatalogService"/>
        public void AddModule(ModuleInfo module)
        {
            TheModuleManagerCatalog.AddModule(module);
        }

        /// <inheritdoc cref="IModuleCatalogService"/>
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

        /// <inheritdoc cref="IModuleCatalogService"/>
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
