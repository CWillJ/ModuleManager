namespace ModuleManager.ModuleObjects.Classes
{
    using System;
    using System.Collections.ObjectModel;
    using System.Reflection;
    using System.Xml.Serialization;

    /// <summary>
    /// ModuleMethod object holds the MethodInfo, name, description, the parameters and the return type of a member.
    /// </summary>
    public class ModuleMethod : ModuleMemberData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleMethod"/> class.
        /// </summary>
        public ModuleMethod()
            : this(null, string.Empty, string.Empty, new ObservableCollection<MemberParameter>(), string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleMethod"/> class.
        /// </summary>
        /// <param name="methodInfo"><see cref="MethodInfo"/>.</param>
        /// <param name="name">Method name.</param>
        /// <param name="description">Method description.</param>
        /// <param name="parameters">Method parameters.</param>
        /// <param name="returnType">Method return type.</param>
        /// <param name="returnDescription">Method return description.</param>
        public ModuleMethod(MethodInfo methodInfo, string name, string description, ObservableCollection<MemberParameter> parameters, string returnType, string returnDescription)
        {
            MethodInfo = methodInfo;
            Name = name;
            Description = description;
            Parameters = parameters;

            if (string.IsNullOrEmpty(returnType))
            {
                ReturnType = @"Void";
            }
            else
            {
                ReturnType = returnType;
            }

            ReturnDescription = returnDescription;

            TypeName = GetType().Name;
        }

        /// <summary>
        /// Gets or sets the method parameters.
        /// </summary>
        public ObservableCollection<MemberParameter> Parameters { get; set; }

        /// <summary>
        /// Gets or sets the method return type.
        /// </summary>
        public string ReturnType { get; set; }

        /// <summary>
        /// Gets or sets the method return description.
        /// </summary>
        public string ReturnDescription { get; set; }

        /// <summary>
        /// Gets or sets the actual MethodInfo of the ModuleMethod.
        /// </summary>
        [XmlIgnore]
        public MethodInfo MethodInfo { get; set; }

        /// <summary>
        /// Invokes this method.
        /// </summary>
        /// <param name="args">The arguments needed to invoke this method.</param>
        /// <returns>An object that this method should return.</returns>
        public object Invoke(object[] args)
        {
            if (!TestParameters(args))
            {
                return null;
            }

            Type moduleType = MethodInfo.DeclaringType;
            var typeInstance = Activator.CreateInstance(moduleType);

            return MethodInfo.Invoke(typeInstance, args);
        }

        /// <summary>
        /// Overrides the ToString method and formats the string output
        /// for the UI.
        /// </summary>
        /// <returns>A desired format for the member description, parameters
        /// and return type.</returns>
        public override string ToString()
        {
            string s = Name + "\n";

            if (!string.IsNullOrEmpty(Description))
            {
                s += Description + "\n";
            }

            s += "\n";

            if (Parameters == null)
            {
                s += @"Parameters: None" + "\n\n";
            }
            else if (Parameters.Count == 0)
            {
                s += @"Parameters: None" + "\n\n";
            }
            else
            {
                s += @"Parameters:" + "\n";

                foreach (MemberParameter parameter in Parameters)
                {
                    s += parameter.ToString();
                }

                s += "\n";
            }

            if (string.IsNullOrEmpty(ReturnType))
            {
                ReturnType = @"Void";
            }

            s += @"Return:" + "\n" + ReturnType + "\n";

            if (!string.IsNullOrEmpty(ReturnDescription))
            {
                s += ReturnDescription + "\n";
            }

            return s;
        }

        /// <summary>
        /// Used to test if the passed in object array matches this method's parameter types.
        /// </summary>
        /// <param name="args">An array of objects that represent method parameters.</param>
        /// <returns>True if the object array matches the method's parameter types.</returns>
        private bool TestParameters(object[] args)
        {
            if (args.Length != Parameters.Count)
            {
                return false;
            }

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].GetType() != Parameters[i].Type)
                {
                    return false;
                }
            }

            return true;
        }
    }
}