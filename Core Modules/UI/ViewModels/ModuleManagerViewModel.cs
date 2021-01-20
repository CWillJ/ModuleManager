namespace ModuleManager.Core.UI.ViewModels
{
    using System;
    using ModuleManager.Common.Interfaces;
    using Prism.Mvvm;

    /// <summary>
    /// MainView view model.
    /// </summary>
    public class ModuleManagerViewModel : BindableBase
    {
        private readonly IViewCollectionService _viewCollectionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleManagerViewModel"/> class.
        /// </summary>
        /// <param name="viewCollectionService">The <see cref="IViewCollectionService"/>.</param>
        public ModuleManagerViewModel(IViewCollectionService viewCollectionService)
        {
            _viewCollectionService = viewCollectionService;
        }

        /// <summary>
        /// Gets the <see cref="IViewCollectionService"/>.
        /// </summary>
        public IViewCollectionService ViewCollectionService
        {
            get { return _viewCollectionService; }
        }
    }
}
