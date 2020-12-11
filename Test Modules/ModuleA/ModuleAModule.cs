namespace ModuleManager.TestModules.ModuleA
{
    using ModuleManager.Common.Interfaces;
    using ModuleManager.ModuleA.Views;
    using Prism.Ioc;
    using Prism.Modularity;
    using Prism.Regions;

    /// <summary>
    /// Test module A.
    /// </summary>
    public class ModuleAModule : IModuleManagerTestModule
    {
        /// <inheritdoc cref="IModule"/>
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var startUpService = containerProvider.Resolve<IModuleStartUpService>();
            startUpService.AddViewInjectionAction(() => InjectViewsIntoRegions(containerProvider));
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
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.Regions[@"LoadedViewsRegion"].Add(containerProvider.Resolve<ModuleAView>(), @"ModuleManagerView");
        }
    }
}