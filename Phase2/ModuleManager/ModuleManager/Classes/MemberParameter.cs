namespace ModuleManager.Classes
{
    /// <summary>
    /// An object designed to hold the type and name of a member's parameter.
    /// </summary>
    public class MemberParameter
    {
        private string _type;
        private string _name;
        private string _description;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberParameter"/> class
        /// that sets both name and type to empty strings.
        /// </summary>
        public MemberParameter()
        {
            _type = string.Empty;
            _name = string.Empty;
            _description = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberParameter"/> class
        /// that defines the parameter type and name through parameters.
        /// </summary>
        /// <param name="type">MemberParameter type.</param>
        /// <param name="name">MemberParameter name.</param>
        /// <param name="description">MemberParameter description.</param>
        public MemberParameter(string type, string name, string description)
        {
            _type = type;
            _name = name;
            _description = description;
        }

        /// <summary>
        /// Gets or sets the parameter type.
        /// </summary>
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// Gets or sets the parameter name.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the parameter description.
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// Overrides the ToString method and formats the string output of parameter type and name.
        /// </summary>
        /// <returns>A desired format for the parameter type and name.</returns>
        public override string ToString()
        {
            string s = Type + @" " + Name + "\n";

            if (!string.IsNullOrEmpty(Description))
            {
                s += Description + "\n";
            }

            return s;
        }
    }
}