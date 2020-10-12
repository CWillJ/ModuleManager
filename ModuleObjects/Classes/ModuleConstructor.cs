namespace ModuleManager.ModuleObjects.Classes
{
    using System.Collections.ObjectModel;
    using System.Reflection;
    using System.Xml.Serialization;

    /// <summary>
    /// ModuleConstructor object holds the class name, description and the parameters a constructor.
    /// </summary>
    public class ModuleConstructor : ModuleMember
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleConstructor"/> class. Default constructor.
        /// </summary>
        public ModuleConstructor()
        {
            ConstructorInfo = null;
            Name = string.Empty;
            Description = string.Empty;
            Parameters = new ObservableCollection<MemberParameter>();
            TypeName = GetType().Name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleConstructor"/> class
        /// with specified name, description, and parameters.
        /// </summary>
        /// <param name="constructorInfo">ConstructorInfo for this ModuleConstructor.</param>
        /// <param name="className">Class name.</param>
        /// <param name="description">Constructor description.</param>
        /// <param name="parameters">Constructor parameters.</param>
        public ModuleConstructor(ConstructorInfo constructorInfo, string className, string description, ObservableCollection<MemberParameter> parameters)
        {
            ConstructorInfo = constructorInfo;
            Name = @"Constructor For " + className;
            Description = description;
            Parameters = parameters;
            TypeName = GetType().Name;
        }

        /// <summary>
        /// Gets or sets the member parameters.
        /// </summary>
        public ObservableCollection<MemberParameter> Parameters { get; set; }

        /// <summary>
        /// Gets or sets the actuall ConstructorInfo for this ModuleConstructor.
        /// </summary>
        [XmlIgnore]
        public ConstructorInfo ConstructorInfo { get; set; }

        /// <summary>
        /// Overrides the ToString method and formats the string output.
        /// </summary>
        /// <returns>A desired format for the constructor name, description, and parameters.</returns>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(Description) && Parameters.Count == 0)
            {
                return string.Empty;
            }

            string s = Name + "\n";

            if (!string.IsNullOrEmpty(Description))
            {
                s += Description + "\n";
            }

            s += "\n";

            if (Parameters == null)
            {
                s += @"Parameters: None" + "\n";
            }
            else if (Parameters.Count == 0)
            {
                s += @"Parameters: None" + "\n";
            }
            else
            {
                s += @"Parameters:" + "\n";

                foreach (MemberParameter parameter in Parameters)
                {
                    s += parameter.ToString();
                }
            }

            return s;
        }
    }
}