namespace ModuleManager.Common.Classes
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using ModuleManager.Common.Interfaces;
    using Prism.Regions;

    /// <summary>
    /// Retrieves assemblies from dll files.
    /// </summary>
    public class AssemblyDataLoader
    {
        private readonly IRegionManager _regionManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyDataLoader"/> class.
        /// </summary>
        /// <param name="regionManager">The <see cref="IRegionManager"/>.</param>
        public AssemblyDataLoader(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            DllDirectory = string.Empty;
            DllFilePath = string.Empty;
            CurrentAssemblyName = string.Empty;
            CurrentTypeName = string.Empty;
            PercentOfAssemblyLoaded = 0;
            DescriptionRetriever = new XmlDescriptionRetriever();

            LoadedAssemblies = new ObservableCollection<AssemblyName>();
        }

        /// <summary>
        /// Gets or sets the <see cref="string"/> directory path of the .dll files.
        /// </summary>
        public string DllDirectory { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="string"/> path of the .dll file.
        /// </summary>
        public string DllFilePath { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="string"/> name of the assembly being loaded.
        /// </summary>
        public string CurrentAssemblyName { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="string"/> name of the type being loaded.
        /// </summary>
        public string CurrentTypeName { get; set; }

        /// <summary>
        /// Gets or sets the current <see cref="double"/> percentage of load compleation of the current assembly.
        /// </summary>
        public double PercentOfAssemblyLoaded { get; set; }

        private ObservableCollection<AssemblyName> LoadedAssemblies { get; set; }

        /// <summary>
        /// Gets or sets all xml descriptions.
        /// </summary>
        private XmlDescriptionRetriever DescriptionRetriever { get; set; }

        /// <summary>
        /// Initialize properties.
        /// </summary>
        /// <param name="modulesDirectory">Directory containing <see cref="string"/> dll files.</param>
        /// <param name="assemblyFilePath">Name of the specific <see cref="string"/> dll file.</param>
        public void Initialize(string modulesDirectory, string assemblyFilePath)
        {
            DllDirectory = modulesDirectory;
            DllFilePath = assemblyFilePath;
            DescriptionRetriever = new XmlDescriptionRetriever(assemblyFilePath);
        }

        /// <summary>
        /// Creates an <see cref="ObservableCollection{AssemblyData}"/> to organize
        /// the information from the dll file and its related xml file.
        /// </summary>
        /// <param name="dllFiles">A <see cref="string"/> array containing the names of all dll files in the DllDirectory.</param>
        /// <returns>Returns an <see cref="ObservableCollection{AssemblyData}"/>.</returns>
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

                // IF the ModuleType has not been set, it does not have an IModule
                if (assemblyData.ModuleType != null)
                {
                    assemblies.Add(assemblyData);
                }
            }

            return assemblies;
        }

        /// <summary>
        /// Loads all enabled <see cref="AssemblyData"/> and unloads the disabled ones.
        /// </summary>
        /// <param name="assemblyData">An <see cref="AssemblyData"/> object passed by reference.</param>
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

        /// <summary>
        /// Loads all enabled <see cref="AssemblyData"/> and unloads the disabled ones.
        /// </summary>
        /// <param name="assemblies">A <see cref="ObservableCollection{AssemblyData}"/> passed by reference.</param>
        public void LoadUnload(ref ObservableCollection<AssemblyData> assemblies)
        {
            AssemblyData assemblyData;

            for (int i = 0; i < assemblies.Count; i++)
            {
                assemblyData = assemblies[i];
                LoadUnload(ref assemblyData);
                assemblies[i] = assemblyData;
            }
        }

        /// <summary>
        /// Loads all <see cref="AssemblyData"/> in a <see cref="ObservableCollection{AssemblyData}"/>.
        /// </summary>
        /// <param name="assemblies">A <see cref="ObservableCollection{AssemblyData}"/> objects.</param>
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

        /// <summary>
        /// Load an <see cref="AssemblyData"/> by reference.
        /// </summary>
        /// <param name="assemblyData"><see cref="AssemblyData"/> to load passed by reference.</param>
        public void Load(ref AssemblyData assemblyData)
        {
            if (!string.IsNullOrEmpty(assemblyData.FilePath))
            {
                Initialize(assemblyData.FilePath.Substring(0, assemblyData.FilePath.LastIndexOf(".")), assemblyData.FilePath);
            }

            if (assemblyData.Loader == null)
            {
                assemblyData.Loader = new AssemblyLoader(DllFilePath);
            }

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

                    Debug.WriteLine(@"Adding Module: " + CurrentTypeName + @" From " + CurrentAssemblyName);
                    TypeData tempModule = GetTypeData(type);

                    if (tempModule != null)
                    {
                        Type[] typeInterfaces = type.GetInterfaces();

                        if ((assemblyData.ModuleType == null) && (typeInterfaces.Contains(typeof(IModuleManagerTestModule)) || typeInterfaces.Contains(typeof(IModuleManagerCoreModule))))
                        {
                            assemblyData.ModuleType = type;
                        }

                        assemblyData.Types.Add(tempModule);
                    }
                }
            }
        }

        /// <summary>
        /// Unload an <see cref="AssemblyData"/> by reference.
        /// </summary>
        /// <param name="assemblyData"><see cref="AssemblyData"/> to unload passed by reference.</param>
        public void Unload(ref AssemblyData assemblyData)
        {
            if (_regionManager.Regions.ContainsRegionWithName(@"LoadedViewsRegion"))
            {
                foreach (TypeData typeData in assemblyData.Types)
                {
                    if (typeData.IsView)
                    {
                        foreach (var view in _regionManager.Regions[@"LoadedViewsRegion"].Views)
                        {
                            if (view.GetType().FullName == typeData.FullName)
                            {
                                _regionManager.Regions[@"LoadedViewsRegion"].Remove(view);
                            }
                        }
                    }
                }
            }

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