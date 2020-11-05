namespace ModuleManager.UI.ViewModels
{
    using ModuleManager.UI.Interfaces;
    using Prism.Mvvm;

    /// <summary>
    /// View model for assembly data area.
    /// </summary>
    public class AssemblyDataViewModel : BindableBase
    {
        private IAssemblyCollectionService _assemblyCollectionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyDataViewModel"/> class.
        /// </summary>
        /// <param name="assemblyCollectionService">IAssemblyCollectionService.</param>
        public AssemblyDataViewModel(IAssemblyCollectionService assemblyCollectionService)
        {
            _assemblyCollectionService = assemblyCollectionService;
        }

        /// <summary>
        /// Gets or sets a collection of ModuleManager.ModuleObjects.Classes.AssemblyData.
        /// </summary>
        public IAssemblyCollectionService AssemblyCollectionService
        {
            get { return _assemblyCollectionService; }
            set { SetProperty(ref _assemblyCollectionService, value); }
        }
    }
}