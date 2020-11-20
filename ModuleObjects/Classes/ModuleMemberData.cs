namespace ModuleManager.ModuleObjects.Classes
{
    using ModuleManager.ModuleObjects.Interfaces;

    /// <summary>
    /// The base class for module members.
    /// </summary>
    public class ModuleMemberData : IModuleMemberData
    {
        /// <inheritdoc cref="IModuleMemberData"/>
        public string Name { get; set; }

        /// <inheritdoc cref="IModuleMemberData"/>
        public string Description { get; set; }

        /// <inheritdoc cref="IModuleMemberData"/>
        public string TypeName { get; set; }
    }
}