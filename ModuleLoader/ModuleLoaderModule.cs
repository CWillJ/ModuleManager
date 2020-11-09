namespace ModuleManager.ModuleLoader
{
    using Prism.Ioc;
    using Prism.Modularity;

    /// <summary>
    /// ModuleLoaderModule handles how modules are loaded.
    /// </summary>
    public class ModuleLoaderModule : IModule
    {
        /// <summary>
        /// Perform required initialization methods for this Module.
        /// </summary>
        /// <param name="containerProvider">A <see cref="IContainerProvider"/> used for progam-wide type resolving.</param>
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        /// <summary>
        /// Register types with the container that will be used by the application.
        /// </summary>
        /// <param name="containerRegistry"><see cref="IContainerRegistry"/> used for program-wide type registration.</param>
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}
