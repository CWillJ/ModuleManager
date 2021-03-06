﻿namespace ModuleManager.Core.UI.ViewModels
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
            _regionManager = regionManager;

            _assemblyCollectionService = assemblyCollectionService;
            _assemblyDataLoaderService = assemblyDataLoaderService;
            _viewCollectionService = viewCollectionService;
            _progressBarService = progressBarService;
            _loadedViewNamesService = loadedViewNamesService;

            UseSaveFileDialog = false;

            NavigateCommand = new Prism.Commands.DelegateCommand<string>(Navigate);
            SaveConfigCommand = new Prism.Commands.DelegateCommand(SaveConfig, CanExecute);
            AddSelectedViewCommand = new Prism.Commands.DelegateCommand(AddSelectedView, CanExecute);
            RemoveSelectedViewCommand = new Prism.Commands.DelegateCommand(RemoveSelectedView, CanExecute);
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
        /// The task that updates the <see cref="IProgressBarService"/>.
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
                        AddViewToRegion(_viewCollectionService.GetViewObjectByName(typeData.FullName), @"LoadedViewsRegion");
                    }
                }
            }
            else if (_assemblyCollectionService.SelectedItem is TypeData typeData && (typeData.ViewInfo != null))
            {
                if (typeData.ViewInfo != null)
                {
                    AddViewToRegion(_viewCollectionService.GetViewObjectByName(typeData.FullName), @"LoadedViewsRegion");
                }
            }
        }

        /// <summary>
        /// Adds the view <see cref="object"/> to the region.
        /// </summary>
        /// <param name="viewObject">The view <see cref="object"/> to be added.</param>
        /// <param name="regionName">The <see cref="string"/> name of the region.</param>
        private void AddViewToRegion(object viewObject, string regionName)
        {
            if (viewObject != null)
            {
                Type type = viewObject.GetType();
                object instance = Activator.CreateInstance(type);
                _regionManager.AddToRegion(regionName, instance);
            }
        }

        /// <summary>
        /// Removes the selected view from the views region.
        /// </summary>
        private void RemoveSelectedView()
        {
            if (ViewCollectionService.SelectedView != null && _regionManager.Regions[@"LoadedViewsRegion"].Views.Contains(ViewCollectionService.SelectedView))
            {
                _regionManager.Regions[@"LoadedViewsRegion"].Remove(ViewCollectionService.SelectedView);
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
