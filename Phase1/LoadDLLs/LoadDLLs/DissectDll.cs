namespace LoadDLLs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;

    /// <summary>
    /// DissectDll is used to get information from a .dll file
    /// </summary>
    public class DissectDll
    {

        public string Dll { get; set; }

        public DissectDll()
        {
            Dll = @"C:/Users/wjohnson/source/repos/NextGen/NextGen/bin/x64/Debug/netcoreapp3.1/CommonServiceLocator.dll";
        }

        public DissectDll(string fileName)
        {
            Dll = fileName;
        }

        public void GetDllInfo()
        {
            Assembly a = typeof(DissectDll).Assembly;

            Type[] types = a.GetTypes();

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
                    MessageBox.Show(type.Name + "." + member.Name);
                }
            }
        }
    }
}
