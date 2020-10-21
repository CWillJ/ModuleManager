namespace ModuleManager.ModuleObjects.Classes
{
    using System.Reflection;
    using System.Xml.Serialization;

    /// <summary>
    /// ModuleProperty object holds the PropertyInfo, name, description, data type and accessor level of a property.
    /// </summary>
    public class ModuleProperty : ModuleMember
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleProperty"/> class.
        /// </summary>
        public ModuleProperty()
        {
            PropertyInfo = null;
            Name = string.Empty;
            Description = string.Empty;
            DataType = string.Empty;
            CanRead = false;
            CanWrite = false;
            TypeName = GetType().Name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleProperty"/> class
        /// </summary>
        /// <param name="propertyInfo">PropertyInfo for this ModuleProperty.</param>
        /// <param name="name">Property name.</param>
        /// <param name="description">Property description.</param>
        /// <param name="dataType">Property data type.</param>
        /// <param name="canRead">Property has a 'get' accessor.</param>
        /// <param name="canWrite">Property has a 'set' accessor.</param>
        public ModuleProperty(PropertyInfo propertyInfo, string name, string description, string dataType, bool canRead, bool canWrite)
        {
            PropertyInfo = propertyInfo;
            Name = name;
            Description = description;
            DataType = dataType;
            CanRead = canRead;
            CanWrite = canWrite;
            TypeName = GetType().Name;
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
        /// Gets or sets the actual PropertyInfo for this ModuleProperty.
        /// </summary>
        [XmlIgnore]
        public PropertyInfo PropertyInfo { get; set; }

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