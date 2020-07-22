namespace ModuleManager.ViewModels
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using ModuleManager.Classes;

    /// <summary>
    /// MemberDescription view model.
    /// </summary>
    public class MemberDescriptionViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Module> _modules;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberDescriptionViewModel"/> class.
        /// </summary>
        public MemberDescriptionViewModel()
        {
        }

        /// <summary>
        /// PropertyChanged event handler.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets a collection of Module objects.
        /// </summary>
        public ObservableCollection<Module> Modules
        {
            get
            {
                return _modules;
            }

            set
            {
                _modules = value;
                RaisePropertyChanged("Modules");
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
    }
}