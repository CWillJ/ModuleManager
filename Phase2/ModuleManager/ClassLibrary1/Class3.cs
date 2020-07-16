namespace ClassLibrary1
{
    /// <summary>
    /// Class3 only exists to test getting info from a .dll and the
    /// related .xml files.
    /// </summary>
    public class Class3
    {
        private int _num;
        private string _str;

        /// <summary>
        /// Initializes a new instance of the <see cref="Class3"/> class.
        /// </summary>
        public Class3()
        {
            _num = 21;
            Method3();
            Method1(@"This is a another fake class");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Class3"/> class.
        /// </summary>
        /// <param name="constructorParameter">An int parameter.</param>
        public Class3(int constructorParameter)
        {
            _num = constructorParameter;
            Method3();
            Method1(@"This is a another fake class");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Class3"/> class.
        /// </summary>
        /// <param name="constructorStringParameter">A string parameter.</param>
        public Class3(string constructorStringParameter)
        {
            _str = constructorStringParameter;
            Method4();
            Method1(@"This is a another fake class");
        }

        /// <summary>
        /// Comments for Method1.
        /// </summary>
        /// <param name="str">String parameter for Method1.</param>
        public void Method1(string str)
        {
            System.Console.WriteLine(Method2(str + _str, _num));
        }

        /// <summary>
        /// Adds the integer num to the passed in string, str.
        /// </summary>
        /// <param name="str">String parameter.</param>
        /// <param name="num">Integer parameter.</param>
        /// <returns>str plus a string of the integer num.</returns>
        public string Method2(string str, int num)
        {
            return str + num.ToString();
        }

        /// <summary>
        /// This is a third class with no return and no parameters.
        /// </summary>
        public void Method3()
        {
            _num += 21;
        }

        /// <summary>
        /// Method 4 sets a default string.
        /// </summary>
        public void Method4()
        {
            _str = @" that doesn't do anything";
        }
    }
}
