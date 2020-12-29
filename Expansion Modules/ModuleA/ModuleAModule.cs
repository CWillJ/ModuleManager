namespace ModuleManager.Expansion.ModuleA
{
    using ModuleManager.Common.Interfaces;
    using ModuleManager.Expansion.ModuleA.Views;
    using Prism.Ioc;

    /// <summary>
    /// Test module A.
    /// </summary>
    public class ModuleAModule : IExpansionModule
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

        /// <inheritdoc/>
        public void OnInitialized(IContainerProvider containerProvider)
        {
            string moduleName = GetType().Name;

            var moduleLoadingService = containerProvider.Resolve<IModuleLoadingService>();
            moduleLoadingService.AddStoreViewAction(() => StoreViews(containerProvider));
            moduleLoadingService.UnloadModule(moduleName, () => Unload(containerProvider));
            moduleLoadingService.ReloadModule(moduleName, () => Reload(containerProvider));
        }

        /// <inheritdoc/>
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

        /// <summary>
        /// Unloads the views associated with this module.
        /// </summary>
        /// <param name="containerProvider">The <see cref="IContainerProvider"/>.</param>
        private void Unload(IContainerProvider containerProvider)
        {
            RemoveViews(containerProvider.Resolve<ModuleAView>());
            RemoveViews(containerProvider.Resolve<ModuleA2View>());
        }

        /// <summary>
        /// Reloads the views associated with this module.
        /// </summary>
        /// <param name="containerProvider">The <see cref="IContainerProvider"/>.</param>
        private void Reload(IContainerProvider containerProvider)
        {
            StoreViews(containerProvider);
        }

        /// <summary>
        /// Remove the view object from the service.
        /// </summary>
        /// <param name="viewObject">The view <see cref="object"/> to remove from the <see cref="IViewCollectionService"/>.</param>
        private void RemoveViews(object viewObject)
        {
            _viewCollectionService.RemoveView(viewObject);
        }
    }
}