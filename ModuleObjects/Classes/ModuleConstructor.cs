namespace ModuleManager.ModuleObjects.Classes
{
    using System.Collections.ObjectModel;
    using System.Reflection;
    using System.Xml.Serialization;

    /// <summary>
    /// Holds the <see cref="ConstructorInfo"/>, class name, description and the parameters a constructor.
    /// </summary>
    public class ModuleConstructor : ModuleMemberData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleConstructor"/> class.
        /// </summary>
        public ModuleConstructor()
            : this(null, string.Empty, string.Empty, new ObservableCollection<MemberParameter>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleConstructor"/> class.
        /// </summary>
        /// <param name="constructorInfo"><see cref="ConstructorInfo"/> for this <see cref="ModuleConstructor"/>.</param>
        /// <param name="className">Class name.</param>
        /// <param name="description">Constructor description.</param>
        /// <param name="parameters">A <see cref="ObservableCollection{MemberParameter}"/>, constructor parameters.</param>
        public ModuleConstructor(ConstructorInfo constructorInfo, string className, string description, ObservableCollection<MemberParameter> parameters)
        {
            ConstructorInfo = constructorInfo;

            if (string.IsNullOrEmpty(className))
            {
                Name = className;
            }
            else
            {
                Name = @"Constructor For " + className;
            }

            Description = description;
            Parameters = parameters;
            TypeName = GetType().Name;
        }

        /// <summary>
        /// Gets or sets an <see cref="ObservableCollection{MemberParameter}"/>, the constructor parameters.
        /// </summary>
        public ObservableCollection<MemberParameter> Parameters { get; set; }

        /// <summary>
        /// Gets or sets the actuall <see cref="ConstructorInfo"/>.
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