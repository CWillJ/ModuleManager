namespace ModuleManager.ModuleObjects.Classes
{
    using System;
    using System.Collections.Generic;
    using ModuleManager.ModuleObjects.Interfaces;
    using Prism.Modularity;

    /// <summary>
    /// The <see cref="ModuleCatalog"/> holding the loaded modules.
    /// </summary>
    public class ModuleManagerCatalog : ModuleCatalog, IModuleManagerCatalog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleManagerCatalog"/> class.
        /// </summary>
        public ModuleManagerCatalog()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleManagerCatalog"/> class while providing an
        /// initial list of <see cref="ModuleInfo"/>s.
        /// </summary>
        /// <param name="modules">The initial list of modules.</param>
        public ModuleManagerCatalog(IEnumerable<ModuleInfo> modules)
            : base(modules)
        {
        }

        /// <summary>
        /// Adds a module to the module catalog based on type.
        /// </summary>
        /// <param name="type"><see cref="Type"/> representing the ModuleInfo to add.</param>
        public void AddModule(Type type)
        {
            AddModule(new ModuleInfo()
            {
                ModuleName = type.Name,
                ModuleType = type.AssemblyQualifiedName,
                InitializationMode = InitializationMode.OnDemand,
            });
        }

        /// <summary>
        /// Does the actual work of loading the catalog.  The base implementation does nothing.
        /// </summary>
        protected override void InnerLoad()
        {
            return;
        }
    }
}