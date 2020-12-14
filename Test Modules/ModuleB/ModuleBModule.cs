namespace ModuleManager.TestModules.ModuleB
{
    using ModuleManager.Common.Interfaces;
    using ModuleManager.TestModules.ModuleB.Views;
    using Prism.Ioc;
    using Prism.Modularity;
    using Prism.Regions;

    /// <summary>
    /// Test module B.
    /// </summary>
    public class ModuleBModule : IModuleManagerTestModule
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
            regionManager.Regions[@"LoadedViewsRegion"].Add(containerProvider.Resolve<ModuleBView>(), @"ModuleBView");
        }
    }
}
