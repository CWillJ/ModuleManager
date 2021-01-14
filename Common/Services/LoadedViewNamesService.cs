namespace ModuleManager.Common.Services
{
    using System.Collections.ObjectModel;
    using ModuleManager.Common.Interfaces;

    /// <inheritdoc cref="ILoadedViewNamesService"/>
    public class LoadedViewNamesService : ILoadedViewNamesService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoadedViewNamesService"/> class.
        /// </summary>
        public LoadedViewNamesService()
        {
            LoadedViewNames = new ObservableCollection<string>();
        }

        /// <inheritdoc/>
        public ObservableCollection<string> LoadedViewNames { get; set; }
    }
}