namespace ModuleManager.Core.UI.ViewModels
{
    using System;
    using ModuleManager.Common.Interfaces;

    /// <summary>
    /// ViewObject model for the AssemblyData TreeView.
    /// </summary>
    public class AssemblyDataTreeViewModel
    {
        private readonly IAssemblyCollectionService _assemblyCollectionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyDataTreeViewModel"/> class.
        /// </summary>
        /// <param name="assemblyCollectionService">Injected <see cref="IAssemblyCollectionService"/>.</param>
        public AssemblyDataTreeViewModel(IAssemblyCollectionService assemblyCollectionService)
        {
            _assemblyCollectionService = assemblyCollectionService ?? throw new ArgumentNullException("AssemblyCollectionService");
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
