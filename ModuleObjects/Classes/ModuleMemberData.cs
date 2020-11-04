namespace ModuleManager.ModuleObjects.Classes
{
    using ModuleManager.ModuleObjects.Interfaces;

    /// <summary>
    /// The base class for module members.
    /// </summary>
    public class ModuleMemberData : IModuleMemberData
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
        /// Gets or sets the name of the Type.
        /// </summary>
        public string TypeName { get; set; }
    }
}