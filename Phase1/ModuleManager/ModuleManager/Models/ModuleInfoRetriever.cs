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
            ObservableCollection<ModuleMethod> methods = new ObservableCollection<ModuleMethod>();
            ObservableCollection<MethodParameter> parameters = new ObservableCollection<MethodParameter>();

            if (!type.IsPublic)
            {
                return null;
            }

            // start with empty method and parameter collections
            methods.Clear();
            parameters.Clear();

            // get constructor information and loop through the class's public constructors
            ConstructorInfo[] constructors = type.GetConstructors();

            foreach (var constructor in constructors)
            {
                // loop through the constructor's parameters
                ParameterInfo[] paramList = constructor.GetParameters();
                foreach (var p in paramList)
                {
                    parameters.Add(new MethodParameter(
                        p.ParameterType.ToString(),
                        p.Name,
                        @"Constructor parameter description"));
                }

                // TODO need to actually get the constructor description. Will be similar to
                // GetSummaryFromXML but with a ConstructorInfo type instead of a MemberInfo type
                methods.Add(new ModuleMethod(
                    @"Constructor for " + type.Name,
                    @"Constructor description",
                    parameters,
                    null));
            }

            // get public methods from the class and loop through them
            MemberInfo[] members = type.GetMembers(BindingFlags.Public
                                                  | BindingFlags.Instance
                                                  | BindingFlags.InvokeMethod);

            foreach (var member in members)
            {
                MethodInfo method = type.GetMethod(member.Name);

                // skip if method is null (if the method is a constructor or property)
                if (method == null)
                {
                    continue;
                }

                // loop through the method's parameters
                ParameterInfo[] paramList = method.GetParameters();
                parameters.Clear();
                foreach (var p in paramList)
                {
                    parameters.Add(new MethodParameter(
                        p.ParameterType.ToString(),
                        p.Name,
                        GetParamFromXML(Dll, member)));
                }

                methods.Add(new ModuleMethod(
                    member.Name,
                    GetSummaryFromXML(Dll, member),
                    parameters,
                    method.ReturnType.ToString()));

                parameters.Clear();
            }

            return new Classes.Module(type.Name, GetModuleInfoFromXML(Dll, type), methods);
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
