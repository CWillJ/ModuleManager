namespace ModuleManager.Common.Classes
{
    /// <summary>
    /// The base class for module members.
    /// </summary>
    public class TypeMemberData
    {
        /// <summary>
        /// Gets or sets the <see cref="string"/> name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="string"/> description of the module.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="string"/> name of the Type.
        /// </summary>
        public string TypeName { get; set; }
    }
}