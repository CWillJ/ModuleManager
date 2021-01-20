namespace ModuleManager.Core.UI.Interfaces
{
    /// <summary>
    /// Service providing concrete <see cref="IProgressBarService"/> implementations.
    /// </summary>
    public interface IProgressBarService
    {
        /// <summary>
        /// Gets or sets a <see cref="string"/> of the name of the assembly currently being loaded.
        /// </summary>
        public string AssemblyName { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="double"/> for current progress of a progress bar.
        /// </summary>
        public double CurrentProgress { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="string"/> of the text of a progress bar.
        /// </summary>
        public string Text { get; set; }
    }
}
