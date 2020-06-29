namespace ModuleManager.Classes
{
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// ModuleMethod object holds the description, the parameters and the return type of a method.
    /// </summary>
    public class ModuleMethod
    {
        private string _methodName;
        private string _methodDescription;
        private ObservableCollection<MethodParameter> _methodParameters;
        private string _methodReturnType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleMethod"/> class.
        /// </summary>
        public ModuleMethod()
        {
            Name = string.Empty;
            Description = string.Empty;
            Parameters = new ObservableCollection<MethodParameter>();
            ReturnType = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleMethod"/> class
        /// with specified name, description, parameters and type through passed in parameters.
        /// </summary>
        /// <param name="name">Method name.</param>
        /// <param name="description">Method description.</param>
        /// <param name="parameters">Method parameters.</param>
        /// <param name="returnType">Method return type.</param>
        public ModuleMethod(string name, string description, ObservableCollection<MethodParameter> parameters, string returnType)
        {
            Name = name;
            Description = description;
            Parameters = parameters;
            ReturnType = returnType;
        }

        /// <summary>
        /// Gets or sets the name of the method.
        /// </summary>
        public string Name
        {
            get { return _methodName; }
            set { _methodName = value; }
        }

        /// <summary>
        /// Gets or sets the description of the method.
        /// </summary>
        public string Description
        {
            get { return _methodDescription; }
            set { _methodDescription = value; }
        }

        /// <summary>
        /// Gets or sets the method parameters.
        /// </summary>
        public ObservableCollection<MethodParameter> Parameters
        {
            get { return _methodParameters; }
            set { _methodParameters = value; }
        }

        /// <summary>
        /// Gets or sets the method return type.
        /// </summary>
        public string ReturnType
        {
            get { return _methodReturnType; }
            set { _methodReturnType = value; }
        }

        /// <summary>
        /// AddMethodParameter adds a parameter to the Parameters collection.
        /// </summary>
        /// <param name="parameter">parameter of type MethodParameter.</param>
        public void AddMethodParameter(MethodParameter parameter)
        {
            Parameters.Add(parameter);
        }

        /// <summary>
        /// AddMethodParameter adds a parameter with specifiec type and name to the Parameters collection.
        /// </summary>
        /// <param name="type">MethodParameter type.</param>
        /// <param name="name">MethodParameter name.</param>
        /// <param name="description">MethodParameter description.</param>
        public void AddMethodParameter(string type, string name, string description)
        {
            AddMethodParameter(new MethodParameter(type, name, description));
        }

        /// <summary>
        /// ParameterCount gets the number of parameters in Parameters.
        /// </summary>
        /// <returns>Parameters.Count().</returns>
        public int ParameterCount()
        {
            return Parameters.Count();
        }

        /// <summary>
        /// Overrides the ToString method and formats the string output.
        /// </summary>
        /// <returns>A desired format for the Method description, parameters and return type.</returns>
        public override string ToString()
        {
            string s = Name + @":" + "\n" + Description;

            foreach (MethodParameter parameter in Parameters)
            {
                s += parameter.ToString() + @", ";
            }

            if (ReturnType != string.Empty && ReturnType != null)
            {
                s += "\n" + "Return Type: " + ReturnType;
            }

            return s;
        }
    }
}