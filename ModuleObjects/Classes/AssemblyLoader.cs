namespace ModuleManager.ModuleObjects.Classes
{
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
        /// Overrides the Load method.
        /// </summary>
        /// <param name="name">The assembly name.</param>
        /// <returns>A loaded assembly.</returns>
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