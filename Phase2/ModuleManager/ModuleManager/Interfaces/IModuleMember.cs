namespace ModuleManager.Interfaces
{
    /// <summary>
    /// An interface for module members (constructors, properties, methods).
    /// </summary>
    public interface IModuleMember
    {
        /// <summary>
        /// Gets or sets the name of the member.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Description of the member.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Override the ToString method.
        /// </summary>
        /// <returns>String object.</returns>
        string ToString();
    }
}
