namespace ModuleManager.ViewModels
{
    using ModuleManager.Classes;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Method class view model.
    /// </summary>
    public class MemberViewModel
    {
        /// <summary>
        /// Gets or sets the Methods from a module.
        /// </summary>
        public ObservableCollection<ModuleMember> Members { get; set; }

        /// <summary>
        /// Loads methods of the current module.
        /// </summary>
        public void LoadMethods()
        {
            return;
        }
    }
}