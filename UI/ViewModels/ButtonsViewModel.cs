namespace ModuleManager.UI.ViewModels
{
    using System;
    using Prism.Commands;
    using Prism.Mvvm;

    /// <summary>
    /// View model for the buttons area.
    /// </summary>
    public class ButtonsViewModel : BindableBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonsViewModel"/> class.
        /// </summary>
        public ButtonsViewModel()
        {
            LoadModulesCommand = new DelegateCommand(StoreModules, CanExecute);
            SaveConfigCommand = new DelegateCommand(SaveConfig, CanExecute);
            LoadUnloadCommand = new DelegateCommand(LoadUnload, CanExecute);
        }

        /// <summary>
        /// Gets or sets the LoadModulesCommand as a ModuleManagerICommand.
        /// </summary>
        public DelegateCommand LoadModulesCommand { get; set; }

        /// <summary>
        /// Gets or sets the SaveConfigCommand as a ModuleManagerICommand.
        /// </summary>
        public DelegateCommand SaveConfigCommand { get; set; }

        /// <summary>
        /// Gets or sets the LoadUnloadCommand as a ModuleManagerICommand.
        /// </summary>
        public DelegateCommand LoadUnloadCommand { get; set; }

        private void LoadUnload()
        {
            throw new NotImplementedException();
        }

        private void SaveConfig()
        {
            throw new NotImplementedException();
        }

        private void StoreModules()
        {
            throw new NotImplementedException();
        }

        private bool CanExecute()
        {
            return true;
        }
    }
}
