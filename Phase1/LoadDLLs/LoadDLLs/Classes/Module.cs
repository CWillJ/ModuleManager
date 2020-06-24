namespace LoadDLLs.Classes
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Security.Cryptography;

    /// <summary>
    /// Module object holds the name and description of a module
    /// </summary>
    public class Module
    {
        private string _moduleName;
        private string _moduleDescription;
        private ObservableCollection<ModuleMethod> _moduleMethods;

        /// <summary>
        /// Constructor initializes properties to empty strings/empty collections
        /// </summary>
        public Module()
        {
            Name = string.Empty;
            Description = string.Empty;
            Methods = new ObservableCollection<ModuleMethod>();
        }

        /// <summary>
        /// Constructor that initializes a Module specifying the name, description and methods
        /// </summary>
        /// <param name="name">Module name</param>
        /// <param name="description">Module description</param>
        /// <param name="methods">Module methods</param>
        public Module(string name, string description, ObservableCollection<ModuleMethod> methods)
        {
            Name = name;
            Description = description;
            Methods = methods;
        }

        /// <summary>
        /// Property that gets or sets the module name
        /// </summary>
        public string Name
        {
            get { return _moduleName; }
            set { _moduleName = value; }
        }

        /// <summary>
        /// Property that gets or sets the description of the module
        /// </summary>
        public string Description
        {
            get { return _moduleDescription; }
            set { _moduleDescription = value; }
        }

        /// <summary>
        /// Property that gets or sets the methods in the current module
        /// </summary>
        public ObservableCollection<ModuleMethod> Methods
        {
            get { return _moduleMethods; }
            set { _moduleMethods = value; }
        }

        /// <summary>
        /// AddMethod adds a method to the current module
        /// </summary>
        /// <param name="method">Specifiex module method</param>
        public void AddMethod(ModuleMethod method)
        {
            Methods.Add(method);
        }

        /// <summary>
        /// AddMethod adds a method to the current module with the specified properties
        /// </summary>
        /// <param name="name">Method name</param>
        /// <param name="description">Method description</param>
        /// <param name="parameters">Method parameters</param>
        /// <param name="returnType">Method return type</param>
        public void AddMethod(string name, string description, ObservableCollection<MethodParameter> parameters, string returnType)
        {
            AddMethod(new ModuleMethod(name, description, parameters, returnType));
        }

        /// <summary>
        /// MethodCount gets the number of methods in Methods
        /// </summary>
        /// <returns>Methods.Count()</returns>
        public int MethodCount()
        {
            return Methods.Count();
        }

        /// <summary>
        /// Overrides the ToString method and formats the string output
        /// </summary>
        /// <returns>A desired format for the module name, description and all methods contained in module</returns>
        public override string ToString()
        {
            string s = Name + @":\n" + Description;

            foreach(ModuleMethod method in Methods)
            {
                s += @"\n" + method.ToString();
            }

            return s;
        }
    }
}
