namespace ModuleManager.Models
{
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Xml.Serialization;
    using ModuleManager.Classes;

    /// <summary>
    /// ConfigFileManager is used to save an ObservableCollection of type Module to
    /// an xml file and to load an ObservableCollection of type Module to an xml file.
    /// </summary>
    public class ConfigFileManager
    {
        private string _configFileLocation;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigFileManager"/> class.
        /// </summary>
        public ConfigFileManager()
        {
            _configFileLocation = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigFileManager"/> class.
        /// </summary>
        /// <param name="filePath">Specified file path.</param>
        public ConfigFileManager(string filePath)
        {
            _configFileLocation = filePath;
        }

        /// <summary>
        /// Gets or sets the ConfigFileLocation.
        /// </summary>
        public string ConfigFileLocation
        {
            get { return _configFileLocation; }
            set { _configFileLocation = value; }
        }

        /// <summary>
        /// Saves an ObservableCollection of type Module to an xml file.
        /// </summary>
        /// <param name="modules">ObservableCollection of type Module.</param>
        public void Save(ObservableCollection<Module> modules)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Module>));

            using (StreamWriter wr = new StreamWriter(ConfigFileLocation))
            {
                serializer.Serialize(wr, modules);
            }
        }

        /// <summary>
        /// Saves an ObservableCollection of type Module to an xml file.
        /// </summary>
        /// <param name="fileLocation">A string representing a file location.</param>
        /// <param name="modules">ObservableCollection of type Module.</param>
        public void Save(string fileLocation, ObservableCollection<Module> modules)
        {
            ConfigFileLocation = fileLocation;
            Save(modules);
        }

        /// <summary>
        /// Loads an ObservableCollection of type Module from an xml file.
        /// </summary>
        /// <returns>ObservableCollection of type Module.</returns>
        public ObservableCollection<Module> Load()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Module>));
            ObservableCollection<Module> modules = new ObservableCollection<Module>();

            using (StreamReader rd = new StreamReader(ConfigFileLocation))
            {
                modules = serializer.Deserialize(rd) as ObservableCollection<Module>;
            }

            return modules;
        }
    }
}
