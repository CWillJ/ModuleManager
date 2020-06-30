﻿namespace ModuleManager.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows.Forms;
    using ModuleManager.Classes;
    using ModuleManager.Models;

    /// <summary>
    /// LoadDLLsViewModel will handle commands from the main view.
    /// </summary>
    public class ModuleManagerViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleManagerViewModel"/> class.
        /// </summary>
        public ModuleManagerViewModel()
        {
            FileLocation = string.Empty;
            Modules = new ObservableCollection<Module>();
            LoadMyFileCommand = new MyICommand(FindDLLs);
            SaveConfigCommand = new MyICommand(SaveConfig);
        }

        /// <summary>
        /// Gets or sets the LoadMyFileCommand as a MyICommand.
        /// </summary>
        public MyICommand LoadMyFileCommand { get; set; }

        /// <summary>
        /// Gets or sets the SaveConfigCommand as a MyICommand.
        /// </summary>
        public MyICommand SaveConfigCommand { get; set; }

        /// <summary>
        /// Gets or sets the file location as a string.
        /// </summary>
        public string FileLocation { get; set; }

        /// <summary>
        /// Gets or sets the collection of Modules.
        /// </summary>
        public ObservableCollection<Module> Modules { get; set; }

        private void FindDLLs()
        {
            //// ModuleInfoRetriever infoRetriever = new ModuleInfoRetriever(LookForFile());
            //// ModuleInfoRetriever infoRetriever = new ModuleInfoRetriever();

            // Bring up explorer to allow user to choose a file location
            FileLocation = LookForFile();
            MessageBox.Show(FileLocation);
            ModuleInfoRetriever infoRetriever = new ModuleInfoRetriever();

            // Check the file location for any .dll's
            foreach (var mod in infoRetriever.GetInfoFromDll())
            {
                Modules.Add(new Module(mod.Name, mod.Description, mod.Methods));
            }
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