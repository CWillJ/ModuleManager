namespace ModuleManager.Common.Services
{
    using System;
    using System.Collections.ObjectModel;
    using ModuleManager.Common.Classes.Data;
    using ModuleManager.Common.Interfaces;
    using Prism.Mvvm;

    /// <inheritdoc cref="IViewCollectionService"/>
    public class ViewCollectionService : BindableBase, IViewCollectionService
    {
        private readonly ObservableCollection<ViewData> _viewDataCollection;
        private object _selectedView;
        private string _selectedViewName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewCollectionService"/> class.
        /// </summary>
        public ViewCollectionService()
        {
            _viewDataCollection = new ObservableCollection<ViewData>();
            _selectedView = null;
            _selectedViewName = @"Loaded ViewDataCollection";
        }

        /// <inheritdoc/>
        public ObservableCollection<ViewData> ViewDataCollection
        {
            get { return _viewDataCollection; }
        }

        /// <inheritdoc/>
        public object SelectedView
        {
            get
            {
                return _selectedView;
            }

            set
            {
                SetProperty(ref _selectedView, value);
                SelectedViewToDo();
            }
        }

        /// <inheritdoc/>
        public string SelectedViewName
        {
            get { return _selectedViewName; }
            set { SetProperty(ref _selectedViewName, value); }
        }

        /// <inheritdoc/>
        public void AddView(object viewObject)
        {
            Type viewType = viewObject.GetType();
            string assemblyName = viewType.Assembly.FullName;
            string viewName = viewType.FullName;
            bool wasAdded = false;

            foreach (var viewData in ViewDataCollection)
            {
                if (viewData.AssemblyName == assemblyName)
                {
                    foreach (var view1 in viewData.ViewObjects)
                    {
                        string view1Name = view1.GetType().FullName;
                        if (viewName == view1Name)
                        {
                            viewData.ViewObjects[viewData.ViewObjects.IndexOf(view1)] = viewObject;
                            wasAdded = true;
                            break;
                        }
                    }

                    if (!wasAdded)
                    {
                        viewData.ViewObjects.Add(viewObject);
                        wasAdded = true;
                    }
                }
            }

            if (!wasAdded)
            {
                ViewDataCollection.Add(new ViewData(assemblyName, new ObservableCollection<object>() { viewObject }));
            }
        }

        /// <inheritdoc/>
        public void RemoveView(object viewObject)
        {
            string assemblyName = viewObject.GetType().Assembly.FullName;

            if (!ViewDataAssemblyNameContainsViewObject(assemblyName, viewObject))
            {
                return;
            }

            foreach (var viewData in ViewDataCollection)
            {
                if (viewData.AssemblyName == assemblyName)
                {
                    viewData.ViewObjects.Remove(viewData);

                    if (viewData.ViewObjects.Count == 0)
                    {
                        ViewDataCollection.Remove(viewData);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public bool CollectionContainsAssemblyName(string assemblyName)
        {
            foreach (var viewData in ViewDataCollection)
            {
                if (viewData.AssemblyName == assemblyName)
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public bool ViewDataAssemblyNameContainsViewObject(string assemblyName, object viewObject)
        {
            foreach (var viewData in ViewDataCollection)
            {
                if (viewData.AssemblyName == assemblyName)
                {
                    foreach (var view in viewData.ViewObjects)
                    {
                        string viewName1 = view.GetType().FullName;
                        string viewName2 = viewObject.GetType().FullName;
                        if (viewName1 == viewName2)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public object GetViewObjectByName(string viewName)
        {
            foreach (var viewData in ViewDataCollection)
            {
                foreach (var viewObject in viewData.ViewObjects)
                {
                    string viewObjectNameFromCollection;

                    if (viewObject == null)
                    {
                        return null;
                    }
                    else
                    {
                        viewObjectNameFromCollection = viewObject.GetType().FullName;
                    }

                    if (viewObjectNameFromCollection == viewName)
                    {
                        return viewObject;
                    }
                }
            }

            return null;
        }

        private void SelectedViewToDo()
        {
            if (_selectedView != null)
            {
                SelectedViewName = _selectedView.GetType().Name;
            }
            else
            {
                SelectedViewName = @"Loaded ViewDataCollection";
            }
        }
    }
}
