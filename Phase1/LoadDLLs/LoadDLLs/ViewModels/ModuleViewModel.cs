namespace LoadDLLs.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using LoadDLLs.Classes;

    public class ModuleViewModel
    {
        /// <summary>
        ///  (Future Implementation)
        /// Gets or sets the Methods from a module
        /// </summary>
        public ObservableCollection<Module> Modules { get; set; }

        /// <summary>
        /// Loads methods of the current module
        /// </summary>
        public void LoadModules()
        {

        }
    }
}
