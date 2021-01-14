namespace ModuleManager.Common.Classes
{
    using System;
    using System.Collections.ObjectModel;
    using ModuleManager.Common.Classes.Data;
    using Newtonsoft.Json;

    /// <summary>
    /// Module type object.
    /// </summary>
    public class TypeData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeData"/> class.
        /// </summary>
        public TypeData()
            : this(null, string.Empty, string.Empty, new ObservableCollection<TypeConstructor>(), new ObservableCollection<TypeProperty>(), new ObservableCollection<TypeMethod>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeData"/> class.
        /// </summary>
        /// <param name="type">Module <see cref="System.Type"/>.</param>
        /// <param name="name"><see cref="string"/> Module name.</param>
        /// <param name="description"><see cref="string"/> Module description.</param>
        /// <param name="constructors">An <see cref="ObservableCollection{TypeConstructor}"/> of type constructors.</param>
        /// <param name="properties">An <see cref="ObservableCollection{TypeProperty}"/> of type properties.</param>
        /// <param name="methods">An <see cref="ObservableCollection{TypeMethod}"/> of type methods.</param>
        public TypeData(
            Type type,
            string name,
            string description,
            ObservableCollection<TypeConstructor> constructors,
            ObservableCollection<TypeProperty> properties,
            ObservableCollection<TypeMethod> methods)
        {
            Type = type;
            Name = name;

            if (type == null)
            {
                FullName = string.Empty;
            }
            else
            {
                FullName = type.FullName;
            }

            Description = description;

            InitializeMembers(constructors, properties, methods);
            SetIsView();
        }

        /// <summary>
        /// Gets or sets the <see cref="string"/> name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets this Module's Type's FullName <see cref="string"/>.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="string"/> description of the module.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets an <see cref="ObservableCollection{TypeMemberData}"/> of the module members.
        /// </summary>
        public ObservableCollection<TypeMemberData> Members { get; set; }

        /// <summary>
        /// Gets or sets an <see cref="ObservableCollection{TypeConstructor}"/> containing the type constructors.
        /// </summary>
        public ObservableCollection<TypeConstructor> Constructors { get; set; }

        /// <summary>
        /// Gets or sets an <see cref="ObservableCollection{TypeProperty}"/> containing the type properties.
        /// </summary>
        public ObservableCollection<TypeProperty> Properties { get; set; }

        /// <summary>
        /// Gets or sets an <see cref="ObservableCollection{TypeMethod}"/> containing the type methods.
        /// </summary>
        public ObservableCollection<TypeMethod> Methods { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="ViewData"/> object that containts information about this view type.
        /// </summary>
        public ViewData ViewInfo { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="System.Type"/> of the TypeData.
        /// </summary>
        [JsonIgnore]
        public Type Type { get; set; }

        /// <summary>
        /// Stores all <see cref="TypeConstructor"/>, <see cref="TypeProperty"/> and <see cref="TypeMethod"/> in the Members property.
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
        /// Sets the IsView property.
        /// </summary>
        private void SetIsView()
        {
            // (Type.GetProperty("Tag") != null)
            if (Type != null && Type.BaseType != null && (Type.BaseType.Name == @"UserControl" || Type.BaseType.Name == @"RadWindow"))
            {
                ViewInfo = new ViewData();
            }
            else
            {
                ViewInfo = null;
            }
        }

        /// <summary>
        /// This will add <see cref="ObservableCollection{TypeConstructor}"/> to the Constructors and Members properties,
        /// <see cref="ObservableCollection{TypeProperty}"/> to the Properties and Members properties, and
        /// <see cref="ObservableCollection{TypeMethod}"/> to the Methods and Members properties.
        /// </summary>
        /// <param name="constructors">An <see cref="ObservableCollection{TypeConstructor}"/>.</param>
        /// <param name="properties">An <see cref="ObservableCollection{TypeProperty}"/>.</param>
        /// <param name="methods">An <see cref="ObservableCollection{TypeMethod}"/>.</param>
        private void InitializeMembers(ObservableCollection<TypeConstructor> constructors, ObservableCollection<TypeProperty> properties, ObservableCollection<TypeMethod> methods)
        {
            if (Members == null)
            {
                Members = new ObservableCollection<TypeMemberData>();
            }

            InitializeConstructors(constructors);
            InitializeProperties(properties);
            InitializeMethods(methods);
        }

        /// <summary>
        /// Adds a <see cref="ObservableCollection{TypeConstructor}"/> to the Constructors and Members properties.
        /// </summary>
        /// <param name="constructors">A <see cref="ObservableCollection{TypeConstructor}"/>.</param>
        private void InitializeConstructors(ObservableCollection<TypeConstructor> constructors)
        {
            Constructors = new ObservableCollection<TypeConstructor>();

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
        /// Adds a <see cref="ObservableCollection{TypeProperty}"/> to the Properties and Members properties.
        /// </summary>
        /// <param name="properties">A <see cref="ObservableCollection{TypeProperty}"/>.</param>
        private void InitializeProperties(ObservableCollection<TypeProperty> properties)
        {
            Properties = new ObservableCollection<TypeProperty>();

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
        /// Adds a <see cref="ObservableCollection{TypeMethod}"/> to the Methods and Members properties.
        /// </summary>
        /// <param name="methods">An <see cref="ObservableCollection{TypeMethod}"/>.</param>
        private void InitializeMethods(ObservableCollection<TypeMethod> methods)
        {
            Methods = new ObservableCollection<TypeMethod>();

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