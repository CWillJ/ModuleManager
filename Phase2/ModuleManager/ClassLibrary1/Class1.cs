﻿namespace ClassLibrary1
{
    using System.Collections.Specialized;
    using System.ComponentModel;

    /// <summary>
    /// Class1 only exists to test getting info from a .dll and the related .xml files.
    /// Also, I'm going to try to get this to appear on a new line.
    /// </summary>
    public class Class1 : INotifyCollectionChanged
    {
        private string _property1;
        private int _property2;

        /// <summary>
        /// Initializes a new instance of the <see cref="Class1"/> class. Also trying to get this to appear on a new line... Trying, trying, trying... Maybe this will do it. Testing, 1, 2, 3.
        /// </summary>
        public Class1()
        {
            _property1 = string.Empty;
            _property2 = 0;
            System.Console.WriteLine(Method1("This is a fake class").ToString());
            _ = CollectionChanged;
        }

        /// <summary>
        /// Property handler for tests.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Document for CollectionChanged.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Gets or sets Property1 as a string.
        /// </summary>
        public string Property1
        {
            get
            {
                return _property1;
            }

            set
            {
                _property1 = value;
                RaisePropertyChanged("Property1");
            }
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
        /// Raise a property changed event.
        /// </summary>
        /// <param name="property">Property passed in as a string.</param>
        public void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        /// <summary>
        /// Needed to use CollectionChanged so here it is.
        /// </summary>
        public void RaiseCollectionChanged()
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(default));
        }

        /// <summary>
        /// Comments for Method1
        /// </summary>
        /// <param name="str">Description of only parameter.</param>
        /// <returns>a SomeObject.</returns>
        public SomeObject Method1(string str)
        {
            SomeObject so = new SomeObject(str);
            Property1 = so.Property;
            Property2 = 21;
            System.Console.WriteLine(Method1(Property1, Property2).ToString());

            return so;
        }

        /// <summary>
        /// Adds the integer num to the passed in string, str.
        /// Here is an extra line!
        /// </summary>
        /// <param name="str">This parameter is a string.</param>
        /// <param name="num">And this one is an integer.</param>
        /// <returns>SomeObject str plus a string of the integer num.</returns>
        public SomeObject Method1(string str, int num)
        {
            SomeObject so = new SomeObject(str + num.ToString());
            return so;
        }
    }
}
