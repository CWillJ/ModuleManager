namespace ModuleManager.ViewModels
{
    using ModuleManager.UI.Views;
    using Prism.Mvvm;
    using Prism.Regions;

    /// <summary>
    /// ModuleManagerViewModel will handle commands from the main view.
    /// </summary>
    public class ShellViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellViewModel"/> class.
        /// </summary>
        /// <param name="regionManager">Gets the <see cref="IRegionManager"/> to register shell region.</param>
        public ShellViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _regionManager.RegisterViewWithRegion("ContentRegion", typeof(ModuleManagerView));
        }
    }
}