namespace ModuleManager.ModuleObjects.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ModuleManager.ModuleObjects.Interfaces;
    using Prism.Modularity;

    /// <summary>
    /// The <see cref="ModuleCatalog"/> holding the loaded modules.
    /// </summary>
    public class ModuleManagerCatalog : ModuleCatalog, IModuleManagerCatalog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleManagerCatalog"/> class.
        /// </summary>
        public ModuleManagerCatalog()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleManagerCatalog"/> class while providing an
        /// initial list of <see cref="ModuleInfo"/>s.
        /// </summary>
        /// <param name="modules">The initial list of modules.</param>
        public ModuleManagerCatalog(IEnumerable<ModuleInfo> modules)
            : base(modules)
        {
        }

        /// <summary>
        /// Adds a module to the module catalog based on type.
        /// </summary>
        /// <param name="type"><see cref="Type"/> representing the ModuleInfo to add.</param>
        public void AddModule(Type type)
        {
            AddModule(new ModuleInfo()
            {
                ModuleName = type.Name,
                ModuleType = type.AssemblyQualifiedName,
                InitializationMode = InitializationMode.OnDemand,
            });
        }

        /// <summary>
        /// Removes the module of the <see cref="ModuleInfo"/> passed in.
        /// </summary>
        /// <param name="module">Module to remove's <see cref="ModuleInfo"/>.</param>
        public void RemoveModule(ModuleInfo module)
        {
            if (Modules.Contains(module))
            {
                // Cannot set the Modules property
                ////Modules = Modules.Where(mod => mod != module);
            }
        }

        /// <summary>
        /// Removes the module of the <see cref="Type"/> passed in.
        /// </summary>
        /// <param name="type">Module to remove's <see cref="Type"/>.</param>
        public void RemoveModule(Type type)
        {
            ModuleInfo module = new ModuleInfo
            {
                ModuleName = type.Name,
                ModuleType = type.AssemblyQualifiedName,
            };

            if (Modules.Contains(module))
            {
                ////Modules = Modules.Where(mod => mod != module);
            }
        }

        private bool ValidateModule()
        {
            return true;
        }
    }
}