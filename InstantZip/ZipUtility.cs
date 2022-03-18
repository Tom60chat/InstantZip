using System.IO.Compression;

namespace InstantZip
{
    public static class ZipUtility
    {
        #region Variables
        public const string ZipExtension = ".zip";
        #endregion

        #region Methods
        /// <summary>
        /// Zip the file to the destination
        /// </summary>
        /// <param name="source">File to compress</param>
        /// <param name="destination">Destination zip file</param>
        /// <returns>if successful</returns>
        public static bool Zip(FileInfo source, string destinationZip)
        {
            if (!source.Exists)
                return false;

            ZipArchive(destinationZip, (archive) =>
                archive.CreateEntryFromFile(source.FullName, source.Name, AppOptions.CompressionLevel));

            return true;
        }

        /// <summary>
        /// Zip the directory to the destination
        /// </summary>
        /// <param name="source">Directory to compress</param>
        /// <param name="destination">Destination zip file</param>
        /// <returns>if successful</returns>
        public static bool Zip(DirectoryInfo source, string destinationZip)
        {
            if (!source.Exists)
                return false;

            ZipArchive(destinationZip, (archive) =>
                FillZip(archive, source));

            return true;
        }

        /// <summary>
        /// Zip the directory to the destination
        /// </summary>
        /// <param name="source">Directory to compress</param>
        /// <param name="destination">Destination zip file</param>
        /// <returns>if successful</returns>
        public static bool Zip(ICollection<DirectoryInfo> source, string destinationZip)
        {
            bool result = true;

            ZipArchive(destinationZip, (archive) =>
            {
                foreach (var directory in source)
                {
                    if (!directory.Exists)
                    {
                        File.Delete(destinationZip);
                        result = false;
                        return;
                    }

                    FillZip(archive, directory);
                }
            });

            return result;
        }

        public static string GetDestination(string source, string destination)
        {
            /// ./Destination/ -> ./Destination/source.zip
            if (Path.EndsInDirectorySeparator(destination) || Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
                return Path.Combine(destination, source + ZipExtension);
            }
            /// ./source.zip -> ./source.zip
            else
            {
                var directory = Path.GetDirectoryName(destination);
                if (directory != null)
                    Directory.CreateDirectory(directory);
                return destination;
            }
        }

        private static void FillZip(ZipArchive archive, DirectoryInfo sourceDirectory, string? directoryName = null)
        {
            if (directoryName == null)
                directoryName = sourceDirectory.Name;

            // Files
            foreach (var file in sourceDirectory.GetFiles())
                archive.CreateEntryFromFile(file.FullName, Path.Combine(directoryName, file.Name), AppOptions.CompressionLevel);

            // Directories
            foreach (var directory in sourceDirectory.GetDirectories())
                FillZip(archive, directory, Path.Combine(directoryName, directory.Name));
        }

        private static void ZipArchive(string destinationZip, Action<ZipArchive> action)
        {
            using FileStream zipToOpen = new(destinationZip, FileMode.OpenOrCreate);
            using ZipArchive archive = new(zipToOpen, ZipArchiveMode.Update);

            action.Invoke(archive);
        }
        #endregion
    }
}
