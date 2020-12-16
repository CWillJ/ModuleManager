namespace ModuleManager.TestModules.ModuleB
{
    using ModuleManager.Common.Interfaces;
    using ModuleManager.TestModules.ModuleB.Views;
    using Prism.Ioc;
    using Prism.Modularity;

    /// <summary>
    /// Test module B.
    /// </summary>
    public class ModuleBModule : IModuleManagerTestModule
    {
        private readonly IViewCollectionService _viewCollectionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleBModule"/> class.
        /// </summary>
        /// <param name="viewCollectionService">The <see cref="IViewCollectionService"/>.</param>
        public ModuleBModule(IViewCollectionService viewCollectionService)
        {
            _viewCollectionService = viewCollectionService;
        }

        /// <inheritdoc cref="IModule"/>
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var startUpService = containerProvider.Resolve<IModuleStartUpService>();
            startUpService.AddStoreViewAction(() => InjectViewsIntoRegions(containerProvider));
        }

        /// <inheritdoc cref="IModule"/>
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        /// <summary>
        /// Injects this modules views into the LoadedViewsRegion.
        /// </summary>
        /// <param name="containerProvider">The <see cref="IContainerProvider"/>.</param>
        private void InjectViewsIntoRegions(IContainerProvider containerProvider)
        {
            _viewCollectionService.AddView(containerProvider.Resolve<ModuleBView>());
        }
    }
}