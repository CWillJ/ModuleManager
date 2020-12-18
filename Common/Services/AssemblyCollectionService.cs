namespace ModuleManager.Common.Services
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using ModuleManager.Common.Classes;
    using ModuleManager.Common.Interfaces;
    using Prism.Mvvm;
    using Prism.Regions;

    /// <inheritdoc cref="IAssemblyCollectionService"/>
    public class AssemblyCollectionService : BindableBase, IAssemblyCollectionService
    {
        private ObservableCollection<AssemblyData> _assemblies;
        private object _selectedItem;
        private string _selectedItemName;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyCollectionService"/> class.
        /// </summary>
        /// <param name="regionManager">The <see cref="IRegionManager"/>.</param>
        public AssemblyCollectionService(IRegionManager regionManager)
        {
            DataLoader = new AssemblyDataLoader(regionManager);

            _assemblies = new ObservableCollection<AssemblyData>();
            _selectedItem = null;
            _selectedItemName = @"Description";
        }

        /// <inheritdoc/>
        public AssemblyDataLoader DataLoader { get; }

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
            DataLoader.DllDirectory = dllDirectory;
            Assemblies = DataLoader.GetAssemblies(dllFiles);
        }

        /// <summary>
        /// Sets the name of the selected item.
        /// </summary>
        private void SetSelectedItemName()
        {
            if (SelectedItem is AssemblyData assemblyData)
            {
                SelectedItemName = assemblyData.Name;
            }
            else if (SelectedItem is TypeData typeData)
            {
                SelectedItemName = typeData.Name;
            }
            else if (SelectedItem is TypeMemberData memberData)
            {
                SelectedItemName = memberData.Name;
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

                DataLoader.LoadUnload(ref assembly);
                Assemblies[i] = assembly;
            }
        }
    }
}