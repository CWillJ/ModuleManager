namespace ModuleManager.Common.Interfaces
{
    using ModuleManager.Common.Classes;

    /// <summary>
    /// Manages the collection of loaded views.
    /// </summary>
    public interface ILoadedViewsService
    {
        /// <summary>
        /// Adds all views from an <see cref="AssemblyData"/> to the LoadedViews property.
        /// </summary>
        /// <param name="assemblyData">The <see cref="AssemblyData"/> to get the views from.</param>
        public void AddViewsFromAssemblyData(AssemblyData assemblyData);

        /// <summary>
        /// Removes all views in an <see cref="AssemblyData"/> from the LoadedViews property.
        /// </summary>
        /// <param name="assemblyData">The <see cref="AssemblyData"/> that holds the views that needs to be removed.</param>
        public void RemoveViewsFromAssemblyData(AssemblyData assemblyData);
    }
}
