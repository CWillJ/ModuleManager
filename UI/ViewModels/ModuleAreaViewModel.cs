namespace ModuleManager.UI.ViewModels
{
    using System.Collections.ObjectModel;
    using ModuleManager.UI.Events;
    using ModuleObjects.Classes;
    using Prism.Events;
    using Prism.Mvvm;

    /// <summary>
    /// View model for module area.
    /// </summary>
    public class ModuleAreaViewModel : BindableBase
    {
        private ObservableCollection<Module> _modules;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleAreaViewModel"/> class.
        /// </summary>
        /// <param name="eventAggregator">Event aggregator.</param>
        public ModuleAreaViewModel(IEventAggregator eventAggregator)
        {
            _modules = new ObservableCollection<Module>();

            eventAggregator.GetEvent<UpdateModuleCollectionEvent>().Subscribe(ModuleCollectionUpdated);
        }

        /// <summary>
        /// Gets or sets a collection of ModuleManager.ModuleObjects.Classes.Module.
        /// </summary>
        public ObservableCollection<Module> Modules
        {
            get { return _modules; }
            set { SetProperty(ref _modules, value); }
        }

        private void ModuleCollectionUpdated(ObservableCollection<Module> modules)
        {
            Modules = modules;
        }
    }
}
