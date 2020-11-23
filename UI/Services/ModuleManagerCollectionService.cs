namespace ModuleManager.UI.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Xml.Serialization;
    using ModuleManager.ModuleLoader.Interfaces;
    using ModuleManager.ModuleObjects.Classes;
    using ModuleManager.UI.Interfaces;
    using Prism.Modularity;
    using Prism.Mvvm;

    /// <summary>
    /// Service providing concrete <see cref="IModuleManagerCollectionService"/> implementations.
    /// </summary>
    public class ModuleManagerCollectionService : BindableBase, IModuleManagerCollectionService
    {
        private readonly IAssemblyLoaderService _assemblyLoaderService;

        private ObservableCollection<AssemblyData> _assemblies;
        private ModuleManagerCatalog _assemblyCatalog;
        private object _selectedItem;
        private string _selectedItemName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleManagerCollectionService"/> class.
        /// </summary>
        /// <param name="asssemblyLoaderService">The <see cref="IAssemblyLoaderService"/>.</param>
        public ModuleManagerCollectionService(IAssemblyLoaderService asssemblyLoaderService)
        {
            _assemblies = new ObservableCollection<AssemblyData>();
            _assemblyCatalog = new ModuleManagerCatalog();
            _selectedItem = null;
            _selectedItemName = @"Description";

            _assemblyLoaderService = asssemblyLoaderService;
        }

        /// <inheritdoc cref="IModuleManagerCollectionService"/>
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

        /// <inheritdoc cref="IModuleManagerCollectionService"/>
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

        /// <inheritdoc cref="IModuleManagerCollectionService"/>
        public string SelectedItemName
        {
            get { return _selectedItemName; }
            set { SetProperty(ref _selectedItemName, value); }
        }

        /// <inheritdoc cref="IModuleManagerCollectionService"/>
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