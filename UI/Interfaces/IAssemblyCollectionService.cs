namespace ModuleManager.UI.Interfaces
{
    using System.Collections.ObjectModel;
    using ModuleManager.ModuleObjects.Classes;

    /// <summary>
    /// An Assembly Collection.
    /// </summary>
    public interface IAssemblyCollectionService
    {
        /// <summary>
        /// Gets or sets a <see cref="ObservableCollection{AssemblyData}"/>.
        /// </summary>
        public ObservableCollection<AssemblyData> Assemblies { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="ModuleManagerCatalog"/> objects.
        /// </summary>
        public ModuleManagerCatalog AssemblyCatalog { get; set; }

        /// <summary>
        /// Gets or sets an <see cref="object"/> object hopefully.
        /// </summary>
        public object SelectedItem { get; set; }

        /// <summary>
        /// Gets or sets name of the selected item.
        /// </summary>
        public string SelectedItemName { get; set; }

        /// <summary>
        /// Adds all modules stored in the <see cref="AssemblyData"/> ModuleType properties.
        /// </summary>
        public void AddModulesToCatalog();
    }
}