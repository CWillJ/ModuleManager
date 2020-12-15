namespace ModuleManager.Common.Services
{
    using System.Collections.ObjectModel;
    using ModuleManager.Common.Interfaces;
    using Prism.Mvvm;

    /// <inheritdoc cref="IViewCollectionService"/>
    public class ViewCollectionService : BindableBase, IViewCollectionService
    {
        private readonly ObservableCollection<object> _views;
        private ObservableCollection<object> _activeViews;
        private object _selectedView;
        private string _selectedViewName;
        private int _selectedViewIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewCollectionService"/> class.
        /// </summary>
        public ViewCollectionService()
        {
            _views = new ObservableCollection<object>();
            _activeViews = new ObservableCollection<object>();
            _selectedView = null;
            _selectedViewName = @"Loaded Views";
            _selectedViewIndex = 0;
        }

        /// <inheritdoc/>
        public ObservableCollection<object> Views
        {
            get { return _views; }
        }

        /// <inheritdoc/>
        public ObservableCollection<object> ActiveViews
        {
            get { return _activeViews; }
            set { SetProperty(ref _activeViews, value); }
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
                if (_selectedView != null)
                {
                    SelectedViewName = _selectedView.GetType().Name;
                }
                else
                {
                    SelectedViewName = @"Loaded Views";
                }
            }
        }

        /// <inheritdoc/>
        public string SelectedViewName
        {
            get
            {
                return _selectedViewName;
            }

            set
            {
                SetProperty(ref _selectedViewName, value);
            }
        }

        /// <inheritdoc/>
        public int SelectedViewIndex
        {
            get
            {
                return _selectedViewIndex;
            }

            set
            {
                SetProperty(ref _selectedViewIndex, value);
            }
        }

        /// <inheritdoc/>
        public void AddView(object viewObject)
        {
            Views.Add(viewObject);
        }

        /// <inheritdoc/>
        public void RemoveView(object viewObject)
        {
            foreach (var view in Views)
            {
                if (view == viewObject)
                {
                    Views.Remove(view);
                }
            }
        }
    }
}