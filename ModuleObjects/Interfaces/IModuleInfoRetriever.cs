namespace ModuleManager.ModuleObjects.Interfaces
{
    using System.Collections.ObjectModel;
    using ModuleManager.ModuleObjects.Classes;

    /// <summary>
    /// Service designed to abstract calls to retrieve modules from dll files.
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
        /// GetModules will create an ObservableCollection of type Module to organize
        /// the information from the dll file and its related .xml file.
        /// </summary>
        /// <param name="dllFiles">A string array containing the names of all dll files in the DllDirectory.</param>
        /// <returns>Returns an collection of Module objects.</returns>
        ObservableCollection<AssemblyData> GetModules(string[] dllFiles);
    }
}