using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    /// <summary>
    /// Class1 only exists to test getting info from a .dll and the
    /// related .xml files.
    /// </summary>
    public class Class1
    {

        /// <summary>
        /// Constructor for Class1
        /// </summary>
        Class1()
        {
            Method1("This is a fake class");
        }

        /// <summary>
        /// Comments for Method1
        /// </summary>
        /// <param name="str"></param>
        public void Method1(string str)
        {
            System.Console.WriteLine(Method2(str,21));
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
