namespace ModuleManager.Common.Interfaces
{
    using System.Collections.ObjectModel;
    using ModuleManager.Common.Classes.Data;

    /// <summary>
    /// Service providing concrete <see cref="IViewCollectionService"/> implementations.
    /// </summary>
    public interface IViewCollectionService
    {
        /// <summary>
        /// Gets the <see cref="ObservableCollection{ViewData}"/> containing an assembly name and a collection
        /// of associated view objects.
        /// </summary>
        public ObservableCollection<ViewData> ViewDataCollection { get; }

        /// <summary>
        /// Gets or sets the selected <see cref="object"/>.
        /// </summary>
        public object SelectedView { get; set; }

        /// <summary>
        /// Gets or sets the selected <see cref="string"/>.
        /// </summary>
        public string SelectedViewName { get; set; }

        /// <summary>
        /// Adds a view <see cref="object"/> to the collection.
        /// </summary>
        /// <param name="viewObject">The view <see cref="object"/> to add to the collection.</param>
        public void AddView(object viewObject);

        /// <summary>
        /// Removes a view <see cref="object"/> from the collection if it exists.
        /// </summary>
        /// <param name="viewObject">The view <see cref="object"/> to remove from the collection.</param>
        public void RemoveView(object viewObject);

        /// <summary>
        /// Return true if the assembly name is found in the <see cref="ViewData"/> collection.
        /// </summary>
        /// <param name="assemblyName">The <see cref="string"/> assembly name.</param>
        /// <returns>True if the assembly name exists in the <see cref="ViewData"/> collection.</returns>
        public bool CollectionContainsAssemblyName(string assemblyName);

        /// <summary>
        /// Return true if, a) the assembly name exists in the <see cref="ViewData"/> colleciton and, b) the
        /// view object exsists in the associated view object collection.
        /// </summary>
        /// <param name="assemblyName">The <see cref="string"/> name of the assembly to check.</param>
        /// <param name="viewObject">The view <see cref="object"/> to check.</param>
        /// <returns>True if both the assembly name and the view object exist in the <see cref="ViewData"/> collection.</returns>
        public bool ViewDataAssemblyNameContainsViewObject(string assemblyName, object viewObject);

        /// <summary>
        /// Gets a view object with the associated <see cref="string"/> name from the collection.
        /// </summary>
        /// <param name="viewName">The <see cref="string"/> name of the view object.</param>
        /// <returns>The found view object or null if not found.</returns>
        public object GetViewObjectByName(string viewName);
    }
}
