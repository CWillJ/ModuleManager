namespace ClassLibrary1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Some object.
    /// </summary>
    public class SomeObject
    {
        private string _property;

        /// <summary>
        /// Constructor for some object.
        /// </summary>
        public SomeObject()
        {
            _property = string.Empty;
        }

        /// <summary>
        /// Second constructor for some object.
        /// </summary>
        /// <param name="propertyString">string.</param>
        public SomeObject(string propertyString)
        {
            _property = propertyString;
            AddNewLine();
        }

        /// <summary>
        /// Gets or sets a string.
        /// </summary>
        public string Property
        {
            get { return _property; }
            set { _property = value; }
        }

        /// <summary>
        /// Overrides the ToString method
        /// </summary>
        /// <returns>string object.</returns>
        public override string ToString()
        {
            return Property;
        }

        private void AddNewLine()
        {
            Property += Property + "\n";
        }
    }
}
