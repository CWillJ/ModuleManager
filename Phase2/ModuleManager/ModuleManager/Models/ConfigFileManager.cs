namespace ModuleManager.Models
{
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Windows.Forms;
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

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
                saveFileDialog.Filter = "xml files (*.xml)|*.xml";
                saveFileDialog.Title = "Save Configuration File";
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter wr = new StreamWriter(saveFileDialog.FileName))
                    {
                        serializer.Serialize(wr, modules);
                    }
                }
            }
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
