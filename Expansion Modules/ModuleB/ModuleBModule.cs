namespace ModuleManager.Expansion.ModuleB
{
    using ModuleManager.Common.Interfaces;
    using ModuleManager.Expansion.ModuleB.ViewModels;
    using ModuleManager.Expansion.ModuleB.Views;
    using Prism.Ioc;
    using Prism.Regions;
    using System;
    using System.Reflection;

    /// <summary>
    /// Test module B.
    /// </summary>
    public class ModuleBModule : IExpansionModule
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
            string moduleName = GetType().Name;

            var moduleLoadingService = containerProvider.Resolve<IModuleLoadingService>();
            moduleLoadingService.AddStoreViewAction(() => StoreViews(containerProvider));
            moduleLoadingService.UnloadModule(moduleName, () => Unload(containerProvider));
            moduleLoadingService.ReloadModule(moduleName, () => StoreViews(containerProvider));
        }

        /// <inheritdoc/>
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<object, ModuleBView>(typeof(ModuleBView).FullName);
        }

        /// <summary>
        /// Stores this modules views into the <see cref="IViewCollectionService"/>.
        /// </summary>
        /// <param name="containerProvider">The <see cref="IContainerProvider"/>.</param>
        private void StoreViews(IContainerProvider containerProvider)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var something1 = assembly.GetManifestResourceNames();
            var something2 = assembly.GetTypes();
            var something3 = assembly.GetType(@"ModuleManager.Expansion.ModuleB.Views.ModuleBView");

            object instance = Activator.CreateInstance(something3);

            _viewCollectionService.AddView(instance);
        }

        /// <summary>
        /// Unloads the views associated with this module.
        /// </summary>
        /// <param name="containerProvider">The <see cref="IContainerProvider"/>.</param>
        private void Unload(IContainerProvider containerProvider)
        {
            RemoveViews(containerProvider.Resolve<ModuleBView>());
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