namespace ModuleManager
{
    /// <summary>
    /// The base class for ModuleConstructor, ModuleProperty, and ModuleMethod.
    /// </summary>
    public class ModuleMember
    {
        private string _name;
        private string _description;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleMember"/> class.
        /// </summary>
        public ModuleMember()
        {
            _name = string.Empty;
            _description = string.Empty;
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
    }
}
