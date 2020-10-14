﻿namespace ModuleManager.ModuleObjects.Classes
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// An object designed to hold the type and name of a member's parameter.
    /// </summary>
    public class MemberParameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberParameter"/> class.
        /// Default constructor.
        /// </summary>
        public MemberParameter()
        {
            Type = null;
            TypeName = string.Empty;
            Name = @"None";
            Description = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberParameter"/> class.
        /// Used when the parameter Type is available.
        /// </summary>
        /// <param name="type">MemberParameter type.</param>
        /// <param name="name">MemberParameter name.</param>
        /// <param name="description">MemberParameter description.</param>
        public MemberParameter(Type type, string name, string description)
        {
            Type = type;
            TypeName = type.Name;
            Name = name;
            Description = description;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberParameter"/> class.
        /// Used when the actual Type cannot be reached.
        /// </summary>
        /// <param name="typeName">MemberParameter type.</param>
        /// <param name="name">MemberParameter name.</param>
        /// <param name="description">MemberParameter description.</param>
        public MemberParameter(string typeName, string name, string description)
        {
            Type = null;
            TypeName = typeName;
            Name = name;
            Description = description;
        }

        /// <summary>
        /// Gets or sets the parameter type.
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Gets or sets the parameter name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the parameter description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the actuall Type of the MemberParameter.
        /// </summary>
        [XmlIgnore]
        public Type Type { get; set; }

        /// <summary>
        /// Check to see if the MemberParameter is empty.
        /// </summary>
        /// <returns>Returns true if the name is not an empty string or null.</returns>
        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(Description) && string.IsNullOrEmpty(TypeName) && Name == @"None";
        }

        /// <summary>
        /// Overrides the ToString method and formats the string output of parameter type and name.
        /// </summary>
        /// <returns>A desired format for the parameter type and name.</returns>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(TypeName) || string.IsNullOrEmpty(Name))
            {
                return @"None" + "\n";
            }

            string s = TypeName + @", " + Name + "\n";

            if (!string.IsNullOrEmpty(Description))
            {
                s += Description + "\n";
            }

            return s;
        }
    }
}