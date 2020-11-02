namespace ModuleManager.UI.Interfaces
{
    /// <summary>
    /// A progress bar service.
    /// </summary>
    public interface IProgressBarService
    {
        /// <summary>
        /// Gets or sets the name of the assembly currently being loaded.
        /// </summary>
        public string AssemblyName { get; set; }

        /// <summary>
        /// Gets or sets the current progress of a progress bar.
        /// </summary>
        public double CurrentProgress { get; set; }

        /// <summary>
        /// Gets or sets the text of a progress bar.
        /// </summary>
        public string Text { get; set; }
    }
}
