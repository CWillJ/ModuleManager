namespace ModuleObjects.Interfaces
{
    /// <summary>
    /// Interface for module members such as constructors, properties and methods.
    /// </summary>
    public interface IModuleMember
    {
        /// <summary>
        /// Gets or sets the name of the member.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the member.
        /// </summary>
        string Description { get; set; }
    }
}
