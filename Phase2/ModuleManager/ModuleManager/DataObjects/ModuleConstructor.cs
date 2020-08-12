namespace ModuleManager.DataObjects
{
    using System.Collections.ObjectModel;

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
            Name = string.Empty;
            Description = string.Empty;
            Parameters = new ObservableCollection<MemberParameter>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleConstructor"/> class
        /// with specified name, description, and parameters.
        /// </summary>
        /// <param name="className">Class name.</param>
        /// <param name="description">Constructor description.</param>
        /// <param name="parameters">Constructor parameters.</param>
        public ModuleConstructor(string className, string description, ObservableCollection<MemberParameter> parameters)
        {
            Name = @"Constructor For " + className;
            Description = description;
            Parameters = parameters;
        }

        /// <summary>
        /// Gets or sets the member parameters.
        /// </summary>
        public ObservableCollection<MemberParameter> Parameters { get; set; }

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