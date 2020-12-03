namespace ModuleManager.ModuleObjects.Classes
{
    using System.Reflection;
    using System.Xml.Serialization;

    /// <summary>
    /// TypeProperty object holds the PropertyInfo, name, description, data type and accessor level of a property.
    /// </summary>
    public class TypeProperty : TypeMemberData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeProperty"/> class.
        /// </summary>
        public TypeProperty()
            : this(null, string.Empty, string.Empty, string.Empty, false, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeProperty"/> class.
        /// </summary>
        /// <param name="propertyInfo"><see cref="System.Reflection.PropertyInfo"/> for this.</param>
        /// <param name="name"><see cref="string"/> property name.</param>
        /// <param name="description"><see cref="string"/> property description.</param>
        /// <param name="dataType"><see cref="string"/> property data type.</param>
        /// <param name="canRead"><see cref="string"/> property has a 'get' accessor.</param>
        /// <param name="canWrite"><see cref="string"/> property has a 'set' accessor.</param>
        public TypeProperty(PropertyInfo propertyInfo, string name, string description, string dataType, bool canRead, bool canWrite)
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
        /// Gets or sets the <see cref="string"/> type of the property.
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
        /// Gets or sets the actual <see cref="System.Reflection.PropertyInfo"/> for this.
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