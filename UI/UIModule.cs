namespace ModuleManager.UI
{
    using ModuleRetriever;
    using ModuleRetriever.Interfaces;
    using Prism.Ioc;
    using Prism.Modularity;
    using Prism.Regions;

    /// <summary>
    /// The UI Module Class.
    /// </summary>
    public class UIModule : IModule
    {
        /// <summary>
        /// OnInitialized method.
        /// </summary>
        /// <param name="containerProvider">The container.</param>
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
        }

        /// <summary>
        /// Registers types for UI project.
        /// </summary>
        /// <param name="containerRegistry">The container to register types in.</param>
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IModuleInfoRetriever, ModuleInfoRetriever>();
        }
    }
}