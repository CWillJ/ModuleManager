namespace ModuleManager.Common.Classes
{
    using System;
    using System.Collections.ObjectModel;
    using System.Reflection;
    using System.Xml.Serialization;
    using Prism.Mvvm;

    /// <summary>
    /// Assembly object.
    /// </summary>
    public class AssemblyData : BindableBase
    {
        private bool _isEnabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyData"/> class.
        /// </summary>
        public AssemblyData()
            : this(string.Empty, string.Empty, new ObservableCollection<TypeData>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyData"/> class.
        /// </summary>
        /// <param name="name">Name of the assembly.</param>
        /// <param name="filePath">File path to the assembly.</param>
        /// <param name="modules">An <see cref="ObservableCollection{TypeData}"/> of modules.</param>
        public AssemblyData(string name, string filePath, ObservableCollection<TypeData> modules)
        {
            Name = name;
            _isEnabled = false;
            FilePath = filePath;
            ModuleType = null;
            Types = modules;
            Loader = null;
            Assembly = null;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the assembly is enabled or disabled.
        /// </summary>
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { SetProperty(ref _isEnabled, value); }
        }

        /// <summary>
        /// Gets or sets the file path to assembly.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets an <see cref="ObservableCollection{TypeData}"/> of modules contained in the assembly.
        /// </summary>
        public ObservableCollection<TypeData> Types { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Type"/> of the module in this assembly.
        /// </summary>
        [XmlIgnore]
        public Type ModuleType { get; set; }

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
        /// Overrides the ToString method and formats the string output.
        /// </summary>
        /// <returns>A desired format for the assembly.</returns>
        public override string ToString()
        {
            string s = Name + @" located: " + FilePath + "\n\n";

            foreach (var module in Types)
            {
                s += module.ToString() + "\n";
            }

            return s;
        }
    }
}