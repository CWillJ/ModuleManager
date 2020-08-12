namespace ModuleManager.DataObjects
{
    /// <summary>
    /// The base class for ModuleConstructor, ModuleProperty, and ModuleMethod.
    /// </summary>
    public class ModuleMember
    {
        /// <summary>
        /// Gets or sets the name of the member.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the member.
        /// </summary>
        public string Description { get; set; }
    }
}