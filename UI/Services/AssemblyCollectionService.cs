namespace ModuleManager.UI.Services
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using ModuleManager.ModuleLoader.Interfaces;
    using ModuleManager.ModuleObjects.Classes;
    using ModuleManager.ModuleObjects.Interfaces;
    using ModuleManager.UI.Interfaces;
    using Prism.Mvvm;

    /// <summary>
    /// Service providing concrete <see cref="IAssemblyCollectionService"/> implementations.
    /// </summary>
    public class AssemblyCollectionService : BindableBase, IAssemblyCollectionService
    {
        private readonly IModuleCatalogService _moduleCatalogService;
        private readonly IAssemblyLoaderService _assemblyLoaderService;

        private ObservableCollection<AssemblyData> _assemblies;
        private object _selectedItem;
        private string _selectedItemName;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyCollectionService"/> class.
        /// </summary>
        /// <param name="moduleCatalogService">The program's <see cref="IModuleCatalogService"/>.</param>
        /// <param name="asssemblyLoaderService">The <see cref="IAssemblyLoaderService"/>.</param>
        public AssemblyCollectionService(IModuleCatalogService moduleCatalogService, IAssemblyLoaderService asssemblyLoaderService)
        {
            _assemblies = new ObservableCollection<AssemblyData>();
            _selectedItem = null;
            _selectedItemName = @"Description";

            _moduleCatalogService = moduleCatalogService;
            _assemblyLoaderService = asssemblyLoaderService;
        }

        /// <inheritdoc cref="IAssemblyCollectionService"/>
        public ObservableCollection<AssemblyData> Assemblies
        {
            get { return _assemblies; }
            set { SetProperty(ref _assemblies, value, CollectionPropertyChanged); }
        }

        /// <inheritdoc cref="IAssemblyCollectionService"/>
        public object SelectedItem
        {
            get
            {
                return _selectedItem;
            }

            set
            {
                SetProperty(ref _selectedItem, value);
                SetSelectedItemName();
                LoadModuleFromCatalog();
            }
        }

        /// <summary>
        /// Gets or sets name of the selected item.
        /// </summary>
        public string SelectedItemName
        {
            get { return _selectedItemName; }
            set { SetProperty(ref _selectedItemName, value); }
        }

        private void SetSelectedItemName()
        {
            if (SelectedItem is AssemblyData assembly)
            {
                SelectedItemName = assembly.Name;
            }
            else if (SelectedItem is ModuleData module)
            {
                SelectedItemName = module.Name;
            }
            else if (SelectedItem is ModuleMemberData member)
            {
                SelectedItemName = member.Name;
            }
            else
            {
                SelectedItemName = @"Description";
            }
        }

        private void LoadModuleFromCatalog()
        {
            if (SelectedItem is AssemblyData assembly)
            {
                if (assembly.IsEnabled)
                {
                    //// _moduleCatalogService.TheModuleManagerCatalog.Initialize();
                }
            }
        }

        private void CollectionPropertyChanged()
        {
            foreach (AssemblyData item in Assemblies)
            {
                item.PropertyChanged += (s, e) => LoadUnload(s, e);
            }
        }

        private void LoadUnload(object s, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == @"IsEnabled")
            {
                AssemblyData assembly = (AssemblyData)s;
                int i = Assemblies.IndexOf(assembly);

                _assemblyLoaderService.LoadUnload(ref assembly);
                Assemblies[i] = assembly;
            }
        }
    }
}