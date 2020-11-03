namespace ModuleManager.UI.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using ModuleManager.ModuleObjects.Classes;
    using ModuleManager.ModuleObjects.Interfaces;
    using ModuleManager.UI.Interfaces;
    using Prism.Mvvm;
    using Prism.Regions;
    using Telerik.Windows.Controls;

    /// <summary>
    /// View model for the header buttons.
    /// </summary>
    public class ButtonsViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly IAssemblyLoaderService _assemblyLoaderService;
        private IProgressBarService _progressBarService;
        private IAssemblyCollectionService _assemblyCollectionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonsViewModel"/> class.
        /// </summary>
        /// <param name="regionManager">Region manager.</param>
        /// <param name="assemblyLoaderService">The IModuleInfoRetriever.</param>
        /// <param name="progressBarService">IProgressBarService.</param>
        /// <param name="assemblyCollectionService">IAssemblyCollectionService.</param>
        public ButtonsViewModel(
            IRegionManager regionManager,
            IAssemblyLoaderService assemblyLoaderService,
            IProgressBarService progressBarService,
            IAssemblyCollectionService assemblyCollectionService)
        {
            _regionManager = regionManager ?? throw new ArgumentNullException("RegionManager");
            _assemblyLoaderService = assemblyLoaderService ?? throw new ArgumentNullException("ModuleInfoRetriever");

            _progressBarService = progressBarService ?? throw new ArgumentNullException("ProgressBarService");
            _assemblyCollectionService = assemblyCollectionService ?? throw new ArgumentNullException("AssemblyCollectionService");

            UseSaveFileDialog = false;

            NavigateCommand = new Prism.Commands.DelegateCommand<string>(Navigate);

            // Load modules from a folder location.
            LoadModulesCommand = new Prism.Commands.DelegateCommand(StoreModules, CanExecute);

            // Save the current module setup, checkboxes and all, to an xml file.
            SaveConfigCommand = new Prism.Commands.DelegateCommand(SaveConfig, CanExecute);

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
        /// Gets or sets the IProgressBarService.
        /// </summary>
        public IProgressBarService ProgressBarService
        {
            get { return _progressBarService; }
            set { _progressBarService = value; }
        }

        /// <summary>
        /// Gets or sets the IAssemblyCollectionService.
        /// </summary>
        public IAssemblyCollectionService AssemblyCollectionService
        {
            get { return _assemblyCollectionService; }
            set { _assemblyCollectionService = value; }
        }

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
        /// Gets or sets the TestCommand.
        /// </summary>
        public Prism.Commands.DelegateCommand TestCommand { get; set; }

        /// <summary>
        /// StoreModules will attempt to get all assemblies from a dll and store it
        /// as an AssemblyData in the AssemblyData collection.
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

            _assemblyLoaderService.DllDirectory = moduleDirectory;

            // Show progress bar
            AssemblyCollectionService.Assemblies = new ObservableCollection<AssemblyData>();
            ProgressBarService.CurrentProgress = 0.0;
            ProgressBarService.AssemblyName = string.Empty;
            ProgressBarService.Text = string.Empty;

            NavigateCommand.Execute("ProgressBarView");

            LoadingModules = true;

            Thread thread = new Thread(new ThreadStart(UpdateProgressBarText))
            {
                IsBackground = true,
            };

            thread.Start();

            // Run async to allow UI thread to update UI with the property changes above.
            AssemblyCollectionService.Assemblies = await Task.Run(() => _assemblyLoaderService.GetAssemblies(dllFiles));

            // Kill progress bar
            LoadingModules = false;

            NavigateCommand.Execute("ModuleManagerView");

            ProgressBarService.AssemblyName = string.Empty;
            ProgressBarService.Text = string.Empty;
        }

        /// <summary>
        /// Runs async to update the progress bar with current module text.
        /// </summary>
        private void UpdateProgressBarText()
        {
            while (LoadingModules)
            {
                ProgressBarService.AssemblyName = _assemblyLoaderService.CurrentAssemblyName;
                ProgressBarService.CurrentProgress = _assemblyLoaderService.PercentOfAssemblyLoaded;
                ProgressBarService.Text = @"Loading Module: " + _assemblyLoaderService.CurrentTypeName;
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
            string saveFile;

            try
            {
                serializer = new XmlSerializer(assemblyType);
            }
            catch (Exception e)
            {
                RadWindow.Alert(@"Cannot Save Modules to xml File Due to:" + "\n" + e.ToString());
                return;
            }

            saveFile = Directory.GetCurrentDirectory() + @"\ConfigFile.xml";

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
            serializer.Serialize(wr, AssemblyCollectionService.Assemblies);
            wr.Close();

            RadWindow.Alert(@"Configuration Saved");
        }

        /// <summary>
        /// View navigation method.
        /// </summary>
        /// <param name="navigatePath">The path of the view to navigate to.</param>
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

            foreach (var assembly in AssemblyCollectionService.Assemblies)
            {
                if (assembly.IsEnabled && (assembly.Assembly != null))
                {
                    test = true;

                    // parameters for ClassLibrary1.Class2.Method2
                    object[] parameters = { @"A string and the number ", (int)21 };

                    string some = await Task.Run(() => assembly.Modules[1].Methods[1].Invoke(parameters).ToString());

                    RadWindow.Alert(some);
                }
            }

            if (!test)
            {
                RadWindow.Alert(@"No Assemblies Are Loaded");
            }
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