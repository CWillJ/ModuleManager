namespace ModuleManager.Classes
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;

    /// <summary>
    /// Module object holds the name and description of a module.
    /// </summary>
    public class Module : INotifyPropertyChanged
    {
        private string _moduleName;
        private string _moduleDescription;
        private string _methodsString;
        private ObservableCollection<ModuleMethod> _moduleMethods;

        /// <summary>
        /// Initializes a new instance of the <see cref="Module"/> class
        /// initializes properties to empty strings/empty collections.
        /// </summary>
        public Module()
        {
            Name = string.Empty;
            Description = string.Empty;
            MethodsString = string.Empty;
            Methods = new ObservableCollection<ModuleMethod>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Module"/> class
        /// initializes a Module specifying the name, description and methods.
        /// </summary>
        /// <param name="name">Module name.</param>
        /// <param name="description">Module description.</param>
        /// <param name="methods">Module methods.</param>
        public Module(string name, string description, ObservableCollection<ModuleMethod> methods)
        {
            Name = name;
            Description = description;
            MethodsString = MethodsToString(methods);
            Methods = methods;
            RaisePropertyChanged("Modules");
        }

        /// <summary>
        /// The event handler that handles a property change.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the module name.
        /// </summary>
        public string Name
        {
            get
            {
                return _moduleName;
            }

            set
            {
                if (_moduleName != value)
                {
                    _moduleName = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        /// <summary>
        /// Gets or sets the description of the module.
        /// </summary>
        public string Description
        {
            get
            {
                return _moduleDescription;
            }

            set
            {
                if (_moduleDescription != value)
                {
                    _moduleDescription = value;
                    RaisePropertyChanged("Description");
                }
            }
        }

        /// <summary>
        /// Gets or sets the methods as a string.
        /// </summary>
        public string MethodsString
        {
            get
            {
                return _methodsString;
            }

            set
            {
                if (_methodsString != value)
                {
                    _methodsString = value;
                    RaisePropertyChanged("MethodsString");
                }
            }
        }

        /// <summary>
        /// Gets or sets the methods in the current module.
        /// </summary>
        public ObservableCollection<ModuleMethod> Methods
        {
            get
            {
                return _moduleMethods;
            }

            set
            {
                if (_moduleMethods != value)
                {
                    _moduleMethods = value;
                    RaisePropertyChanged("Methods");
                }
            }
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
        /// AddMethod adds a method to the current module.
        /// </summary>
        /// <param name="method">Specifiex module method.</param>
        public void AddMethod(ModuleMethod method)
        {
            Methods.Add(method);
        }

        /// <summary>
        /// AddMethod adds a method to the current module with the specified properties.
        /// </summary>
        /// <param name="name">Method name.</param>
        /// <param name="description">Method description.</param>
        /// <param name="parameters">Method parameters.</param>
        /// <param name="returnType">Method return type.</param>
        public void AddMethod(string name, string description, ObservableCollection<MethodParameter> parameters, string returnType)
        {
            AddMethod(new ModuleMethod(name, description, parameters, returnType));
        }

        /// <summary>
        /// MethodCount gets the number of methods in Methods.
        /// </summary>
        /// <returns>Methods.Count().</returns>
        public int MethodCount()
        {
            return Methods.Count();
        }

        /// <summary>
        /// Overrides the ToString method and formats the string output.
        /// </summary>
        /// <returns>A desired format for the module name, description and all methods contained in module.</returns>
        public override string ToString()
        {
            string s = Name + @":" + "\n" + Description;

            s += "\n" + MethodsToString(Methods);

            return s;
        }

        /// <summary>
        /// Returns a collection of ModuleMethod objects as a string.
        /// </summary>
        /// <param name="methods">A collection of ModuleMethods.</param>
        /// <returns>String of the ModuleMethods.</returns>
        public string MethodsToString(ObservableCollection<ModuleMethod> methods)
        {
            string s = string.Empty;

            foreach (ModuleMethod method in methods)
            {
                s += method.ToString() + "\n";
            }

            return s;
        }
    }
}
