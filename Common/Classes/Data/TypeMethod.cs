namespace ModuleManager.Common.Classes
{
    using System;
    using System.Collections.ObjectModel;
    using System.Reflection;
    using Newtonsoft.Json;

    /// <summary>
    /// TypeMethod object holds the MethodInfo, name, description, the parameters and the return type of a member.
    /// </summary>
    public class TypeMethod : TypeMemberData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeMethod"/> class.
        /// </summary>
        public TypeMethod()
            : this(null, string.Empty, string.Empty, new ObservableCollection<MemberParameter>(), string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeMethod"/> class.
        /// </summary>
        /// <param name="methodInfo"><see cref="System.Reflection.MethodInfo"/>.</param>
        /// <param name="name"><see cref="string"/> method name.</param>
        /// <param name="description"><see cref="string"/> method description.</param>
        /// <param name="parameters"><see cref="ObservableCollection{MemberParameter}"/> method parameters.</param>
        /// <param name="returnType"><see cref="string"/> method return type.</param>
        /// <param name="returnDescription"><see cref="string"/> method return description.</param>
        public TypeMethod(MethodInfo methodInfo, string name, string description, ObservableCollection<MemberParameter> parameters, string returnType, string returnDescription)
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
        /// Gets or sets the <see cref="ObservableCollection{MemberParameter}"/>s.
        /// </summary>
        public ObservableCollection<MemberParameter> Parameters { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="string"/> method return type.
        /// </summary>
        public string ReturnType { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="string"/> method return description.
        /// </summary>
        public string ReturnDescription { get; set; }

        /// <summary>
        /// Gets or sets the actual <see cref="System.Reflection.MethodInfo"/> of the TypeMethod.
        /// </summary>
        [JsonIgnore]
        public MethodInfo MethodInfo { get; set; }

        /// <summary>
        /// Invokes this method.
        /// </summary>
        /// <param name="args">The <see cref="object"/> array arguments needed to invoke this method.</param>
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
        /// Overrides the ToString method and formats the string output.
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
        /// Used to test if the passed in <see cref="object"/> array matches this method's parameter types.
        /// </summary>
        /// <param name="args">An array of <see cref="object"/>s that represent method parameters.</param>
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
