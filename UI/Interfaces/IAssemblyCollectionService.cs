namespace ModuleManager.UI.Interfaces
{
    using System.Collections.ObjectModel;
    using ModuleManager.ModuleObjects.Classes;
    using ModuleManager.ModuleObjects.Interfaces;

    /// <summary>
    /// An Assembly Collection.
    /// </summary>
    public interface IAssemblyCollectionService
    {
        /// <summary>
        /// Gets or sets a collection of <see cref="AssemblyData"/> objects.
        /// </summary>
        public ObservableCollection<AssemblyData> Assemblies { get; set; }

        /// <summary>
        /// Gets or sets an <see cref="ITreeViewData"/> object hopefully.
        /// </summary>
        public ITreeViewData SelectedItem { get; set; }
    }
}