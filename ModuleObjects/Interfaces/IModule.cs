namespace ModuleObjects.Interfaces
{
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;
    using ModuleObjects.Classes;

    /// <summary>
    /// Module object interface.
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// Gets or sets the name of the module.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the module.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets all of the module members.
        /// </summary>
        public ObservableCollection<ModuleMember> Members { get; set; }
    }
}
