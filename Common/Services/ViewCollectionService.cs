namespace ModuleManager.Common.Services
{
    using System.Collections.ObjectModel;
    using ModuleManager.Common.Interfaces;

    /// <inheritdoc cref="IViewCollectionService"/>
    public class ViewCollectionService : IViewCollectionService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewCollectionService"/> class.
        /// </summary>
        public ViewCollectionService()
        {
            ViewNameIndex = 0;
            Views = new ObservableCollection<object>();
        }

        /// <inheritdoc/>
        public ObservableCollection<object> Views { get; }

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