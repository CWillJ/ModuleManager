namespace ModuleManager.Common.Interfaces
{
    using System.Collections.ObjectModel;
    using ModuleManager.Common.Classes;

    /// <summary>
    /// Service providing concrete <see cref="IAssemblyDataLoaderService"/> implementations.
    /// </summary>
    public interface IAssemblyDataLoaderService
    {
        /// <summary>
        /// Gets or sets the <see cref="string"/> directory path of the .dll files.
        /// </summary>
        public string DllDirectory { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="string"/> path of the .dll file.
        /// </summary>
        public string DllFilePath { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="string"/> name of the assembly being loaded.
        /// </summary>
        public string CurrentAssemblyName { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="string"/> name of the type being loaded.
        /// </summary>
        public string CurrentTypeName { get; set; }

        /// <summary>
        /// Gets or sets the current <see cref="double"/> percentage of load compleation of the current assembly.
        /// </summary>
        public double PercentOfAssemblyLoaded { get; set; }

        /// <summary>
        /// Initialize properties.
        /// </summary>
        /// <param name="modulesDirectory">Directory containing <see cref="string"/> dll files.</param>
        /// <param name="assemblyFilePath">Name of the specific <see cref="string"/> dll file.</param>
        public void Initialize(string modulesDirectory, string assemblyFilePath);

        /// <summary>
        /// Creates an <see cref="ObservableCollection{AssemblyData}"/> to organize
        /// the information from the dll file and its related xml file.
        /// </summary>
        /// <param name="dllFiles">A <see cref="string"/> array containing the names of all dll files in the DllDirectory.</param>
        /// <returns>Returns an <see cref="ObservableCollection{AssemblyData}"/>.</returns>
        public ObservableCollection<AssemblyData> GetAssemblies(string[] dllFiles);

        /// <summary>
        /// Loads an assembly and creates an <see cref="AssemblyData"/>.
        /// </summary>
        /// <param name="dllFile">The dll file to retrieve the assembly from.</param>
        /// <returns>An <see cref="AssemblyData"/>.</returns>
        public AssemblyData GetAssembly(string dllFile);

        /// <summary>
        /// Loads all enabled <see cref="AssemblyData"/> and unloads the disabled ones.
        /// </summary>
        /// <param name="assemblies">A <see cref="ObservableCollection{AssemblyData}"/> passed by reference.</param>
        public void LoadUnload(ref ObservableCollection<AssemblyData> assemblies);

        /// <summary>
        /// Loads all enabled <see cref="AssemblyData"/> and unloads the disabled ones.
        /// </summary>
        /// <param name="assemblyData">An <see cref="AssemblyData"/> object passed by reference.</param>
        public void LoadUnload(ref AssemblyData assemblyData);

        /// <summary>
        /// Loads all <see cref="AssemblyData"/> in a <see cref="ObservableCollection{AssemblyData}"/>.
        /// </summary>
        /// <param name="assemblies">A <see cref="ObservableCollection{AssemblyData}"/> objects.</param>
        public void LoadAll(ref ObservableCollection<AssemblyData> assemblies);

        /// <summary>
        /// Load an <see cref="AssemblyData"/> by reference.
        /// </summary>
        /// <param name="assemblyData"><see cref="AssemblyData"/> to load passed by reference.</param>
        public void Load(ref AssemblyData assemblyData);

        /// <summary>
        /// Unload an <see cref="AssemblyData"/> by reference.
        /// </summary>
        /// <param name="assemblyData"><see cref="AssemblyData"/> to unload passed by reference.</param>
        public void Unload(ref AssemblyData assemblyData);
    }
}
