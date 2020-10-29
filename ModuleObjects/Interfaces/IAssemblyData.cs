namespace ModuleManager.ModuleObjects.Interfaces
{
    using System.Collections.ObjectModel;
    using System.Reflection;
    using System.Xml.Serialization;
    using ModuleManager.ModuleObjects.Classes;
    using ModuleManager.ModuleObjects.Loaders;

    /// <summary>
    /// Module object interface.
    /// </summary>
    public interface IAssemblyData
    {
        /// <summary>
        /// Gets or sets the name of the assembly.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the assembly is enabled or disabled.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Gets or sets the file path to assembly.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets a collection of modules contained in the assembly.
        /// </summary>
        public ObservableCollection<ModuleData> Modules { get; set; }

        /// <summary>
        /// Gets the AssemblyLoader to load/unload this assembly.
        /// </summary>
        [XmlIgnore]
        public AssemblyLoader Loader { get; }

        /// <summary>
        /// Gets or sets the actual Assembly of this AssemblyData.
        /// </summary>
        [XmlIgnore]
        public Assembly Assembly { get; set; }

        /// <summary>
        /// Loads all assemblies that are enabled and
        /// unloads the ones that are not.
        /// </summary>
        /// <param name="moduleInfoRetriever">ModuleInfoRetriever.</param>
        public void LoadUnload(IModuleInfoRetriever moduleInfoRetriever);
    }
}
