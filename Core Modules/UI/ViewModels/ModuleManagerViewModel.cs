namespace ModuleManager.Core.UI.ViewModels
{
    using ModuleManager.Common.Interfaces;
    using Prism.Mvvm;

    /// <summary>
    /// MainView view model.
    /// </summary>
    public class ModuleManagerViewModel : BindableBase
    {
        private readonly ILoadedViewsService _loadedViewsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleManagerViewModel"/> class.
        /// </summary>
        /// <param name="loadedViewsService">The program's <see cref="ILoadedViewsService"/>.</param>
        public ModuleManagerViewModel(ILoadedViewsService loadedViewsService)
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