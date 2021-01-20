namespace ModuleManager.Core.UI.Services
{
    using ModuleManager.Core.UI.Interfaces;
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

        /// <inheritdoc/>
        public string AssemblyName
        {
            get { return _assemblyName; }
            set { SetProperty(ref _assemblyName, value); }
        }

        /// <inheritdoc/>
        public double CurrentProgress
        {
            get { return _currentProgress; }
            set { SetProperty(ref _currentProgress, value); }
        }

        /// <inheritdoc/>
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }
    }
}
