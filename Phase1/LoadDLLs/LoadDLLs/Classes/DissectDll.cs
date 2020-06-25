namespace LoadDLLs.Classes
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Xml;

    /// <summary>
    /// DissectDll is used to get information from a .dll file
    /// </summary>
    public class DissectDll
    {
        public DissectDll()
        {
            Modules = new ObservableCollection<Module>();

            //// Dll = @"C:\Users\wjohnson\source\repos\ModuleManager\Phase1\LoadDLLs\ClassLibrary1\bin\Debug\ClassLibrary1.dll";
            Dll = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName +
                @"\ClassLibrary1\bin\Debug\ClassLibrary1.dll";
        }

        /// <summary>
        /// Constructor for DissectDll specifing a file name
        /// </summary>
        /// <param name="fileName">File name of the .dll file</param>
        public DissectDll(string fileName)
        {
            Modules = new ObservableCollection<Module>();
            Dll = fileName;
        }

        /// <summary>
        /// Dll is the directory path of the .dll files
        /// </summary>
        public string Dll { get; set; }

        /// <summary>
        /// Modules is a collection of the .dll files (modules)
        /// </summary>
        public ObservableCollection<Module> Modules { get; set; }

        /// <summary>
        /// GetInfoFromDll will create an ObservableCollection<Module> to organize
        /// the information from the dll file and its related xml file.
        /// From .dll
        /// Class Name:                   type.Name
        /// Method Name:                  member.Name
        /// Method Parameter Name:        p.Name.ToString()
        /// Method Parameter Type:        p.ParameterType.ToString()
        /// Method Return Parameter:      method.ReturnParameter.ToString()
        /// Method Return Parameter:      Type: method.ReturnType.ToString()
        /// From .xml
        /// Method Description:           GetSummaryFromXML(Dll, member)
        /// Method Return Description:    GetReturnFromXML(Dll, member)
        /// Method Parameter Description: GetParamFromXML(Dll, member)
        /// Class Description:            GetModuleInfoFromXML(Dll, Type)
        /// </summary>
        public void GetInfoFromDll()
        {
            //// MessageBox.Show(Dll);
            Assembly a;
            ObservableCollection<ModuleMethod> methods = new ObservableCollection<ModuleMethod>();
            ObservableCollection<MethodParameter> parameters = new ObservableCollection<MethodParameter>();

            // try to load the assembly from the .dll
            try
            {
                a = Assembly.Load(File.ReadAllBytes(Dll));
            }
            catch (Exception e)
            {
                // TODO need to catch each specific exception
                MessageBox.Show(e.ToString());
                return;
            }

            Type[] types = a.GetTypes();

            foreach (Type type in types)
            {
                if (!type.IsPublic)
                {
                    continue;
                }

                Module module;
                methods.Clear();
                MemberInfo[] members = type.GetMembers(BindingFlags.Public
                                                      | BindingFlags.Instance
                                                      | BindingFlags.InvokeMethod);

                foreach (MemberInfo member in members)
                {
                    MethodInfo method = type.GetMethod(member.Name);
                    ParameterInfo[] pars = method.GetParameters();
                    parameters.Clear();

                    foreach (ParameterInfo p in pars)
                    {
                        parameters.Add(new MethodParameter(p.ParameterType.ToString(), p.Name.ToString(), GetParamFromXML(Dll, member)));
                    }

                    methods.Add(new ModuleMethod(member.Name, GetSummaryFromXML(Dll, member), parameters, method.ReturnType.ToString()));
                }

                module = new Module(type.Name, GetModuleInfoFromXML(Dll, type), methods);
                Modules.Add(module);
            }
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

                    return descript.Substring(descript.IndexOf(start) + start.Length,
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

                    return descript.Substring(descript.IndexOf(start) + start.Length,
                        descript.IndexOf(end) - descript.IndexOf(start) - start.Length);
                }
            }

            return null;
        }
    }
}
