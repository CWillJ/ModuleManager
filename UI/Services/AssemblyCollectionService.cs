namespace ModuleManager.UI.Services
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Xml.Serialization;
    using ModuleManager.ModuleLoader.Interfaces;
    using ModuleManager.ModuleObjects.Classes;
    using ModuleManager.UI.Interfaces;
    using ModuleManager.UI.Views;
    using Prism.Ioc;
    using Prism.Modularity;
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
        private ModuleManagerCatalog _assemblyCatalog;
        private object _selectedItem;
        private string _selectedItemName;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyCollectionService"/> class.
        /// </summary>
        /// <param name="asssemblyLoaderService">The <see cref="IAssemblyLoaderService"/>.</param>
        /// <param name="regionManager">The <see cref="IRegionManager"/>.</param>
        public AssemblyCollectionService(IAssemblyLoaderService asssemblyLoaderService, IRegionManager regionManager)
        {
            _assemblies = new ObservableCollection<AssemblyData>();
            _assemblyCatalog = new ModuleManagerCatalog();
            _selectedItem = null;
            _selectedItemName = @"Description";

            _assemblyLoaderService = asssemblyLoaderService;
            _regionManager = regionManager;
        }

        /// <inheritdoc cref="IAssemblyCollectionService"/>
        public ObservableCollection<AssemblyData> Assemblies
        {
            get { return _assemblies; }
            set { SetProperty(ref _assemblies, value, CollectionPropertyChanged); }
        }

        /// <summary>
        /// Gets or sets a <see cref="ModuleManagerCatalog"/>.
        /// </summary>
        public ModuleManagerCatalog AssemblyCatalog
        {
            get { return _assemblyCatalog; }
            set { _assemblyCatalog = value; }
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
                UpdateDescriptionRegion();
            }
        }

        /// <inheritdoc cref="IAssemblyCollectionService"/>
        public string SelectedItemName
        {
            get { return _selectedItemName; }
            set { SetProperty(ref _selectedItemName, value); }
        }

        /// <inheritdoc cref="IAssemblyCollectionService"/>
        public void AddModulesToCatalog()
        {
            foreach (AssemblyData assembly in Assemblies)
            {
                if (assembly.ModuleType != null)
                {
                    AssemblyCatalog.AddModule(assembly.ModuleType);
                }
            }
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

        private void UpdateDescriptionRegion()
        {
            if (SelectedItem is ModuleData moduleData)
            {
                if (moduleData.IsView)
                {
                    try
                    {
                        _regionManager.RegisterViewWithRegion(@"ModuleDataViewRegion", moduleData.Type);
                    }
                    catch (Exception)
                    {
                        _regionManager.RegisterViewWithRegion(@"ModuleDataViewRegion", typeof(CannotDisplayView));
                    }
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

        private void PopulateModuleCatalog(AssemblyData assembly)
        {
            ObservableCollection<ModuleInfo> moduleCatalog = (ObservableCollection<ModuleInfo>)AssemblyCatalog.Modules;
            moduleCatalog.Add(new ModuleInfo(assembly.ModuleType));
        }
    }
}