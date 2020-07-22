﻿namespace ModuleManager.Classes
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// ModuleMember object holds the description, the parameters and the return type of a member.
    /// </summary>
    public class ModuleMember
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleMember"/> class. Default constructor.
        /// </summary>
        public ModuleMember()
        {
            Name = string.Empty;
            Description = string.Empty;
            Parameters = new ObservableCollection<MemberParameter>();
            ReturnType = @"void";
            ReturnDescription = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleMember"/> class
        /// with specified name, description, parameters return type and return
        /// description through passed in parameters.
        /// </summary>
        /// <param name="name">Member name.</param>
        /// <param name="description">Member description.</param>
        /// <param name="parameters">Member parameters.</param>
        /// <param name="returnType">Member return type.</param>
        /// <param name="returnDescription">Member return description.</param>
        public ModuleMember(string name, string description, ObservableCollection<MemberParameter> parameters, string returnType, string returnDescription)
        {
            Name = name;
            Description = description;
            Parameters = parameters;

            if (string.IsNullOrEmpty(returnType))
            {
                ReturnType = @"void";
            }
            else
            {
                ReturnType = returnType;
            }

            ReturnDescription = returnDescription;
        }

        /// <summary>
        /// Gets or sets the name of the member.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the member.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the member parameters.
        /// </summary>
        public ObservableCollection<MemberParameter> Parameters { get; set; }

        /// <summary>
        /// Gets or sets the member return type.
        /// </summary>
        public string ReturnType { get; set; }

        /// <summary>
        /// Gets or sets the member return description.
        /// </summary>
        public string ReturnDescription { get; set; }

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
                s += @"Parameters: none" + "\n\n";
            }
            else if (Parameters.Count == 0)
            {
                s += @"Parameters: none" + "\n\n";
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

            if (!string.IsNullOrEmpty(ReturnType))
            {
                ReturnType = @"void";
            }

            s += @"Return: " + ReturnType + "\n";

            if (!string.IsNullOrEmpty(ReturnDescription))
            {
                s += ReturnDescription + "\n";
            }

            return s;
        }
    }
}