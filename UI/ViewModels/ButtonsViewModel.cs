namespace ModuleManager.UI.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using ModuleManager.ModuleObjects.Classes;
    using ModuleManager.ModuleObjects.Interfaces;
    using ModuleManager.ModuleObjects.Loaders;
    using ModuleManager.UI.Events;
    using Prism.Events;
    using Prism.Mvvm;
    using Prism.Regions;
    using Telerik.Windows.Controls;

    /// <summary>
    /// View model for the buttons area.
    /// </summary>
    public class ButtonsViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonsViewModel"/> class.
        /// </summary>
        /// <param name="eventAggregator">Event aggregator.</param>
        /// <param name="regionManager">Region manager.</param>
        public ButtonsViewModel(IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;

            InfoRetriever = new ModuleInfoRetriever(string.Empty);

            eventAggregator.GetEvent<UpdateAssemblyCollectionEvent>().Subscribe(AssemblyCollectionUpdated);

            // Boolean value for testing.
            UseSaveFileDialog = false;

            NavigateCommand = new Prism.Commands.DelegateCommand<string>(Navigate);

            // Load modules from a folder location.
            LoadModulesCommand = new Prism.Commands.DelegateCommand(StoreModules, CanExecute);

            // Save the current module setup, checkboxes and all, to an xml file.
            SaveConfigCommand = new Prism.Commands.DelegateCommand(SaveConfig, CanExecute);

            // Load/unload the current assembly/module selection base on the checkboxes.
            LoadUnloadCommand = new Prism.Commands.DelegateCommand(LoadUnload, CanExecute);

            // Load/unload the current assembly/module selection base on the checkboxes.
            TestCommand = new Prism.Commands.DelegateCommand(TestMethod, CanExecute);
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
        public ObservableCollection<AssemblyData> Assemblies { get; set; }

        /// <summary>
        /// Gets or sets a ModuleInfoRetriever.
        /// </summary>
        public IModuleInfoRetriever InfoRetriever { get; set; }

        /// <summary>
        /// Gets the Navigate command.
        /// </summary>
        public Prism.Commands.DelegateCommand<string> NavigateCommand { get; private set; }

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
        /// Gets or sets the TestCommand.
        /// </summary>
        public Prism.Commands.DelegateCommand TestCommand { get; set; }

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

            ObservableCollection<AssemblyData> assemblies = new ObservableCollection<AssemblyData>();

            InfoRetriever.DllDirectory = moduleDirectory;

            // Show progress bar
            _eventAggregator.GetEvent<UpdateProgressBarCurrentProgressEvent>().Publish(0.0);
            _eventAggregator.GetEvent<UpdateProgressBarAssemblyNameEvent>().Publish(string.Empty);
            _eventAggregator.GetEvent<UpdateProgressBarTextEvent>().Publish(string.Empty);
            _eventAggregator.GetEvent<UpdateAssemblyCollectionEvent>().Publish(assemblies);

            NavigateCommand.Execute("ProgressBarView");

            LoadingModules = true;

            Thread thread = new Thread(new ThreadStart(UpdateProgressBarText))
            {
                IsBackground = true,
            };

            thread.Start();

            // Run async to allow UI thread to update UI with the property changes above.
            assemblies = await Task.Run(() => InfoRetriever.GetModules(dllFiles));

            // Kill progress bar
            LoadingModules = false;

            NavigateCommand.Execute("ModuleManagerView");

            _eventAggregator.GetEvent<UpdateProgressBarAssemblyNameEvent>().Publish(string.Empty);
            _eventAggregator.GetEvent<UpdateProgressBarTextEvent>().Publish(string.Empty);
            _eventAggregator.GetEvent<UpdateAssemblyCollectionEvent>().Publish(assemblies);
        }

        /// <summary>
        /// Runs async to update the progress bar with current module text.
        /// </summary>
        private void UpdateProgressBarText()
        {
            while (LoadingModules)
            {
                _eventAggregator.GetEvent<UpdateProgressBarAssemblyNameEvent>().Publish(InfoRetriever.CurrentAssemblyName);
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
        /// SaveConfig will save an ObservableCollection of AssemblyData
        /// to an xml file.
        /// The boolean, UseSaveFileDialog will be tested to see if the
        /// SaveFileDialog will be used or if the hardcoded file location
        /// will be used.
        /// </summary>
        private void SaveConfig()
        {
            Type assemblyType = typeof(ObservableCollection<AssemblyData>);
            XmlSerializer serializer;

            try
            {
                serializer = new XmlSerializer(assemblyType);
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
            serializer.Serialize(wr, Assemblies);
            wr.Close();

            RadWindow.Alert(@"Configuration Saved");
        }

        /// <summary>
        /// Loads all assemblies with checked boxes and
        /// unloads the unchecked ones.
        /// </summary>
        private async void LoadUnload()
        {
            foreach (var assembly in Assemblies)
            {
                await Task.Run(assembly.LoadUnload);
            }

            RadWindow.Alert(@"Checked Modules Have Been Loaded" + "\n" + @"Unchecked Modules Have Been Unloaded");
        }

        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
            {
                _regionManager.RequestNavigate("ContentRegion", navigatePath);
            }
        }

        private async void TestMethod()
        {
            bool test = false;

            foreach (var assembly in Assemblies)
            {
                if (assembly.AreAnyModulesChecked() && (assembly.Assembly != null))
                {
                    test = true;

                    string[] s = { @"This string written twice. " };

                    string some = await Task.Run(() => assembly.Modules[0].Methods[2].Invoke(s).ToString());

                    RadWindow.Alert(some);
                }
            }

            if (!test)
            {
                RadWindow.Alert(@"No Modules Are Loaded");
            }
        }

        /// <summary>
        /// Sets the local property Assemblies to the published ObservableCollection of AssemblyData.
        /// </summary>
        /// <param name="assemblies">Published ObservableCollection of AssemblyData.</param>
        private void AssemblyCollectionUpdated(ObservableCollection<AssemblyData> assemblies)
        {
            Assemblies = assemblies;
        }

        /// <summary>
        /// Can always execute.
        /// </summary>
        /// <returns>True.</returns>
        private bool CanExecute()
        {
            return true;
        }
    }
}