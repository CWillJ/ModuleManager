namespace ModuleManager.Classes
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Security.Cryptography;

    /// <summary>
    /// Module object holds the name and description of a module.
    /// </summary>
    public class Module : INotifyPropertyChanged
    {
        private string _name;
        private string _description;
        private string _membersString;
        private ObservableCollection<ModuleMember> _members;

        /// <summary>
        /// Initializes a new instance of the <see cref="Module"/> class
        /// initializes properties to empty strings/empty collections.
        /// </summary>
        public Module()
        {
            _name = string.Empty;
            _description = string.Empty;
            _membersString = string.Empty;
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
            _name = name;
            _description = description;
            _membersString = MembersToString(members);
            _members = members;
            RaisePropertyChanged("Modules");
        }

        /// <summary>
        /// The event handler that handles a property change.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

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
        /// Gets or sets the members as a string.
        /// </summary>
        public string MembersString
        {
            get
            {
                return _membersString;
            }

            set
            {
                if (_membersString != value)
                {
                    _membersString = value;
                    RaisePropertyChanged("MembersString");
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
        public void AddMember(string name, string description, ObservableCollection<MemberParameter> parameters, string returnType)
        {
            AddMember(new ModuleMember(name, description, parameters, returnType));
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
            string s = Name + @":" + "\n";

            if (Description != null || Description != string.Empty)
            {
                s += Description + "\n\n";
            }

            s += MembersToString(Members);

            return s;
        }

        /// <summary>
        /// Returns a collection of ModuleMember objects as a string.
        /// </summary>
        /// <param name="members">A collection of ModuleMembers.</param>
        /// <returns>String of the ModuleMembers.</returns>
        public string MembersToString(ObservableCollection<ModuleMember> members)
        {
            string s = string.Empty;

            foreach (ModuleMember member in members)
            {
                s += member.ToString() + "\n";
            }

            return s;
        }
    }
}
