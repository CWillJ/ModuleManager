namespace ModuleManager.Common.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using ModuleManager.Common.Interfaces;
    using Prism.Modularity;

    /// <summary>
    /// A <see cref="ModuleCatalog"/> that loads modules from a directory.
    /// </summary>
    public class DirectoryLoaderModuleCatalog : ModuleCatalog
    {
        private readonly IAssemblyDataLoaderService _assemblyDataLoaderService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryLoaderModuleCatalog"/> class.
        /// </summary>
        /// <param name="assemblyDataLoaderService">The <see cref="IAssemblyDataLoaderService"/>.</param>
        public DirectoryLoaderModuleCatalog(IAssemblyDataLoaderService assemblyDataLoaderService)
            : base()
        {
            _assemblyDataLoaderService = assemblyDataLoaderService;
        }

        /// <summary>
        /// Gets or sets the directory containing modules to search for.
        /// </summary>
        #nullable enable
        public string? ModulePath { get; set; }

        /// <summary>
        /// Creates a <see cref="IModuleInfo"/> from a <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to get a <see cref="IModuleInfo"/> from.</param>
        /// <returns>A <see cref="IModuleInfo"/>.</returns>
        public static IModuleInfo CreateModuleInfo(Type type)
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

        /// <summary>
        /// Adds a <see cref="IModuleInfo"/> to the <see cref="IModuleCatalog"/>.
        /// </summary>
        /// <param name="moduleInfo">The <see cref="IModuleInfo"/> to add.</param>
        /// <returns>The <see cref="IModuleCatalog"/> for easily adding multiple modules.</returns>
        public override IModuleCatalog AddModule(IModuleInfo moduleInfo)
        {
            Items.Add(moduleInfo);
            return this;
        }

        /// <summary>
        /// Adds a <see cref="IModuleInfo"/> to the <see cref="IModuleCatalog"/>.
        /// </summary>
        /// <param name="dllFilePath">The <see cref="string"/> to the dll file to get the <see cref="IModuleInfo"/> from.</param>
        /// <returns>The <see cref="IModuleCatalog"/> for easily adding multiple modules.</returns>
        public IModuleCatalog AddModule(string dllFilePath)
        {
            AssemblyData assemblyData = _assemblyDataLoaderService.GetAssembly(dllFilePath);

            if (assemblyData.ModuleType != null)
            {
                Items.Add(CreateModuleInfo(assemblyData.ModuleType));
            }

            Initialize();

            return this;
        }

        /// <summary>
        /// Removes a <see cref="IModuleInfo"/> from the <see cref="IModuleCatalog"/>.
        /// </summary>
        /// <param name="moduleInfo">The <see cref="IModuleInfo"/> to remove from the list.</param>
        /// <returns>The <see cref="IModuleCatalog"/> for easily removing multiple modules.</returns>
        public IModuleCatalog RemoveModule(IModuleInfo moduleInfo)
        {
            foreach (var moduleCatalogItem in Items)
            {
                IModuleInfo something = (IModuleInfo)moduleCatalogItem;
                if (something.ModuleName == moduleInfo.ModuleName)
                {
                    Items.Remove(moduleCatalogItem);
                    break;
                }
            }

            return this;
        }

        /// <summary>
        /// Get a <see cref="IModuleInfo"/> from a <see cref="string"/> file path.
        /// </summary>
        /// <param name="dllFilePath">The <see cref="string"/> file path to the dll.</param>
        /// <returns>A <see cref="IModuleInfo"/>.</returns>
        public IModuleInfo GetModuleInfoFromFile(string dllFilePath)
        {
            AssemblyData assemblyData = _assemblyDataLoaderService.GetAssembly(dllFilePath);
            return CreateModuleInfo(assemblyData.ModuleType);
        }

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

            foreach (string dllFile in dllFiles)
            {
                AddModule(dllFile);
            }
        }
    }
}