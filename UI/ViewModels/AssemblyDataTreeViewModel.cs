namespace ModuleManager.UI.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Xml.Serialization;
    using ModuleManager.ModuleLoader.Interfaces;
    using ModuleManager.ModuleObjects.Classes;
    using ModuleManager.UI.Interfaces;
    using Prism.Mvvm;

    /// <summary>
    /// View model for module area.
    /// </summary>
    public class AssemblyDataTreeViewModel : BindableBase
    {
        private readonly IAssemblyLoaderService _assemblyLoaderService;
        private IAssemblyCollectionService _assemblyCollectionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyDataTreeViewModel"/> class.
        /// </summary>
        /// <param name="assemblyLoaderService">ModuleInfoRetriever.</param>
        /// <param name="assemblyCollectionService">IAssemblyCollectionService.</param>
        public AssemblyDataTreeViewModel(IAssemblyLoaderService assemblyLoaderService, IAssemblyCollectionService assemblyCollectionService)
        {
            _assemblyCollectionService = assemblyCollectionService ?? throw new ArgumentNullException("AssemblyCollectionService");
            _assemblyLoaderService = assemblyLoaderService ?? throw new ArgumentNullException("ModuleInfoRetriever");

            // Load previously saved module configuration if the ConfigFile exists
            if (File.Exists(Directory.GetCurrentDirectory() + @"\ConfigFile.xml"))
            {
                AssemblyCollectionService.Assemblies = LoadConfig();
            }
        }

        /// <summary>
        /// Gets or sets a collection of ModuleManager.ModuleObjects.Classes.AssemblyData.
        /// </summary>
        public IAssemblyCollectionService AssemblyCollectionService
        {
            get { return _assemblyCollectionService; }
            set { SetProperty(ref _assemblyCollectionService, value); }
        }

        /// <summary>
        /// LoadConfig will load an ObservableCollection of AssemblyData from an xml file.
        /// </summary>
        /// <returns>A collection of AssemblyData objects.</returns>
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

            _assemblyLoaderService.LoadAll(ref assemblies);
            _assemblyLoaderService.LoadUnload(ref assemblies);

            return assemblies;
        }

        /// <summary>
        /// Can always execute.
        /// </summary>
        /// <returns>True.</returns>
        private bool CanExecute()
        {
            return true;
        }
    }
}