namespace ModuleManager.ModuleLoader.Interfaces
{
    using System;
    using Prism.Regions;

    /// <summary>
    /// Service providing concrete <see cref="IModuleViewRegionService"/> implementations.
    /// </summary>
    public interface IModuleViewRegionService
    {
        /// <summary>
        /// Gets the <see cref="RegionManager"/> property.
        /// </summary>
        public IRegionManager RegionManager { get; }

        /// <summary>
        /// Gets or sets the region to store the view in.
        /// </summary>
        public string RegionName { get; set; }

        /// <summary>
        /// Adds the view to the <see cref="RegionManager"/>.
        /// </summary>
        /// <param name="view">The <see cref="Type"/> to add to the <see cref="RegionManager"/>.</param>
        public void AddViewToRegion(Type view);

        /// <summary>
        /// Removes the view to the <see cref="RegionManager"/>.
        /// </summary>
        /// <param name="view">The <see cref="Type"/> to remove from the <see cref="RegionManager"/>.</param>
        public void RemoveViewFromRegion(Type view);
    }
}