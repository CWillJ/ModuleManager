namespace ModuleManager.TestModules.ModuleA
{
    using ModuleManager.Common.Interfaces;
    using ModuleManager.TestModules.ModuleA.Views;
    using Prism.Ioc;
    using Prism.Modularity;
    using Prism.Regions;

    /// <summary>
    /// Test module A.
    /// </summary>
    public class ModuleAModule : IModuleManagerTestModule
    {
        private readonly IViewCollectionService _viewCollectionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleAModule"/> class.
        /// </summary>
        /// <param name="viewCollectionService">The <see cref="IViewCollectionService"/>.</param>
        public ModuleAModule(IViewCollectionService viewCollectionService)
        {
            _viewCollectionService = viewCollectionService;
        }

        /// <inheritdoc cref="IModule"/>
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var startUpService = containerProvider.Resolve<IModuleStartUpService>();
            startUpService.AddStoreViewAction(() => StoreViews(containerProvider));
        }

        /// <inheritdoc cref="IModule"/>
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        /// <summary>
        /// Stores this modules views into the <see cref="IViewCollectionService"/>.
        /// </summary>
        /// <param name="containerProvider">The <see cref="IContainerProvider"/>.</param>
        private void StoreViews(IContainerProvider containerProvider)
        {
            _viewCollectionService.AddView(containerProvider.Resolve<ModuleAView>());
            _viewCollectionService.AddView(containerProvider.Resolve<ModuleA2View>());
        }
    }
}