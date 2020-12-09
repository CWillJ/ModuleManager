namespace ModuleManager.UI.Services
{
    using System;
    using System.Collections.ObjectModel;
    using ModuleManager.ModuleObjects.Classes;
    using ModuleManager.UI.Interfaces;
    using Prism.Ioc;
    using Prism.Regions;

    /// <summary>
    /// Service providing concrete <see cref="ILoadedViewsService"/> implementations.
    /// </summary>
    public class LoadedViewsService : ILoadedViewsService
    {
        private readonly IAssemblyCollectionService _assemblyCollectionService;
        private IRegionManager _regionManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadedViewsService"/> class.
        /// </summary>
        /// <param name="assemblyCollectionService">The <see cref="IAssemblyCollectionService"/>.</param>
        /// <param name="regionManager">The <see cref="IRegionManager"/>.</param>
        /// <param name="containerProvider">The <see cref="IContainerProvider"/>.</param>
        public LoadedViewsService(IAssemblyCollectionService assemblyCollectionService, IRegionManager regionManager, IContainerProvider containerProvider)
        {
            _assemblyCollectionService = assemblyCollectionService;
            _regionManager = regionManager;
        }

        /// <inheritdoc cref="ILoadedViewsService"/>
        public ObservableCollection<object> LoadedViews { get; set; }

        /// <inheritdoc cref="ILoadedViewsService"/>
        public void LoadViewsFromAssemblyService()
        {
            foreach (var assemblyData in _assemblyCollectionService.Assemblies)
            {
                AddViewsFromAssemblyData(assemblyData);
            }
        }

        /// <inheritdoc cref="ILoadedViewsService"/>
        public void AddViewsFromAssemblyData(AssemblyData assemblyData)
        {
            foreach (var typeData in assemblyData.Types)
            {
                AddTypeDataIfIsView(typeData);
            }
        }

        /// <inheritdoc cref="ILoadedViewsService"/>
        public void RemoveViewsFromAssemblyData(AssemblyData assemblyData)
        {
            foreach (var typeData in assemblyData.Types)
            {
                RemoveTypeDataIfIsView(typeData);
            }
        }

        /// <summary>
        /// Adds the <see cref="TypeData"/>'s <see cref="Type"/> to LoadedViews property if IsView is true.
        /// </summary>
        /// <param name="typeData">The <see cref="TypeData"/> to get the <see cref="Type"/> from to add to LoadedViews.</param>
        private void AddTypeDataIfIsView(TypeData typeData)
        {
            if (typeData.IsView)
            {
                AddViewFromType(typeData.Type);
            }
        }

        /// <summary>
        /// Removes the <see cref="TypeData"/>'s <see cref="Type"/> from LoadedViews property if IsView is true.
        /// </summary>
        /// <param name="typeData">The <see cref="TypeData"/> to get the <see cref="Type"/> from to remove from LoadedViews.</param>
        private void RemoveTypeDataIfIsView(TypeData typeData)
        {
            if (typeData.IsView)
            {
                RemoveViewFromType(typeData.Type);
            }
        }

        /// <summary>
        /// Adds a view <see cref="Type"/> to the LoadedViews collection.
        /// </summary>
        /// <param name="type">The view <see cref="Type"/> to be added.</param>
        private void AddViewFromType(Type type)
        {
            ////IRegion region = _regionManager.Regions[@"LoadedViewDisplayRegion"];

            ////_regionManager.AddToRegion("LoadedViewDisplayRegion", type);
            LoadedViews.Add(type);
        }

        /// <summary>
        /// Removes a view <see cref="Type"/> from the LoadedViews collection.
        /// </summary>
        /// <param name="type">The view <see cref="Type"/> to be removed.</param>
        private void RemoveViewFromType(Type type)
        {
            LoadedViews.Remove(type);
        }
    }
}