namespace ModuleManager.ModuleObjects.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;
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

        /// <inheritdoc cref="IModuleManagerCatalog"/>
        public void AddModule(Type type)
        {
            AddModule(CreateModuleInfo(type));
        }

        /// <summary>
        /// Does the actual work of loading the catalog. The base implementation does nothing.
        /// </summary>
        protected override void InnerLoad()
        {
            return;
        }

        private static IModuleInfo CreateModuleInfo(Type type)
        {
            #nullable enable
            string? moduleName = type.Name;
            List<string> dependsOn = new List<string>();
            bool onDemand = false;

            CustomAttributeData? moduleAttribute;

            try
            {
                moduleAttribute =
                    CustomAttributeData.GetCustomAttributes(type).FirstOrDefault(
                        cad =>
                        {
                            var declaringType = cad.Constructor.DeclaringType ?? typeof(object);
                            return declaringType.FullName == typeof(ModuleAttribute).FullName;
                        });
            }
            catch (System.IO.FileNotFoundException)
            {
                moduleAttribute = null;
            }

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

            try
            {
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
            }
            catch (System.IO.FileNotFoundException)
            {
                // Do nothing.
            }

            IModuleInfo moduleInfo = new ModuleInfo(moduleName, type.AssemblyQualifiedName)
            {
                InitializationMode = onDemand ? InitializationMode.OnDemand : InitializationMode.WhenAvailable,
                Ref = type.Assembly.Location,
            };
            moduleInfo.DependsOn.AddRange(dependsOn);
            return moduleInfo;
        }
    }
}