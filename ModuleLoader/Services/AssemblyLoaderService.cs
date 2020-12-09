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

            LoadedAssemblies = new ObservableCollection<AssemblyName>();
        }

        /// <inheritdoc cref="IAssemblyLoaderService"/>
        public string DllDirectory { get; set; }

        /// <inheritdoc cref="IAssemblyLoaderService"/>
        public string DllFilePath { get; set; }

        /// <inheritdoc cref="IAssemblyLoaderService"/>
        public string CurrentAssemblyName { get; set; }

        /// <inheritdoc cref="IAssemblyLoaderService"/>
        public string CurrentTypeName { get; set; }

        /// <inheritdoc cref="IAssemblyLoaderService"/>
        public double PercentOfAssemblyLoaded { get; set; }

        private ObservableCollection<AssemblyName> LoadedAssemblies { get; set; }

        /// <summary>
        /// Gets or sets all xml descriptions.
        /// </summary>
        private XmlDescriptionRetriever DescriptionRetriever { get; set; }

        /// <inheritdoc cref="IAssemblyLoaderService"/>
        public void Initialize(string modulesDirectory, string assemblyFilePath)
        {
            DllDirectory = modulesDirectory;
            DllFilePath = assemblyFilePath;
            DescriptionRetriever = new XmlDescriptionRetriever(assemblyFilePath);
        }

        /// <inheritdoc cref="IAssemblyLoaderService"/>
        public ObservableCollection<AssemblyData> GetAssemblies(string[] dllFiles)
        {
            if (string.IsNullOrEmpty(DllDirectory))
            {
                return null;
            }

            ObservableCollection<AssemblyData> assemblies = new ObservableCollection<AssemblyData>();
            AssemblyData assemblyData;

            foreach (var dllFile in dllFiles)
            {
                Initialize(dllFile.Substring(0, dllFile.LastIndexOf(".")), dllFile);

                assemblyData = new AssemblyData
                {
                    FilePath = dllFile,
                };

                Load(ref assemblyData);
                Unload(ref assemblyData);

                // IF the ModuleType has not been set, it is not a NextGen module
                if (assemblyData.ModuleType != null)
                {
                    assemblies.Add(assemblyData);
                }
            }

            return assemblies;
        }

        /// <inheritdoc cref="IAssemblyLoaderService"/>
        public void LoadUnload(ref AssemblyData assemblyData)
        {
            if (assemblyData.IsEnabled)
            {
                Load(ref assemblyData);
            }
            else
            {
                Unload(ref assemblyData);
            }
        }

        /// <inheritdoc cref="IAssemblyLoaderService"/>
        public void LoadUnload(ref ObservableCollection<AssemblyData> assemblies)
        {
            AssemblyData assemblyData;

            for (int i = 0; i < assemblies.Count; i++)
            {
                assemblyData = assemblies[i];

                if (assemblyData.IsEnabled)
                {
                    Load(ref assemblyData);
                }
                else
                {
                    Unload(ref assemblyData);
                }

                assemblies[i] = assemblyData;
            }
        }

        /// <inheritdoc cref="IAssemblyLoaderService"/>
        public void LoadAll(ref ObservableCollection<AssemblyData> assemblies)
        {
            AssemblyData assemblyData;

            for (int i = 0; i < assemblies.Count; i++)
            {
                assemblyData = assemblies[i];
                Load(ref assemblyData);
                assemblies[i] = assemblyData;
            }
        }

        /// <inheritdoc cref="IAssemblyLoaderService"/>
        public void Load(ref AssemblyData assemblyData)
        {
            if (!string.IsNullOrEmpty(assemblyData.FilePath))
            {
                Initialize(assemblyData.FilePath.Substring(0, assemblyData.FilePath.LastIndexOf(".")), assemblyData.FilePath);
            }

            assemblyData.Loader = new AssemblyLoader(DllFilePath);
            assemblyData.Assembly = assemblyData.Loader.LoadFromAssemblyPath(DllFilePath);
            assemblyData.FilePath = DllFilePath;

            CopyDllToCurrentDirectory(assemblyData.FilePath);

            LoadReferencedAssembly(assemblyData.Assembly);

            Type[] types = null;
            string name = assemblyData.Assembly.GetName().Name;

            try
            {
                assemblyData.Name = name[(name.LastIndexOf(@".") + 1) ..];
            }
            catch (ArgumentOutOfRangeException)
            {
                assemblyData.Name = name;
            }

            types = GetAllTypesFromAssembly(assemblyData.Assembly);

            assemblyData.Types.Clear();

            int typeNumber = 0;
            foreach (var type in types)
            {
                typeNumber++;

                if (type != null)
                {
                    CurrentAssemblyName = assemblyData.Name;
                    CurrentTypeName = type.Name;
                    PercentOfAssemblyLoaded = ((double)typeNumber / (double)types.Length) * 100;

                    ////Debug.WriteLine(@"Adding Module: " + CurrentTypeName + @" From " + CurrentAssemblyName);
                    TypeData tempModule = GetTypeData(type);

                    if (tempModule != null)
                    {
                        Type[] typeInterfaces = type.GetInterfaces();
                        string[] typeFullNames = new string[typeInterfaces.Length];

                        for (int i = 0; i < typeInterfaces.Length; i++)
                        {
                            typeFullNames[i] = typeInterfaces[i].FullName;
                        }

                        if (typeFullNames.Contains(@"PVA.NextGen.Common.Interfaces.IExpansionModule") || typeFullNames.Contains(@"PVA.NextGen.Common.Interfaces.ICoreModule"))
                        {
                            assemblyData.ModuleType = type;
                        }

                        assemblyData.Types.Add(tempModule);
                    }
                }
            }
        }

        /// <inheritdoc cref="IAssemblyLoaderService"/>
        public void Unload(ref AssemblyData assemblyData)
        {
            if (assemblyData.Loader == null)
            {
                return;
            }

            assemblyData.Loader.Unload();
            assemblyData.Loader = null;
            assemblyData.Assembly = null;
        }

        /// <summary>
        /// Loads all referenced assemblies.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/> to get all referenced assemblies from.</param>
        private void LoadReferencedAssembly(Assembly assembly)
        {
            foreach (AssemblyName name in assembly.GetReferencedAssemblies())
            {
                if (!AppDomain.CurrentDomain.GetAssemblies().Any(a => a.FullName == name.FullName))
                {
                    try
                    {
                        LoadReferencedAssembly(Assembly.Load(name));
                    }
                    catch (FileNotFoundException)
                    {
                        FindInLibFolder(name.Name);
                    }
                }
            }
        }

        /// <summary>
        /// Tries to find the missing file from the Lib folder.
        /// </summary>
        /// <param name="fileName">The name of the dll file to load.</param>
        private void FindInLibFolder(string fileName)
        {
            string directoryPath = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName;

            string loadFilePath = Path.Combine(
                directoryPath,
                @"Lib",
                fileName + @".dll");

            AssemblyName assemblyName = Assembly.LoadFrom(CopyDllToCurrentDirectory(loadFilePath)).GetName();

            Assembly.Load(assemblyName);
        }

        /// <summary>
        /// This will get all the <see cref="Type"/>s from an <see cref="Assembly"/>.
        /// THIS WILL THROW ERRORS AND NOT GET ALL OF THE TYPES IF THE DLL FILES ARE DIFFERENT FROM THE LOADED ASSEMBLIES!!!.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/> to get the <see cref="Type"/>s from.</param>
        /// <returns>A <see cref="Type"/> array.</returns>
        private Type[] GetAllTypesFromAssembly(Assembly assembly)
        {
            Type[] types;

            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                types = ex.Types.Where(t => t != null).ToArray();
            }

            return types;
        }

        /// <summary>
        /// Copy the loaded assembly dll into the bin folder.
        /// </summary>
        /// <param name="assemblyDataFilePath">The <see cref="string"/> of the AssemblyData FilePath.</param>
        /// <returns>A <see cref="string"/> value representing where the dll file was moved.</returns>
        private string CopyDllToCurrentDirectory(string assemblyDataFilePath)
        {
            string sourceFile = assemblyDataFilePath;
            string targetFile = Path.Combine(
                Directory.GetCurrentDirectory(),
                assemblyDataFilePath[(assemblyDataFilePath.LastIndexOf(@"\") + 1) ..]);

            if (!File.Exists(targetFile))
            {
                File.Copy(sourceFile, targetFile, true);
            }

            return targetFile;
        }

        /// <summary>
        /// Builds a singls <see cref="TypeData"/> from the given <see cref="Type"/>.
        /// </summary>
        /// <param name="type"><see cref="Type"/> from an assembly.</param>
        /// <returns>A <see cref="TypeData"/>.</returns>
        private TypeData GetTypeData(Type type)
        {
            if (!type.IsPublic || type.IsInterface)
            {
                return null;
            }

            return new TypeData(
                type,
                type.Name,
                DescriptionRetriever.GetModuleDescription(type),
                AddConstructorsToCollection(type),
                AddPropertiesToCollection(type),
                AddMethodsToCollection(type));
        }

        /// <summary>
        /// Get all <see cref="TypeConstructor"/>s from a <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> where the members are coming from.</param>
        /// <returns>An <see cref="ObservableCollection{TypeConstructor}"/> objects.</returns>
        private ObservableCollection<TypeConstructor> AddConstructorsToCollection(Type type)
        {
            ObservableCollection<TypeConstructor> constructors = new ObservableCollection<TypeConstructor>();
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

                constructors.Add(new TypeConstructor(
                    constructor,
                    name,
                    description,
                    parameters));
            }

            return constructors;
        }

        /// <summary>
        /// Gets all <see cref="TypeProperty"/> from the passed in <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> where the members are coming from.</param>
        /// <returns>An <see cref="ObservableCollection{TypeProperty}"/> objects.</returns>
        private ObservableCollection<TypeProperty> AddPropertiesToCollection(Type type)
        {
            ObservableCollection<TypeProperty> properties = new ObservableCollection<TypeProperty>();
            string name, description, dataType;
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
                    Debug.WriteLine(@"Cannot Load Type For " + property.Name);
                    dataType = string.Empty;
                }
                catch (TypeLoadException)
                {
                    Debug.WriteLine(@"Cannot Load Type For " + property.Name);
                    dataType = string.Empty;
                }

                properties.Add(new TypeProperty(
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
        /// Gets all <see cref="TypeMethod"/> from the passed in <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> where the methods are coming from.</param>
        /// <returns>An <see cref="ObservableCollection{TypeMethod}"/> objects.</returns>
        private ObservableCollection<TypeMethod> AddMethodsToCollection(Type type)
        {
            ObservableCollection<TypeMethod> methods = new ObservableCollection<TypeMethod>();
            ObservableCollection<MemberParameter> parameters;
            string lastMethodName = string.Empty;
            string description, returnType, returnDescription;
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
                    returnType = string.Empty;
                }
                catch (FileLoadException)
                {
                    Debug.WriteLine("Cannot Load Return Type For " + method.Name);
                    returnType = string.Empty;
                }
                catch (TypeLoadException)
                {
                    Debug.WriteLine("Cannot Load Return Type For " + method.Name);
                    returnType = string.Empty;
                }

                returnDescription = DescriptionRetriever.GetMemberReturnDescription(method);

                methods.Add(new TypeMethod(
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