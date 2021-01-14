namespace ModuleManager.Common.Classes.Data
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// An object that stores info about the view type.
    /// </summary>
    public class ViewData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewData"/> class.
        /// </summary>
        public ViewData()
            : this(string.Empty, new ObservableCollection<object>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewData"/> class.
        /// </summary>
        /// <param name="assemblyName">The name of the assembly that contains these view objects.</param>
        /// <param name="viewsObjects">The view objects that this assembly contains.</param>
        public ViewData(string assemblyName, ObservableCollection<object> viewsObjects)
        {
            AssemblyName = assemblyName;
            ViewObjects = viewsObjects;
        }

        /// <summary>
        /// Gets or sets the <see cref="string"/> name of the assembly that contains the view objects.
        /// </summary>
        public string AssemblyName { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ObservableCollection{Object}"/> of view objects.
        /// </summary>
        public ObservableCollection<object> ViewObjects { get; set; }
    }
}
