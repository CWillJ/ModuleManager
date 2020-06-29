namespace ModuleManager.ViewModels
{
    using System.Collections.ObjectModel;
    using ModuleManager.Classes;

    /// <summary>
    /// Module view model class.
    /// </summary>
    public class ModuleViewModel
    {
        /// <summary>
        /// Gets or sets the Methods from a module.
        /// </summary>
        public ObservableCollection<Module> Modules { get; set; }

        /// <summary>
        /// Loads methods of the current module.
        /// </summary>
        public void LoadModules()
        {
            return;
        }
    }
}