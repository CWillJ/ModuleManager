namespace ModuleManager.ModuleObjects.Interfaces
{
    using System.Collections.ObjectModel;
    using ModuleManager.ModuleObjects.Classes;

    /// <summary>
    /// Module member object interface.
    /// </summary>
    public interface IModuleData : ITreeViewData
    {
        /// <summary>
        /// Gets or sets the description of the module.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets an <see cref="ObservableCollection{ModuleMemberData}"/> of the module members.
        /// </summary>
        public ObservableCollection<ModuleMemberData> Members { get; set; }
    }
}