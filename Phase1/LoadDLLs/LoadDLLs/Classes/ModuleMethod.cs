namespace LoadDLLs.Classes
{
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// ModuleMethod object holds the description, the parameters and the return type of a method.
    /// </summary>
    public class ModuleMethod
    {
        private string methodName;
        private string methodDescription;
        private ObservableCollection<MethodParameter> methodParameters;
        private string methodReturnType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleMethod"/> class.
        /// </summary>
        public ModuleMethod()
        {
            this.Name = string.Empty;
            this.Description = string.Empty;
            this.Parameters = new ObservableCollection<MethodParameter>();
            this.ReturnType = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleMethod"/> class
        /// with specified name, description, parameters and type through passed in parameters.
        /// </summary>
        /// <param name="name">Method name.</param>
        /// <param name="description">Method description.</param>
        /// <param name="parameters">Method parameters.</param>
        /// <param name="returnType">Method return type.</param>
        /// <param name="type">Method type.</param>
        public ModuleMethod(string name, string description, ObservableCollection<MethodParameter> parameters, string returnType)
        {
            this.Name = name;
            this.Description = description;
            this.Parameters = parameters;
            this.ReturnType = returnType;
        }

        /// <summary>
        /// Gets or sets the name of the method.
        /// </summary>
        public string Name
        {
            get { return this.methodName; }
            set { this.methodName = value; }
        }

        /// <summary>
        /// Gets or sets the description of the method.
        /// </summary>
        public string Description
        {
            get { return this.methodDescription; }
            set { this.methodDescription = value; }
        }

        /// <summary>
        /// Gets or sets the method parameters.
        /// </summary>
        public ObservableCollection<MethodParameter> Parameters
        {
            get { return this.methodParameters; }
            set { this.methodParameters = value; }
        }

        /// <summary>
        /// Gets or sets the method return type.
        /// </summary>
        public string ReturnType
        {
            get { return this.methodReturnType; }
            set { this.methodReturnType = value; }
        }

        /// <summary>
        /// AddMethodParameter adds a parameter to the Parameters collection.
        /// </summary>
        /// <param name="parameter">parameter of type MethodParameter.</param>
        public void AddMethodParameter(MethodParameter parameter)
        {
            this.Parameters.Add(parameter);
        }

        /// <summary>
        /// AddMethodParameter adds a parameter with specifiec type and name to the Parameters collection.
        /// </summary>
        /// <param name="type">MethodParameter type.</param>
        /// <param name="name">MethodParameter name.</param>
        /// <param name="description">MethodParameter description.</param>
        public void AddMethodParameter(string type, string name, string description)
        {
            this.AddMethodParameter(new MethodParameter(type, name, description));
        }

        /// <summary>
        /// ParameterCount gets the number of parameters in Parameters.
        /// </summary>
        /// <returns>Parameters.Count().</returns>
        public int ParameterCount()
        {
            return this.Parameters.Count();
        }

        /// <summary>
        /// Overrides the ToString method and formats the string output.
        /// </summary>
        /// <returns>A desired format for the Method description, parameters and return type.</returns>
        public override string ToString()
        {
            string s = this.Name + @":" + "\n" + this.Description;

            foreach (MethodParameter parameter in this.Parameters)
            {
                s += parameter.ToString() + @", ";
            }

            if (this.ReturnType != string.Empty && this.ReturnType != null)
            {
                s += "\n" + "Return Type: " + this.ReturnType;
            }

            return s;
        }
    }
}