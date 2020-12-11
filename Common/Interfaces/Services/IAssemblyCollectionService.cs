namespace ModuleManager.Common.Interfaces
{
    using System.Collections.ObjectModel;
    using ModuleManager.Common.Classes;

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
        /// Gets or sets an <see cref="object"/> object hopefully.
        /// </summary>
        public object SelectedItem { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="string"/> name of the selected item.
        /// </summary>
        public string SelectedItemName { get; set; }

        /// <summary>
        /// Uses the <see cref="IAssemblyLoaderService"/> to populate the <see cref="ObservableCollection{AssemblyData}"/>.
        /// </summary>
        /// <param name="dllFiles">A <see cref="string"/> array that contains all dll files to load.</param>
        public void PopulateAssemblyCollection(string[] dllFiles);
    }
}