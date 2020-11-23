namespace ModuleManager.ModuleLoader.Services
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using ModuleManager.ModuleLoader.Classes;
    using ModuleManager.ModuleLoader.Interfaces;
    using ModuleManager.ModuleObjects.Classes;
    using ModuleManager.ModuleObjects.Interfaces;

    /// <inheritdoc cref="IAssemblyLoaderService"/>
    public class AssemblyLoaderService : IAssemblyLoaderService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyLoaderService"/> class.
        /// </summary>
        public AssemblyLoaderService()
        {
            DllDirectory = string.Empty;
            DllFilePath = string.Empty;
            CurrentAssemblyName = string.Empty;
            CurrentTypeName = string.Empty;
            PercentOfAssemblyLoaded = 0;
            DescriptionRetriever = new XmlDescriptionRetriever();
        }

        /// <inheritdoc cref="IAssemblyLoaderService"/>
        public string DllDirectory { get; set; }

        /// <summary>
        /// Gets or sets DllFilePath is the path of the .dll file.
        /// </summary>
        public string DllFilePath { get; set; }

        /// <inheritdoc cref="IAssemblyLoaderService"/>
        public string CurrentAssemblyName { get; set; }

        /// <inheritdoc cref="IAssemblyLoaderService"/>
        public string CurrentTypeName { get; set; }

        /// <inheritdoc cref="IAssemblyLoaderService"/>
        public double PercentOfAssemblyLoaded { get; set; }

        /// <summary>
        /// Gets or sets all xml descriptions.
        /// </summary>
        public XmlDescriptionRetriever DescriptionRetriever { get; set; }

        /// <inheritdoc cref="IAssemblyLoaderService"/>
        public void Initialize(string moduleDirectory, string moduleFilePath)
        {
            DllDirectory = moduleDirectory;
            DllFilePath = moduleFilePath;
            DescriptionRetriever = new XmlDescriptionRetriever(moduleFilePath);
        }

        /// <inheritdoc cref="IAssemblyLoaderService"/>
        public ObservableCollection<AssemblyData> GetAssemblies(string[] dllFiles)
        {
            if (string.IsNullOrEmpty(DllDirectory))
            {
                return null;
            }

            ObservableCollection<AssemblyData> assemblies = new ObservableCollection<AssemblyData>();
            AssemblyData assembly;

            foreach (var dllFile in dllFiles)
            {
                Initialize(dllFile.Substring(0, dllFile.LastIndexOf(".")), dllFile);

                assembly = new AssemblyData
                {
                    FilePath = dllFile,
                };

                Load(ref assembly);
                Unload(ref assembly);

                assemblies.Add(assembly);
            }

            return assemblies;
        }

        /// <inheritdoc cref="IAssemblyLoaderService"/>
        public void LoadUnload(ref AssemblyData assembly)
        {
            if (assembly.IsEnabled)
            {
                Load(ref assembly);
            }
            else
            {
                Unload(ref assembly);
            }
        }

        /// <inheritdoc cref="IAssemblyLoaderService"/>
        public void LoadUnload(ref ObservableCollection<AssemblyData> assemblies)
        {
            AssemblyData assembly;

            for (int i = 0; i < assemblies.Count; i++)
            {
                assembly = assemblies[i];

                if (assembly.IsEnabled)
                {
                    Load(ref assembly);
                }
                else
                {
                    Unload(ref assembly);
                }

                assemblies[i] = assembly;
            }
        }

        /// <inheritdoc cref="IAssemblyLoaderService"/>
        public void LoadAll(ref ObservableCollection<AssemblyData> assemblies)
        {
            AssemblyData assembly;

            for (int i = 0; i < assemblies.Count; i++)
            {
                assembly = assemblies[i];
                Load(ref assembly);
                assemblies[i] = assembly;
            }
        }

        /// <inheritdoc cref="IAssemblyLoaderService"/>
        public void Load(ref AssemblyData assembly)
        {
            if (!string.IsNullOrEmpty(assembly.FilePath))
            {
                Initialize(assembly.FilePath.Substring(0, assembly.FilePath.LastIndexOf(".")), assembly.FilePath);
            }

            assembly.Loader = new AssemblyLoader(DllFilePath);
            assembly.Assembly = assembly.Loader.LoadFromAssemblyPath(DllFilePath);

            assembly.Name = assembly.Assembly.GetName().Name;
            assembly.FilePath = DllFilePath;

            Type[] types = null;

            try
            {
                types = assembly.Assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                types = ex.Types.Where(t => t != null).ToArray();
            }

            assembly.Modules.Clear();

            int typeNumber = 0;
            foreach (var type in types)
            {
                typeNumber++;

                if (type != null)
                {
                    CurrentAssemblyName = assembly.Name;
                    CurrentTypeName = type.Name;
                    PercentOfAssemblyLoaded = ((double)typeNumber / (double)types.Length) * 100;

                    Debug.WriteLine("Adding Module: " + CurrentTypeName + " From " + CurrentAssemblyName);
                    ModuleData tempModule = GetSingleModule(type);

                    if (tempModule != null)
                    {
                        assembly.Modules.Add(tempModule);
                    }
                }
            }
        }

        /// <inheritdoc cref="IAssemblyLoaderService"/>
        public void Unload(ref AssemblyData assembly)
        {
            if (assembly.Loader == null)
            {
                return;
            }

            assembly.Loader.Unload();
            assembly.Loader = null;
            assembly.Assembly = null;
        }

        /// <summary>
        /// Builds a singls module from the given Type.
        /// </summary>
        /// <param name="type">Type from an assembly.</param>
        /// <returns>A Module type.</returns>
        private ModuleData GetSingleModule(Type type)
        {
            if (!type.IsPublic || type.IsInterface)
            {
                return null;
            }

            return new ModuleData(
                type,
                type.Name,
                DescriptionRetriever.GetModuleDescription(type),
                AddConstructorsToCollection(type),
                AddPropertiesToCollection(type),
                AddMethodsToCollection(type));
        }

        /// <summary>
        /// AddConstructorsToCollection get all constructors from a Type.
        /// </summary>
        /// <param name="type">The Type where the members are coming from.</param>
        /// <returns>An ObservableCollection of ModuleConstructor objects.</returns>
        private ObservableCollection<ModuleConstructor> AddConstructorsToCollection(Type type)
        {
            ObservableCollection<ModuleConstructor> constructors = new ObservableCollection<ModuleConstructor>();
            ConstructorInfo[] conInfo = type.GetConstructors();
            ObservableCollection<MemberParameter> parameters;
            int constructorIndex;
            string name, description;

            foreach (var constructor in conInfo)
            {
                constructorIndex = Array.IndexOf(conInfo, constructor);

                name = type.Name;
                description = DescriptionRetriever.GetConstructorDescription(constructor, constructorIndex);
                parameters = DescriptionRetriever.GetParametersFromList(constructor, constructorIndex);

                constructors.Add(new ModuleConstructor(
                    constructor,
                    name,
                    description,
                    parameters));
            }

            return constructors;
        }

        /// <summary>
        /// AddPropertiesToCollection gets all properties from the passed in Type.
        /// </summary>
        /// <param name="type">The Type where the members are coming from.</param>
        /// <returns>An ObservableCollection of ModulePropery objects.</returns>
        private ObservableCollection<ModuleProperty> AddPropertiesToCollection(Type type)
        {
            ObservableCollection<ModuleProperty> properties = new ObservableCollection<ModuleProperty>();
            string name, description, dataType, propertyString;
            bool canRead, canWrite;

            foreach (var property in type.GetProperties())
            {
                name = property.Name;
                description = DescriptionRetriever.GetPropertyDescription(property);
                canRead = property.CanRead;
                canWrite = property.CanWrite;

                try
                {
                    dataType = property.PropertyType.FullName;
                }
                catch (FileNotFoundException)
                {
                    propertyString = property.ToString();
                    dataType = propertyString.Substring(0, propertyString.IndexOf(@" "));
                }
                catch (TypeLoadException)
                {
                    propertyString = property.ToString();
                    dataType = propertyString.Substring(0, propertyString.IndexOf(@" "));
                }

                properties.Add(new ModuleProperty(
                    property,
                    name,
                    description,
                    dataType,
                    canRead,
                    canWrite));
            }

            return properties;
        }

        /// <summary>
        /// AddMethodsToCollection gets all methods from the passed in Type.
        /// </summary>
        /// <param name="type">The Type where the methods are coming from.</param>
        /// <returns>An ObservableCollection of ModuleMethod objects.</returns>
        private ObservableCollection<ModuleMethod> AddMethodsToCollection(Type type)
        {
            ObservableCollection<ModuleMethod> methods = new ObservableCollection<ModuleMethod>();
            ObservableCollection<MemberParameter> parameters;
            string lastMethodName = string.Empty;
            string description, returnType, returnDescription, methodString;
            int methodIndex = 0;

            foreach (var method in type.GetMethods(BindingFlags.Public
                                      | BindingFlags.Instance
                                      | BindingFlags.InvokeMethod
                                      | BindingFlags.DeclaredOnly))
            {
                // skip if method is null (if the method is a constructor or property)
                // or if the method starts with the get_, set_, add_ or remove_ special names
                if (method == null || method.IsSpecialName)
                {
                    continue;
                }

                // The index is used to get the correct description from methods with the same name.
                string name = method.Name;
                if (name == lastMethodName)
                {
                    methodIndex++;
                }
                else
                {
                    methodIndex = 0;
                }

                lastMethodName = name;
                description = DescriptionRetriever.GetMethodDescription(method, methodIndex);
                parameters = DescriptionRetriever.GetParametersFromList(method, methodIndex);

                try
                {
                    returnType = method.ReturnType.FullName;
                }
                catch (FileNotFoundException)
                {
                    Debug.WriteLine("Cannot Load Return Type For " + method.Name);

                    methodString = method.ToString();
                    returnType = methodString.Substring(0, methodString.IndexOf(@" "));
                }
                catch (FileLoadException)
                {
                    Debug.WriteLine("Cannot Load Return Type For " + method.Name);

                    methodString = method.ToString();
                    returnType = methodString.Substring(0, methodString.IndexOf(@" "));
                }
                catch (TypeLoadException)
                {
                    Debug.WriteLine("Cannot Load Return Type For " + method.Name);

                    methodString = method.ToString();
                    returnType = methodString.Substring(0, methodString.IndexOf(@" "));
                }

                returnDescription = DescriptionRetriever.GetMemberReturnDescription(method);

                methods.Add(new ModuleMethod(
                    method,
                    name,
                    description,
                    parameters,
                    returnType,
                    returnDescription));
            }

            return methods;
        }
    }
}