namespace ModuleManager.Classes
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;

    /// <summary>
    /// Module object holds the name and description of a module.
    /// </summary>
    public class Module : INotifyPropertyChanged
    {
        private bool _isSelected;
        private bool _enabled;
        private string _name;
        private string _description;
        private ObservableCollection<ModuleMember> _members;

        /// <summary>
        /// Initializes a new instance of the <see cref="Module"/> class
        /// initializes properties to empty strings/empty collections.
        /// </summary>
        public Module()
        {
            _isSelected = false;
            _enabled = false;
            _name = string.Empty;
            _description = string.Empty;
            _members = new ObservableCollection<ModuleMember>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Module"/> class
        /// initializes a Module specifying the name, description and methods.
        /// </summary>
        /// <param name="name">Module name.</param>
        /// <param name="description">Module description.</param>
        /// <param name="members">Module methods.</param>
        public Module(string name, string description, ObservableCollection<ModuleMember> members)
        {
            _isSelected = false;
            _enabled = false;
            _name = name;
            _description = description;
            _members = members;
            RaisePropertyChanged("Modules");
        }

        /// <summary>
        /// The event handler that handles a property change.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets a value indicating whether the member is selected.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }

            set
            {
                _isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the module is enabled or disabled.
        /// </summary>
        public bool Enabled
        {
            get
            {
                return _enabled;
            }

            set
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    RaisePropertyChanged("Enabled");
                }
            }
        }

        /// <summary>
        /// Gets or sets the module name.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                if (_name != value)
                {
                    _name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        /// <summary>
        /// Gets or sets the description of the module.
        /// </summary>
        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                if (_description != value)
                {
                    _description = value;
                    RaisePropertyChanged("Description");
                }
            }
        }

        /// <summary>
        /// Gets or sets the members in the current module.
        /// </summary>
        public ObservableCollection<ModuleMember> Members
        {
            get
            {
                return _members;
            }

            set
            {
                if (_members != value)
                {
                    _members = value;
                    RaisePropertyChanged("Members");
                }
            }
        }

        /// <summary>
        /// Raise a property changed event.
        /// </summary>
        /// <param name="property">Property passed in as a string.</param>
        public void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        /// <summary>
        /// AddMember adds a member to the current module.
        /// </summary>
        /// <param name="member">Specifies module member.</param>
        public void AddMember(ModuleMember member)
        {
            Members.Add(member);
        }

        /// <summary>
        /// AddMember adds a member to the current module with the specified properties.
        /// </summary>
        /// <param name="name">Member name.</param>
        /// <param name="description">Member description.</param>
        /// <param name="parameters">Member parameters.</param>
        /// <param name="returnType">Member return type.</param>
        /// <param name="returnDescription">Member return description.</param>
        public void AddMember(string name, string description, ObservableCollection<MemberParameter> parameters, string returnType, string returnDescription)
        {
            AddMember(new ModuleMember(name, description, parameters, returnType, returnDescription));
        }

        /// <summary>
        /// MemberCount gets the number of members in Members.
        /// </summary>
        /// <returns>Members.Count().</returns>
        public int MemberCount()
        {
            return Members.Count();
        }

        /// <summary>
        /// Overrides the ToString method and formats the string output.
        /// </summary>
        /// <returns>A desired format for the module name, description and all members contained in module.</returns>
        public override string ToString()
        {
            string s = string.Empty;

            //// s += Enabled.ToString() + "\n";
            s += Name + "\n";

            if (!string.IsNullOrEmpty(Description))
            {
                s += Description + "\n\n";
            }

            foreach (var member in Members)
            {
                s += member.ToString() + "\n";
            }

            return s;
        }
    }
}
