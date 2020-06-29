namespace LoadDLLs.ViewModels
{
    using System.Collections.ObjectModel;
    using LoadDLLs.Classes;

    /// <summary>
    /// Method class view model.
    /// </summary>
    public class MethodViewModel
    {
        /// <summary>
        /// Gets or sets the Methods from a module.
        /// </summary>
        public ObservableCollection<ModuleMethod> Methods { get; set; }

        /// <summary>
        /// Loads methods of the current module.
        /// </summary>
        public void LoadMethods()
        {
            return;
        }
    }
}
