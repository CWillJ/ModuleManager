namespace ModuleManager.ModuleObjects.Classes
{
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;
    using ModuleManager.ModuleObjects.Loaders;

    /// <summary>
    /// AssemblyData holds the file path to the assembly so it can be loaded and
    /// the collection of modules in the assembly.
    /// </summary>
    public class AssemblyData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyData"/> class.
        /// </summary>
        public AssemblyData()
        {
            Name = string.Empty;
            IsEnabled = false;
            FilePath = string.Empty;
            Modules = new ObservableCollection<ModuleData>();
            Loader = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyData"/> class.
        /// Specify the path to the assembly and the collection of modules.
        /// </summary>
        /// <param name="name">Name of the assembly.</param>
        /// <param name="filePath">File path to the assembly.</param>
        /// <param name="modules">Collection of modules contained in the assembly.</param>
        public AssemblyData(string name, string filePath, ObservableCollection<ModuleData> modules)
        {
            Name = name;
            IsEnabled = false;
            FilePath = filePath;
            Modules = modules;
            Loader = null;
        }

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
        /// Ignored by the XmlSerializer when saving the configuration.
        /// </summary>
        [XmlIgnore]
        public AssemblyLoader Loader { get; private set; }

        /// <summary>
        /// Load this assembly.
        /// </summary>
        public void Load()
        {
            Loader = new AssemblyLoader(FilePath);
            Loader.LoadFromAssemblyPath(FilePath);
        }

        /// <summary>
        /// Unload this assembly.
        /// </summary>
        public void Unload()
        {
            if (Loader == null)
            {
                return;
            }

            Loader.Unload();
            Loader = null;
        }

        /// <summary>
        /// Loads all assemblies with checked boxes and
        /// unloads the unchecked ones.
        /// </summary>
        public void LoadUnload()
        {
            if (IsEnabled || AreAnyModulesChecked())
            {
                Load();
            }
            else
            {
                Unload();
            }
        }

        /// <summary>
        /// Checks to see if any modules in the assembly are checked or enabled.
        /// </summary>
        /// <returns>True if any ModuleData's are checked, false otherwise.</returns>
        public bool AreAnyModulesChecked()
        {
            foreach (var module in Modules)
            {
                if (module.IsEnabled)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Overrides the ToString method and formats the string output.
        /// </summary>
        /// <returns>A desired format for the assembly.</returns>
        public override string ToString()
        {
            string s = Name + @" located: " + FilePath + "\n\n";

            foreach (var module in Modules)
            {
                s += module.ToString() + "\n";
            }

            return s;
        }
    }
}