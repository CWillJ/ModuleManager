namespace ModuleManager.ModuleLoader.Interfaces
{
    using System.Collections.ObjectModel;
    using ModuleManager.ModuleObjects.Classes;

    /// <summary>
    /// Retrieves assemblies from dll files.
    /// </summary>
    public interface IAssemblyLoaderService
    {
        /// <summary>
        /// Gets or sets CurrentAssemblyName is the name of the assembly being loaded.
        /// </summary>
        string CurrentAssemblyName { get; set; }

        /// <summary>
        /// Gets or sets CurrentTypeName is the name of the type being loaded.
        /// </summary>
        string CurrentTypeName { get; set; }

        /// <summary>
        /// Gets or sets the current percentage of load compleation of the current assembly.
        /// </summary>
        double PercentOfAssemblyLoaded { get; set; }

        /// <summary>
        /// Gets or sets DllDirectory is the directory path of the .dll files.
        /// </summary>
        string DllDirectory { get; set; }

        /// <summary>
        /// Initialize properties.
        /// </summary>
        /// <param name="moduleDirectory">Directory containing dll files.</param>
        /// <param name="moduleFilePath">Name of the specific dll file.</param>
        public void Initialize(string moduleDirectory, string moduleFilePath);

        /// <summary>
        /// Creates an <see cref="ObservableCollection{ModuleInfoData}"/> to organize
        /// the information from the dll file and its related xml file.
        /// </summary>
        /// <param name="dllFiles">A string array containing the names of all dll files in the DllDirectory.</param>
        /// <returns>Returns an <see cref="ObservableCollection{ModuleInfoData}"/>.</returns>
        public ObservableCollection<AssemblyData> GetAssemblies(string[] dllFiles);

        /// <summary>
        /// Loads all enabled <see cref="ModuleInfoData"/> and unloads the disabled ones.
        /// </summary>
        /// <param name="assembly">An <see cref="ModuleInfoData"/> object passed by reference.</param>
        public void LoadUnload(ref AssemblyData assembly);

        /// <summary>
        /// Loads all enabled <see cref="ModuleInfoData"/> and unloads the disabled ones.
        /// </summary>
        /// <param name="assemblies">A <see cref="ObservableCollection{ModuleInfoData}"/> passed by reference.</param>
        public void LoadUnload(ref ObservableCollection<AssemblyData> assemblies);

        /// <summary>
        /// Loads all <see cref="ModuleInfoData"/> in a <see cref="ObservableCollection{ModuleInfoData}"/>.
        /// </summary>
        /// <param name="assemblies">A <see cref="ObservableCollection{ModuleInfoData}"/> objects.</param>
        public void LoadAll(ref ObservableCollection<AssemblyData> assemblies);

        /// <summary>
        /// Load an <see cref="ModuleInfoData"/>.
        /// </summary>
        /// <param name="assembly">Assembly to load passed by reference.</param>
        public void Load(ref AssemblyData assembly);

        /// <summary>
        /// Unload an <see cref="ModuleInfoData"/>.
        /// </summary>
        /// <param name="assembly"><see cref="ModuleInfoData"/> to unload passed by reference.</param>
        public void Unload(ref AssemblyData assembly);
    }
}