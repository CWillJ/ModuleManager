namespace ModuleManager.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
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
            assemblyCollectionService.PopulateAssemblyCollection(ModulePath, dllFiles);

            // Create instances of assemblies
            foreach (var assembly in assemblyCollectionService.Assemblies)
            {
                if (assembly.ModuleType != null)
                {
                    Items.Add(CreateModuleInfo(assembly.ModuleType));
                }
            }
        }

        private static IModuleInfo CreateModuleInfo(Type type)
        {
            string? moduleName = type.Name;
            List<string> dependsOn = new List<string>();
            bool onDemand = false;
            var moduleAttribute =
                CustomAttributeData.GetCustomAttributes(type).FirstOrDefault(
                    cad =>
                    {
                        var declaringType = cad.Constructor.DeclaringType ?? typeof(object);
                        return declaringType.FullName == typeof(ModuleAttribute).FullName;
                    });

            if (moduleAttribute != null)
            {
                foreach (CustomAttributeNamedArgument argument in moduleAttribute.NamedArguments)
                {
                    string argumentName = argument.MemberInfo.Name;
                    object argumentValue = argument.TypedValue.Value ?? new object();
                    switch (argumentName)
                    {
                        case "ModuleName":
                            moduleName = (string)argumentValue;
                            break;

                        case "OnDemand":
                            onDemand = (bool)argumentValue;
                            break;

                        case "StartupLoaded":
                            onDemand = !((bool)argumentValue);
                            break;
                    }
                }
            }

            var moduleDependencyAttributes =
                CustomAttributeData.GetCustomAttributes(type).Where(
                    cad =>
                    {
                        var declaringType = cad.Constructor.DeclaringType ?? typeof(object);
                        return declaringType.FullName == typeof(ModuleDependencyAttribute).FullName;
                    });

            foreach (CustomAttributeData cad in moduleDependencyAttributes)
            {
                dependsOn.Add((string)(cad.ConstructorArguments[0].Value ?? string.Empty));
            }

            IModuleInfo moduleInfo = new ModuleInfo(moduleName, type.AssemblyQualifiedName)
            {
                InitializationMode = onDemand ? InitializationMode.OnDemand : InitializationMode.WhenAvailable,
                Ref = new Uri(type.Assembly.Location, UriKind.RelativeOrAbsolute).AbsoluteUri,
            };

            moduleInfo.DependsOn.AddRange(dependsOn);
            return moduleInfo;
        }
    }
}