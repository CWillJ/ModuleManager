namespace ModuleManager.ViewModels
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Forms;
    using ModuleManager.Classes;
    using ModuleManager.Models;

    /// <summary>
    /// ModuleManagerViewModel will handle commands from the main view.
    /// </summary>
    public class ModuleManagerViewModel : INotifyPropertyChanged
    {
        private object _selectedModule;
        private string _memberText;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleManagerViewModel"/> class.
        /// </summary>
        public ModuleManagerViewModel()
        {
            _selectedModule = null;
            _memberText = string.Empty;
            FileLocation = string.Empty;
            Modules = new ObservableCollection<Module>();
            LoadMyFileCommand = new MyICommand(FindDLLs);
            SaveConfigCommand = new MyICommand(SaveConfig);
            DisplayCommand = new MyICommand(DisplayMethodData);
        }

        /// <summary>
        /// PropertyChanged event handler.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the LoadMyFileCommand as a MyICommand.
        /// </summary>
        public MyICommand LoadMyFileCommand { get; set; }

        /// <summary>
        /// Gets or sets the SaveConfigCommand as a MyICommand.
        /// </summary>
        public MyICommand SaveConfigCommand { get; set; }

        /// <summary>
        /// Gets or sets the DisplayCommand as a MyICommand.
        /// </summary>
        public MyICommand DisplayCommand { get; set; }

        /// <summary>
        /// Gets or sets the file location as a string.
        /// </summary>
        public string FileLocation { get; set; }

        /// <summary>
        /// Gets or sets MemberText.
        /// </summary>
        public string MemberText
        {
            get
            {
                return _memberText;
            }

            set
            {
                _memberText = value;
                RaisePropertyChanged("MemberText");
            }
        }

        /// <summary>
        /// Gets or sets the selected module from the TreeView.
        /// </summary>
        public object SelectedModule
        {
            get
            {
                return _selectedModule;
            }

            set
            {
                _selectedModule = value;
                MemberText = _selectedModule.ToString();
                RaisePropertyChanged("SelectedModule");
            }
        }

        /// <summary>
        /// Gets or sets the collection of Modules.
        /// </summary>
        public ObservableCollection<Module> Modules { get; set; }

        /// <summary>
        /// Raise a property changed event.
        /// </summary>
        /// <param name="property">Property passed in as a string.</param>
        public void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void DisplayMethodData()
        {
            return;
        }

        private void FindDLLs()
        {
            //// ModuleInfoRetriever infoRetriever = new ModuleInfoRetriever(LookForFile());
            //// ModuleInfoRetriever infoRetriever = new ModuleInfoRetriever();

            // Bring up explorer to allow user to choose a file location
            FileLocation = LookForFile();
            ModuleInfoRetriever infoRetriever = new ModuleInfoRetriever();

            // Check the file location for any .dll's
            foreach (var mod in infoRetriever.GetInfoFromDll())
            {
                Modules.Add(new Module(mod.Name, mod.Description, mod.Members));
            }

            MemberText = Modules[0].Members[4].ToString();
        }

        private void SaveConfig()
        {
            MessageBox.Show(@"Configuration File Saved (not yet)");
        }

        private void LoadConfig()
        {
            return;
        }

        private string LookForFile()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK
                && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
            {
                return folderBrowserDialog.SelectedPath;
            }

            return null;
        }
    }
}