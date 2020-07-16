namespace ClassLibrary1
{
    /// <summary>
    /// Some object.
    /// </summary>
    public class SomeObject
    {
        private string _property;

        /// <summary>
        /// Initializes a new instance of the <see cref="SomeObject"/> class.
        /// </summary>
        public SomeObject()
        {
            _property = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SomeObject"/> class.
        /// </summary>
        /// <param name="propertyString">A string parameter for SomeObject.</param>
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
