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
        private ObservableCollection<Module> _modules;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleAreaViewModel"/> class.
        /// </summary>
        /// <param name="eventAggregator">Event aggregator.</param>
        public ModuleAreaViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            eventAggregator.GetEvent<UpdateModuleCollectionEvent>().Subscribe(ModuleCollectionUpdated);

            // Load previously saved module configuration if the ConfigFile exists
            if (File.Exists(Directory.GetCurrentDirectory() + @"\ConfigFile.xml"))
            {
                ObservableCollection<Module> modules = LoadConfig();
                _eventAggregator.GetEvent<UpdateModuleCollectionEvent>().Publish(modules);
            }
        }

        /// <summary>
        /// Gets or sets a collection of ModuleManager.ModuleObjects.Classes.Module.
        /// </summary>
        public ObservableCollection<Module> Modules
        {
            get { return _modules; }
            set { SetProperty(ref _modules, value); }
        }

        private void ModuleCollectionUpdated(ObservableCollection<Module> modules)
        {
            Modules = modules;
        }

        /// <summary>
        /// LoadConfig will load an ObservableCollection of type Module from an xml file.
        /// </summary>
        /// <returns>ObservableCollection of type Module.</returns>
        private ObservableCollection<Module> LoadConfig()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Module>));
            ObservableCollection<Module> modules = new ObservableCollection<Module>();
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
                    modules = serializer.Deserialize(rd) as ObservableCollection<Module>;
                }
                catch (InvalidOperationException)
                {
                    // There is something wrong with the xml file.
                    // Return an empty collection of modules.
                    return modules;
                }
            }

            return modules;
        }
    }
}