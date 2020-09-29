namespace ModuleManager.UI.ViewModels
{
    using ModuleManager.UI.Events;
    using Prism.Events;
    using Prism.Mvvm;

    /// <summary>
    /// View model for progress bar view.
    /// </summary>
    public class ProgressBarViewModel : BindableBase
    {
        private string _progressBarText;
        private bool _progressBarIsVisible;
        private double _currentProgress;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressBarViewModel"/> class.
        /// </summary>
        /// <param name="eventAggregator">Event aggregator.</param>
        public ProgressBarViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<UpdateProgressBarTextEvent>().Subscribe(ProgressBarTextUpdated);
            eventAggregator.GetEvent<UpdateProgressBarVisibilityEvent>().Subscribe(ProgressBarVisibilityUpdated);
            eventAggregator.GetEvent<UpdateProgressBarCurrentProgressEvent>().Subscribe(ProgressBarCurrentProgressUpdated);

            _progressBarText = string.Empty;
            _currentProgress = 0;
            _progressBarIsVisible = false;
        }

        /// <summary>
        /// Gets or sets the progress bar text.
        /// </summary>
        public string ProgressBarText
        {
            get { return _progressBarText; }
            set { SetProperty(ref _progressBarText, value); }
        }

        /// <summary>
        /// Gets the current progress of the status bar.
        /// </summary>
        public double CurrentProgress
        {
            get { return _currentProgress; }
            private set { SetProperty(ref _currentProgress, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a progress bar is visible.
        /// </summary>
        public bool ProgressBarIsVisible
        {
            get { return _progressBarIsVisible; }
            set { SetProperty(ref _progressBarIsVisible, value); }
        }

        private void ProgressBarTextUpdated(string obj)
        {
            ProgressBarText = obj;
        }

        private void ProgressBarVisibilityUpdated(bool visible)
        {
            ProgressBarIsVisible = visible;
        }

        private void ProgressBarCurrentProgressUpdated(double progress)
        {
            CurrentProgress = progress;
        }
    }
}
