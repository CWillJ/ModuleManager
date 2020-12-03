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
        /// Adds a <see cref="ModuleInfo"/> to the module catalog based on <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> representing the <see cref="ModuleInfo"/> to add.</param>
        public void AddModule(Type type);
    }
}