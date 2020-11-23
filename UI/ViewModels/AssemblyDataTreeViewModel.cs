﻿namespace ModuleManager.UI.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Xml.Serialization;
    using ModuleManager.ModuleLoader.Interfaces;
    using ModuleManager.ModuleObjects.Classes;
    using ModuleManager.UI.Interfaces;

    /// <summary>
    /// View model for module area.
    /// </summary>
    public class AssemblyDataTreeViewModel
    {
        private readonly IModuleManagerCollectionService _moduleManagerCollectionService;
        private readonly IAssemblyLoaderService _assemblyLoaderService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyDataTreeViewModel"/> class.
        /// </summary>
        /// <param name="assemblyLoaderService">Injected <see cref="IAssemblyLoaderService"/>.</param>
        /// <param name="moduleManagerCollectionService">Injected <see cref="IModuleManagerCollectionService"/>.</param>
        public AssemblyDataTreeViewModel(IAssemblyLoaderService assemblyLoaderService, IModuleManagerCollectionService moduleManagerCollectionService)
        {
            _moduleManagerCollectionService = moduleManagerCollectionService ?? throw new ArgumentNullException("ModuleManagerCollectionService");
            _assemblyLoaderService = assemblyLoaderService ?? throw new ArgumentNullException("ModuleInfoRetriever");

            // Load previously saved module configuration if the ConfigFile exists
            if (File.Exists(Directory.GetCurrentDirectory() + @"\ConfigFile.xml"))
            {
                _moduleManagerCollectionService.Assemblies = LoadConfig();
            }
        }

        /// <summary>
        /// Gets a collection of ModuleManager.ModuleObjects.Classes.AssemblyData.
        /// </summary>
        public IModuleManagerCollectionService ModuleManagerCollectionService
        {
            get { return _moduleManagerCollectionService; }
        }

        /// <summary>
        /// LoadConfig will load an ObservableCollection of AssemblyData from an xml file.
        /// </summary>
        /// <returns>An <see cref="ObservableCollection{AssemblyData}"/>.</returns>
        private ObservableCollection<AssemblyData> LoadConfig()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<AssemblyData>));
            ObservableCollection<AssemblyData> assemblies = new ObservableCollection<AssemblyData>();
            string loadFile = Directory.GetCurrentDirectory() + @"\ConfigFile.xml";

            ////RadOpenFileDialog openFileDialog = new RadOpenFileDialog
            ////{
            ////    InitialDirectory = Directory.GetCurrentDirectory(),
            ////    Filter = "xml files (*.xml)|*.xml",
            ////    Header = "Load Configuration File",
            ////    RestoreDirectory = true,
            ////};

            ////openFileDialog.ShowDialog();

            ////if (openFileDialog.DialogResult == true)
            ////{
            ////    loadFile = openFileDialog.FileName;
            ////}

            ////if (loadFile == string.Empty)
            ////{
            ////    return assemblies;
            ////}

            using (StreamReader rd = new StreamReader(loadFile))
            {
                try
                {
                    assemblies = serializer.Deserialize(rd) as ObservableCollection<AssemblyData>;
                }
                catch (InvalidOperationException)
                {
                    // There is something wrong with the xml file.
                    // Return an empty collection of assemblies.
                    return assemblies;
                }
            }

            // Load and get data.
            _assemblyLoaderService.LoadAll(ref assemblies);

            // Unload all disabled assemblies.
            _assemblyLoaderService.LoadUnload(ref assemblies);

            // Add all modules to the module catalog
            ModuleManagerCollectionService.AddModulesToCatalog();

            return assemblies;
        }
    }
}