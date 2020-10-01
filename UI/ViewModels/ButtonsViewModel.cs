namespace ModuleManager.UI.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using ModuleManager.UI.Events;
    using ModuleObjects.Classes;
    using ModuleRetriever;
    using ModuleRetriever.Interfaces;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Mvvm;
    using Telerik.Windows.Controls;

    /// <summary>
    /// View model for the buttons area.
    /// </summary>
    public class ButtonsViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonsViewModel"/> class.
        /// </summary>
        /// <param name="eventAggregator">Event aggregator.</param>
        public ButtonsViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            InfoRetriever = new ModuleInfoRetriever(string.Empty);

            eventAggregator.GetEvent<UpdateModuleCollectionEvent>().Subscribe(ModuleCollectionUpdated);

            // Boolean value for testing.
            UseSaveFileDialog = false;

            // Load modules from a folder location.
            LoadModulesCommand = new Prism.Commands.DelegateCommand(StoreModules, CanExecute);

            // Save the current module setup, checkboxes and all, to an xml file.
            SaveConfigCommand = new Prism.Commands.DelegateCommand(SaveConfig, CanExecute);
        }

        /// <summary>
        /// Gets or sets a value indicating whether SaveFileDialog is used on save settings.
        /// </summary>
        public bool UseSaveFileDialog { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the application is currently loading modules.
        /// </summary>
        public bool LoadingModules { get; set; }

        /// <summary>
        /// Gets or sets the collection of Modules.
        /// </summary>
        public ObservableCollection<Module> Modules { get; set; }

        /// <summary>
        /// Gets or sets a ModuleInfoRetriever.
        /// </summary>
        public IModuleInfoRetriever InfoRetriever { get; set; }

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
                RadWindow.Alert("No .dll Files Found In " + moduleDirectory);
                return;
            }

            ObservableCollection<Module> modules = new ObservableCollection<Module>();

            InfoRetriever.DllDirectory = moduleDirectory;

            // Show progress bar
            _eventAggregator.GetEvent<UpdateProgressBarCurrentProgressEvent>().Publish(0.0);
            _eventAggregator.GetEvent<UpdateProgressBarTextEvent>().Publish(string.Empty);
            _eventAggregator.GetEvent<UpdateProgressBarVisibilityEvent>().Publish(true);
            _eventAggregator.GetEvent<UpdateModuleCollectionEvent>().Publish(modules);

            LoadingModules = true;

            Thread thread = new Thread(new ThreadStart(UpdateProgressBarText))
            {
                IsBackground = true,
            };

            thread.Start();

            // Run async to allow UI thread to update UI with the property changes above.
            modules = await Task.Run(() => InfoRetriever.GetModules(dllFiles));

            // Kill progress bar
            LoadingModules = false;
            _eventAggregator.GetEvent<UpdateModuleCollectionEvent>().Publish(modules);
            _eventAggregator.GetEvent<UpdateProgressBarTextEvent>().Publish(string.Empty);
            _eventAggregator.GetEvent<UpdateProgressBarVisibilityEvent>().Publish(false);
        }

        /// <summary>
        /// Runs async to update the progress bar with current module text.
        /// </summary>
        private void UpdateProgressBarText()
        {
            while (LoadingModules)
            {
                _eventAggregator.GetEvent<UpdateProgressBarCurrentProgressEvent>().Publish(InfoRetriever.PercentOfAssemblyLoaded);
                _eventAggregator.GetEvent<UpdateProgressBarTextEvent>().Publish(@"Loading Module: " + InfoRetriever.CurrentTypeName);
            }
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

        /// <summary>
        /// SaveConfig will save an ObservableCollection to an xml file.
        /// The boolean, UseSaveFileDialog will be tested to see if the
        /// SaveFileDialog will be used or if the hardcoded file location
        /// will be used.
        /// </summary>
        private void SaveConfig()
        {
            Type moduleType = typeof(ObservableCollection<Module>);
            XmlSerializer serializer;

            try
            {
                serializer = new XmlSerializer(moduleType);
            }
            catch (Exception e)
            {
                RadWindow.Alert(@"Cannot Save Modules to xml File Due to:" + "\n" + e.ToString());
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
                    RadWindow.Alert(@"Invalid File Path");
                    return;
                }
            }

            using StreamWriter wr = new StreamWriter(saveFile);
            serializer.Serialize(wr, Modules);
            wr.Close();

            RadWindow.Alert(@"Config File Saved at: " + saveFile);
        }

        private void ModuleCollectionUpdated(ObservableCollection<Module> modules)
        {
            Modules = modules;
        }

        private bool CanExecute()
        {
            return true;
        }
    }
}