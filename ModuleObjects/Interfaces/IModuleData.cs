namespace ModuleManager.ModuleObjects.Interfaces
{
    using System;
    using System.Collections.ObjectModel;
    using ModuleManager.ModuleObjects.Classes;

    /// <summary>
    /// Module member object interface.
    /// </summary>
    public interface IModuleData
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the module.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets an <see cref="ObservableCollection{ModuleMemberData}"/> of the module members.
        /// </summary>
        public ObservableCollection<ModuleMemberData> Members { get; set; }
    }
}