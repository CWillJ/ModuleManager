namespace ModuleManager.UI.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Xml.Serialization;
    using ModuleManager.ModuleObjects.Classes;
    using ModuleManager.UI.Events;
    using Prism.Events;
    using Prism.Mvvm;

    /// <summary>
    /// View model for module area.
    /// </summary>
    public class ModuleAreaViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private ObservableCollection<AssemblyData> _assemblies;
        private ObservableCollection<ModuleData> _modules;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleAreaViewModel"/> class.
        /// </summary>
        /// <param name="eventAggregator">Event aggregator.</param>
        public ModuleAreaViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            eventAggregator.GetEvent<UpdateAssemblyCollectionEvent>().Subscribe(AssemblyCollectionUpdated);

            // Load previously saved module configuration if the ConfigFile exists
            if (File.Exists(Directory.GetCurrentDirectory() + @"\ConfigFile.xml"))
            {
                ObservableCollection<AssemblyData> assemblies = LoadConfig();
                _eventAggregator.GetEvent<UpdateAssemblyCollectionEvent>().Publish(assemblies);
            }
        }

        /// <summary>
        /// Gets or sets a collection of ModuleManager.ModuleObjects.Classes.AssemblyData.
        /// </summary>
        public ObservableCollection<AssemblyData> Assemblies
        {
            get { return _assemblies; }
            set { SetProperty(ref _assemblies, value); }
        }

        /// <summary>
        /// Gets or sets a collection of ModuleManager.ModuleObjects.Classes.ModuleData.
        /// </summary>
        public ObservableCollection<ModuleData> Modules
        {
            get { return _modules; }
            set { SetProperty(ref _modules, value); }
        }

        private void ModuleCollectionUpdated(ObservableCollection<ModuleData> modules)
        {
            Modules = modules;
        }

        private void AssemblyCollectionUpdated(ObservableCollection<AssemblyData> assemblies)
        {
            Assemblies = assemblies;
        }

        /// <summary>
        /// LoadConfig will load an ObservableCollection of type Module from an xml file.
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
            ////    return modules;
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
                    // Return an empty collection of modules.
                    return assemblies;
                }
            }

            return assemblies;
        }
    }
}