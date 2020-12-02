namespace ModuleManager.ModuleObjects.Classes
{
    using System;
    using System.Collections.ObjectModel;
    using System.Reflection;
    using System.Xml.Serialization;
    using ModuleManager.ModuleObjects.Interfaces;
    using Prism.Mvvm;

    /// <inheritdoc cref="IAssemblyData"/>
    public class AssemblyData : BindableBase, IAssemblyData
    {
        private bool _isEnabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyData"/> class.
        /// </summary>
        public AssemblyData()
            : this(string.Empty, string.Empty, new ObservableCollection<ModuleData>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyData"/> class.
        /// </summary>
        /// <param name="name">Name of the assembly.</param>
        /// <param name="filePath">File path to the assembly.</param>
        /// <param name="modules">An <see cref="ObservableCollection{ModuleData}"/> of modules.</param>
        public AssemblyData(string name, string filePath, ObservableCollection<ModuleData> modules)
        {
            Name = name;
            _isEnabled = false;
            FilePath = filePath;
            ModuleType = null;
            Modules = modules;
            Loader = null;
            Assembly = null;
            ViewTypes = new ObservableCollection<Type>();
        }

        /// <inheritdoc cref="IAssemblyData"/>
        public string Name { get; set; }

        /// <inheritdoc cref="IAssemblyData"/>
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { SetProperty(ref _isEnabled, value); }
        }

        /// <inheritdoc cref="IAssemblyData"/>
        public string FilePath { get; set; }

        /// <inheritdoc cref="IAssemblyData"/>
        public ObservableCollection<ModuleData> Modules { get; set; }

        /// <inheritdoc cref="IAssemblyData"/>
        [XmlIgnore]
        public Type ModuleType { get; set; }

        /// <inheritdoc cref="IAssemblyData"/>
        [XmlIgnore]
        public AssemblyLoader Loader { get; set; }

        /// <inheritdoc cref="IAssemblyData"/>
        [XmlIgnore]
        public Assembly Assembly { get; set; }

        /// <inheritdoc cref="IAssemblyData"/>
        [XmlIgnore]
        public ObservableCollection<Type> ViewTypes { get; set; }

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