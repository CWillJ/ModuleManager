namespace ModuleManager.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Xml.Serialization;
    using Microsoft.Win32;
    using ModuleManager.Classes;
    using Ookii.Dialogs.Wpf;

    /// <summary>
    /// ModuleManagerViewModel will handle commands from the main view.
    /// </summary>
    public class ModuleManagerViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Module> _modules;
        private object _memberText;
        private double _currentProgress;
        private string _progressBarText;
        private bool _isVisible;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleManagerViewModel"/> class.
        /// </summary>
        public ModuleManagerViewModel()
        {
            Modules = new ObservableCollection<Module>();
            ////_memberText = string.Empty;
            _progressBarText = string.Empty;
            _currentProgress = 0;
            _isVisible = false;

            UseSaveFileDialog = false;
            ModuleDirectory = string.Empty;
            LoadModulesCommand = new MyICommand(StoreModules);
            SaveConfigCommand = new MyICommand(SaveConfig);

            // Load previously saved module configuration if the ConfigFile exists
            if (File.Exists(Directory.GetCurrentDirectory() + @"\ConfigFile.xml"))
            {
                Modules = LoadConfig();
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
        public bool ProgressBarIsVisible
        {
            get
            {
                return _isVisible;
            }

            set
            {
                _isVisible = value;
                RaisePropertyChanged("ProgressBarIsVisible");
            }
        }

        /// <summary>
        /// Gets or sets MemberText.
        /// </summary>
        public object MemberText
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

        /// <summary>
        /// StoreModules will attempt to get all assemblies from a dll and store it
        /// as a Module in the Modules collection.
        /// </summary>
        private async void StoreModules()
        {
            string moduleDirectory = GetModuleDirectory();

            ModuleInfoRetriever infoRetriever = new ModuleInfoRetriever(moduleDirectory);

            // Show progress bar
            ProgressBarIsVisible = true;
            ProgressBarText = "Loading Modules";

            Modules.Clear();

            // Run async to allow UI thread to update UI with the property changes above.
            Modules = await Task.Run(() => infoRetriever.GetModules());

            // Kill progress bar
            ProgressBarText = string.Empty;
            ProgressBarIsVisible = false;
        }

        /// <summary>
        /// SaveConfig will save an ObservableCollection to an xml file.
        /// The boolean, UseSaveFileDialog will be tested to see if the
        /// SaveFileDialog will be used or if the hardcoded file location
        /// will be used.
        /// </summary>
        private void SaveConfig()
        {
            Type mType = typeof(ObservableCollection<Module>);
            XmlSerializer serializer;

            try
            {
                serializer = new XmlSerializer(mType);
            }
            catch (Exception e)
            {
                MessageBox.Show(@"Cannot Save Modules to xml File Due to:" + "\n" + e.ToString());
                return;
            }

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

            using StreamWriter wr = new StreamWriter(saveFile);
            serializer.Serialize(wr, Modules);
            wr.Close();
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
                    return modules;
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
            VistaFolderBrowserDialog folderBrowserDialog = new VistaFolderBrowserDialog()
            {
                Description = @"Select a Folder That Contains .dll Files"
            };

            if (folderBrowserDialog.ShowDialog() == true)
            {
                string s = folderBrowserDialog.SelectedPath;
                return s;
            }

            return null;
        }
    }
}