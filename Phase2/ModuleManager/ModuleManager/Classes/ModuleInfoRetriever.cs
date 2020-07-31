namespace ModuleManager.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using System.Windows;

    /// <summary>
    /// ModuleInfoRetriever is used to get information from a .dll file.
    /// </summary>
    public class ModuleInfoRetriever
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleInfoRetriever"/> class.
        /// </summary>
        /// <param name="moduleDirectory">File name of the .dll file.</param>
        public ModuleInfoRetriever(string moduleDirectory)
        {
            DllDirectory = moduleDirectory;
            DllFilePath = string.Empty;
            CurrentTypeName = string.Empty;
            PercentOfAssemblyLoaded = 0;
            DescriptionRetriever = new XmlDescriptionRetriever();
            LoadedAssemblies = new ObservableCollection<AssemblyName>();
        }

        /// <summary>
        /// Gets or sets DllDirectory is the directory path of the .dll files.
        /// </summary>
        public string DllDirectory { get; set; }

        /// <summary>
        /// Gets or sets DllFilePath is the path of the .dll file.
        /// </summary>
        public string DllFilePath { get; set; }

        /// <summary>
        /// Gets or sets CurrentTypeName is the name of the type being loaded.
        /// </summary>
        public string CurrentTypeName { get; set; }

        /// <summary>
        /// Gets or sets the current percentage of load compleation of the current assembly.
        /// </summary>
        public double PercentOfAssemblyLoaded { get; set; }

        /// <summary>
        /// Gets or sets all xml descriptions.
        /// </summary>
        public XmlDescriptionRetriever DescriptionRetriever { get; set; }

        /// <summary>
        /// Gets or sets LoadedAssemblies which stores all assemblies that have already been loaded.
        /// </summary>
        public ObservableCollection<AssemblyName> LoadedAssemblies { get; set; }

        /// <summary>
        /// GetModules will create an ObservableCollection of type Module to organize
        /// the information from the dll file and its related .xml file.
        /// </summary>
        /// <returns>Returns an collection of Module objects.</returns>
        public ObservableCollection<Module> GetModules()
        {
            if (string.IsNullOrEmpty(DllDirectory))
            {
                MessageBox.Show(@"The Directory Path Cannot Be Empty");
                return null;
            }

            ObservableCollection<Module> modules = new ObservableCollection<Module>();
            ObservableCollection<Assembly> assemblies = new ObservableCollection<Assembly>();

            // add all the possible referenced assemblies
            string[] runtimeEnvirnmentFiles = Directory.GetFiles(RuntimeEnvironment.GetRuntimeDirectory(), @"*.dll");

            // add all the files of the assemblies you actually want info from
            string[] dllFiles = Directory.GetFiles(DllDirectory, @"*.dll");

            var paths = new List<string>(runtimeEnvirnmentFiles);
            paths.AddRange(dllFiles);

            var resolver = new PathAssemblyResolver(paths);
            var metaDataLoader = new MetadataLoadContext(resolver);

            foreach (var dllFile in dllFiles)
            {
                DllFilePath = dllFile;
                DescriptionRetriever.DllFilePath = dllFile;

                assemblies.Add(metaDataLoader.LoadFromAssemblyPath(dllFile));
            }

            Parallel.ForEach(assemblies, (assembly) =>
            {
                Type[] types = null;

                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    types = ex.Types.Where(t => t != null).ToArray();
                }

                int someNum = 0;
                foreach (var type in types)
                {
                    someNum++;

                    if (type != null)
                    {
                        CurrentTypeName = type.Name;
                        PercentOfAssemblyLoaded = ((double)someNum / (double)types.Length) * 100;

                        Debug.WriteLine("Adding Module: " + type.Name + " From " + assembly.FullName);
                        Module tempModule = GetSingleModule(type);

                         // Add all non-null modules
                         if (tempModule != null)
                         {
                             modules.Add(tempModule);
                         }
                    }
                }
            });

            // Return an alphabetized collection of the found modules.
            return new ObservableCollection<Module>(modules.ToList().OrderBy(mod => mod.Name));
        }

        /// <summary>
        /// Builds a singls module from the given Type.
        /// </summary>
        /// <param name="type">Type from an assembly.</param>
        /// <returns>A Module type.</returns>
        private Module GetSingleModule(Type type)
        {
            // Don't load non-public or interface classes
            if (!type.IsPublic || type.IsInterface)
            {
                return null;
            }

            return new Module(
                type.Name,
                DescriptionRetriever.GetModuleDescription(type),
                AddConstructorsToCollection(type),
                AddPropertiesToCollection(type),
                AddMethodsToCollection(type));
        }

        /// <summary>
        /// AddConstructorsToCollection get all constructors from the passed in Type.
        /// </summary>
        /// <param name="type">The Type where the members are coming from.</param>
        /// <returns>An ObservableCollection of ModuleConstructor objects.</returns>
        private ObservableCollection<ModuleConstructor> AddConstructorsToCollection(Type type)
        {
            ObservableCollection<ModuleConstructor> constructors = new ObservableCollection<ModuleConstructor>();
            ConstructorInfo[] conInfo = type.GetConstructors();

            foreach (var constructor in conInfo)
            {
                int constructorIndex = Array.IndexOf(conInfo, constructor);
                string name = type.Name;
                string description =
                    DescriptionRetriever.GetConstructorDescription(constructor, constructorIndex);

                ObservableCollection<MemberParameter> parameters;
                try
                {
                    parameters =
                        DescriptionRetriever.GetParametersFromList(constructor.GetParameters(), constructorIndex);
                }
                catch (FileNotFoundException)
                {
                    Debug.WriteLine("Cannot Load Parameters For " + constructor.Name + " Constructor");
                    parameters = null;
                }

                constructors.Add(new ModuleConstructor(
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
            foreach (var property in type.GetProperties(BindingFlags.Public
                                      | BindingFlags.Instance
                                      | BindingFlags.DeclaredOnly))
            {
                string name = property.Name;
                string description = DescriptionRetriever.GetPropertyDescription(property);
                string dataType;
                bool canRead = property.CanRead;
                bool canWrite = property.CanWrite;

                try
                {
                    dataType = property.PropertyType.Name.ToString();
                }
                catch (FileNotFoundException)
                {
                    int start = description.IndexOf(@"cref=");
                    int end = description.IndexOf(@"/>");
                    int index = end - start;

                    if (start >= 0)
                    {
                        string typeFromXml = description.Substring(start, index);

                        dataType = typeFromXml.Substring(typeFromXml.LastIndexOf(@".") + 1);
                        dataType = dataType[0..^2];
                    }
                    else
                    {
                        dataType = string.Empty;
                    }
                }

                properties.Add(new ModuleProperty(
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
            string lastMethodName = string.Empty;
            int methodIndex = 0;

            foreach (var method in type.GetMethods(BindingFlags.Public
                                      | BindingFlags.Instance
                                      | BindingFlags.InvokeMethod
                                      | BindingFlags.DeclaredOnly))
            {
                // skip if method is null (if the method is a constructor or property)
                if (method == null)
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

                string description =
                    DescriptionRetriever.GetMethodDescription(method, methodIndex);

                ParameterInfo[] paramInfo;
                try
                {
                    paramInfo = method.GetParameters();
                }
                catch (FileNotFoundException)
                {
                    Debug.WriteLine("Cannot Load Parameters For " + method.Name);
                    paramInfo = null;
                }
                catch (FileLoadException)
                {
                    Debug.WriteLine("Cannot Load Parameters For " + method.Name);
                    paramInfo = null;
                }
                catch (TypeLoadException)
                {
                    Debug.WriteLine("Cannot Load Parameters For " + method.Name);
                    paramInfo = null;
                }

                ObservableCollection<MemberParameter> parameters =
                    DescriptionRetriever.GetParametersFromList(paramInfo, methodIndex);

                string returnType;
                try
                {
                    returnType = method.ReturnType.Name.ToString();
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

                string returnDescription =
                    DescriptionRetriever.GetMemberReturnDescription(method);

                methods.Add(new ModuleMethod(
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