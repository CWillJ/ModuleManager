namespace ModuleManager.ViewModels
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml.Serialization;
    using ModuleManager.Classes;
    using ModuleManager.Models;

    /// <summary>
    /// ModuleManagerViewModel will handle commands from the main view.
    /// </summary>
    public class ModuleManagerViewModel : INotifyPropertyChanged
    {
        private string _memberText;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleManagerViewModel"/> class.
        /// </summary>
        public ModuleManagerViewModel()
        {
            _memberText = string.Empty;
            UseSaveFileDialog = true;
            FileLocation = string.Empty;
            ConfigFileHandler = new ConfigFileManager("ConfigFile.xml");
            LoadMyFileCommand = new MyICommand(FindDLLs);
            SaveConfigCommand = new MyICommand(SaveConfig);

            if (File.Exists(Directory.GetCurrentDirectory() + @"\ConfigFile.xml"))
            {
                Modules = LoadConfig();
                if (Modules == null)
                {
                    Modules = new ObservableCollection<Module>();
                }
            }
            else
            {
                Modules = new ObservableCollection<Module>();
            }
        }

        /// <summary>
        /// PropertyChanged event handler.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets a value indicating whether SaveFileDialog is used on save settings.
        /// </summary>
        public bool UseSaveFileDialog { get; set; }

        /// <summary>
        /// Gets or sets the LoadMyFileCommand as a MyICommand.
        /// </summary>
        public MyICommand LoadMyFileCommand { get; set; }

        /// <summary>
        /// Gets or sets the SaveConfigCommand as a MyICommand.
        /// </summary>
        public MyICommand SaveConfigCommand { get; set; }

        /// <summary>
        /// Gets or sets the file location as a string.
        /// </summary>
        public string FileLocation { get; set; }

        /// <summary>
        /// Gets or sets a ConfigFileManager object.
        /// </summary>
        public ConfigFileManager ConfigFileHandler { get; set; }

        /// <summary>
        /// Gets or sets MemberText.
        /// </summary>
        public string MemberText
        {
            get
            {
                return _memberText;
            }

            set
            {
                _memberText = value;
                RaisePropertyChanged("MemberText");
            }
        }

        /// <summary>
        /// Gets or sets the collection of Modules.
        /// </summary>
        public ObservableCollection<Module> Modules { get; set; }

        /// <summary>
        /// Raise a property changed event.
        /// </summary>
        /// <param name="property">Property passed in as a string.</param>
        public void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void FindDLLs()
        {
            //// ModuleInfoRetriever infoRetriever = new ModuleInfoRetriever(GetModuleDirectory());
            //// ModuleInfoRetriever infoRetriever = new ModuleInfoRetriever();

            // Bring up explorer to allow user to choose a file location
            FileLocation = GetModuleDirectory();
            ModuleInfoRetriever infoRetriever = new ModuleInfoRetriever();

            Modules.Clear();

            // Check the file location for any .dll's
            foreach (var mod in infoRetriever.GetInfoFromDll())
            {
                Modules.Add(new Module(mod.Name, mod.Description, mod.Members));
            }

            MemberText = Modules[0].ToString();

            //// Modules[0].IsSelected = true;
            //// MessageBox.Show(Modules[0].IsSelected.ToString());
        }

        private void SaveConfig()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Module>));
            string saveFile = string.Empty;

            if (UseSaveFileDialog)
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
                    saveFileDialog.Filter = "xml files (*.xml)|*.xml";
                    saveFileDialog.Title = "Save Configuration File";
                    saveFileDialog.RestoreDirectory = true;

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        saveFile = saveFileDialog.FileName;
                    }

                    if (saveFile == string.Empty)
                    {
                        return;
                    }
                }
            }
            else
            {
                saveFile = Directory.GetCurrentDirectory() + @"\ConfigFile.xml";
            }

            using (StreamWriter wr = new StreamWriter(saveFile))
            {
                serializer.Serialize(wr, Modules);
            }
        }

        private ObservableCollection<Module> LoadConfig()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Module>));
            ObservableCollection<Module> modules = new ObservableCollection<Module>();

            using (StreamReader rd = new StreamReader(Directory.GetCurrentDirectory() + @"\ConfigFile.xml"))
            {
                modules = serializer.Deserialize(rd) as ObservableCollection<Module>;
            }

            return modules;
        }

        private string GetModuleDirectory()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK
                && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
            {
                return folderBrowserDialog.SelectedPath;
            }

            return null;
        }
    }
}