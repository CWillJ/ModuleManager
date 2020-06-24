namespace LoadDLLs
{
    using System;
    using System.Collections.Generic;
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

        public string Dll { get; set; }

        public DissectDll()
        {
            Dll = @"C:\Users\wjohnson\source\repos\ModuleManager\Phase1\LoadDLLs\ClassLibrary1\bin\Debug\ClassLibrary1.dll";
        }

        public DissectDll(string fileName)
        {
            Dll = fileName;
        }

        /// <summary>
        /// TODO need to get this method to sucessfully get info from a .dll
        /// </summary>
        public void GetInfoFromDll()
        {
            MessageBox.Show(Dll);
            Assembly a;

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
            //// Module[] idk = a.GetModules();

            foreach (Type type in types)
            {
                if (!type.IsPublic)
                {
                    continue;
                }
            
                MemberInfo[] members = type.GetMembers(BindingFlags.Public
                                                      | BindingFlags.Instance
                                                      | BindingFlags.InvokeMethod);

                foreach (MemberInfo member in members)
                {
                    MethodInfo method = type.GetMethod(member.Name);
                    ParameterInfo[] pars = method.GetParameters();

                    foreach (ParameterInfo p in pars)
                    {
                        MessageBox.Show(
                            "Class: " + type.Name + "\n" +                          // Class Name
                            "Method: " + member.Name + "\n" +                       // Method Name
                            "Param: " + p.Name.ToString() + "\n" +                  // Parameter Name
                            "Type: " + p.ParameterType.ToString() + "\n" +          // Parameter Type
                            "Return: " + method.ReturnParameter.ToString() + "\n" + // Return Parameter
                            "Type: " + method.ReturnType.ToString() + "\n" +        // Return Type
                            "Description: " + GetSummaryFromXML(Dll, member));      // Summary from xml
                    }
                }
            }
        }

        private string GetSummaryFromXML(string dllPath, MemberInfo member)
        {
            string start = @"<summary>";
            string end = @"</summary>";
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

                    return descript.Substring(descript.IndexOf(start) + start.Length + 1,
                        descript.IndexOf(end) - descript.IndexOf(start) - start.Length - 1);
                }
            }

            return null;
        }
    }
}
