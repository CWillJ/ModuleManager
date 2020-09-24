namespace ModuleManager.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Xml.Serialization;
    using ModuleObjects.Classes;
    using ModuleRetriever;
    using ModuleRetriever.Interfaces;
    using Prism.Commands;
    using Prism.Mvvm;
    using Prism.Regions;
    using Telerik.Windows.Controls;

    /// <summary>
    /// ModuleManagerViewModel will handle commands from the main view.
    /// </summary>
    public class ShellViewModel : BindableBase
    {
        private ObservableCollection<Module> _modules;
        private double _currentProgress;
        private string _progressBarText;
        private bool _progressBarIsVisible;
        private IRegionManager _region;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellViewModel"/> class.
        /// </summary>
        public ShellViewModel()
        {
            _modules = new ObservableCollection<Module>();
            _progressBarText = string.Empty;
            _currentProgress = 0;
            _progressBarIsVisible = false;

            LoadingModules = false;

            ModuleDirectory = string.Empty;
            LoadModulesCommand = new Prism.Commands.DelegateCommand(StoreModules, CanExecute);
            SaveConfigCommand = new Prism.Commands.DelegateCommand(SaveConfig, CanExecute);
            LoadUnloadCommand = new Prism.Commands.DelegateCommand(LoadUnload, CanExecute);

            InfoRetriever = new ModuleInfoRetriever(string.Empty);

            // Load previously saved module configuration if the ConfigFile exists
            if (File.Exists(Directory.GetCurrentDirectory() + @"\ConfigFile.xml"))
            {
                _modules = LoadConfig();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether SaveFileDialog is used on save settings.
        /// </summary>
        public bool UseSaveFileDialog { get; set; }

        /// <summary>
        /// Gets or sets the LoadModulesCommand as a ModuleManagerICommand.
        /// </summary>
        public Prism.Commands.DelegateCommand LoadModulesCommand { get; set; }

        /// <summary>
        /// Gets or sets the SaveConfigCommand as a ModuleManagerICommand.
        /// </summary>
        public Prism.Commands.DelegateCommand SaveConfigCommand { get; set; }

        /// <summary>
        /// Gets or sets the LoadUnloadCommand as a ModuleManagerICommand.
        /// </summary>
        public Prism.Commands.DelegateCommand LoadUnloadCommand { get; set; }

        /// <summary>
        /// Gets or sets the file location as a string.
        /// </summary>
        public string ModuleDirectory { get; set; }

        /// <summary>
        /// Gets or sets a ModuleInfoRetriever.
        /// </summary>
        public IModuleInfoRetriever InfoRetriever { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the application is currently loading modules.
        /// </summary>
        public bool LoadingModules { get; set; }

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
                    SetProperty(ref _progressBarText, value);
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
                    SetProperty(ref _currentProgress, value);
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
                return _progressBarIsVisible;
            }

            set
            {
                SetProperty(ref _progressBarIsVisible, value);
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
                SetProperty(ref _modules, value);
            }
        }

        /// <summary>
        /// StoreModules will attempt to get all assemblies from a dll and store it
        /// as a Module in the Modules collection.
        /// </summary>
        private async void StoreModules()
        {
            string moduleDirectory = GetModuleDirectory();

            // If directory selection was cancled, exit this method.
            if (moduleDirectory == null)
            {
                return;
            }

            // add all the files of the assemblies you actually want info from
            string[] dllFiles = Directory.GetFiles(moduleDirectory, @"*.dll");

            if (dllFiles.Length == 0)
            {
                MessageBox.Show("No .dll Files Found In " + moduleDirectory);
                return;
            }

            InfoRetriever.DllDirectory = moduleDirectory;

            // Show progress bar
            CurrentProgress = 0;
            ProgressBarText = string.Empty;
            ProgressBarIsVisible = true;

            Modules.Clear();

            LoadingModules = true;

            Thread thread = new Thread(new ThreadStart(UpdateProgressBarText))
            {
                IsBackground = true,
            };

            thread.Start();

            // Run async to allow UI thread to update UI with the property changes above.
            Modules = await Task.Run(() => InfoRetriever.GetModules(dllFiles));

            // Kill progress bar
            LoadingModules = false;
            ProgressBarText = string.Empty;
            ProgressBarIsVisible = false;
        }

        /// <summary>
        /// Runs async to update the progress bar with current module text.
        /// </summary>
        private void UpdateProgressBarText()
        {
            while (LoadingModules)
            {
                CurrentProgress = InfoRetriever.PercentOfAssemblyLoaded;
                ProgressBarText = @"Loading Module: " + InfoRetriever.CurrentTypeName;
            }
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
                RadSaveFileDialog saveFileDialog = new RadSaveFileDialog
                {
                    InitialDirectory = Directory.GetCurrentDirectory(),
                    Filter = "xml files (*.xml)|*.xml",
                    Header = "Save Configuration File",
                    RestoreDirectory = true,
                };

                saveFileDialog.ShowDialog();

                if (saveFileDialog.DialogResult == true)
                {
                    saveFile = saveFileDialog.FileName;
                }

                if (saveFile == string.Empty)
                {
                    MessageBox.Show(@"Invalid File Path");
                    return;
                }
            }

            using StreamWriter wr = new StreamWriter(saveFile);
            serializer.Serialize(wr, Modules);
            wr.Close();

            MessageBox.Show(@"Config File Saved at: " + saveFile);
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
                RadOpenFileDialog openFileDialog = new RadOpenFileDialog
                {
                    InitialDirectory = Directory.GetCurrentDirectory(),
                    Filter = "xml files (*.xml)|*.xml",
                    Header = "Load Configuration File",
                    RestoreDirectory = true,
                };

                openFileDialog.ShowDialog();

                if (openFileDialog.DialogResult == true)
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

        private bool CanExecute()
        {
            return true;
        }

        private void LoadUnload()
        {
            foreach (var module in Modules)
            {
                if (module.IsEnabled)
                {
                    InfoRetriever.LoadModule(module.Type.Assembly);
                }
            }

            MessageBox.Show("Command Worked!");
        }

        /// <summary>
        /// Gets the directory selected by the user that should contain dll files.
        /// </summary>
        /// <returns>String of the directory path.</returns>
        private string GetModuleDirectory()
        {
            RadOpenFolderDialog folderBrowserDialog = new RadOpenFolderDialog();

            folderBrowserDialog.ShowDialog();

            if (folderBrowserDialog.DialogResult == true)
            {
                return folderBrowserDialog.FileName;
            }

            return null;
        }
    }
}