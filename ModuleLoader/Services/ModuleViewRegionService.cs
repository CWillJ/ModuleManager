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
        private readonly IRegionManager _regionManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleViewRegionService"/> class.
        /// </summary>
        /// <param name="regionManager"><see cref="IRegionManager"/>.</param>
        public ModuleViewRegionService(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            RegionName = @"AssemblyDataRegion";
        }

        /// <inheritdoc cref="IModuleViewRegionService"/>
        public string RegionName { get; set; }

        /// <inheritdoc cref="IModuleViewRegionService"/>
        public IRegionManager RegionManager
        {
            get { return _regionManager; }
        }

        /// <inheritdoc cref="IModuleViewRegionService"/>
        public void AddViewToRegion(Type view)
        {
            ////RegionManager.RegisterViewWithRegion(RegionName, view);
        }

        /// <inheritdoc cref="IModuleViewRegionService"/>
        public void RemoveViewFromRegion(Type view)
        {
            return;
        }
    }
}