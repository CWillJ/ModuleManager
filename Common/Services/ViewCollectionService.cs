namespace ModuleManager.Common.Services
{
    using System.Collections.ObjectModel;
    using ModuleManager.Common.Interfaces;
    using Prism.Mvvm;

    /// <inheritdoc cref="IViewCollectionService"/>
    public class ViewCollectionService : BindableBase, IViewCollectionService
    {
        private readonly ObservableCollection<object> _views;
        private object _selectedView;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewCollectionService"/> class.
        /// </summary>
        public ViewCollectionService()
        {
            ViewNameIndex = 0;
            _views = new ObservableCollection<object>();
            _selectedView = null;
        }

        /// <inheritdoc/>
        public ObservableCollection<object> Views
        {
            get { return _views; }
        }

        /// <inheritdoc/>
        public object SelectedView
        {
            get { return _selectedView; }
            set { SetProperty(ref _selectedView, value); }
        }

        private int ViewNameIndex { get; set; }

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