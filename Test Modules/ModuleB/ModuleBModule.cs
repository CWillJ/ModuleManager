namespace ModuleManager.TestModules.ModuleB
{
    using ModuleManager.Common.Interfaces;
    using ModuleManager.TestModules.ModuleB.Views;
    using Prism.Ioc;

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

        /// <inheritdoc/>
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var startUpService = containerProvider.Resolve<IModuleStartUpService>();
            startUpService.AddStoreViewAction(() => StoreViews(containerProvider));
        }

        /// <inheritdoc/>
        public void Unload(IContainerProvider containerProvider)
        {
            RemoveViewObject(containerProvider.Resolve<ModuleBView>());
        }

        /// <inheritdoc/>
        public void ReLoad(IContainerProvider containerProvider)
        {
            StoreViews(containerProvider);
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
            _viewCollectionService.AddView(containerProvider.Resolve<ModuleBView>());
        }

        /// <summary>
        /// Remove the view object from the service.
        /// </summary>
        /// <param name="viewObject">The view <see cref="object"/> to remove from the <see cref="IViewCollectionService"/>.</param>
        /// <returns>True if the view object was removed, false otherwise.</returns>
        private bool RemoveViewObject(object viewObject)
        {
            if (_viewCollectionService.Views.Contains(viewObject))
            {
                _viewCollectionService.RemoveView(viewObject);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}