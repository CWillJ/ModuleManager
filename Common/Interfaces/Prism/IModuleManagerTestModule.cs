namespace ModuleManager.Common.Interfaces
{
    using Prism.Ioc;
    using Prism.Modularity;

    /// <summary>
    /// Interface that implements <see cref="IModule"/>. Used to define this application's test modules.
    /// </summary>
    public interface IModuleManagerTestModule : IModule
    {
        /// <summary>
        /// Unloads this module from the container provider.
        /// </summary>
        /// <param name="containerProvider">The <see cref="IContainerProvider"/>.</param>
        public void Unload(IContainerProvider containerProvider);

        /// <summary>
        /// Reloads this module into the collection service.
        /// </summary>
        /// <param name="containerProvider">The <see cref="IContainerProvider"/>.</param>
        public void ReLoad(IContainerProvider containerProvider);
    }
}