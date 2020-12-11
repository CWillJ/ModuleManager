namespace ModuleManager.Common.Services
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Xml.Serialization;
    using ModuleManager.Common.Classes;
    using ModuleManager.Common.Interfaces;
    using Prism.Mvvm;
    using Prism.Regions;

    /// <summary>
    /// Service providing concrete <see cref="IAssemblyCollectionService"/> implementations.
    /// </summary>
    public class AssemblyCollectionService : BindableBase, IAssemblyCollectionService
    {
        private readonly IAssemblyLoaderService _assemblyLoaderService;
        private readonly IRegionManager _regionManager;

        private ObservableCollection<AssemblyData> _assemblies;
        private object _selectedItem;
        private string _selectedItemName;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyCollectionService"/> class.
        /// </summary>
        /// <param name="regionManager">The <see cref="IRegionManager"/>.</param>
        /// <param name="asssemblyLoaderService">The <see cref="IAssemblyLoaderService"/>.</param>
        public AssemblyCollectionService(IRegionManager regionManager, IAssemblyLoaderService asssemblyLoaderService)
        {
            _regionManager = regionManager;
            _assemblyLoaderService = asssemblyLoaderService ?? throw new ArgumentNullException("AssemblyLoaderService");

            _assemblies = new ObservableCollection<AssemblyData>();
            _selectedItem = null;
            _selectedItemName = @"Description";
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
            }
        }

        /// <inheritdoc cref="IAssemblyCollectionService"/>
        public string SelectedItemName
        {
            get { return _selectedItemName; }
            set { SetProperty(ref _selectedItemName, value); }
        }

        /// <inheritdoc cref="IAssemblyCollectionService"/>
        public void PopulateAssemblyCollection(string[] dllFiles)
        {
            Assemblies = _assemblyLoaderService.GetAssemblies(dllFiles);
        }

        /// <summary>
        /// Serializes the module catalog to an xml file.
        /// </summary>
        /// <param name="fileName">The full file path and name.</param>
        /// <returns>True if can be serialized, false otherwise.</returns>
        public bool SerializeToXML(string fileName)
        {
            Type assemblyType = typeof(ObservableCollection<AssemblyData>);
            XmlSerializer serializer;

            try
            {
                serializer = new XmlSerializer(assemblyType);
            }
            catch (Exception)
            {
                return false;
            }

            using StreamWriter wr = new StreamWriter(fileName);
            serializer.Serialize(wr, Assemblies);
            wr.Close();

            return true;
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

                _assemblyLoaderService.LoadUnload(ref assembly);
                Assemblies[i] = assembly;
            }
        }
    }
}