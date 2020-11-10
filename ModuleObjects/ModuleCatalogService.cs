namespace ModuleManager.ModuleObjects
{
    using Prism.Modularity;
    using Prism.Mvvm;

    /// <summary>
    /// The <see cref="ModuleCatalog"/> holding the loaded modules.
    /// </summary>
    public class ModuleCatalogService : BindableBase
    {
        private ModuleCatalog _moduleCatalog;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleCatalogService"/> class.
        /// </summary>
        public ModuleCatalogService()
        {
            _moduleCatalog = new ModuleCatalog();
        }

        /// <summary>
        /// Gets or sets the ModuleCatalog.
        /// </summary>
        public ModuleCatalog ModuleCatalog
        {
            get { return _moduleCatalog; }
            set { SetProperty(ref _moduleCatalog, value); }
        }
    }
}