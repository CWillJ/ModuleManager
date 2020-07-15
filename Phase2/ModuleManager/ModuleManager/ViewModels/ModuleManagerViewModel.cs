namespace ModuleManager.ViewModels
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.IO.Enumeration;
    using System.Xml.Serialization;
    using Microsoft.Win32;
    using ModuleManager.Classes;
    using ModuleManager.Models;

    /// <summary>
    /// ModuleManagerViewModel will handle commands from the main view.
    /// </summary>
    public class ModuleManagerViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Module> _modules;
        private string _memberText;
        private double _currentProgress;
        private string _progressBarText;
        private bool _progressBarVisible;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleManagerViewModel"/> class.
        /// </summary>
        public ModuleManagerViewModel()
        {
            _modules = new ObservableCollection<Module>();
            _memberText = string.Empty;
            _progressBarText = string.Empty;
            _currentProgress = 0;
            _progressBarVisible = false;

            UseSaveFileDialog = false;
            ModuleDirectory = string.Empty;
            LoadModulesCommand = new MyICommand(FindDLLs);
            SaveConfigCommand = new MyICommand(SaveConfig);

            // Load previously saved module configuration if the ConfigFile exists
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
        /// Gets or sets a value indicating whether SaveFileDialog is used on save settings
        /// </summary>
        public bool UseSaveFileDialog { get; set; }

        /// <summary>
        /// Gets or sets the LoadModulesCommand as a MyICommand.
        /// </summary>
        public MyICommand LoadModulesCommand { get; set; }

        /// <summary>
        /// Gets or sets the SaveConfigCommand as a MyICommand.
        /// </summary>
        public MyICommand SaveConfigCommand { get; set; }

        /// <summary>
        /// Gets or sets the file location as a string.
        /// </summary>
        public string ModuleDirectory { get; set; }

        /// <summary>
        /// Gets or sets the progress bar text.
        /// </summary>
        public string ProgressBarText
        {
            get
            {
                return _progressBarText;
            }

            set
            {
                if (_progressBarText != value)
                {
                    _progressBarText = value;
                    RaisePropertyChanged("ProgressBarText");
                }
            }
        }

        /// <summary>
        /// Gets the current progress of the status bar.
        /// </summary>
        public double CurrentProgress
        {
            get
            {
                return _currentProgress;
            }

            private set
            {
                if (_currentProgress != value)
                {
                    _currentProgress = value;
                    RaisePropertyChanged("CurrentProgress");
                    RaisePropertyChanged("ProgressBarVisible");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a progress bar is visible.
        /// </summary>
        public bool ProgressBarVisible
        {
            get
            {
                return _progressBarVisible;
            }

            set
            {
                _progressBarVisible = value;
                RaisePropertyChanged("ProgressBarVisible");
            }
        }

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
        public ObservableCollection<Module> Modules
        {
            get
            {
                return _modules;
            }

            set
            {
                _modules = value;
                RaisePropertyChanged("Modules");
            }
        }

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
            ModuleInfoRetriever infoRetriever = new ModuleInfoRetriever();
            ObservableCollection<Module> modules;

            infoRetriever.DllDirectory = GetModuleDirectory();

            // These two things arent showing up on the UI
            ProgressBarVisible = true;
            ProgressBarText = "Loading Modules";
            CurrentProgress = 25;
            Modules.Clear();

            modules = infoRetriever.GetModules();

            if (modules != null)
            {
                foreach (var mod in modules)
                {
                    if (mod != null)
                    {
                        Modules.Add(new Module(mod.Name, mod.Description, mod.Members));
                    }
                }

                // TODO in the future, possibly kill loading bar
                ////MessageBox.Show(@"Done Loading Modules");
            }

            CurrentProgress = 100;
            ProgressBarText = string.Empty;
            ProgressBarVisible = false;

            // Future TreeViewSelectedItem Binding
            //// MemberText = Modules[0].ToString();
        }

        /// <summary>
        /// SaveConfig will save an ObservableCollection to an xml file.
        /// The boolean, UseSaveFileDialog will be tested to see if the
        /// SaveFileDialog will be used or if the hardcoded file location
        /// will be used.
        /// </summary>
        private void SaveConfig()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Module>));
            string saveFile = Directory.GetCurrentDirectory() + @"\ConfigFile.xml";

            if (UseSaveFileDialog)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    InitialDirectory = Directory.GetCurrentDirectory(),
                    Filter = "xml files (*.xml)|*.xml",
                    Title = "Save Configuration File",
                    RestoreDirectory = true
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    saveFile = saveFileDialog.FileName;
                }

                if (saveFile == string.Empty)
                {
                    System.Windows.MessageBox.Show(@"Invalid File Path");
                    return;
                }
            }
            else
            {
                System.Windows.MessageBox.Show(@"Config File Saved at: " + saveFile);
            }

            using (StreamWriter wr = new StreamWriter(saveFile))
            {
                serializer.Serialize(wr, Modules);
            }
        }

        /// <summary>
        /// LoadConfig will load an ObservableCollection of type Module from an xml file
        /// </summary>
        /// <returns>ObservableCollection of type Module.</returns>
        private ObservableCollection<Module> LoadConfig()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Module>));
            ObservableCollection<Module> modules = new ObservableCollection<Module>();
            string loadFile = Directory.GetCurrentDirectory() + @"\ConfigFile.xml";

            if (UseSaveFileDialog)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    InitialDirectory = Directory.GetCurrentDirectory(),
                    Filter = "xml files (*.xml)|*.xml",
                    Title = "Load Configuration File",
                    RestoreDirectory = true
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    loadFile = openFileDialog.FileName;
                }

                if (loadFile == string.Empty)
                {
                    return null;
                }
            }

            using (StreamReader rd = new StreamReader(loadFile))
            {
                modules = serializer.Deserialize(rd) as ObservableCollection<Module>;
            }

            return modules;
        }

        private string GetModuleDirectory()
        {
            return @"C:\Users\wjohnson\Desktop\Modules";

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                ValidateNames = false,
                CheckFileExists = false,
                CheckPathExists = true,
                FileName = "Folder Selection"
            };

            if (openFileDialog.ShowDialog() == true && openFileDialog.CheckPathExists)
            {
                string s = openFileDialog.FileName.Remove(openFileDialog.FileName.LastIndexOf(@"\Folder Selection"), openFileDialog.FileName.Length);
                return s;
            }

            return null;
        }
    }
}