namespace ModuleManager.Common.Interfaces
{
    using System.Collections.ObjectModel;
    using ModuleManager.Common.Classes;

    /// <summary>
    /// Module member object interface.
    /// </summary>
    public interface ITypeData
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
        /// Gets or sets an <see cref="ObservableCollection{TypeMemberData}"/> of the module members.
        /// </summary>
        public ObservableCollection<TypeMemberData> Members { get; set; }
    }
}