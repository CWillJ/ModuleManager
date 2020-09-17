namespace ModuleObjects.Classes
{
    using System;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;
    using ModuleObjects.Interfaces;

    /// <summary>
    /// Module object holds the name and description of a module.
    /// </summary>
    public class Module : IModule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Module"/> class. Default constructor.
        /// initializes properties to empty strings/empty collections.
        /// </summary>
        public Module()
        {
            Type = null;
            Name = string.Empty;
            Description = string.Empty;

            IsEnabled = false;
            Members = new ObservableCollection<ModuleMember>();
            Constructors = new ObservableCollection<ModuleConstructor>();
            Properties = new ObservableCollection<ModuleProperty>();
            Methods = new ObservableCollection<ModuleMethod>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Module"/> class specifying the name,
        /// description and methods.
        /// </summary>
        /// <param name="type">Module Type.</param>
        /// <param name="name">Module name.</param>
        /// <param name="description">Module description.</param>
        /// <param name="constructors">Module constructors.</param>
        /// <param name="properties">Module properties.</param>
        /// <param name="methods">Module methods.</param>
        public Module(
            Type type,
            string name,
            string description,
            ObservableCollection<ModuleConstructor> constructors,
            ObservableCollection<ModuleProperty> properties,
            ObservableCollection<ModuleMethod> methods)
        {
            Type = type;
            IsEnabled = false;
            Name = name;
            Description = description;
            Members = new ObservableCollection<ModuleMember>();
            Constructors = new ObservableCollection<ModuleConstructor>();
            Properties = properties;
            Methods = methods;

            foreach (var constructor in constructors)
            {
                if (!string.IsNullOrEmpty(constructor.Description) ||
                    !constructor.Parameters[0].IsEmpty())
                {
                    Constructors.Add(constructor);
                    Members.Add(constructor);
                }
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
        /// Initializes a new instance of the <see cref="Module"/> class specifing a Type.
        /// </summary>
        /// <param name="type">Type object found in an Assembly.</param>
        /// <param name="dllFileName">File name of the dll file.</param>
        public Module(Type type, string dllFileName)
        {
            Type = type;
            IsEnabled = false;
            Name = type.Name;
            Description = dllFileName;
            Methods = new ObservableCollection<ModuleMethod>();
        }

        /// <summary>
        /// Gets or sets the Type of the module.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Gets or sets the module name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the module.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the module is enabled or disabled.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Gets or sets all of the module members.
        /// </summary>
        public ObservableCollection<ModuleMember> Members { get; set; }

        /// <summary>
        /// Gets or sets the constructors in the current module.
        /// </summary>
        public ObservableCollection<ModuleConstructor> Constructors { get; set; }

        /// <summary>
        /// Gets or sets the properties in the current module.
        /// </summary>
        public ObservableCollection<ModuleProperty> Properties { get; set; }

        /// <summary>
        /// Gets or sets the members in the current module.
        /// </summary>
        public ObservableCollection<ModuleMethod> Methods { get; set; }

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
        /// Implemented from ISerializable.
        /// </summary>
        /// <param name="info">Info to serialize.</param>
        /// <param name="context">Context to serialize.</param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name);
            info.AddValue("Description", Description);
        }
    }
}