﻿namespace ModuleManager.Common.Interfaces
{
    using System;
    using System.Collections.ObjectModel;
    using System.Reflection;
    using System.Xml.Serialization;
    using ModuleManager.Common.Classes;

    /// <summary>
    /// Assembly object interface.
    /// </summary>
    public interface IAssemblyData
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
    }
}