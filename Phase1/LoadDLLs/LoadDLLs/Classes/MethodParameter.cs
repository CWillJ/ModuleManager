namespace LoadDLLs.Classes
{
    using System.Security.Cryptography;

    /// <summary>
    /// An object designed to hold the type and name of a method's parameter.
    /// </summary>
    public class MethodParameter
    {
        private string parameterType;
        private string parameterName;
        private string parameterDescription;

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodParameter"/> class
        /// that sets both name and type to empty strings.
        /// </summary>
        public MethodParameter()
        {
            this.Type = string.Empty;
            this.Name = string.Empty;
            this.Description = string.Empty;
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
            this.Type = type;
            this.Name = name;
            this.Description = description;
        }

        /// <summary>
        /// Gets or sets the parameter type.
        /// </summary>
        public string Type
        {
            get { return this.parameterType; }
            set { this.parameterType = value; }
        }

        /// <summary>
        /// Gets or sets the parameter name.
        /// </summary>
        public string Name
        {
            get { return this.parameterName; }
            set { this.parameterName = value; }
        }

        /// <summary>
        /// Gets or sets the parameter description.
        /// </summary>
        public string Description
        {
            get { return this.parameterDescription; }
            set { this.parameterDescription = value; }
        }

        /// <summary>
        /// Overrides the ToString method and formats the string output of parameter type and name.
        /// </summary>
        /// <returns>A desired format for the parameter type and name.</returns>
        public override string ToString()
        {
            return this.Type + @" " + this.Name + "\n" + this.Description;
        }
    }
}
