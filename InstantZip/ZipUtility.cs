using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstantZip
{
    public static class ZipUtility
    {
        public const string ZipExtension = ".zip";

        /// <summary>
        /// Zip the file to the destination
        /// </summary>
        /// <param name="source">File to compress</param>
        /// <param name="destination">Destination zip file</param>
        public static void Zip(FileInfo source, string destinationZip)
        {
            ZipArchive(destinationZip, (archive) =>
                archive.CreateEntryFromFile(source.FullName, source.Name, AppOptions.CompressionLevel));
            
        }

        /// <summary>
        /// Zip the files to the destination
        /// </summary>
        /// <param name="source">Files to compress</param>
        /// <param name="destination">Destination zip file</param>
        public static void Zip(FileInfo[] source, string destination)
        {
            foreach (FileInfo file in source)
                Zip(file, destination);
        }

        /// <summary>
        /// Zip the directory to the destination
        /// </summary>
        /// <param name="source">Directory to compress</param>
        /// <param name="destination">Destination zip file</param>
        public static void Zip(DirectoryInfo source, string destinationZip)
        {
            ZipArchive(destinationZip, (archive) =>
                FillZip(archive, source));
        }

        /// <summary>
        /// Zip the directories to the destination
        /// </summary>
        /// <param name="source">Directories to compress</param>
        /// <param name="destination">Destination zip file</param>
        public static void Zip(DirectoryInfo[] source, string destination)
        {
            foreach (DirectoryInfo directory in source)
                Zip(directory, destination);
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
    }
}
