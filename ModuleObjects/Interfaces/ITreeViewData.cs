namespace ModuleManager.ModuleObjects.Interfaces
{
    /// <summary>
    /// The base interface for IAssemblyData and IModuleData.
    /// </summary>
    public interface ITreeViewData
    {
        /// <summary>
        /// Gets or sets the name of the assembly.
        /// </summary>
        public string Name { get; set; }
    }
}