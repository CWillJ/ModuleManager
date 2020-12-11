namespace ModuleManager.Classes
{
    using System;
    using System.Globalization;
    using System.IO;
    using ModuleManager.Common.Interfaces;
    using Prism.Ioc;
    using Prism.Modularity;

    /// <summary>
    /// A <see cref="ModuleCatalog"/> that loads modules from a directory.
    /// </summary>
    public class DirectoryLoaderModuleCatalog : ModuleCatalog
    {
        private readonly IContainerProvider _containerProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryLoaderModuleCatalog"/> class.
        /// </summary>
        /// <param name="containerProvider">The <see cref="IContainerProvider"/>.</param>
        public DirectoryLoaderModuleCatalog(IContainerProvider containerProvider)
            : base()
        {
            _containerProvider = containerProvider;
        }

        /// <summary>
        /// Gets or sets the directory containing modules to search for.
        /// </summary>
        #nullable enable
        public string? ModulePath { get; set; }

        /// <summary>
        /// Drives the main logic of building the child domain and searching for the assemblies.
        /// </summary>
        protected override void InnerLoad()
        {
            if (string.IsNullOrEmpty(ModulePath))
            {
                throw new InvalidOperationException("Module Path Cannot Be Null Or Empty");
            }

            if (!Directory.Exists(ModulePath))
            {
                return;
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Directory for module {0} not found", ModulePath));
            }

            string[] dllFiles = Directory.GetFiles(ModulePath, @"*.dll");
            if (dllFiles.Length == 0)
            {
                throw new InvalidOperationException("No .dll Files Found In " + ModulePath);
            }

            // Load assemblies
            var assemblyCollectionService = _containerProvider.Resolve<IAssemblyCollectionService>();
            assemblyCollectionService.PopulateAssemblyCollection(dllFiles);

            // Create instances of assemblies
        }
    }
}
