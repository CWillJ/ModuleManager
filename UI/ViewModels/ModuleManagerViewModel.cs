namespace ModuleManager.UI.ViewModels
{
    using Prism.Commands;
    using Prism.Mvvm;
    using Prism.Regions;

    /// <summary>
    /// MainView view model.
    /// </summary>
    public class ModuleManagerViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleManagerViewModel"/> class.
        /// </summary>
        /// <param name="regionManager">Region manager.</param>
        public ModuleManagerViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }
    }
}