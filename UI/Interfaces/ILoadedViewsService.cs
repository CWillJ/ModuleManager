namespace ModuleManager.UI.Interfaces
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Manages the collection of loaded views.
    /// </summary>
    public interface ILoadedViewsService
    {
        /// <summary>
        /// Gets or sets a collection of loaded view types.
        /// </summary>
        public ObservableCollection<object> LoadedViews { get; set; }

        /// <summary>
        /// Loads all views from any loaded assemblies in the <see cref="IAssemblyCollectionService"/>.
        /// </summary>
        public void LoadViewsFromAssemblyService();
    }
}
