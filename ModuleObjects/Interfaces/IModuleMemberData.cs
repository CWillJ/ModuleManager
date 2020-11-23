namespace ModuleManager.ModuleObjects.Interfaces
{
    /// <summary>
    /// Module member object interface.
    /// </summary>
    public interface IModuleMemberData
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
        /// Gets or sets the name of the Type.
        /// </summary>
        public string TypeName { get; set; }
    }
}