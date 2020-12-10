namespace ModuleManager.UI.ViewModels
{
    using ModuleManager.ModuleLoader.Interfaces;

    /// <summary>
    /// View model for the loaded views.
    /// </summary>
    public class LoadedViewsViewModel
    {
        private readonly ILoadedViewsService _loadedViewsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadedViewsViewModel"/> class.
        /// </summary>
        /// <param name="loadedViewsService">The program's <see cref="ILoadedViewsService"/>.</param>
        public LoadedViewsViewModel(ILoadedViewsService loadedViewsService)
        {
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
