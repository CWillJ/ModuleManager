namespace ModuleManager.ModuleLoader.Services
{
    using System;
    using ModuleManager.ModuleLoader.Interfaces;
    using Prism.Regions;

    /// <summary>
    /// Service providing concrete <see cref="IModuleViewRegionService"/> implementations.
    /// </summary>
    public class ModuleViewRegionService : IModuleViewRegionService
    {
        private readonly RegionManager _regionManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleViewRegionService"/> class.
        /// </summary>
        public ModuleViewRegionService()
        {
            _regionManager = new RegionManager();
            Region = @"ButtonsRegion";
        }

        /// <inheritdoc cref="IModuleViewRegionService"/>
        public string Region { get; set; }

        /// <inheritdoc cref="IModuleViewRegionService"/>
        public RegionManager RegionManager
        {
            get { return _regionManager; }
        }

        /// <inheritdoc cref="IModuleViewRegionService"/>
        public void AddViewToRegion(Type view)
        {
            ////RegionManager.RegisterViewWithRegion(Region, view);
        }

        /// <inheritdoc cref="IModuleViewRegionService"/>
        public void RemoveViewFromRegion(Type view)
        {
            return;
        }
    }
}