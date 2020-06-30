namespace ModuleManager.Classes
{
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// ModuleMember object holds the description, the parameters and the return type of a member.
    /// </summary>
    public class ModuleMember
    {
        private string _name;
        private string _description;
        private ObservableCollection<MemberParameter> _parameters;
        private string _returnType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleMember"/> class.
        /// </summary>
        public ModuleMember()
        {
            _name = string.Empty;
            _description = string.Empty;
            _parameters = new ObservableCollection<MemberParameter>();
            _returnType = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleMember"/> class
        /// with specified name, description, parameters and type through passed in parameters.
        /// </summary>
        /// <param name="name">Member name.</param>
        /// <param name="description">Member description.</param>
        /// <param name="parameters">Member parameters.</param>
        /// <param name="returnType">Member return type.</param>
        public ModuleMember(string name, string description, ObservableCollection<MemberParameter> parameters, string returnType)
        {
            Name = name;
            Description = description;
            Parameters = parameters;
            ReturnType = returnType;
        }

        /// <summary>
        /// Gets or sets the name of the member.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the description of the member.
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// Gets or sets the member parameters.
        /// </summary>
        public ObservableCollection<MemberParameter> Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        /// <summary>
        /// Gets or sets the member return type.
        /// </summary>
        public string ReturnType
        {
            get { return _returnType; }
            set { _returnType = value; }
        }

        /// <summary>
        /// AddMemberParameter adds a parameter to the Parameters collection.
        /// </summary>
        /// <param name="parameter">parameter of type MemberParameter.</param>
        public void AddMemberParameter(MemberParameter parameter)
        {
            Parameters.Add(parameter);
        }

        /// <summary>
        /// AddMemberParameter adds a parameter with specifiec type and name to the Parameters collection.
        /// </summary>
        /// <param name="type">MemberParameter type.</param>
        /// <param name="name">MemberParameter name.</param>
        /// <param name="description">MemberParameter description.</param>
        public void AddMemberParameter(string type, string name, string description)
        {
            AddMemberParameter(new MemberParameter(type, name, description));
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
        /// <returns>A desired format for the member description, parameters and return type.</returns>
        public override string ToString()
        {
            string s = Name + @":" + "\n" + Description;

            foreach (MemberParameter parameter in Parameters)
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