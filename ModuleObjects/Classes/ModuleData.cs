namespace ModuleManager.ModuleObjects.Classes
{
    using System;
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;
    using ModuleManager.ModuleObjects.Interfaces;

    /// <summary>
    /// Module object holds the name, description, members, methods, constructors and properties of a module.
    /// </summary>
    public class ModuleData : IModuleData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleData"/> class.
        /// </summary>
        public ModuleData()
            : this(null, string.Empty, string.Empty, new ObservableCollection<ModuleConstructor>(), new ObservableCollection<ModuleProperty>(), new ObservableCollection<ModuleMethod>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleData"/> class specifying the name,
        /// description and methods.
        /// </summary>
        /// <param name="type">Module type.</param>
        /// <param name="name">Module name.</param>
        /// <param name="description">Module description.</param>
        /// <param name="constructors">A collection of module constructors.</param>
        /// <param name="properties">A collection of module properties.</param>
        /// <param name="methods">A collection of module methods.</param>
        public ModuleData(
            Type type,
            string name,
            string description,
            ObservableCollection<ModuleConstructor> constructors,
            ObservableCollection<ModuleProperty> properties,
            ObservableCollection<ModuleMethod> methods)
        {
            Type = type;
            Name = name;
            FullName = type.FullName;
            Description = description;

            InitializeMembers(constructors, properties, methods);
        }

        /// <summary>
        /// Gets or sets the module name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets this Module's Type's FullName.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the description of the module.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets all of the module members.
        /// </summary>
        public ObservableCollection<ModuleMemberData> Members { get; set; }

        /// <summary>
        /// Gets or sets the module constructors.
        /// </summary>
        public ObservableCollection<ModuleConstructor> Constructors { get; set; }

        /// <summary>
        /// Gets or sets the module properties.
        /// </summary>
        public ObservableCollection<ModuleProperty> Properties { get; set; }

        /// <summary>
        /// Gets or sets the module methods.
        /// </summary>
        public ObservableCollection<ModuleMethod> Methods { get; set; }

        /// <summary>
        /// Gets or sets the actual Type of the ModuleData.
        /// </summary>
        [XmlIgnore]
        public Type Type { get; set; }

        /// <summary>
        /// Store module constructors, properties and methods in the Members property.
        /// </summary>
        public void StoreModuleMembers()
        {
            Members.Clear();

            foreach (var constructor in Constructors)
            {
                Members.Add(constructor);
            }

            foreach (var property in Properties)
            {
                Members.Add(property);
            }

            foreach (var method in Methods)
            {
                Members.Add(method);
            }
        }

        /// <summary>
        /// Overrides the ToString method and formats the string output.
        /// </summary>
        /// <returns>A desired format for the module name, description and all members contained in module.</returns>
        public override string ToString()
        {
            string s = Name + "\n";

            if (!string.IsNullOrEmpty(Description))
            {
                s += Description + "\n\n";
            }

            foreach (var constructor in Constructors)
            {
                s += constructor.ToString() + "\n";
            }

            foreach (var property in Properties)
            {
                s += property.ToString() + "\n";
            }

            foreach (var method in Methods)
            {
                s += method.ToString() + "\n";
            }

            return s;
        }

        /// <summary>
        /// This will add <see cref="ObservableCollection{ModuleConstructor}"/> to the Constructors and Members properties,
        /// <see cref="ObservableCollection{ModuleProperty}"/> to the Properties and Members properties, and
        /// <see cref="ObservableCollection{ModuleMethod}"/> to the Methods and Members properties.
        /// </summary>
        /// <param name="constructors">A <see cref="ObservableCollection{ModuleConstructor}"/>.</param>
        /// <param name="properties">A <see cref="ObservableCollection{ModuleProperty}"/>.</param>
        /// <param name="methods">A <see cref="ObservableCollection{ModuleMethod}"/>.</param>
        private void InitializeMembers(ObservableCollection<ModuleConstructor> constructors, ObservableCollection<ModuleProperty> properties, ObservableCollection<ModuleMethod> methods)
        {
            if (Members == null)
            {
                Members = new ObservableCollection<ModuleMemberData>();
            }

            InitializeConstructors(constructors);
            InitializeProperties(properties);
            InitializeMethods(methods);
        }

        /// <summary>
        /// Adds a <see cref="ObservableCollection{ModuleConstructor}"/> to the Constructors and Members properties.
        /// </summary>
        /// <param name="constructors">A <see cref="ObservableCollection{ModuleConstructor}"/>.</param>
        private void InitializeConstructors(ObservableCollection<ModuleConstructor> constructors)
        {
            Constructors = new ObservableCollection<ModuleConstructor>();

            if (constructors != null)
            {
                foreach (var constructor in constructors)
                {
                    if (!string.IsNullOrEmpty(constructor.Description) ||
                        !constructor.Parameters[0].IsEmpty())
                    {
                        Constructors.Add(constructor);
                        Members.Add(constructor);
                    }
                }
            }
        }

        /// <summary>
        /// Adds a <see cref="ObservableCollection{ModuleProperty}"/> to the Properties and Members properties.
        /// </summary>
        /// <param name="properties">A <see cref="ObservableCollection{ModuleProperty}"/>.</param>
        private void InitializeProperties(ObservableCollection<ModuleProperty> properties)
        {
            Properties = new ObservableCollection<ModuleProperty>();

            if (properties != null)
            {
                Properties = properties;

                foreach (var property in Properties)
                {
                    Members.Add(property);
                }
            }
        }

        /// <summary>
        /// Adds a <see cref="ObservableCollection{ModuleMethod}"/> to the Methods and Members properties.
        /// </summary>
        /// <param name="methods">A <see cref="ObservableCollection{ModuleMethod}"/>.</param>
        private void InitializeMethods(ObservableCollection<ModuleMethod> methods)
        {
            Methods = new ObservableCollection<ModuleMethod>();

            if (methods != null)
            {
                Methods = methods;

                foreach (var method in Methods)
                {
                    Members.Add(method);
                }
            }
        }
    }
}