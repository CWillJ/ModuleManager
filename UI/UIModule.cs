namespace ModuleManager.UI
{
    using ModuleManager.UI.Views;
    using Prism.Ioc;
    using Prism.Modularity;
    using Prism.Regions;

    /// <summary>
    /// The UI Module Class.
    /// </summary>
    public class UIModule : IModule
    {
        private readonly IRegionManager _regionManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UIModule"/> class.
        /// </summary>
        /// <param name="regionManager">Injected <see cref="IRegionManager"/>.</param>
        public UIModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        /// <summary>
        /// Perform required initialization methods for this Module.
        /// </summary>
        /// <param name="containerProvider">A <see cref="IContainerProvider"/> used for progam-wide type resolving.</param>
        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion("ButtonsRegion", typeof(ButtonsView));
            _regionManager.RegisterViewWithRegion("AssemblyDataTreeRegion", typeof(AssemblyDataTreeView));
            _regionManager.RegisterViewWithRegion("AssemblyDataRegion", typeof(AssemblyDataView));
        }

        /// <summary>
        /// Register types with the container that will be used by the application.
        /// </summary>
        /// <param name="containerRegistry"><see cref="IContainerRegistry"/> used for program-wide type registration.</param>
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ButtonsView>();
            containerRegistry.RegisterForNavigation<AssemblyDataTreeView>();
            containerRegistry.RegisterForNavigation<AssemblyDataView>();
        }
    }
}
