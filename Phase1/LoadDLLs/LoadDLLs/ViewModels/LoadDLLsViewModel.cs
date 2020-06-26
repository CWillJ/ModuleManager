namespace LoadDLLs.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Forms;
    using LoadDLLs.Classes;

    /// <summary>
    /// LoadDLLsViewModel will handle commands from the main view
    /// </summary>
    public class LoadDLLsViewModel
    {
        /// <summary>
        /// Constructor for LoadDLLsViewModel that initializes FileLocation as an empty string
        /// and initializes the LoadMyFileCommand
        /// </summary>
        public LoadDLLsViewModel()
        {
            FileLocation = string.Empty;
            Modules = new ObservableCollection<Module>();
            LoadMyFileCommand = new MyICommand(FindDLLs);
        }

        /// <summary>
        /// Gets and sets the LoadMyFileCommand as a MyICommand
        /// </summary>
        public MyICommand LoadMyFileCommand { get; set; }

        /// <summary>
        /// Gets and sets the file location as a string
        /// </summary>
        public string FileLocation { get; set; }

        public ObservableCollection<Module> Modules { get; set; }

        private void FindDLLs()
        {
            DissectDll dll = new DissectDll();

            // Bring up explorer to allow user to choose a file location
            //// FileLocation = LookForFile();

            // Check the file location for any .dll's
            foreach (var mod in dll.GetInfoFromDll())
            {
                Modules.Add(new Module(mod.Name, mod.Description, mod.Methods));
            }
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

        private bool CanOpen()
        {
            return true;
        }
    }
}
