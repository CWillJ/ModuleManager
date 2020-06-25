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
            OneModule = new Module();
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

        public Module OneModule { get; set; }

        private void FindDLLs()
        {
            DissectDll dll = new DissectDll();

            // Bring up explorer to allow user to choose a file location
            //// FileLocation = LookForFile();
            //// MessageBox.Show(FileLocation);

            // Check the file location for any .dll's
            dll.GetInfoFromDll();
            MessageBox.Show(dll.Modules.ElementAt(0).ToString());
            //// OneModule = dll.Modules.ElementAt(0);
            OneModule.Name = dll.Modules.ElementAt(0).Name;
            OneModule.Description = dll.Modules.ElementAt(0).Description;
            OneModule.MethodsString = dll.Modules.ElementAt(0).MethodsString;
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
