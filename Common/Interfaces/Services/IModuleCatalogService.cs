namespace ModuleManager.Common.Interfaces
{
    using ModuleManager.Common.Classes;
    using Prism.Modularity;

    /// <summary>
    /// Service providing concrete <see cref="IModuleCatalogService"/> implementations.
    /// </summary>
    public interface IModuleCatalogService
    {
        /// <summary>
        /// Gets the <see cref="AggregateModuleCatalog"/> used in this application.
        /// </summary>
        public AggregateModuleCatalog ModuleCatalog { get; }

        /// <summary>
        /// Unloads an <see cref="IModuleInfo"/> from the <see cref="IModuleCatalog"/> and removes the module's view from
        /// the <see cref="IViewCollectionService"/>.
        /// </summary>
        /// <param name="moduleInfoName">The <see cref="string"/> of the <see cref="IModuleInfo"/> to unload from the <see cref="IModuleCatalog"/>.</param>
        public void UnloadExpansionModule(string moduleInfoName);

        /// <summary>
        /// Reloads an <see cref="IModuleInfo"/> to the <see cref="IModuleCatalog"/> and adds any associated views
        /// to the <see cref="IViewCollectionService"/> NOT WORKING.
        /// </summary>
        /// <param name="dllFilePath">The <see cref="string"/> of the dll file.</param>
        public void ReloadExpansionModule(string dllFilePath);
    }
}