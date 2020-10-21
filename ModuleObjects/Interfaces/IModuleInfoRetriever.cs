namespace ModuleManager.ModuleObjects.Interfaces
{
    using System;
    using System.Collections.ObjectModel;
    using ModuleManager.ModuleObjects.Classes;

    /// <summary>
    /// Retrieves assemblies from dll files.
    /// </summary>
    public interface IModuleInfoRetriever
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
        /// Creates an ObservableCollection of AssemblyData to organize
        /// the information from the dll file and its related xml file.
        /// </summary>
        /// <param name="dllFiles">A string array containing the names of all dll files in the DllDirectory.</param>
        /// <returns>Returns an collection of AssemblyData objects.</returns>
        ObservableCollection<AssemblyData> GetAssemblies(string[] dllFiles);

        /// <summary>
        /// Initialized ModuleInfoRetriever's properties.
        /// </summary>
        /// <param name="moduleDirectory">Directory containing dll files.</param>
        /// <param name="moduleFilePath">Name of the specific dll file.</param>
        public void Initialize(string moduleDirectory, string moduleFilePath);

        /// <summary>
        /// AddConstructorsToCollection get all constructors from the passed in Type.
        /// </summary>
        /// <param name="type">The Type where the members are coming from.</param>
        /// <returns>An ObservableCollection of ModuleConstructor objects.</returns>
        public ObservableCollection<ModuleConstructor> AddConstructorsToCollection(Type type);

        /// <summary>
        /// AddPropertiesToCollection gets all properties from the passed in Type.
        /// </summary>
        /// <param name="type">The Type where the members are coming from.</param>
        /// <returns>An ObservableCollection of ModulePropery objects.</returns>
        public ObservableCollection<ModuleProperty> AddPropertiesToCollection(Type type);

        /// <summary>
        /// AddMethodsToCollection gets all methods from the passed in Type.
        /// </summary>
        /// <param name="type">The Type where the methods are coming from.</param>
        /// <returns>An ObservableCollection of ModuleMethod objects.</returns>
        public ObservableCollection<ModuleMethod> AddMethodsToCollection(Type type);
    }
}