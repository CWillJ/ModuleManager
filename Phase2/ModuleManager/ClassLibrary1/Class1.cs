namespace ClassLibrary1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Class1 only exists to test getting info from a .dll and the
    /// related .xml files.
    /// </summary>
    public class Class1
    {
        private string _property1;
        private int _property2;

        /// <summary>
        /// Constructor for Class1
        /// </summary>
        public Class1()
        {
            _property1 = string.Empty;
            _property2 = 0;
            Method1("This is a fake class");
        }

        /// <summary>
        /// Gets or sets Property1 as a string.
        /// </summary>
        public string Property1
        {
            get { return _property1; }
            set { _property1 = value; }
        }

        /// <summary>
        /// Gets or sets Property2 as an integer.
        /// </summary>
        public int Property2
        {
            get { return _property2; }
            set { _property2 = value; }
        }

        /// <summary>
        /// Comments for Method1
        /// </summary>
        /// <param name="str"></param>
        public void Method1(string str)
        {
            Property1 = str;
            Property2 = 21;
            System.Console.WriteLine(Method2(Property1, Property2));
        }

        /// <summary>
        /// Adds the integer num to the passed in string, str.
        /// Here is an extra line!
        /// </summary>
        /// <param name="str"></param>
        /// <param name="num"></param>
        /// <returns>str plus a string of the integer num</returns>
        public string Method2(string str, int num)
        {
            return str + num.ToString();
        }
    }
}
