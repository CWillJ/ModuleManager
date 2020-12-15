﻿namespace ModuleManager.Core.UI.ViewModels
{
    using System;
    using ModuleManager.Common.Classes;
    using ModuleManager.Common.Interfaces;
    using Prism.Mvvm;

    /// <summary>
    /// View model for the buttons view.
    /// </summary>
    public class ViewDisplayViewModel : BindableBase
    {
        private readonly IAssemblyCollectionService _assemblyCollectionService;
        private readonly IViewCollectionService _viewCollectionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewDisplayViewModel"/> class.
        /// </summary>
        /// <param name="assemblyCollectionService">The <see cref="IAssemblyCollectionService"/>.</param>
        /// <param name="viewCollectionService">The <see cref="IViewCollectionService"/>.</param>
        public ViewDisplayViewModel(IAssemblyCollectionService assemblyCollectionService, IViewCollectionService viewCollectionService)
        {
            _assemblyCollectionService = assemblyCollectionService ?? throw new ArgumentNullException("AssemblyCollectionService");
            _viewCollectionService = viewCollectionService ?? throw new ArgumentNullException("ViewCollectionService");

            AddSelectedViewCommand = new Prism.Commands.DelegateCommand(AddSelectedView, CanExecute);
            RemoveSelectedViewCommand = new Prism.Commands.DelegateCommand(RemoveSelectedView, CanExecute);
        }

        /// <summary>
        /// Gets the <see cref="IViewCollectionService"/>.
        /// </summary>
        public IViewCollectionService ViewCollectionService
        {
            get { return _viewCollectionService; }
        }

        /// <summary>
        /// Gets or sets the AddSelectedViewCommand as a <see cref="Prism.Commands.DelegateCommand"/>.
        /// </summary>
        public Prism.Commands.DelegateCommand AddSelectedViewCommand { get; set; }

        /// <summary>
        /// Gets or sets the RemoveSelectedViewCommand as a <see cref="Prism.Commands.DelegateCommand"/>.
        /// </summary>
        public Prism.Commands.DelegateCommand RemoveSelectedViewCommand { get; set; }

        /// <summary>
        /// Adds all the selected assembly's views or the selected view to the views region.
        /// </summary>
        private void AddSelectedView()
        {
            if (_assemblyCollectionService.SelectedItem is AssemblyData assemblyData && assemblyData.IsEnabled)
            {
                foreach (TypeData typeData in assemblyData.Types)
                {
                    if (typeData.IsView)
                    {
                        foreach (var viewObject in ViewCollectionService.Views)
                        {
                            if (typeData.FullName == viewObject.GetType().FullName)
                            {
                                ViewCollectionService.ActiveViews.Add(viewObject);
                                ////_regionManager.Regions[@"LoadedViewsRegion"].Add(viewObject);
                            }
                        }
                    }
                }
            }
            else if (_assemblyCollectionService.SelectedItem is TypeData typeData && typeData.IsView)
            {
                foreach (var viewObject in ViewCollectionService.Views)
                {
                    if (typeData.FullName == viewObject.GetType().FullName)
                    {
                        ViewCollectionService.ActiveViews.Add(viewObject);
                        ////_regionManager.Regions[@"LoadedViewsRegion"].Add(viewObject);
                    }
                }
            }
        }

        /// <summary>
        /// Removes the selected view from the views region.
        /// </summary>
        private void RemoveSelectedView()
        {
            if (ViewCollectionService.SelectedView != null && ViewCollectionService.ActiveViews.Contains(ViewCollectionService.SelectedView))
            {
                ViewCollectionService.ActiveViews.RemoveAt(ViewCollectionService.ActiveViews.Count - 1 - ViewCollectionService.SelectedViewIndex);
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
                    if (typeData.IsView)
                    {
                        return true;
                    }
                }

                return false;
            }
            else if (_assemblyCollectionService.SelectedItem is TypeData typeData)
            {
                if (typeData.IsView)
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
            if (ViewCollectionService.ActiveViews.Contains(ViewCollectionService.SelectedView))
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