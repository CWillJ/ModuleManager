namespace ModuleManager.UI.ViewModels
{
    using ModuleManager.UI.Interfaces;
    using Prism.Mvvm;

    /// <summary>
    /// View model for progress bar view.
    /// </summary>
    public class ProgressBarViewModel : BindableBase
    {
        private IProgressBarService _progressBarService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressBarViewModel"/> class.
        /// </summary>
        /// <param name="progressBarService">Progress bar service.</param>
        public ProgressBarViewModel(IProgressBarService progressBarService)
        {
            _progressBarService = progressBarService;
        }

        /// <summary>
        /// Gets or sets the IProgressBarService.
        /// </summary>
        public IProgressBarService ProgressBarService
        {
            get { return _progressBarService; }
            set { _progressBarService = value; }
        }
    }
}