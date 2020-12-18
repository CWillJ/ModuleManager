namespace ModuleManager.Common.Classes.Data
{
    /// <summary>
    /// An object that stores info about the view type.
    /// </summary>
    public class ViewData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewData"/> class.
        /// </summary>
        public ViewData()
        {
            NumberOfViewInstances = 0;
            ViewPosition = 0;
        }

        /// <summary>
        /// Gets or sets the nubmer of view instances of this type.
        /// </summary>
        public int NumberOfViewInstances { get; set; }

        /// <summary>
        /// Gets or sets the position of this view type in the region.
        /// </summary>
        public int ViewPosition { get; set; }
    }
}