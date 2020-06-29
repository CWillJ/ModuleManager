namespace LoadDLLs.Classes
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;

    /// <summary>
    /// Module object holds the name and description of a module.
    /// </summary>
    public class Module : INotifyPropertyChanged
    {
        private string moduleName;
        private string moduleDescription;
        private string methodsString;
        private ObservableCollection<ModuleMethod> moduleMethods;

        /// <summary>
        /// Initializes a new instance of the <see cref="Module"/> class
        /// initializes properties to empty strings/empty collections.
        /// </summary>
        public Module()
        {
            this.Name = string.Empty;
            this.Description = string.Empty;
            this.MethodsString = string.Empty;
            this.Methods = new ObservableCollection<ModuleMethod>();
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
            this.Name = name;
            this.Description = description;
            this.MethodsString = this.MethodsToString(methods);
            this.Methods = methods;
            this.RaisePropertyChanged("Modules");
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
                return this.moduleName;
            }

            set
            {
                if (this.moduleName != value)
                {
                    this.moduleName = value;
                    this.RaisePropertyChanged("Name");
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
                return this.moduleDescription;
            }

            set
            {
                if (this.moduleDescription != value)
                {
                    this.moduleDescription = value;
                    this.RaisePropertyChanged("Description");
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
                return this.methodsString;
            }

            set
            {
                if (this.methodsString != value)
                {
                    this.methodsString = value;
                    this.RaisePropertyChanged("MethodsString");
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
                return this.moduleMethods;
            }

            set
            {
                if (this.moduleMethods != value)
                {
                    this.moduleMethods = value;
                    this.RaisePropertyChanged("Methods");
                }
            }
        }

        /// <summary>
        /// Raise a property changed event.
        /// </summary>
        /// <param name="property">Property passed in as a string.</param>
        public void RaisePropertyChanged(string property)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        /// <summary>
        /// AddMethod adds a method to the current module.
        /// </summary>
        /// <param name="method">Specifiex module method.</param>
        public void AddMethod(ModuleMethod method)
        {
            this.Methods.Add(method);
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
            this.AddMethod(new ModuleMethod(name, description, parameters, returnType));
        }

        /// <summary>
        /// MethodCount gets the number of methods in Methods.
        /// </summary>
        /// <returns>Methods.Count().</returns>
        public int MethodCount()
        {
            return this.Methods.Count();
        }

        /// <summary>
        /// Overrides the ToString method and formats the string output.
        /// </summary>
        /// <returns>A desired format for the module name, description and all methods contained in module.</returns>
        public override string ToString()
        {
            string s = this.Name + @":" + "\n" + this.Description;

            s += "\n" + this.MethodsToString(this.Methods);

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
