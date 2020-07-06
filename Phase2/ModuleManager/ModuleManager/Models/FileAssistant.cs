namespace ModuleManager.Models
{
    using System.IO;

    /// <summary>
    /// FileAssistant is used for file operations from the view model.
    /// </summary>
    public class FileAssistant
    {
        private string[] _dllFiles;
        private string[] _xmlFiles;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileAssistant"/> class.
        /// </summary>
        public FileAssistant()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileAssistant"/> class.
        /// </summary>
        /// <param name="directoryName">The directory that contains the .dll and .xml files.</param>
        public FileAssistant(string directoryName)
        {
            LoadFileNames(directoryName);
        }

        /// <summary>
        /// Gets or sets the DllFiles string array that stores .dll files.
        /// </summary>
        public string[] DllFiles
        {
            get { return _dllFiles; }
            set { _dllFiles = value; }
        }

        /// <summary>
        /// Gets or sets the XMLFiles string array that stores .xml files.
        /// </summary>
        public string[] XmlFiles
        {
            get { return _xmlFiles; }
            set { _xmlFiles = value; }
        }

        /// <summary>
        /// LoadFileNames sets the DllFiles and XmlFiles string arrays.
        /// </summary>
        /// <param name="directoryName">The directory that contains the .dll and .xml files.</param>
        public void LoadFileNames(string directoryName)
        {
            string[] directories = Directory.GetDirectories(directoryName);

            if (directories.Length == 0)
            {
                DllFiles = Directory.GetFiles(directoryName, @"*.dll");
                XmlFiles = Directory.GetFiles(directoryName, @"*.xml");
            }
            else
            {
                foreach (string directory in directories)
                {
                    DllFiles = Directory.GetFiles(directory, @"*.dll");
                    XmlFiles = Directory.GetFiles(directory, @"*.xml");
                }
            }
        }
    }
}
