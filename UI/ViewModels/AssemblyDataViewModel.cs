namespace ModuleManager.UI.ViewModels
{
    using ModuleManager.UI.Interfaces;
    using Prism.Mvvm;

    /// <summary>
    /// View model for assembly data area.
    /// </summary>
    public class AssemblyDataViewModel : BindableBase
    {
        private readonly IAssemblyCollectionService _assemblyCollectionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyDataViewModel"/> class.
        /// </summary>
        /// <param name="assemblyCollectionService">Injected <see cref="IAssemblyCollectionService"/>.</param>
        public AssemblyDataViewModel(IAssemblyCollectionService assemblyCollectionService)
        {
            _assemblyCollectionService = assemblyCollectionService;
        }

        /// <summary>
        /// Gets a collection of ModuleManager.ModuleObjects.Classes.AssemblyData.
        /// </summary>
        public IAssemblyCollectionService AssemblyCollectionService
        {
            get { return _assemblyCollectionService; }
        }
    }
}