namespace ModuleManager.ModuleObjects.Classes
{
    using System.Collections.ObjectModel;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using ModuleManager.ModuleObjects.Interfaces;
    using ModuleManager.ModuleObjects.Loaders;

    /// <summary>
    /// AssemblyData holds the file path to the assembly so it can be loaded and
    /// the collection of modules in the assembly.
    /// </summary>
    public class AssemblyData : IAssemblyData
    {
        private readonly IModuleInfoRetriever _moduleInfoRetriever;
        private bool _isEnabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyData"/> class.
        /// </summary>
        public AssemblyData()
        {
            _moduleInfoRetriever = null;
            Name = string.Empty;
            IsEnabled = false;
            FilePath = string.Empty;
            Modules = new ObservableCollection<ModuleData>();
            Loader = null;
            Assembly = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyData"/> class.
        /// Specify the path to the assembly and the collection of modules.
        /// </summary>
        /// <param name="moduleInfoRetriever">The IModuleInfoRetriever creating this AssemblyData.</param>
        /// <param name="name">Name of the assembly.</param>
        /// <param name="filePath">File path to the assembly.</param>
        /// <param name="modules">Collection of modules contained in the assembly.</param>
        public AssemblyData(IModuleInfoRetriever moduleInfoRetriever, string name, string filePath, ObservableCollection<ModuleData> modules)
        {
            _moduleInfoRetriever = moduleInfoRetriever;
            Name = name;
            IsEnabled = false;
            FilePath = filePath;
            Modules = modules;
            Loader = null;
            Assembly = null;
        }

        /// <summary>
        /// Gets or sets the name of the assembly.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the assembly is enabled or disabled.
        /// Nullable to handle three state checkbox.
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }

            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    LoadUnload(_moduleInfoRetriever);
                }
            }
        }

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
        /// Gets or sets the actual Assembly of this AssemblyData.
        /// </summary>
        [XmlIgnore]
        public Assembly Assembly { get; set; }

        /// <summary>
        /// Load this assembly.
        /// </summary>
        /// <param name="moduleInfoRetriever">ModuleInfoRetriever.</param>
        public async void Load(IModuleInfoRetriever moduleInfoRetriever)
        {
            Loader = await Task.Run(() => new AssemblyLoader(FilePath));
            Assembly = await Task.Run(() => Loader.LoadFromAssemblyPath(FilePath));

            string fileDirectory = FilePath.Substring(0, FilePath.LastIndexOf("."));

            moduleInfoRetriever.Initialize(fileDirectory, FilePath);

            // Store Types in ModuleData
            foreach (var module in Modules)
            {
                if (module.Type == null)
                {
                    module.Type = Assembly.GetType(module.FullName);
                }

                module.Constructors = moduleInfoRetriever.AddConstructorsToCollection(module.Type);
                module.Properties = moduleInfoRetriever.AddPropertiesToCollection(module.Type);
                module.Methods = moduleInfoRetriever.AddMethodsToCollection(module.Type);
            }
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
            Assembly = null;
        }

        /// <summary>
        /// Loads all assemblies with checked boxes and
        /// unloads the unchecked ones.
        /// </summary>
        /// <param name="moduleInfoRetriever">IModuleInfoRetriever.</param>
        public void LoadUnload(IModuleInfoRetriever moduleInfoRetriever)
        {
            if (IsEnabled || AreAnyModulesChecked())
            {
                Load(moduleInfoRetriever);
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
        /// Checks to see if all modules in the assembly are checked or enabled.
        /// </summary>
        /// <returns>True if all ModuleData's are checked, false otherwise.</returns>
        public bool AreAllModulesChecked()
        {
            foreach (var module in Modules)
            {
                if (!module.IsEnabled)
                {
                    return false;
                }
            }

            return true;
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