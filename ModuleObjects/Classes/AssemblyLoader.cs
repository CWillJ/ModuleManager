namespace ModuleManager.Common.Classes
{
    using System.IO;
    using System.Reflection;
    using System.Runtime.Loader;

    /// <summary>
    /// The class that loads and unloads assemblies.
    /// </summary>
    public class AssemblyLoader : AssemblyLoadContext
    {
        private readonly AssemblyDependencyResolver _resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyLoader"/> class.
        /// </summary>
        public AssemblyLoader()
            : base(isCollectible: true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyLoader"/> class.
        /// </summary>
        /// <param name="mainAssemblyToLoadPath">The path to the assembly.</param>
        public AssemblyLoader(string mainAssemblyToLoadPath)
            : base(isCollectible: true)
        {
            _resolver = new AssemblyDependencyResolver(mainAssemblyToLoadPath);
        }

        /// <summary>
        /// Loads all referenced assemblies from the <see cref="Assembly"/>.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/> to load all referenced assemblies from.</param>
        public void LoadAllReferencedAssemblies(Assembly assembly)
        {
            if (assembly == null)
            {
                return;
            }

            foreach (var referencedAssembly in assembly.GetReferencedAssemblies())
            {
                try
                {
                    LoadFromAssemblyName(referencedAssembly);
                }
                catch (FileNotFoundException)
                {
                    // Don't load it
                }
            }
        }

        /// <summary>
        /// Overrides the Load method.
        /// </summary>
        /// <param name="name">The <see cref="AssemblyName"/> of the <see cref="Assembly"/> to be loaded.</param>
        /// <returns>A loaded <see cref="Assembly"/>.</returns>
        protected override Assembly Load(AssemblyName name)
        {
            string assemblyPath = _resolver.ResolveAssemblyToPath(name);
            if (assemblyPath != null)
            {
                return LoadFromAssemblyPath(assemblyPath);
            }

            return null;
        }
    }
}