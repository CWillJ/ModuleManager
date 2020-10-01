namespace ModuleManager.ViewModels
{
    using Prism.Commands;
    using Prism.Events;
    using Prism.Mvvm;

    /// <summary>
    /// ModuleManagerViewModel will handle commands from the main view.
    /// </summary>
    public class ShellViewModel : BindableBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShellViewModel"/> class.
        /// </summary>
        public ShellViewModel()
        {
        }

        /// <summary>
        /// Gets or sets the Navigation command for the view.
        /// </summary>
        public DelegateCommand<string> NavigateCommand { get; set; }
    }
}