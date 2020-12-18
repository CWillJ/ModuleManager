namespace ModuleManager.Core.UI.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using ModuleManager.Common.Classes;
    using ModuleManager.Common.Interfaces;
    using ModuleManager.Core.UI.Interfaces;
    using Prism.Mvvm;
    using Prism.Regions;
    using Telerik.Windows.Controls;

    /// <summary>
    /// View model for the buttons view.
    /// </summary>
    public class ViewDisplayViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly IAssemblyCollectionService _assemblyCollectionService;
        private readonly IAssemblyDataLoaderService _assemblyDataLoaderService;
        private readonly IViewCollectionService _viewCollectionService;
        private readonly IProgressBarService _progressBarService;
        private readonly ILoadedViewNamesService _loadedViewNamesService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewDisplayViewModel"/> class.
        /// </summary>
        /// <param name="regionManager">The <see cref="IRegionManager"/>.</param>
        /// <param name="assemblyCollectionService">The <see cref="IAssemblyCollectionService"/>.</param>
        /// <param name="assemblyDataLoaderService">The <see cref="IAssemblyDataLoaderService"/>.</param>
        /// <param name="viewCollectionService">The <see cref="IViewCollectionService"/>.</param>
        /// <param name="progressBarService">The <see cref="IProgressBarService"/>.</param>
        /// <param name="loadedViewNamesService">The <see cref="ILoadedViewNamesService"/>.</param>
        public ViewDisplayViewModel(
            IRegionManager regionManager,
            IAssemblyCollectionService assemblyCollectionService,
            IAssemblyDataLoaderService assemblyDataLoaderService,
            IViewCollectionService viewCollectionService,
            IProgressBarService progressBarService,
            ILoadedViewNamesService loadedViewNamesService)
        {
            _regionManager = regionManager ?? throw new ArgumentNullException("RegionManager");
            _assemblyCollectionService = assemblyCollectionService ?? throw new ArgumentNullException("AssemblyCollectionService");
            _assemblyDataLoaderService = assemblyDataLoaderService ?? throw new ArgumentNullException("AssemblyDataLoaderService");
            _viewCollectionService = viewCollectionService ?? throw new ArgumentNullException("ViewCollectionService");
            _progressBarService = progressBarService ?? throw new ArgumentNullException("ProgressBarService");
            _loadedViewNamesService = loadedViewNamesService ?? throw new ArgumentNullException("LoadedViewNamesService");

            UseSaveFileDialog = false;

            NavigateCommand = new Prism.Commands.DelegateCommand<string>(Navigate);
            LoadModulesCommand = new Prism.Commands.DelegateCommand(StoreModules, CanExecute);
            SaveConfigCommand = new Prism.Commands.DelegateCommand(SaveConfig, CanExecute);
            AddSelectedViewCommand = new Prism.Commands.DelegateCommand(AddSelectedView, CanExecute);
            RemoveSelectedViewCommand = new Prism.Commands.DelegateCommand(RemoveSelectedView, CanExecute);
            ViewDoubleClickCommand = new Prism.Commands.DelegateCommand(DisplaySelectedView, CanExecute);
        }

        /// <summary>
        /// Gets the <see cref="IAssemblyCollectionService"/>.
        /// </summary>
        public IAssemblyCollectionService AssemblyCollectionService
        {
            get { return _assemblyCollectionService; }
        }

        /// <summary>
        /// Gets the <see cref="IViewCollectionService"/>.
        /// </summary>
        public IViewCollectionService ViewCollectionService
        {
            get { return _viewCollectionService; }
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
        /// Gets the Navigate command.
        /// </summary>
        public Prism.Commands.DelegateCommand<string> NavigateCommand { get; private set; }

        /// <summary>
        /// Gets or sets the LoadModulesCommand as a <see cref="Prism.Commands.DelegateCommand"/>.
        /// </summary>
        public Prism.Commands.DelegateCommand LoadModulesCommand { get; set; }

        /// <summary>
        /// Gets or sets the SaveConfigCommand as a <see cref="Prism.Commands.DelegateCommand"/>.
        /// </summary>
        public Prism.Commands.DelegateCommand SaveConfigCommand { get; set; }

        /// <summary>
        /// Gets or sets the AddSelectedViewCommand as a <see cref="Prism.Commands.DelegateCommand"/>.
        /// </summary>
        public Prism.Commands.DelegateCommand AddSelectedViewCommand { get; set; }

        /// <summary>
        /// Gets or sets the RemoveSelectedViewCommand as a <see cref="Prism.Commands.DelegateCommand"/>.
        /// </summary>
        public Prism.Commands.DelegateCommand RemoveSelectedViewCommand { get; set; }

        /// <summary>
        /// Gets or sets the ViewDoubleClickCommand as a <see cref="Prism.Commands.DelegateCommand"/>.
        /// </summary>
        public Prism.Commands.DelegateCommand ViewDoubleClickCommand { get; set; }

        /// <summary>
        /// StoreModules will attempt to get all assemblies from a dll and store it
        /// as an AssemblyData in the AssemblyData collection.
        /// </summary>
        private async void StoreModules()
        {
            ////string moduleDirectory = GetModuleDirectory();
            string moduleDirectory = Path.Combine(Directory.GetCurrentDirectory(), @"TestModules");

            if (string.IsNullOrEmpty(moduleDirectory))
            {
                return;
            }

            string[] dllFiles = Directory.GetFiles(moduleDirectory, @"*.dll");

            if (dllFiles.Length == 0)
            {
                RadWindow.Alert("No .dll Files Found In " + moduleDirectory);
                return;
            }

            _assemblyDataLoaderService.DllDirectory = moduleDirectory;

            // Show progress bar
            AssemblyCollectionService.Assemblies = new ObservableCollection<AssemblyData>();
            _progressBarService.CurrentProgress = 0.0;
            _progressBarService.AssemblyName = string.Empty;
            _progressBarService.Text = string.Empty;

            NavigateCommand.Execute("ProgressBarView");

            LoadingModules = true;

            Thread thread = new Thread(new ThreadStart(UpdateProgressBarText))
            {
                IsBackground = true,
            };

            thread.Start();

            await Task.Run(() => AssemblyCollectionService.PopulateAssemblyCollection(moduleDirectory, dllFiles));

            // Kill progress bar
            LoadingModules = false;

            NavigateCommand.Execute("ModuleManagerView");

            _progressBarService.AssemblyName = string.Empty;
            _progressBarService.Text = string.Empty;
        }

        /// <summary>
        /// Runs async to update the progress bar with current module text.
        /// </summary>
        private void UpdateProgressBarText()
        {
            while (LoadingModules)
            {
                _progressBarService.AssemblyName = _assemblyDataLoaderService.CurrentAssemblyName;
                _progressBarService.CurrentProgress = _assemblyDataLoaderService.PercentOfAssemblyLoaded;
                _progressBarService.Text = @"Loading Module: " + _assemblyDataLoaderService.CurrentTypeName;
            }
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
            if (AssemblyCollectionService.Assemblies.Count == 0)
            {
                RadWindow.Alert(@"No Modules Detected");
                return;
            }

            _loadedViewNamesService.LoadedViewNames.Clear();

            foreach (var viewObject in _regionManager.Regions[@"LoadedViewsRegion"].Views)
            {
                _loadedViewNamesService.LoadedViewNames.Add(viewObject.GetType().FullName);
            }

            JsonExtensions.Save(AssemblyCollectionService.Assemblies, Path.Combine(Directory.GetCurrentDirectory(), @"ModuleSaveFile.json"));
            JsonExtensions.Save(_loadedViewNamesService.LoadedViewNames, Path.Combine(Directory.GetCurrentDirectory(), @"LoadedViewsSaveFile.json"));

            RadWindow.Alert(@"Configuration Saved");
        }

        /// <summary>
        /// ViewObject navigation method.
        /// </summary>
        /// <param name="navigatePath">The path of the view to navigate to.</param>
        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
            {
                _regionManager.RequestNavigate("ContentRegion", navigatePath);
            }
        }

        /// <summary>
        /// Adds all the selected assembly's views or the selected view to the views region.
        /// </summary>
        private void AddSelectedView()
        {
            if (_assemblyCollectionService.SelectedItem is AssemblyData assemblyData && assemblyData.IsEnabled)
            {
                foreach (TypeData typeData in assemblyData.Types)
                {
                    if (typeData.ViewInfo != null)
                    {
                        foreach (var viewObject in ViewCollectionService.Views)
                        {
                            Type type = viewObject.GetType();
                            if (typeData.FullName == type.FullName)
                            {
                                object instance = Activator.CreateInstance(type);
                                _regionManager.AddToRegion(@"LoadedViewsRegion", instance);
                                typeData.ViewInfo.NumberOfViewInstances++;
                            }
                        }
                    }
                }
            }
            else if (_assemblyCollectionService.SelectedItem is TypeData typeData && (typeData.ViewInfo != null))
            {
                foreach (var viewObject in ViewCollectionService.Views)
                {
                    if (typeData.FullName == viewObject.GetType().FullName)
                    {
                        object instance = Activator.CreateInstance(viewObject.GetType());
                        _regionManager.AddToRegion(@"LoadedViewsRegion", instance);
                        typeData.ViewInfo.NumberOfViewInstances++;
                    }
                }
            }
        }

        /// <summary>
        /// Removes the selected view from the views region.
        /// </summary>
        private void RemoveSelectedView()
        {
            if (ViewCollectionService.SelectedView != null && _regionManager.Regions[@"LoadedViewsRegion"].Views.Contains(ViewCollectionService.SelectedView))
            {
                foreach (AssemblyData assemblyData in _assemblyCollectionService.Assemblies)
                {
                    foreach (TypeData typeData in assemblyData.Types)
                    {
                        if (typeData.FullName == ViewCollectionService.SelectedView.GetType().FullName)
                        {
                            typeData.ViewInfo.NumberOfViewInstances--;
                        }
                    }
                }

                _regionManager.Regions[@"LoadedViewsRegion"].Remove(ViewCollectionService.SelectedView);
            }
        }

        /// <summary>
        /// Command for double clicking a view <see cref="object"/>.
        /// </summary>
        private void DisplaySelectedView()
        {
            return;
        }

        /// <summary>
        /// Can always execute.
        /// </summary>
        /// <returns>True.</returns>
        private bool CanExecute()
        {
            return true;
        }

        /// <summary>
        /// Can only add a view if the selected item is an <see cref="AssemblyData"/> that has a <see cref="TypeData"/>
        /// that is a view or the selected item is a <see cref="TypeData"/> that is a view.
        /// </summary>
        /// <returns>True if the selected item can be added to the view collection, false if not.</returns>
        private bool CanAdd()
        {
            if (_assemblyCollectionService.SelectedItem == null)
            {
                return false;
            }

            if (_assemblyCollectionService.SelectedItem is AssemblyData assemblyData)
            {
                foreach (TypeData typeData in assemblyData.Types)
                {
                    if (typeData.ViewInfo != null)
                    {
                        return true;
                    }
                }

                return false;
            }
            else if (_assemblyCollectionService.SelectedItem is TypeData typeData)
            {
                if (typeData.ViewInfo != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Removes the selected view from the active views collection.
        /// </summary>
        /// <returns>True if the view object exists in the active views collection.</returns>
        private bool CanRemove()
        {
            if (_regionManager.Regions[@"LoadedViewsRegion"].Views.Contains(ViewCollectionService.SelectedView))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}