namespace LoadDLLs.ViewModels
{
    using LoadDLLs.Classes;
    using System.Collections.ObjectModel;

    public class MethodViewModel
    {
        /// <summary>
        /// Gets or sets the Methods from a module
        /// </summary>
        public ObservableCollection<ModuleMethod> Methods { get; set; }

        /// <summary>
        /// Loads methods of the current module
        /// </summary>
        public void LoadMethods()
        {

        }
    }
}
