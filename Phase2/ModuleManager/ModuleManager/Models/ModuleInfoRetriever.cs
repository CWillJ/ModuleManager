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
            //// Dll = @"C:\Users\wjohnson\source\repos\ModuleManager\Phase1\ModuleManager\ClassLibrary1\bin\Debug\ClassLibrary1.dll";
            Dll = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName +
                @"\ClassLibrary1\bin\Debug\ClassLibrary1.dll";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleInfoRetriever"/> class.
        /// </summary>
        /// <param name="fileName">File name of the .dll file.</param>
        public ModuleInfoRetriever(string fileName)
        {
            // TODO I need to look for dll files instead of just using a file name.
            Dll = fileName;
        }

        /// <summary>
        /// Gets or sets Dll is the directory path of the .dll files.
        /// TODO should be renamed.
        /// </summary>
        public string Dll { get; set; }

        // Where to Get Certain Information From:
        // Class Name:                   type.Name
        // Method Name:                  member.Name
        // Method Parameter Name:        p.Name.ToString()
        // Method Parameter Type:        p.ParameterType.ToString()
        // Method Return Parameter:      method.ReturnParameter.ToString()
        // Method Return Parameter Type: method.ReturnType.ToString()
        // From .xml
        // Method Description:           GetSummaryFromXML(Dll, member)
        // Method Return Description:    GetReturnFromXML(Dll, member)
        // Method Parameter Description: GetParamFromXML(Dll, member)
        // Class Description:            GetModuleInfoFromXML(Dll, Type)

        /// <returns>Returns an collection of Module objects.</returns>
        /// <summary>
        /// GetInfoFromDll will create an ObservableCollection of type Module to organize
        /// the information from the dll file and its related .xml file.
        /// From .dll.
        /// TODO need to rename because it gets info from .dll and .xml.
        /// </summary>
        public ObservableCollection<Classes.Module> GetInfoFromDll()
        {
            ObservableCollection<Classes.Module> modules = new ObservableCollection<Classes.Module>();
            Assembly a;

            // try to load the assembly from the .dll
            try
            {
                a = Assembly.Load(File.ReadAllBytes(Dll));
            }
            catch (Exception e)
            {
                // TODO need to catch each specific exception
                MessageBox.Show(e.ToString());
                return null;
            }

            // Loop through the types or classes
            Type[] types = a.GetTypes();

            foreach (var type in types)
            {
                modules.Add(GetSingleModule(type));
            }

            return modules;
        }

        private Classes.Module GetSingleModule(Type type)
        {
            ObservableCollection<ModuleMember> members = new ObservableCollection<ModuleMember>();

            if (!type.IsPublic)
            {
                return null;
            }

            // Get Constructrors
            members = AddConstructorsToCollection(members, type);

            // Get Properties
            members = AddPropertiesToCollection(members, type);

            // get public methods from the class and loop through them
            members = AddMethodsToCollection(members, type);

            return new Classes.Module(type.Name, GetModuleInfoFromXML(Dll, type), members);
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
                // TODO need to actually get the constructor description. Will be similar to
                // GetSummaryFromXML but with a ConstructorInfo type instead of a MemberInfo type
                members.Add(new ModuleMember(
                    @"Constructor for " + type.Name,
                    @"Constructor description",
                    GetParametersFromList(constructor.GetParameters()),
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
            foreach (var property in type.GetProperties())
            {
                members.Add(new ModuleMember(
                    property.Name,
                    @"Property description",
                    GetParametersFromList(property.GetIndexParameters()),
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

                members.Add(new ModuleMember(
                    method.Name,
                    GetSummaryFromXML(Dll, method),
                    GetParametersFromList(method.GetParameters()),
                    method.ReturnType.ToString()));
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
                    p.ParameterType.ToString(),
                    p.Name,
                    GetParamFromXML(Dll, p.Member)));
            }

            return parameters;
        }

        private string GetSummaryFromXML(string dllPath, MemberInfo member)
        {
            return GetMethodInfoFromXML(dllPath, member, @"<summary>", @"</summary>");
        }

        private string GetReturnFromXML(string dllPath, MemberInfo member)
        {
            return GetMethodInfoFromXML(dllPath, member, @"<returns>", @"</returns>");
        }

        private string GetParamFromXML(string dllPath, MemberInfo member)
        {
            return GetMethodInfoFromXML(dllPath, member, @"<param name=", @"></param>");
        }

        private string GetMethodInfoFromXML(string dllPath, MemberInfo member, string start, string end)
        {
            // TODO possible pass in two string instead of a MemberInfo type to get
            // member.DeclaringType.FullName and member.Name
            string xmlPath = dllPath.Substring(0, dllPath.LastIndexOf(".")) + @".XML";
            XmlDocument xmlDoc = new XmlDocument();

            if (File.Exists(xmlPath))
            {
                xmlDoc.Load(xmlPath);
                string path = @"M:" + member.DeclaringType.FullName + "." + member.Name;
                XmlNode xmlDocuOfMethod = xmlDoc.SelectSingleNode(@"//member[starts-with(@name, '" + path + @"')]");

                // If the summary comments exist, return them
                if (xmlDocuOfMethod != null)
                {
                    string descript = Regex.Replace(xmlDocuOfMethod.InnerXml, @"\s+", " ");

                    if (!descript.Contains(start) || !descript.Contains(end))
                    {
                        return null;
                    }

                    return descript.Substring(
                        descript.IndexOf(start) + start.Length,
                        descript.IndexOf(end) - descript.IndexOf(start) - start.Length);
                }
            }

            return null;
        }

        private string GetModuleInfoFromXML(string dllPath, Type type)
        {
            string start = @"<summary>";
            string end = @"</summary>";
            string xmlPath = dllPath.Substring(0, dllPath.LastIndexOf(".")) + @".XML";
            XmlDocument xmlDoc = new XmlDocument();

            if (File.Exists(xmlPath))
            {
                xmlDoc.Load(xmlPath);
                string path = @"T:" + type.FullName;
                XmlNode xmlDocuOfMethod = xmlDoc.SelectSingleNode(@"//member[starts-with(@name, '" + path + @"')]");

                // If the summary comments exist, return them
                if (xmlDocuOfMethod != null)
                {
                    string descript = Regex.Replace(xmlDocuOfMethod.InnerXml, @"\s+", " ");

                    if (!descript.Contains(start) || !descript.Contains(end))
                    {
                        return null;
                    }

                    return descript.Substring(
                        descript.IndexOf(start) + start.Length,
                        descript.IndexOf(end) - descript.IndexOf(start) - start.Length);
                }
            }

            return null;
        }
    }
}
