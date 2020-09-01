namespace ModuleObjects.Interfaces
{
    using System.Collections.ObjectModel;

    public interface IModule
    {
        /// <summary>
        /// Gets or sets the name of the module.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the module.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets all of the module members.
        /// </summary>
        ObservableCollection<IModuleMember> Members { set; get; }
    }
}
