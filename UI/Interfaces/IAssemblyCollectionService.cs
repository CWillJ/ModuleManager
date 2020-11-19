namespace ModuleManager.UI.Interfaces
{
    using ModuleManager.ModuleObjects.Classes;
    using System.Collections.ObjectModel;

    /// <summary>
    /// An Assembly Collection.
    /// </summary>
    public interface IAssemblyCollectionService
    {
        /// <summary>
        /// Gets or sets a <see cref="ObservableCollection{AssemblyData}"/> objects.
        /// </summary>
        public ObservableCollection<AssemblyData> Assemblies { get; set; }

        /// <summary>
        /// Gets or sets an <see cref="object"/> object hopefully.
        /// </summary>
        public object SelectedItem { get; set; }
    }
}