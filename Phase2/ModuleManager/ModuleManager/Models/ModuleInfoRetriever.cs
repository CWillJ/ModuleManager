namespace ModuleManager.Models
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Xml;
    using ModuleManager.Classes;
    using Xamarin.Forms.Internals;

    /// <summary>
    /// ModuleInfoRetriever is used to get information from a .dll file.
    /// </summary>
    public class ModuleInfoRetriever
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleInfoRetriever"/> class.
        /// </summary>
        public ModuleInfoRetriever()
        {
            // TODO Only used for testing. Need to set some sort of default path here.
            //// DllFileName = @"C:\Users\wjohnson\source\repos\ModuleManager\Phase1\ModuleManager\ClassLibrary1\bin\Debug\ClassLibrary1.dll";
            //// DllFileName = Directory.GetCurrentDirectory() + @"\" + Assembly.GetCallingAssembly().GetName().Name + @".dll";
            //// DllFileName.Add(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName +
            ////     @"\ClassLibrary1\bin\Debug\ClassLibrary1.dll");
            DllDirectory = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleInfoRetriever"/> class.
        /// </summary>
        /// <param name="moduleDirectory">File name of the .dll file.</param>
        public ModuleInfoRetriever(string moduleDirectory)
        {
            // TODO I need to look for dll files instead of just using a file name.
            DllDirectory = moduleDirectory;
        }

        /// <summary>
        /// Gets or sets DllFileName is the directory path of the .dll files.
        /// TODO should be renamed.
        /// </summary>
        public string DllFileName { get; set; }

        /// <summary>
        /// Gets or sets DllDirectory is the directory path of the .dll files.
        /// TODO should be renamed.
        /// </summary>
        public string DllDirectory { get; set; }

        /// <summary>
        /// GetModules will create an ObservableCollection of type Module to organize
        /// the information from the dll file and its related .xml file.
        /// From .dll.
        /// TODO remove the parameter if I'm going to use the property.
        /// </summary>
        /// <returns>Returns an collection of Module objects.</returns>
        public ObservableCollection<Classes.Module> GetModules()
        {
            // TODO throw an exception
            if (string.IsNullOrEmpty(DllDirectory))
            {
                MessageBox.Show(@"The Directory Path Cannot Be Empty");
                return null;
            }

            ObservableCollection<Classes.Module> modules = new ObservableCollection<Classes.Module>();

            ////string[] moduleDlls = Directory.GetFiles(moduleDirectory, @"*.dll");

            foreach (var dllFile in Directory.GetFiles(DllDirectory, @"*.dll"))
            {
                DllFileName = dllFile;

                Assembly assembly;

                // try to load the assembly from the .dll
                try
                {
                    //// assembly = Assembly.Load(File.ReadAllBytes(dllFile));
                    assembly = Assembly.ReflectionOnlyLoadFrom(dllFile);
                }
                catch (Exception e)
                {
                    // TODO need to catch each specific exception
                    MessageBox.Show(e.ToString());
                    continue;
                }

                Type[] types;

                // Loop through the types or classes
                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    types = ex.Types;
                }

                foreach (var type in types)
                {
                    if (type != null)
                    {
                        modules.Add(GetSingleModule(type));
                    }
                }
            }

            return modules;
        }

        private Classes.Module GetSingleModule(Type type)
        {
            ObservableCollection<ModuleMember> members = new ObservableCollection<ModuleMember>();

            if (!type.IsPublic || type.IsInterface)
            {
                return null;
            }

            // Get Public Constructrors
            members = AddConstructorsToCollection(members, type);

            // Get Public Properties
            members = AddPropertiesToCollection(members, type);

            // Get All Public Methods
            members = AddMethodsToCollection(members, type);

            return new Classes.Module(type.Name, GetModuleDescription(type), members);
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
                members.Add(new ModuleMember(
                    @"Constructor for " + type.Name,
                    GetMethodDescription((MemberInfo)constructor, type.GetConstructors().IndexOf(constructor)),
                    GetParametersFromList(constructor.GetParameters()),
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
                //// MessageBox.Show(property.Name);
                members.Add(new ModuleMember(
                    property.Name,
                    GetMethodDescription((MemberInfo)property),
                    GetParametersFromList(property.GetIndexParameters()),
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
            foreach (var method in type.GetMethods(BindingFlags.Public
                                      | BindingFlags.Instance
                                      | BindingFlags.InvokeMethod))
            {
                // skip if method is null (if the method is a constructor or property)
                if (method == null)
                {
                    continue;
                }

                // TODO need to get this int to be the index of the method with the same name.
                int whatever = type.GetMethods().IndexOf(method);
                whatever = 0;

                members.Add(new ModuleMember(
                    method.Name,
                    GetMethodDescription((MemberInfo)method, whatever),
                    GetParametersFromList(method.GetParameters()),
                    method.ReturnType.Name.ToString(),
                    GetMemberReturnDescription((MemberInfo)method)));
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
            ObservableCollection<MemberParameter> parameters = new ObservableCollection<MemberParameter>();

            foreach (var p in paramList)
            {
                parameters.Add(new MemberParameter(
                    p.ParameterType.Name.ToString(),
                    p.Name,
                    GetMemberParameterDescription(p.Member, paramList.IndexOf(p))));
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
                return null;
            }
            catch (XmlException)
            {
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
