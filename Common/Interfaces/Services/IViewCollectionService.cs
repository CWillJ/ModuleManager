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
        /// Gets the <see cref="ObservableCollection{Object}"/>.
        /// </summary>
        public ObservableCollection<object> Views { get; }

        /// <summary>
        /// Adds a view <see cref="object"/> to the <see cref="ViewData"/> collection.
        /// </summary>
        /// <param name="viewObject">The view <see cref="object"/> to add to the collection.</param>
        public void AddView(object viewObject);

        /// <summary>
        /// Removes a view <see cref="object"/> from the <see cref="ViewData"/> collection if it exists.
        /// </summary>
        /// <param name="viewObject">The view <see cref="object"/> to remove from the collection.</param>
        public void RemoveView(object viewObject);
    }
}