﻿namespace ModuleManager.ModuleObjects.Classes
{
    using System.Collections.ObjectModel;
    using System.Reflection;
    using System.Xml.Serialization;
    using ModuleManager.ModuleObjects.Interfaces;
    using Prism.Mvvm;

    /// <summary>
    /// AssemblyData will load and unload an assembly and stores data about an assembly.
    /// </summary>
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
        /// <param name="modules">Collection of modules contained in the assembly.</param>
        public AssemblyData(string name, string filePath, ObservableCollection<ModuleData> modules)
        {
            Name = name;
            _isEnabled = false;
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
            get { return _isEnabled; }
            set { SetProperty(ref _isEnabled, value); }
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
        /// Gets or sets the AssemblyLoader to load/unload this assembly.
        /// </summary>
        [XmlIgnore]
        public AssemblyLoader Loader { get; set; }

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
    }
}