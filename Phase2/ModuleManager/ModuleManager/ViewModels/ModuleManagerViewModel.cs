﻿namespace ModuleManager.ViewModels
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
        private readonly BackgroundWorker _worker;
        private double _currentProgress;
        private string _currentProgressText;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleManagerViewModel"/> class.
        /// </summary>
        public ModuleManagerViewModel()
        {
            _memberText = string.Empty;
            _currentProgressText = string.Empty;
            _currentProgress = 0;

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
        /// Gets or sets a value indicating whether SaveFileDialog is used on save settings.
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
        public string CurrentProgressText
        {
            get { return _currentProgressText; }
            set
            {
                if (_currentProgressText != value)
                {
                    _currentProgressText = value;
                    RaisePropertyChanged("CurrentProgressText");
                }
            }
        }

        /// <summary>
        /// Gets or sets the current progress of the status bar.
        /// </summary>
        public double CurrentProgress
        {
            get { return _currentProgress; }
            private set
            {
                if (_currentProgress != value)
                {
                    _currentProgress = value;
                    RaisePropertyChanged("CurrentProgress");
                }
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
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += Worker_DoWork;

            Modules.Clear();

            // Bring up explorer to allow user to choose a file location
            //// ModuleDirectory = GetModuleDirectory();
            ModuleInfoRetriever infoRetriever = new ModuleInfoRetriever(GetModuleDirectory());
            ObservableCollection<Module> modules = infoRetriever.GetModules();

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

            // Future TreeViewSelectedItem Binding
            //// MemberText = Modules[0].ToString();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            throw new System.NotImplementedException();
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
                        MessageBox.Show(@"Invalid File Path");
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show(@"Config File Saved at: " + saveFile);
            }

            using (StreamWriter wr = new StreamWriter(saveFile))
            {
                serializer.Serialize(wr, Modules);
            }
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

            if (UseSaveFileDialog)
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
                    openFileDialog.Filter = "xml files (*.xml)|*.xml";
                    openFileDialog.Title = "Save Configuration File";
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        loadFile = openFileDialog.FileName;
                    }

                    if (loadFile == string.Empty)
                    {
                        return null;
                    }
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