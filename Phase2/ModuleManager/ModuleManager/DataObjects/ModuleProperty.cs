namespace ModuleManager.DataObjects
{
    /// <summary>
    /// ModuleProperty object holds the name, description, data type and accessor level of a property.
    /// </summary>
    public class ModuleProperty : ModuleMember
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleProperty"/> class. Default constructor.
        /// </summary>
        public ModuleProperty()
        {
            Name = string.Empty;
            Description = string.Empty;
            DataType = string.Empty;
            CanRead = false;
            CanWrite = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleProperty"/> class
        /// with specified name, description, type and accessors
        /// </summary>
        /// <param name="name">Property name.</param>
        /// <param name="description">Property description.</param>
        /// <param name="dataType">Property data type.</param>
        /// <param name="canRead">Property has a 'get' accessor.</param>
        /// <param name="canWrite">Property has a 'set' accessor.</param>
        public ModuleProperty(string name, string description, string dataType, bool canRead, bool canWrite)
        {
            Name = name;
            Description = description;
            DataType = dataType;
            CanRead = canRead;
            CanWrite = canWrite;
        }

        /// <summary>
        /// Gets or sets the type of the property.
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this property has a get accessor.
        /// </summary>
        public bool CanRead { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this property has a set accessor.
        /// </summary>
        public bool CanWrite { get; set; }

        /// <summary>
        /// Overrides the ToString method and formats the string output.
        /// </summary>
        /// <returns>A desired format for the member description.</returns>
        public override string ToString()
        {
            string s = Name + "\n";

            if (!string.IsNullOrEmpty(Description))
            {
                s += Description + "\n";
            }

            s += @"Can Read: " + CanRead.ToString() + "\n";
            s += @"Can Write: " + CanWrite.ToString() + "\n";

            return s;
        }
    }
}