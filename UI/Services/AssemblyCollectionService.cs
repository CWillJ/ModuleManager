namespace ModuleManager.UI.Services
{
    using System.Collections.ObjectModel;
    using ModuleManager.ModuleObjects.Classes;
    using ModuleManager.UI.Interfaces;
    using Prism.Mvvm;

    /// <summary>
    /// Service providing concrete <see cref="IAssemblyCollectionService"/> implementations.
    /// </summary>
    public class AssemblyCollectionService : BindableBase, IAssemblyCollectionService
    {
        private ObservableCollection<AssemblyData> _assemblies;
        private object _selectedItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyCollectionService"/> class.
        /// </summary>
        public AssemblyCollectionService()
        {
            _assemblies = new ObservableCollection<AssemblyData>();
            _selectedItem = string.Empty;
        }

        /// <summary>
        /// Gets or sets a collection of <see cref="AssemblyData"/> objects.
        /// </summary>
        public ObservableCollection<AssemblyData> Assemblies
        {
            get { return _assemblies; }
            set { SetProperty(ref _assemblies, value); }
        }

        /// <summary>
        /// Gets or sets an <see cref="object"/> object hopefully.
        /// </summary>
        public object SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }
    }
}