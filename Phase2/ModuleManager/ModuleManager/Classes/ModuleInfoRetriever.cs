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
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Xml;

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
            LoadedAssemblies = new ObservableCollection<string>();
        }

        /// <summary>
        /// Gets or sets DllFileName is the directory path of the .dll files.
        /// </summary>
        public string DllFileName { get; set; }

        /// <summary>
        /// Gets or sets DllDirectory is the directory path of the .dll files.
        /// </summary>
        public string DllDirectory { get; set; }

        /// <summary>
        /// Gets or sets LoadedAssemblies which stores all assemblies that have already been loaded.
        /// </summary>
        public ObservableCollection<string> LoadedAssemblies { get; set; }

        /// <summary>
        /// GetModules will create an ObservableCollection of type Module to organize
        /// the information from the dll file and its related .xml file.
        /// From .dll.
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
            string[] dllFiles = Directory.GetFiles(DllDirectory, @"*.dll");
            var paths = new List<string>(runtimeEnvirnmentFiles);

            // add all the files of the assemblies you actually want info from
            foreach (var dllFile in dllFiles)
            {
                paths.Add(dllFile);
            }

            var resolver = new PathAssemblyResolver(paths);
            var mlc = new MetadataLoadContext(resolver);

            foreach (var dllFile in dllFiles)
            {
                DllFileName = dllFile;

                assemblies.Add(mlc.LoadFromAssemblyPath(dllFile));
            }

            // TODO Async this
            Parallel.ForEach(assemblies, (assembly) =>
            {
                LoadAllAssemblies(assembly);

                Type[] types = null;

                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    types = ex.Types.Where(t => t != null).ToArray();
                }

                foreach (var type in types)
                {
                    if (type != null)
                    {
                        ////Debug.WriteLine("Adding Module: " + type.Name);
                        modules.Add(GetSingleModule(type));
                    }
                }
            });

            // Return an alphabetized collection of the found non-null modules
            var noNullsList = modules.Where(x => x != null).ToList();
            return new ObservableCollection<Module>(noNullsList.OrderBy(mod => mod.Name));
        }

        private void LoadAllAssemblies(Assembly assembly)
        {
            AssemblyName[] names = assembly.GetReferencedAssemblies();
            Assembly attemptToLoadAssembly;

            foreach (AssemblyName name in names)
            {
                try
                {
                    if (!LoadedAssemblies.Contains(name.FullName))
                    {
                        attemptToLoadAssembly = Assembly.ReflectionOnlyLoad(name.FullName);
                        LoadedAssemblies.Add(attemptToLoadAssembly.FullName);
                    }
                    else
                    {
                        continue;
                    }
                }
                catch (ArgumentNullException)
                {
                    Debug.WriteLine("Cannot Load " + name.Name);
                    continue;
                }
                catch (BadImageFormatException)
                {
                    Debug.WriteLine("Cannot Load " + name.Name);
                    continue;
                }
                catch (FileLoadException)
                {
                    Debug.WriteLine("Cannot Load " + name.Name);
                    continue;
                }
                catch (PlatformNotSupportedException)
                {
                    Debug.WriteLine("Cannot Load " + name.Name);
                    continue;
                }
                catch (FileNotFoundException)
                {
                    Debug.WriteLine("Cannot Load " + name.Name);
                    continue;
                }

                LoadAllAssemblies(attemptToLoadAssembly);
            }
        }

        private Module GetSingleModule(Type type)
        {
            ObservableCollection<ModuleMember> members = new ObservableCollection<ModuleMember>();

            // Don't load non-public or interface classes
            if (!type.IsPublic || type.IsInterface)
            {
                return null;
            }

            // Get Public Constructrors
            members = AddConstructorsToCollection(members, type);

            // Get Public Properties
            members = AddPropertiesToCollection(members, type);

            // Get Public Methods
            members = AddMethodsToCollection(members, type);

            return new Module(type.Name, GetModuleDescription(type), members);
        }

        /// <summary>
        /// AddConstructorsToCollection takes in an ObservableCollection of ModuleMember type and a
        /// Type in order to get all constructors from that Type and add them to the collection.
        /// </summary>
        /// <param name="members">An ObservableCollection to append members to.</param>
        /// <param name="type">The Type where the members are coming from.</param>
        /// <returns>The original ObservableCollection with appened members from type.</returns>
        private ObservableCollection<ModuleMember> AddConstructorsToCollection(
            ObservableCollection<ModuleMember> members,
            Type type)
        {
            foreach (var constructor in type.GetConstructors())
            {
                string name = @"Constructor for " + type.Name;

                string description =
                    GetMethodDescription(constructor, Array.IndexOf(type.GetConstructors(), constructor));

                ObservableCollection<MemberParameter> parameters;

                try
                {
                    parameters = GetParametersFromList(constructor.GetParameters());
                }
                catch (FileNotFoundException)
                {
                    Debug.WriteLine("Cannot Load " + constructor.Name + " Constructor");
                    parameters = null;
                }

                members.Add(new ModuleMember(
                    name,
                    description,
                    parameters,
                    null,
                    null));
            }

            return members;
        }

        /// <summary>
        /// AddPropertiesToCollection takes in an ObservableCollection of ModuleMember type and a
        /// Type in order to get all properties from that Type and add them to the collection.
        /// </summary>
        /// <param name="members">An ObservableCollection to append members to.</param>
        /// <param name="type">The Type where the members are coming from.</param>
        /// <returns>The original ObservableCollection with appened members from type.</returns>
        private ObservableCollection<ModuleMember> AddPropertiesToCollection(
            ObservableCollection<ModuleMember> members,
            Type type)
        {
            foreach (var property in type.GetProperties(BindingFlags.Public
                                      | BindingFlags.Instance
                                      | BindingFlags.DeclaredOnly))
            {
                string name = property.Name;
                string description = GetPropertyDescription(property);

                // There are no parameters to a property
                ParameterInfo[] paramInfo;
                ObservableCollection<MemberParameter> parameters;
                try
                {
                    paramInfo = property.GetIndexParameters();
                    parameters = GetParametersFromList(paramInfo);
                }
                catch (FileNotFoundException)
                {
                    Debug.WriteLine("Cannot Load Parameters For" + property.Name);
                    parameters = null;
                }
                catch (FileLoadException)
                {
                    Debug.WriteLine("Cannot Load Parameters For" + property.Name);
                    parameters = null;
                }
                catch (TypeLoadException)
                {
                    Debug.WriteLine("Cannot Load Parameters For" + property.Name);
                    parameters = null;
                }

                members.Add(new ModuleMember(
                    name,
                    description,
                    parameters,
                    null,
                    null));
            }

            return members;
        }

        /// <summary>
        /// AddMethodsToCollection takes in an ObservableCollection of ModuleMember type and a
        /// Type in order to get all methods from that Type and add them to the collection.
        /// </summary>
        /// <param name="members">An ObservableCollection to append members to.</param>
        /// <param name="type">The Type where the members are coming from.</param>
        /// <returns>The original ObservableCollection with appened members from type.</returns>
        private ObservableCollection<ModuleMember> AddMethodsToCollection(
            ObservableCollection<ModuleMember> members,
            Type type)
        {
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

                string description = GetMethodDescription(method, methodIndex);

                ParameterInfo[] paramInfo;
                try
                {
                    Debug.WriteLine("Cannot Load Parameters For" + method.Name);
                    paramInfo = method.GetParameters();
                }
                catch (FileNotFoundException)
                {
                    Debug.WriteLine("Cannot Load Parameters For" + method.Name);
                    paramInfo = null;
                }
                catch (FileLoadException)
                {
                    Debug.WriteLine("Cannot Load Parameters For" + method.Name);
                    paramInfo = null;
                }
                catch (TypeLoadException)
                {
                    Debug.WriteLine("Cannot Load Parameters For" + method.Name);
                    paramInfo = null;
                }

                ObservableCollection<MemberParameter> parameters =
                    GetParametersFromList(paramInfo);

                string returnType;
                try
                {
                    Debug.WriteLine("Cannot Load Return Type For" + method.Name);
                    returnType = method.ReturnType.Name.ToString();
                }
                catch (FileNotFoundException)
                {
                    Debug.WriteLine("Cannot Load Return Type For" + method.Name);
                    returnType = string.Empty;
                }
                catch (FileLoadException)
                {
                    Debug.WriteLine("Cannot Load Return Type For" + method.Name);
                    returnType = string.Empty;
                }
                catch (TypeLoadException)
                {
                    Debug.WriteLine("Cannot Load Return Type For" + method.Name);
                    returnType = string.Empty;
                }

                string returnDescription = GetMemberReturnDescription(method);

                members.Add(new ModuleMember(
                    name,
                    description,
                    parameters,
                    returnType,
                    returnDescription));
            }

            return members;
        }

        /// <summary>
        /// GetParametersFromList will return an ObservableCollection of MemberParameter
        /// type from a list of ParameterInfo type.
        /// </summary>
        /// <param name="paramList">A list of ParameterInfo type.</param>
        /// <returns>An ObservableCollection of MemberParameter type.</returns>
        private ObservableCollection<MemberParameter> GetParametersFromList(ParameterInfo[] paramList)
        {
            if (paramList == null)
            {
                return null;
            }

            ObservableCollection<MemberParameter> parameters = new ObservableCollection<MemberParameter>();

            foreach (var p in paramList)
            {
                string pType = p.ParameterType.Name.ToString();
                string pName = p.Name;
                string pDescription =
                    GetMemberParameterDescription(p.Member, Array.IndexOf(paramList, p));

                parameters.Add(new MemberParameter(
                    pType,
                    pName,
                    pDescription));
            }

            return parameters;
        }

        /// <summary>
        /// GetModuleDescription returns a clean string from the inner xml
        /// of the class description of the Type.
        /// </summary>
        /// <param name="type">Type to get the string from.</param>
        /// <returns>String representation of the class description.</returns>
        private string GetModuleDescription(Type type)
        {
            XmlNode xmlNode = GetModuleXmlNode(type);

            if (xmlNode == null)
            {
                return null;
            }

            return GetXmlNodeString(xmlNode, "summary");
        }

        /// <summary>
        /// GetMethodDescription returns a clean string from the inner xml
        /// of the method description of the member.
        /// </summary>
        /// <param name="member">MemberInfo to get the string from.</param>
        /// <param name="index">Index used for methods/constructors with same name.</param>
        /// <returns>String representation of the method description.</returns>
        private string GetMethodDescription(MemberInfo member, int index = 0)
        {
            XmlNode xmlNode = GetMemberXmlNode(member, index);

            if (xmlNode == null)
            {
                return null;
            }

            return GetXmlNodeString(xmlNode, "summary");
        }

        /// <summary>
        /// GetProperyDescription will return a string from the inner xml of
        /// the property desctiption.
        /// </summary>
        /// <param name="property">PropertyInfo to get the description from.</param>
        /// <returns>String representation of the property description.</returns>
        private string GetPropertyDescription(PropertyInfo property)
        {
            string xmlPath = DllFileName.Substring(0, DllFileName.LastIndexOf(".")) + @".XML";
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(xmlPath);
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine("Cannot Load Property Description For" + property.Name);
                return null;
            }
            catch (XmlException)
            {
                Debug.WriteLine("Cannot Load Property Description For" + property.Name);
                return null;
            }

            string path = @"P:" + property.DeclaringType.FullName + @"." + property.Name;

            XmlNode xmlNode = xmlDoc.SelectSingleNode(@"//member[starts-with(@name, '" + path + @"')]");

            if (xmlNode == null)
            {
                return null;
            }

            return GetXmlNodeString(xmlNode, "summary");
        }

        /// <summary>
        /// GetMemberParameterDescription returns a clean string from the inner xml
        /// of the parameter description of the member.
        /// </summary>
        /// <param name="member">MemberInfo to get the string from.</param>
        /// <param name="parameterIndex">Integer index of parameter.</param>
        /// <returns>String representation of the parameter description.</returns>
        private string GetMemberParameterDescription(MemberInfo member, int parameterIndex)
        {
            XmlNode xmlNode = GetMemberXmlNode(member);

            if (xmlNode == null)
            {
                return null;
            }

            return GetXmlNodeString(xmlNode, "param", parameterIndex);
        }

        /// <summary>
        /// GetMemberReturnDescription returns a clean string from the inner xml
        /// of the return description of the member.
        /// </summary>
        /// <param name="member">MemberInfo to get the string from.</param>
        /// <returns>String representation of the return description.</returns>
        private string GetMemberReturnDescription(MemberInfo member)
        {
            XmlNode xmlNode = GetMemberXmlNode(member);

            if (xmlNode == null)
            {
                return null;
            }

            return GetXmlNodeString(xmlNode, "returns");
        }

        /// <summary>
        /// GetMemberXmlNode returns an XmlNode of the specified MemberInfo.
        /// </summary>
        /// <param name="member">The MemberInfo to get the XmlNode from.</param>
        /// <param name="nodeIndex">The specified node index to handle members with the same name.</param>
        /// <returns>XmlNode.</returns>
        private XmlNode GetMemberXmlNode(MemberInfo member, int nodeIndex = 0)
        {
            // Get the xml document from the file path.
            string xmlPath = DllFileName.Substring(0, DllFileName.LastIndexOf(".")) + @".XML";
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(xmlPath);
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine("Cannot Load Member XML For" + member.Name);
                return null;
            }
            catch (XmlException)
            {
                Debug.WriteLine("Cannot Load Member XML For" + member.Name);
                return null;
            }

            string path;
            if (member.Name == ".ctor")
            {
                path = @"M:" + member.DeclaringType.FullName + @".#" + member.Name.Substring(1);
            }
            else
            {
                path = @"M:" + member.DeclaringType.FullName + @"." + member.Name;
            }

            XmlNodeList xmlNodeList = xmlDoc.SelectNodes(@"//member[starts-with(@name, '" + path + @"')]");

            return xmlNodeList[nodeIndex];
        }

        /// <summary>
        /// GetModuleXmlNode returns an XmlNode of the specified Type.
        /// </summary>
        /// <param name="type">The Type to get the XmlNode from.</param>
        /// <returns>XmlNode.</returns>
        private XmlNode GetModuleXmlNode(Type type)
        {
            string xmlPath = DllFileName.Substring(0, DllFileName.LastIndexOf(".")) + @".XML";
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(xmlPath);
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine("Cannot Load Module XML For" + type.Name);
                return null;
            }

            string path = @"T:" + type.FullName;
            XmlNode xmlNode = xmlDoc.SelectSingleNode(@"//member[starts-with(@name, '" + path + @"')]");

            return xmlNode;
        }

        /// <summary>
        /// GetXmlNodeString will take an XmlNode, string xml tag, and an index and return the inner xml.
        /// </summary>
        /// <param name="xmlNode">The member XmlNode.</param>
        /// <param name="xmlTag">This is the string of the xml tag.</param>
        /// <param name="index">Index of the XmlNodeList, defaults to 0. (used for more than one parameter).</param>
        /// <returns>InnerXml of the XmlNode.</returns>
        private string GetXmlNodeString(XmlNode xmlNode, string xmlTag, int index = 0)
        {
            string s = null;

            XmlNodeList xmlNodeList = xmlNode.SelectNodes(xmlTag);
            if (xmlNodeList[index] == null)
            {
                return s;
            }

            s = xmlNodeList[index].InnerXml;
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            s = Regex.Replace(s, @"\s+", " ");
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            return s;
        }
    }
}