namespace ModuleManager.Common.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Prism.Modularity;

    /// <summary>
    /// A basic aggregation of IModuleCatalogs for quickstart purposes.
    /// </summary>
    public class AggregateModuleCatalog : IModuleCatalog
    {
        private List<IModuleCatalog> _catalogs = new List<IModuleCatalog>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateModuleCatalog"/> class.
        /// </summary>
        public AggregateModuleCatalog()
        {
            _catalogs.Add(new ModuleCatalog());
        }

        /// <summary>
        /// Gets or sets the collection of catalogs.
        /// </summary>
        public List<IModuleCatalog> Catalogs
        {
            get { return _catalogs; }
            set { _catalogs = value; }
        }

        /// <summary>
        /// Gets all the <see cref="IModuleInfo"/> classes that are in the <see cref="ModuleCatalog"/>.
        /// </summary>
        public IEnumerable<IModuleInfo> Modules
        {
            get
            {
                return Catalogs.SelectMany(x => x.Modules);
            }
        }

        /// <summary>
        /// Return the list of <see cref="IModuleInfo"/> that the provided module depends on.
        /// </summary>
        /// <param name="moduleInfo">The <see cref="IModuleInfo"/> to get the dependent modules of.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> of <see cref="IModuleInfo"/> dependencies of the provided module.
        /// </returns>
        public IEnumerable<IModuleInfo> GetDependentModules(IModuleInfo moduleInfo)
        {
            var catalog = _catalogs.Single(x => x.Modules.Contains(moduleInfo));
            return catalog.GetDependentModules(moduleInfo);
        }

        /// <summary>
        /// Gets the complete <see cref="IEnumerable{T}"/> of <see cref="IModuleInfo"/> with dependencies for the provided <see cref="IEnumerable{T}"/>
        /// of <see cref="IModuleInfo"/>.
        /// </summary>
        /// <param name="modules">The modules to get the dependencies of.</param>
        /// <returns>
        /// A complete <see cref="IEnumerable{T}"/> of provided <see cref="IModuleInfo"/> and their dependencies.
        /// </returns>
        public IEnumerable<IModuleInfo> CompleteListWithDependencies(IEnumerable<IModuleInfo> modules)
        {
            var modulesGroupedByCatalog = modules.GroupBy<IModuleInfo, IModuleCatalog>(module => _catalogs.Single(catalog => catalog.Modules.Contains(module)));
            return modulesGroupedByCatalog.SelectMany(x => x.Key.CompleteListWithDependencies(x));
        }

        /// <summary>
        /// Initializes the catalog, which may load and validate the modules.
        /// </summary>
        public void Initialize()
        {
            foreach (var catalog in Catalogs)
            {
                catalog.Initialize();
            }
        }

        /// <summary>
        /// Adds the catalog to the list of catalogs.
        /// </summary>
        /// <param name="catalog">The catalog to add.</param>
        public void AddCatalog(IModuleCatalog catalog)
        {
            if (catalog == null)
            {
                throw new ArgumentNullException("catalog");
            }

            _catalogs.Add(catalog);
        }

        /// <summary>
        /// Adds an <see cref="IModuleInfo"/> to the <see cref="ModuleCatalog"/>.
        /// </summary>
        /// <param name="moduleInfo">The <see cref="IModuleInfo"/> to add.</param>
        /// <returns> The updated <see cref="IModuleCatalog"/>.</returns>
        public IModuleCatalog AddModule(IModuleInfo moduleInfo)
        {
            return _catalogs[0].AddModule(moduleInfo);
        }

        /// <summary>
        /// Removes an <see cref="IModuleInfo"/> from the <see cref="ModuleCatalog"/>.
        /// </summary>
        /// <param name="moduleInfo">The <see cref="IModuleInfo"/> to remove.</param>
        /// <returns> The updated <see cref="IModuleCatalog"/>.</returns>
        public IModuleCatalog RemoveModule(IModuleInfo moduleInfo)
        {
            var catalog = new ModuleCatalog();

            foreach (var module in _catalogs[0].Modules)
            {
                if (moduleInfo != module)
                {
                    catalog.AddModule(module);
                }
            }

            return catalog;
        }
    }
}