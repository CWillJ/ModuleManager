namespace ModuleManager.ModuleObjects.Classes
{
    using System.Collections.ObjectModel;

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