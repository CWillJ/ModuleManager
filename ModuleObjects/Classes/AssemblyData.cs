namespace ModuleManager.ModuleObjects.Classes
{
    using System;
    using System.Collections.ObjectModel;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using ModuleManager.ModuleObjects.Interfaces;
    using ModuleManager.ModuleObjects.Loaders;

    /// <summary>
    /// AssemblyData will load and unload an assembly and stores data about an assembly.
    /// </summary>
    public class AssemblyData : IAssemblyData
    {
        private IModuleInfoRetriever _moduleInfoRetriever;
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
        /// </summary>
        /// <param name="moduleInfoRetriever">The IModuleInfoRetriever creating this AssemblyData.</param>
        /// <param name="name">Name of the assembly.</param>
        /// <param name="filePath">File path to the assembly.</param>
        /// <param name="modules">Collection of modules contained in the assembly.</param>
        public AssemblyData(IModuleInfoRetriever moduleInfoRetriever, string name, string filePath, ObservableCollection<ModuleData> modules)
        {
            _moduleInfoRetriever = moduleInfoRetriever ?? throw new ArgumentNullException("ModuleInfoRetriever");

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
        /// Will load or unload this assembly if the IModuleInfoRetriever is not null.
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

                    if (_moduleInfoRetriever != null)
                    {
                        LoadUnload(_moduleInfoRetriever);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the file path to assembly.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the collection of modules contained in the assembly.
        /// </summary>
        public ObservableCollection<ModuleData> Modules { get; set; }

        /// <summary>
        /// Gets the AssemblyLoader to load/unload this assembly.
        /// </summary>
        [XmlIgnore]
        public AssemblyLoader Loader { get; private set; }

        /// <summary>
        /// Gets or sets the actual Assembly of this AssemblyData.
        /// </summary>
        [XmlIgnore]
        public Assembly Assembly { get; set; }

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

        /// <summary>
        /// Loads all enabled assemblies and unloads the disabled ones.
        /// </summary>
        /// <param name="moduleInfoRetriever">IModuleInfoRetriever.</param>
        public void LoadUnload(IModuleInfoRetriever moduleInfoRetriever)
        {
            if (_moduleInfoRetriever == null)
            {
                _moduleInfoRetriever = moduleInfoRetriever ?? throw new ArgumentNullException("ModuleInfoRetriever");
            }

            if (IsEnabled)
            {
                Load(moduleInfoRetriever);
            }
            else
            {
                Unload();
            }
        }

        /// <summary>
        /// Load this assembly.
        /// </summary>
        /// <param name="moduleInfoRetriever">IModuleInfoRetriever used to load this assembly.</param>
        private async void Load(IModuleInfoRetriever moduleInfoRetriever)
        {
            Loader = await Task.Run(() => new AssemblyLoader(FilePath));
            Assembly = await Task.Run(() => Loader.LoadFromAssemblyPath(FilePath));

            moduleInfoRetriever.Initialize(FilePath.Substring(0, FilePath.LastIndexOf(".")), FilePath);

            // Store Types in ModuleData
            foreach (var module in Modules)
            {
                if (module.Type == null)
                {
                    module.Type = Assembly.GetType(module.FullName);
                }

                // Restore each constructor, property and method so that they each have their
                // ConstructorInfo, PropertyInfo, and MethodInfo.
                module.Constructors = moduleInfoRetriever.AddConstructorsToCollection(module.Type);
                module.Properties = moduleInfoRetriever.AddPropertiesToCollection(module.Type);
                module.Methods = moduleInfoRetriever.AddMethodsToCollection(module.Type);
            }
        }

        /// <summary>
        /// Unload this assembly.
        /// </summary>
        private void Unload()
        {
            if (Loader == null)
            {
                return;
            }

            Loader.Unload();
            Loader = null;
            Assembly = null;
        }
    }
}