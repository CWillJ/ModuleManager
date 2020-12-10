namespace ModuleManager.Core.UI.ViewModels
{
    using ModuleManager.Core.UI.Interfaces;
    using Prism.Mvvm;

    /// <summary>
    /// View model for progress bar view.
    /// </summary>
    public class ProgressBarViewModel : BindableBase
    {
        private readonly IProgressBarService _progressBarService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressBarViewModel"/> class.
        /// </summary>
        /// <param name="progressBarService">Injected <see cref="IProgressBarService"/>.</param>
        public ProgressBarViewModel(IProgressBarService progressBarService)
        {
            _progressBarService = progressBarService;
        }

        /// <summary>
        /// Gets the <see cref="IProgressBarService"/>.
        /// </summary>
        public IProgressBarService ProgressBarService
        {
            get { return _progressBarService; }
        }
    }
}