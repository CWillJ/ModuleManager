namespace ModuleManager.Common.Interfaces
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Service providing concrete <see cref="ILoadedViewNamesService"/> implementations.
    /// </summary>
    public interface ILoadedViewNamesService
    {
        /// <summary>
        /// Gets or sets a collection of <see cref="string"/> view object names.
        /// </summary>
        public ObservableCollection<string> LoadedViewNames { get; set; }
    }
}