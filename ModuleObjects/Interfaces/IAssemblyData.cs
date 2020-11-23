namespace ModuleManager.ModuleObjects.Interfaces
{
    using System.Collections.ObjectModel;
    using System.Reflection;
    using System.Xml.Serialization;
    using ModuleManager.ModuleObjects.Classes;
    using Prism.Modularity;

    /// <summary>
    /// Assembly object interface.
    /// </summary>
    public interface IAssemblyData : IModuleInfo
    {
        /// <summary>
        /// Gets or sets the name.
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
        /// Gets or sets an <see cref="ObservableCollection{ModuleData}"/> of modules contained in the assembly.
        /// </summary>
        public ObservableCollection<ModuleData> Modules { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="AssemblyLoader"/> to load/unload this assembly.
        /// </summary>
        [XmlIgnore]
        public AssemblyLoader Loader { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="System.Reflection.Assembly"/> of this AssemblyData.
        /// </summary>
        [XmlIgnore]
        public Assembly Assembly { get; set; }

        /// <summary>
        /// Used to return a <see cref="ModuleInfo"/> from the properties.
        /// </summary>
        /// <returns>A <see cref="ModuleInfo"/>.</returns>
        public ModuleInfo GetModuleInfo();
    }
}
