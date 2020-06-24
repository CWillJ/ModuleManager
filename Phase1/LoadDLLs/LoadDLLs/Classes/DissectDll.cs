namespace LoadDLLs.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
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
        /// TODO need to get this method to sucessfully get info from a .dll
        /// </summary>
        public void GetInfoFromDll()
        {
            MessageBox.Show(Dll);
            Assembly a;
            ObservableCollection<ModuleMethod> methods = new ObservableCollection<ModuleMethod>();
            ObservableCollection<MethodParameter> parameters = new ObservableCollection<MethodParameter>();

            try
            {
                a = Assembly.Load(File.ReadAllBytes(Dll));
            }
            catch (Exception e)
            {
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

                        //// MessageBox.Show(
                        ////     "Class Name: " + type.Name + "\n" +
                        ////     "Method Name: " + member.Name + "\n" +
                        ////     "Method Parameter Name: " + p.Name.ToString() + "\n" +
                        ////     "Method Parameter Type: " + p.ParameterType.ToString() + "\n" +
                        ////     "Method Return Name: " + method.ReturnParameter.ToString() + "\n" +
                        ////     "Method Return Type: " + method.ReturnType.ToString() + "\n" +
                        ////     "Method Description: " + GetSummaryFromXML(Dll, member) + "\n" +
                        ////     "Method Returns (from xml): " + GetReturnFromXML(Dll, member) + "\n" +
                        ////     "Method Parameter Description: " + GetParamFromXML(Dll, member));
                    }

                    methods.Add(new ModuleMethod(member.Name, GetSummaryFromXML(Dll, member), parameters, GetReturnFromXML(Dll, member)));
                }

                module = new Module(type.Name, "Class Description", methods);

                Modules.Add(module);

                MessageBox.Show(module.ToString());
            }
        }

        private string GetSummaryFromXML(string dllPath, MemberInfo member)
        {
            return GetInfoFromXML(dllPath, member, @"<summary>", @"</summary>");
        }

        private string GetReturnFromXML(string dllPath, MemberInfo member)
        {
            return GetInfoFromXML(dllPath, member, @"<returns>", @"</returns>");
        }

        private string GetParamFromXML(string dllPath, MemberInfo member)
        {
            return GetInfoFromXML(dllPath, member, @"<param name=", @"></param>");
        }

        private string GetInfoFromXML(string dllPath, MemberInfo member, string start, string end)
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
    }
}
