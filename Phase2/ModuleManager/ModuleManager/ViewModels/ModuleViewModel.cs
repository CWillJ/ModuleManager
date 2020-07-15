namespace ModuleManager.ViewModels
{
    using ModuleManager.Classes;
    using System.Collections.ObjectModel;

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