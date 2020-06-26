namespace ClassLibrary1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Class2 only exists to test getting info from a .dll and the
    /// related .xml files.
    /// </summary>
    public class Class2
    {
        int _num;

        /// <summary>
        /// Constructor for Class2
        /// </summary>
        public Class2()
        {
            _num = 21;
            Method3();
            Method1("This is a another fake class");
        }

        /// <summary>
        /// Constructor for Class2 that takes 1 parameter
        /// </summary>
        public Class2(int constructorParameter)
        {
            _num = constructorParameter;
            Method3();
            Method1("This is a another fake class");
        }

        /// <summary>
        /// Comments for Method1
        /// </summary>
        /// <param name="str"></param>
        public void Method1(string str)
        {
            System.Console.WriteLine(Method2(str, _num));
        }

        /// <summary>
        /// Adds the integer num to the passed in string, str.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="num"></param>
        /// <returns>str plus a string of the integer num</returns>
        public string Method2(string str, int num)
        {
            return str + num.ToString();
        }

        /// <summary>
        /// This is a third class with no return and no parameters
        /// </summary>
        public void Method3()
        {
            _num = _num + 21;
        }
    }
}
