namespace ModuleManager.UI.Services
{
    using ModuleManager.UI.Interfaces;
    using Prism.Mvvm;

    /// <summary>
    /// Service providing concrete <see cref="IProgressBarService"/> implementations.
    /// </summary>
    public class ProgressBarService : BindableBase, IProgressBarService
    {
        private string _assemblyName;
        private string _text;
        private double _currentProgress;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressBarService"/> class.
        /// </summary>
        public ProgressBarService()
        {
            _assemblyName = string.Empty;
            _text = string.Empty;
            _currentProgress = 0.0;
        }

        /// <summary>
        /// Gets or sets the name of the assembly currently being loaded.
        /// </summary>
        public string AssemblyName
        {
            get { return _assemblyName; }
            set { SetProperty(ref _assemblyName, value); }
        }

        /// <summary>
        /// Gets or sets the text of the progress bar.
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        /// <summary>
        /// Gets or sets the current progress of the progress bar.
        /// </summary>
        public double CurrentProgress
        {
            get { return _currentProgress; }
            set { SetProperty(ref _currentProgress, value); }
        }
    }
}