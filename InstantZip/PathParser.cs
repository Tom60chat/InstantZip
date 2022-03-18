namespace InstantZip
{
    public static class PathParser
    {
        #region Methods
        /// <summary>
        /// If the string it's a directory
        /// </summary>
        /// <param name="source">Source path</param>
        /// <returns>It's a directory</returns>
        public static bool IsDirectory(string source) =>
            /// ./Directory/ -> Directory.zip
            /// ./MyDirectories/*/ -> Directory1.zip, Directory2.zip, ...
            Path.EndsInDirectorySeparator(source) || Directory.Exists(source);

        /// <summary>
        /// Parse a string to directories
        /// </summary>
        /// <param name="source">Source path</param>
        /// <returns>Finded directories</returns>
        public static DirectoryInfo[] StringToDirectories(string source)
        {
            DirectoryInfo directory = new(source);

            // Directories
            /// ./MyDirectories/*/ -> Directory1.zip, Directory2.zip, ...
            if (directory.Name.Contains('*'))
            {
                var parent = directory.GetParentOrRoot();
                return parent.GetDirectories(directory.Name);
            }
            // Directory
            /// ./Directory/ -> Directory.zip
            else
                return new DirectoryInfo[] { directory };
        }

        /// <summary>
        /// Parse a string to files
        /// </summary>
        /// <param name="source">Source path</param>
        /// <returns>Finded files</returns>
        public static FileInfo[] StringToFiles(string source)
        {
            FileInfo file = new(source);

            // Files
            /// ./MyFiles/* -> File1.zip, File2.zip, ...
            if (file.Name.Contains('*'))
            {
                if (file.Directory == null)
                    return Array.Empty<FileInfo>(); // Should not append

                return file.Directory.GetFiles(file.Name);
            }
            // File
            /// ./File -> File.zip
            else
                return new FileInfo[] { file };
        }
        #endregion
    }
}
