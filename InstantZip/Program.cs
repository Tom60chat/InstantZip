using InstantZip;
using System.IO.Compression;

AppOptions.Apply(args);

if (File.Exists(AppOptions.Destination))
    Console.WriteLine("Destination file already exists");
else if (AppOptions.Sources.Count == 0)
    PrintHelp();
else
{
    foreach (var source in AppOptions.Sources)
    {
        bool conflict = false;
        Dictionary<DirectoryInfo, string> directories = new();
        Dictionary<FileInfo, string> files = new();

        if (PathParser.IsDirectory(source))
        {
            var subDirectories = PathParser.StringToDirectories(source);

            foreach (var directory in subDirectories)
            {
                var destinationZip = ZipUtility.GetDestination(directory.Name, AppOptions.Destination);

                if (!directory.Exists)
                {
                    Console.WriteLine($"Directory {directory.Name} don't exists");
                    conflict = true;
                }
                if (File.Exists(destinationZip))
                {
                    Console.WriteLine($"{directory.Name}{ZipUtility.ZipExtension} already exists");
                    conflict = true;
                }

                directories.Add(directory, destinationZip);
            }
        }
        else
        {
            var subFiles = PathParser.StringToFiles(source);

            foreach (var file in subFiles)
            {
                var destinationZip = ZipUtility.GetDestination(file.Name, AppOptions.Destination);

                if (!file.Exists)
                {
                    Console.WriteLine($"File {file.Name} don't exists");
                    conflict = true;
                }
                if (File.Exists(destinationZip))
                {
                    Console.WriteLine($"{file.Name}{ZipUtility.ZipExtension} already exists");
                    conflict = true;
                }

                files.Add(file, destinationZip);
            }
        }

        if (conflict)
        {
            Console.WriteLine($"Extraction aborted for {source}");
        }
        else
        {
            // Extracting directories
            if (PathParser.IsDirectory(AppOptions.Destination) || directories.Count == 1)
                foreach (var subDirectory in directories)
                    ZipFile.CreateFromDirectory(subDirectory.Key.FullName, subDirectory.Value, AppOptions.CompressionLevel, false);
            else if (directories.Count != 0)
                ZipUtility.Zip(directories.Keys, directories.First().Value);

            // Extracting files
            foreach (var subFiles in files)
                ZipUtility.Zip(subFiles.Key, subFiles.Value);
        }
    }
}


static void PrintHelp()
{
    Console.WriteLine(@"
InstantZip by Tom60chat (Tom OLIVIER)
https://github.com/Tom60chat/InstantZip

Usage:
  InstantZip files... [-d destination] [-c compression-level]

  Source path can be written as:
    Directory:      ./Directory/ -> Directory.zip
    Directories:    ./MyDirectories/*/ -> Directory1.zip, Directory2.zip, ...
                    ./MyDirectories/My*/ -> MyDirectory1.zip, MyDirectory2.zip, ...
    File:           ./File -> File.zip
    Files :         ./MyFiles/* -> File1.zip, File2.zip, ...
                    ./MyFiles/My* -> MyFile1.zip, MyFile2.zip, ...

  Destination path can be written as:
    ./Destination/ -> ./Destination/source.zip
    ./source.zip -> ./source.zip

Arguments:
    -d, --destination
      destination directory/filename

    -c, --compression-level
        compression level

        NoCompression, n
          No compression should be performed on the file.
        Fastest, f (Default)
          The compression operation should complete as quickly as possible,
          even if the resulting file is not optimally compressed.
        Optimal, o
          The compression operation should be optimally compressed,
          even if the operation takes a longer time to complete.
        SmallestSize, s
          The compression operation should create output as small as possible,
          even if the operation takes a longer time to complete.
    ");
}