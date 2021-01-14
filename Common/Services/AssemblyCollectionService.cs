namespace ModuleManager.Common.Services
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using ModuleManager.Common.Classes;
    using ModuleManager.Common.Interfaces;
    using Prism.Mvvm;

    /// <inheritdoc cref="IAssemblyCollectionService"/>
    public class AssemblyCollectionService : BindableBase, IAssemblyCollectionService
    {
        private readonly IAssemblyDataLoaderService _assemblyDataLoaderService;
        private readonly IModuleCatalogService _moduleCatalogService;
        private ObservableCollection<AssemblyData> _assemblies;
        private object _selectedItem;
        private string _selectedItemName;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyCollectionService"/> class.
        /// </summary>
        /// <param name="assemblyDataLoaderService">The <see cref="IAssemblyDataLoaderService"/>.</param>
        /// <param name="moduleCatalogService"> The <see cref="IModuleCatalogService"/>.</param>
        public AssemblyCollectionService(IAssemblyDataLoaderService assemblyDataLoaderService, IModuleCatalogService moduleCatalogService)
        {
            _assemblyDataLoaderService = assemblyDataLoaderService ?? throw new ArgumentNullException("AssemblyCollectionService");
            _moduleCatalogService = moduleCatalogService ?? throw new ArgumentNullException("ModuleCatalogService");
            _assemblies = new ObservableCollection<AssemblyData>();
            _selectedItem = null;
            _selectedItemName = @"Description";
        }

        /// <inheritdoc/>
        public ObservableCollection<AssemblyData> Assemblies
        {
            get { return _assemblies; }
            set { SetProperty(ref _assemblies, value, CollectionPropertyChanged); }
        }

        /// <inheritdoc/>
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
            }
        }

        /// <inheritdoc/>
        public string SelectedItemName
        {
            get { return _selectedItemName; }
            set { SetProperty(ref _selectedItemName, value); }
        }

        /// <inheritdoc/>
        public void PopulateAssemblyCollection(string dllDirectory, string[] dllFiles)
        {
            _assemblyDataLoaderService.DllDirectory = dllDirectory;
            Assemblies = _assemblyDataLoaderService.GetAssemblies(dllFiles);
        }

        /// <summary>
        /// Sets the name of the selected item.
        /// </summary>
        private void SetSelectedItemName()
        {
            if (SelectedItem is AssemblyData assemblyData)
            {
                SelectedItemName = assemblyData.Name + " Assembly Description";
            }
            else if (SelectedItem is TypeData typeData)
            {
                SelectedItemName = typeData.Name + " Type Description";
            }
            else if (SelectedItem is TypeMemberData memberData)
            {
                if (SelectedItem is TypeConstructor)
                {
                    SelectedItemName = memberData.Name + " Constructor Description";
                }
                else if (SelectedItem is TypeProperty)
                {
                    SelectedItemName = memberData.Name + " Property Description";
                }
                else if (SelectedItem is TypeMethod)
                {
                    SelectedItemName = memberData.Name + " Method Description";
                }
                else
                {
                    SelectedItemName = memberData.Name + " Description";
                }
            }
            else
            {
                SelectedItemName = @"Description";
            }
        }

        /// <summary>
        /// The method that handles the AssemblyData collection changed event.
        /// </summary>
        private void CollectionPropertyChanged()
        {
            foreach (AssemblyData assemblyData in Assemblies)
            {
                assemblyData.PropertyChanged += (s, e) => LoadUnload(s, e);
            }
        }

        /// <summary>
        /// Will load or unload the passed in <see cref="AssemblyData"/> <see cref="object"/>.
        /// </summary>
        /// <param name="s">The <see cref="object"/>.</param>
        /// <param name="e">The property changed arguments.</param>
        private void LoadUnload(object s, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == @"IsEnabled")
            {
                AssemblyData assembly = (AssemblyData)s;
                int i = Assemblies.IndexOf(assembly);

                _assemblyDataLoaderService.LoadUnload(ref assembly);
                Assemblies[i] = assembly;

                if (Assemblies[i].IsEnabled)
                {
                    _moduleCatalogService.LoadExpansionModule(Assemblies[i].FilePath);
                }
                else
                {
                    _moduleCatalogService.UnloadExpansionModule(Assemblies[i].ModuleType.Name);
                }
            }
        }
    }
}