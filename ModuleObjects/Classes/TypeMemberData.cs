namespace ModuleManager.Common.Classes
{
    using ModuleManager.Common.Interfaces;

    /// <summary>
    /// The base class for module members.
    /// </summary>
    public class TypeMemberData : ITypeMemberData
    {
        /// <inheritdoc cref="ITypeMemberData"/>
        public string Name { get; set; }

        /// <inheritdoc cref="ITypeMemberData"/>
        public string Description { get; set; }

        /// <inheritdoc cref="ITypeMemberData"/>
        public string TypeName { get; set; }
    }
}