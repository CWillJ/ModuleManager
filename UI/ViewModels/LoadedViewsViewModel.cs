namespace ModuleManager.UI.ViewModels
{
    using ModuleManager.UI.Interfaces;
    using Prism.Regions;

    /// <summary>
    /// View model for the loaded views.
    /// </summary>
    public class LoadedViewsViewModel
    {
        private readonly IRegionManager _regionManager;
        private readonly ILoadedViewsService _loadedViewsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadedViewsViewModel"/> class.
        /// </summary>
        /// <param name="regionManager">The <see cref="IRegionManager"/>.</param>
        /// <param name="loadedViewsService">The program's <see cref="ILoadedViewsService"/>.</param>
        public LoadedViewsViewModel(IRegionManager regionManager, ILoadedViewsService loadedViewsService)
        {
            _regionManager = regionManager;
            _loadedViewsService = loadedViewsService;
        }

        /// <summary>
        /// Gets a collection of view types.
        /// </summary>
        public ILoadedViewsService LoadedViewsService
        {
            get { return _loadedViewsService; }
        }
    }
}
