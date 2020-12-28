namespace ModuleManager.Common.Interfaces
{
    using ModuleManager.Common.Classes;
    using Prism.Modularity;

    /// <summary>
    /// Adds on to <see cref="IModuleCatalog"/>.
    /// </summary>
    public interface IModuleManagerCatalog : IModuleCatalog
    {
        /// <summary>
        /// Adds a <see cref="IModuleInfo"/> to the <see cref="DirectoryLoaderModuleCatalog"/>.
        /// </summary>
        /// <param name="moduleInfo">The <see cref="IModuleInfo"/> to add to the catalog.</param>
        public void AddModuleToCatalog(IModuleInfo moduleInfo);

        /// <summary>
        /// Removes a <see cref="IModuleInfo"/> from the <see cref="DirectoryLoaderModuleCatalog"/>.
        /// </summary>
        /// <param name="moduleInfo">The <see cref="IModuleInfo"/> to remove from the catalog.</param>
        public void RemoveModuleFromCatalog(IModuleInfo moduleInfo);
    }
}