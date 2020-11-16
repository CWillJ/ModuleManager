namespace ModuleManager.ModuleObjects.Interfaces
{
    using System;
    using Prism.Modularity;

    /// <summary>
    /// Service providing concrete <see cref="IModuleCatalogService"/> implementations.
    /// </summary>
    public interface IModuleCatalogService
    {
        /// <summary>
        /// Gets or sets the <see cref="IModuleManagerCatalog"/>.
        /// </summary>
        public IModuleManagerCatalog TheModuleManagerCatalog { get; set; }

        /// <summary>
        /// Adds a module to the catalog from a <see cref="Type"/>.
        /// </summary>
        /// <param name="type">Module Type.</param>
        public void AddModule(Type type);

        /// <summary>
        /// Adds a module to the catalog from a <see cref="ModuleInfo"/>.
        /// </summary>
        /// <param name="module">Module ModuleInfo.</param>
        public void AddModule(ModuleInfo module);

        /// <summary>
        /// Removes a module from the catalog from a <see cref="Type"/>.
        /// </summary>
        /// <param name="type">Module Type.</param>
        public void RemoveModule(Type type);

        /// <summary>
        /// Removes a module from the catalog from a <see cref="ModuleInfo"/>.
        /// </summary>
        /// <param name="module">Module ModuleInfo.</param>
        public void RemoveModule(ModuleInfo module);
    }
}