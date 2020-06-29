namespace ModuleManager.Classes
{
    /// <summary>
    /// An object designed to hold the type and name of a method's parameter.
    /// </summary>
    public class MethodParameter
    {
        private string _parameterType;
        private string _parameterName;
        private string _parameterDescription;

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodParameter"/> class
        /// that sets both name and type to empty strings.
        /// </summary>
        public MethodParameter()
        {
            Type = string.Empty;
            Name = string.Empty;
            Description = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodParameter"/> class
        /// that defines the parameter type and name through parameters.
        /// </summary>
        /// <param name="type">MethodParameter type.</param>
        /// <param name="name">MethodParameter name.</param>
        /// <param name="description">MethodParameter description.</param>
        public MethodParameter(string type, string name, string description)
        {
            Type = type;
            Name = name;
            Description = description;
        }

        /// <summary>
        /// Gets or sets the parameter type.
        /// </summary>
        public string Type
        {
            get { return _parameterType; }
            set { _parameterType = value; }
        }

        /// <summary>
        /// Gets or sets the parameter name.
        /// </summary>
        public string Name
        {
            get { return _parameterName; }
            set { _parameterName = value; }
        }

        /// <summary>
        /// Gets or sets the parameter description.
        /// </summary>
        public string Description
        {
            get { return _parameterDescription; }
            set { _parameterDescription = value; }
        }

        /// <summary>
        /// Overrides the ToString method and formats the string output of parameter type and name.
        /// </summary>
        /// <returns>A desired format for the parameter type and name.</returns>
        public override string ToString()
        {
            return Type + @" " + Name + "\n" + Description;
        }
    }
}