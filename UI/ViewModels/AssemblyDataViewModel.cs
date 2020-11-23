namespace ModuleManager.UI.ViewModels
{
    using ModuleManager.UI.Interfaces;
    using Prism.Mvvm;

    /// <summary>
    /// View model for assembly data area.
    /// </summary>
    public class AssemblyDataViewModel : BindableBase
    {
        private readonly IModuleManagerCollectionService _moduleManagerCollectionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyDataViewModel"/> class.
        /// </summary>
        /// <param name="moduleManagerCollectionService">Injected <see cref="IModuleManagerCollectionService"/>.</param>
        public AssemblyDataViewModel(IModuleManagerCollectionService moduleManagerCollectionService)
        {
            _moduleManagerCollectionService = moduleManagerCollectionService;
        }

        /// <summary>
        /// Gets a collection of ModuleManager.ModuleObjects.Classes.AssemblyData.
        /// </summary>
        public IModuleManagerCollectionService ModuleManagerCollectionService
        {
            get { return _moduleManagerCollectionService; }
        }
    }
}