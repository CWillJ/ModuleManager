namespace ModuleManager.ModuleObjects.Interfaces
{
    using System;
    using Prism.Modularity;

    /// <summary>
    /// The interface for a ModuleCatalogService.
    /// </summary>
    public interface IModuleManagerCatalog : IModuleCatalog
    {
        /// <summary>
        /// Adds a module to the module catalog based on type.
        /// </summary>
        /// <param name="type"><see cref="Type"/> representing the ModuleInfo to add.</param>
        public void AddModule(Type type);
    }
}