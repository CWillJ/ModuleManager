namespace LoadDLLs
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    //using System.Windows;
    using System.Windows.Forms;

    public class LoadFile
    {
        string _fileLocation;

        public MyICommand LoadMyFileCommand { get; set; }

        public LoadFile()
        {
            FileLocation = string.Empty;
            LoadMyFileCommand = new MyICommand(FindDLLs, CanOpen);
        }

        public string FileLocation
        {
            get { return _fileLocation; }
            set { _fileLocation = value; }
        }

        private void FindDLLs()
        {
            // Bring up explorer to allow user to choose a file location
            FileLocation = LookForFile();
            MessageBox.Show(FileLocation);

            // Check the file location for any .dll's
            DissectDll dll = new DissectDll();
            dll.GetDllInfo();

            // Message prompt if no .dll's found

            // Display all names of found .dll's
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
